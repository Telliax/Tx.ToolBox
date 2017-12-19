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
            if (item == null) return null;
            var view = GetViewType(item, container);
            if (view == null) return null;
            if (!view.IsPublic) throw new NotSupportedException(view.FullName + " has to be public in order to be used as DataTemplate!");
            var template = CreateTemplate(view);
            return template;
        }

        protected virtual Type GetViewType(object item, DependencyObject container)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttributes(typeof(TemplateAttribute), true).FirstOrDefault() as TemplateAttribute;
            if (attr == null) return null;
            return attr.ViewType;
        }

        protected virtual DataTemplate CreateTemplate(Type view)
        {
            var xaml = String.Format(Template, view.Name);
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

        protected string Template = "<DataTemplate>" +
                                          "<v:{0} />" +
                                    "</DataTemplate>";
    }
}
