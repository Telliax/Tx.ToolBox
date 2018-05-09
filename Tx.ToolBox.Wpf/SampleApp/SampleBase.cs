using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Tx.ToolBox.Wpf.Tools;

namespace Tx.ToolBox.Wpf.SampleApp
{
    public abstract class SampleBase : ISample
    {
        protected SampleBase()
        {
            Name = GetType().Name;
            Description = String.Empty;
        }

        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public virtual FrameworkElement ResolveView()
        {
            return _container.Resolve<FrameworkElement>();
        }

        public void Load(IWindsorContainer appContainer)
        {
            _container = SampleBootstrap.CreateDefaultContainer();
            appContainer.AddChildContainer(_container);
            var installers = CreateInstallers().ToArray();
            _container.Install(installers);
            LoadToolBar(_container);
            OnLoad(_container);
            
        }

        public void Unload()
        {
            OnUnload(_container);
            UnloadToolBar(_container);
            _container.Dispose();
        }

        protected abstract IEnumerable<IWindsorInstaller> CreateInstallers();

        protected virtual void OnLoad(IWindsorContainer sampleContainer)
        {
        }

        protected virtual void OnUnload(IWindsorContainer sampleContainer)
        {
        }

        protected virtual void LoadToolBar(IWindsorContainer container)
        {
            if (container.Kernel.HasComponent(typeof(IToolBar)) && container.Kernel.HasComponent(typeof(ITool)))
            {
                _toolBar = container.Resolve<IToolBar>();
                _tools = container.ResolveAll<ITool>();
                _toolBar.Setup()
                        .Add(_tools)
                        .Complete();
            }
        }

        protected virtual void UnloadToolBar(IWindsorContainer container)
        {
            if (container.Kernel.HasComponent(typeof(IToolBar)) && container.Kernel.HasComponent(typeof(ITool)))
            {
                _toolBar.Setup()
                        .Remove(_tools)
                        .Complete();
            }
        }

        private IWindsorContainer _container;
        private IToolBar _toolBar;
        private ITool[] _tools;
    }
}
