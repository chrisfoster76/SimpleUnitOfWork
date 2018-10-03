using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleUnitOfWork.IntegrationTests.TestHarness;

namespace SimpleUnitOfWork.IntegrationTests.Tests.RetryCapability
{
    [TestClass]
    public class WhenANonTransientFaultOccurs
    {
        private UnitOfWork _unitOfWork;

        private TestRepository _testRepository;

        [TestInitialize]
        public async Task Arrange()
        {
            _testRepository = new TestRepository();
            //_unitOfWork = new UnitOfWork(new WaitAndRetrySchedule { RetryCount = 10, SleepDurationProvider = i => TimeSpan.FromSeconds(0) });

            _unitOfWork = new UnitOfWork(PredefinedWaitAndRetrySchedule.DefaultJob);

            await DbHelper.ClearDown();
        }

        [TestMethod]
        public async Task ThenTheUnitOfWorkIsNotRetried()
        {
            //Arrange
            var retries = 0;

            //Act
            await _unitOfWork
                .OnRetry((retryCount => { retries++; })) //ignore the count provided; let the test count
                .ExecuteAsync(async () =>
                {
                    await _testRepository.InsertTable1Value("TEST1");
                    await _testRepository.NonTransientFailure();
                });

            //Assert
            Assert.AreEqual(0, retries);
        }
    }
}
