using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Settings
{
    public class XmlSettingsStorage : SettingsStorageBase, IDisposable
    {
        public XmlSettingsStorage(FileInfo path) : this(path.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
        }

        public XmlSettingsStorage(Stream stream, bool ownsStream = true)
        {
            _stream = stream;
            _ownsStream = ownsStream;
            Schema = new XmlSchema();
        }

        public XmlSchema Schema { get; set; }

        public override void Load()
        {
            lock (SettingsMap)
            {
                var doc = XDocument.Load(_stream);
                var root = doc.Element(Schema.RootTag);
                if (root == null)
                {
                    SettingsMap.Clear();
                    return;
                }

                var elements = root.Elements(Schema.SettingsTag).ToArray();
                var loadedIds = new HashSet<string>();

                foreach (var element in elements)
                {
                    var id = element.Attribute(Schema.IdAttribute).Value;
                    loadedIds.Add(id);
                    var typeString = element.Attribute(Schema.TypeAttribute).Value;
                    var type = Type.GetType(typeString);
                    var settings = new XmlSerializer(type).Deserialize(element.CreateReader());
                    var container = GetContainer(type, id);
                    if (container.SettingsType != type)
                    {
                        Remove(id);
                        container = GetContainer(type, id);
                    }
                    container.Set(settings);
                }

                SettingsMap.Keys
                           .Where(k => !loadedIds.Contains(k))
                           .ToArray()
                           .ForEach(k => Remove(k));
            }
        }

        public override void Save()
        {
            lock (SettingsMap)
            {
                var root = new XElement(Schema.RootTag);
                foreach (var container in SettingsMap.Values)
                {
                    var ser = new XmlSerializer(container.SettingsType);
                    var element = new XElement(Schema.SettingsTag, new XAttribute(Schema.IdAttribute, container.Id), new XAttribute(Schema.TypeAttribute, container.SettingsType));
                    ser.Serialize(element.CreateWriter(), container.Settings);
                    root.Add(element);
                }
                var doc = new XDocument(root);
                _stream.SetLength(0);
                doc.Save(_stream);
                _stream.Flush();
            }
        }

        public void Dispose()
        {
            if (_ownsStream)
            {
                _stream?.Dispose();
            }
        }

        private readonly FileInfo _path;
        private readonly Stream _stream;
        private readonly bool _ownsStream;
    }

    public class XmlSchema
    {
        public string RootTag { get; set; } = "Configuration";
        public string SettingsTag { get; set; } = "Settings";
        public string IdAttribute { get; set; } = "IdAttribute";
        public string TypeAttribute { get; set; } = "IdAttribute";
    }
}
