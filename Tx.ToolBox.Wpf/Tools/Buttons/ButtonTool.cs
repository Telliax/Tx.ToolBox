﻿using System.Windows.Input;
using System.Windows.Media;
using Tx.ToolBox.Wpf.Helpers;
using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    [Template(typeof(ButtonToolView))]
    public abstract class ButtonTool : ToolBase
    {
        protected ButtonTool()
        {
            Command = new Command(Execute, CanExecute);
        }

        public Command Command { get; } 

        public string ToolTip
        {
            get => _toolTip;
            set => SetField(ref _toolTip, value);
        }

        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetField(ref _image, value.ToFrozen());
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetField(ref _isVisible, value);
        }

        protected abstract void Execute();

        protected virtual bool CanExecute()
        {
            return true;
        }

        private ImageSource _image;
        private string _text;
        private string _toolTip;
        private bool _isVisible;
    }
}
