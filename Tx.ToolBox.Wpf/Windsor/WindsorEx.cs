using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Tx.ToolBox.Wpf.Windsor
{
    public static class WindsorEx
    {
        public static void RegisterView<TView, TViewModel>(this IWindsorContainer container)
            where TView : FrameworkElement
            where TViewModel : class 
        {
            container.Register(Component.For<TViewModel>()
                                        .OnlyNewServices())
                     .Register(Component.For<TView>()
                                        .LifestyleTransient()
                                        .OnCreate((k, v) => v.DataContext = k.Resolve<TViewModel>()));
        }
    }
}
