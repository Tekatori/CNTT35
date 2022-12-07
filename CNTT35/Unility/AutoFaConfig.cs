using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CNTT35.Service;
using System.ComponentModel;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using CNTT35.Service.Service;
using System.Web.Mvc;
using CNTT35.ViewModel;

namespace CNTT35.Unility
{
    public static class AutoFaConfig
    {
        public static void Config()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<CartItem>().As<ICartItem>();
            builder.RegisterType<DonHangService>().As<IDonHangService>();
            builder.RegisterType<LienHeService>().As<ILienHeService>();
            builder.RegisterType<DanhGiaService>().As<IDanhGiaService>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}