using System;

namespace Tx.ToolBox.Wpf.Templates
{
    /// <summary>
    /// AttributeTemplateSelector looks for this attribute on your view model in order to generate an appropriate template.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute : Attribute
    {
        public TemplateAttribute(Type viewType)
        {
            ViewType = viewType;
        }

        /// <summary>
        /// Type of DataTemplate's underlying view.
        /// </summary>
        public Type ViewType { get; private set; }
    }
}
