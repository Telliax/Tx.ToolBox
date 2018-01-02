using System;
using System.Windows.Markup;

namespace Tx.ToolBox.Wpf.Templates
{
    public class AttributeTemplateSelectorEx : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new AttributeTemplateSelector();
        }
    }
}