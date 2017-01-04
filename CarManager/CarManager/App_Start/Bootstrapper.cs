using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer.Service;
using System.Reflection;
using CarManager.Controllers;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using CarManager.Infrastructure.Mapping;


namespace CarManager.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<DataLayer.CarManagerEntities>().As<DataLayer.CarManagerEntities>().InstancePerLifetimeScope();
            // register auto mapper
            builder.Register(
                c => new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new DomainToViewModelMappingProfile());
                    cfg.AddProfile(new ViewModelToDomainMappingProfile());
                }))
                .AsSelf()
                .SingleInstance();
            builder.Register(
              c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
              .As<IMapper>()
              .InstancePerLifetimeScope();
            // end register mapper

            //service
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<BusStationService>().As<IBusStationService>().InstancePerLifetimeScope();
            builder.RegisterType<CarDiagramService>().As<ICarDiagramService>().InstancePerLifetimeScope();
            builder.RegisterType<CarService>().As<ICarService>().InstancePerLifetimeScope();
            builder.RegisterType<ChannelService>().As<IChannelService>().InstancePerLifetimeScope();
            builder.RegisterType<ChannelDetailService>().As<IChannelDetailService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderDetailService>().As<IOrderDetailService>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleService>().As<IScheduleService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();

            IContainer container = builder.Build();
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}