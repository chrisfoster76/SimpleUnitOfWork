using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleUnitOfWork.IntegrationTests.TestHarness;

namespace SimpleUnitOfWork.IntegrationTests.Tests.Atomicity
{
    [TestClass]
    public class GivenATwoPartTransaction
    {
        private UnitOfWork _unitOfWork;

        private TestRepository _testRepository;


        [TestInitialize]
        public async Task Arrange()
        {
            _testRepository = new TestRepository();
            _unitOfWork = new UnitOfWork();

            await DbHelper.ClearDown();
        }

        [TestMethod]
        public async Task ThenBothPartsAreCommitedToTheDatabase()
        {
            //Act
            await _unitOfWork.ExecuteAsync(async () =>
            {
                await _testRepository.InsertTable1Value("TEST1");
                await _testRepository.InsertTable2Value("TEST2");
            });

            //Assert
            Assert.AreEqual("TEST1", await DbHelper.GetTable1Value());
            Assert.AreEqual("TEST2", await DbHelper.GetTable2Value());
        }

        [TestMethod]
        public async Task ThenNothingIsCommittedToTheDatabaseIfTask1Fails()
        {
            //Act
            await _unitOfWork.ExecuteAsync(async () =>
            {
                await _testRepository.NonTransientFailure();
                await _testRepository.InsertTable2Value("TEST2");
            });

            //Assert
            Assert.IsNull(await DbHelper.GetTable1Value());
            Assert.IsNull(await DbHelper.GetTable2Value());
        }


        [TestMethod]
        public async Task ThenNothingIsCommittedToTheDatabaseIfTask2Fails()
        {
            //Act
            await _unitOfWork.ExecuteAsync(async () =>
            {
                await _testRepository.InsertTable1Value("TEST1");
                await _testRepository.NonTransientFailure();
            });

            //Assert
            Assert.IsNull(await DbHelper.GetTable1Value());
            Assert.IsNull(await DbHelper.GetTable2Value());
        }


    }
}
