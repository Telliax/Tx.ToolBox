using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.UI.Templates
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
