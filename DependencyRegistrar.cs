using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Payments.Tpay.Integration;

namespace Nop.Plugin.Payments.Tpay
{
    // ReSharper disable once UnusedMember.Global
    internal class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<TpayPaymentManager>().As<ITpayPaymentManager>().InstancePerLifetimeScope();
        }
    }
}
