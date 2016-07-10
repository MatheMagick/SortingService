using Microsoft.Practices.Unity;
using SortingService.BusinessLayer;
using SortingService.BusinessLayer.SortingAlgorithms;
using SortingService.DataAccess;

namespace SortingService
{
    public static class UnityConfig
    {
        public static UnityContainer Container { get; } = new UnityContainer();

        public static void InitializeBindings()
        {
            BusinessLayerUnityConfig.InitializeBindings(Container);
        }
    }
}