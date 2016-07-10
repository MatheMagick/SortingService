using Microsoft.Practices.Unity;

namespace SortingService.DataAccess
{
    public static class DALUnityConfig
    {
        public static void InitializeBindings(UnityContainer container)
        {
            container.RegisterType<IDataAccessLayer, DataAccessLayer>();
        }
    }
}