using Misaka.Config;
using Misaka.DependencyInjection;
using Misaka.DpendencyInjection;

namespace Misaka.DependencyInject.Autofac
{
    public static class ServicesExtension
    {
        public static Configuration UseAutofac(this Configuration configuration)
        {
            ObjectProviderFactory.Instance.SetProviderBuilder(new AutofacObjectProviderBuilder());
            return configuration;
        }
    }
}
