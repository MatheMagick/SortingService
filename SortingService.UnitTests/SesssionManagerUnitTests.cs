using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SortingService.BusinessLayer;
using SortingService.BusinessLayer.SortingAlgorithms;
using SortingService.DataAccess;

namespace SortingService.UnitTests
{
    [TestClass]
    public class SesssionManagerUnitTests
    {
        [TestMethod]
        public void SessionManger_StartNewSession()
        {
            var mockDataAccess = new Mock<IDataAccessLayer>();
            var mockSorting = new Mock<IImprovedSorting>();
            ISessionManager manager = new SessionManager(mockDataAccess.Object, mockSorting.Object);

            manager.StartNewSession();

            mockDataAccess.Verify(x => x.CreateNewSession());
        }
    }
}