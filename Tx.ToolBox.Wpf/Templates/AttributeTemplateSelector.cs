using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Tx.ToolBox.Wpf.Templates
{
    /// <summary>
    /// Generates DataTemplates for viemodels that were decorated with TemplateAttribute
    /// </summary>
    public class AttributeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var attribute = GetAttribute(item, container);
            if (attribute == null) return null;

            var view = attribute.ViewType;
            var dataContext = attribute.DataContextPath ?? ".";

            if (!view.IsPublic) throw new NotSupportedException(view.FullName + " has to be public in order to be used as DataTemplate! That's a WPF limitation.");

            var xaml = "<DataTemplate>" +
                           $"<v:{view.Name} DataContext=\"{{Binding {dataContext}}}\"/>" +
                       "</DataTemplate>";
            var context = new ParserContext
            {
                XamlTypeMapper = new XamlTypeMapper(new string[0])
            };

            context.XamlTypeMapper.AddMappingProcessingInstruction("v", view.Namespace, view.Assembly.FullName);
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }

        protected virtual TemplateAttribute GetAttribute(object item, DependencyObject container)
        {
            return (TemplateAttribute)item?.GetType().GetCustomAttributes(typeof(TemplateAttribute), true).FirstOrDefault();
        }
    }
}
