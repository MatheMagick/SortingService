using Microsoft.Practices.Unity;
using SortingService.BusinessLayer;

namespace SortingService
{
    /// <summary>
    /// Handles IoC
    /// </summary>
    public static class UnityConfig
    {
        public static UnityContainer Container { get; } = new UnityContainer();

        /// <summary>
        /// Initializes IoC bindings
        /// </summary>
        public static void InitializeBindings()
        {
            BusinessLayerUnityConfig.InitializeBindings(Container);
        }
    }
}