using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Text
{
    [Template(typeof(LabelToolView))]
    public abstract class LabelTool : TextTool
    {
        protected LabelTool()
        {
            Width = double.NaN;
        }

        protected override void OnTextChanged()
        {
        }
    }
}
