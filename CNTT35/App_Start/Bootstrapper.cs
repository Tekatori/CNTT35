using System.Web.Mvc;
using CNTT35.Service.Service;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace CNTT35
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      container.RegisterType<IProductService, ProductService>();
      

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}