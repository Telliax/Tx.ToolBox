using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Storage
{
    public class XmlStorage : StorageBase, IDisposable
    {
        public XmlStorage(FileInfo path) 
            : this(path?.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite) ?? throw new ArgumentNullException(nameof(path)))
        {
        }

        public XmlStorage(Stream stream, bool ownsStream = true)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _ownsStream = ownsStream;
            Schema = new XmlStorageSchema();
        }

        public XmlStorageSchema Schema { get; set; }

        public override void Load()
        {
            lock (Items)
            {
                _stream.Position = 0;
                var doc = XDocument.Load(_stream);
                var root = doc.Element(Schema.RootTag);
                if (root == null)
                {
                    Items.Clear();
                    return;
                }

                var elements = root.Elements(Schema.ItemTag).ToArray();
                var loadedIds = new HashSet<string>();

                foreach (var element in elements)
                {
                    if (element.IsEmpty) continue;
                    var id = element.Attribute(Schema.IdAttribute).Value;
                    loadedIds.Add(id);
                    var typeString = element.Attribute(Schema.TypeAttribute).Value;
                    var type = Type.GetType(typeString);
                    using (var reader = element.FirstNode.CreateReader())
                    {
                        var item = new XmlSerializer(type).Deserialize(reader);
                        var container = GetContainer(type, id);
                        if (container.ItemType != type)
                        {
                            Remove(id);
                            container = GetContainer(type, id);
                        }
                        container.Set(item);
                    }


                }

                Items.Keys
                     .Where(k => !loadedIds.Contains(k))
                     .ToArray()
                     .ForEach(k => Remove(k));
            }
        }

        public override void Save()
        {
            lock (Items)
            {
                var root = new XElement(Schema.RootTag);
                foreach (var container in Items.Values)
                {
                    var ser = new XmlSerializer(container.ItemType);
                    var element = new XElement(Schema.ItemTag, new XAttribute(Schema.IdAttribute, container.Id), new XAttribute(Schema.TypeAttribute, container.ItemType.AssemblyQualifiedName));
                    var xml = new XDocument();
                    using (var writer = xml.CreateWriter())
                    {
                        ser.Serialize(writer, container.Item, null);
                    }
                    element.Add(xml.Root);
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

        private readonly Stream _stream;
        private readonly bool _ownsStream;
    }

    public class XmlStorageSchema
    {
        public string RootTag { get; set; } = "Storage";
        public string ItemTag { get; set; } = "Item";
        public string IdAttribute { get; set; } = "Id";
        public string TypeAttribute { get; set; } = "Type";
    }
}
