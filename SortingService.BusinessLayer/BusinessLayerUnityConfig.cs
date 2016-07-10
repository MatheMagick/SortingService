using Microsoft.Practices.Unity;
using SortingService.BusinessLayer.SortingAlgorithms;

namespace SortingService.BusinessLayer
{
    public static class BusinessLayerUnityConfig
    {
        public static void InitializeBindings(UnityContainer container)
        {
            container.RegisterType<ISessionManager, SessionManager>();
            container.RegisterType<IImprovedSorting, ImprovedSorting>();

            DALUnityConfig.InitializeBindings(container);
        }
    }
}