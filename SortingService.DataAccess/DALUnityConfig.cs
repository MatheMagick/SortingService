using Microsoft.Practices.Unity;
using SortingService.DataAccess;

namespace SortingService
{
    public static class DALUnityConfig
    {
        public static void InitializeBindings(UnityContainer container)
        {
            container.RegisterType<IDataAccessLayer, DataAccessLayer>();
        }
    }
}