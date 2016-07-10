using Microsoft.Practices.Unity;

namespace SortingService
{
    public static class UnityConfig
    {
        public static UnityContainer Container { get; } = new UnityContainer();

        public static void InitializeBindings()
        {
            Container.RegisterTypes(AllClasses.FromLoadedAssemblies(), WithMappings.FromMatchingInterface);
        }
    }
}