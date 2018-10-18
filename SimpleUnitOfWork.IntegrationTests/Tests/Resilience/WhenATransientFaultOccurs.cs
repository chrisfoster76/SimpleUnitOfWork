using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleUnitOfWork.IntegrationTests.TestHarness;

namespace SimpleUnitOfWork.IntegrationTests.Tests.Resilience
{
    [TestClass]
    public class WhenATransientFaultOccurs
    {
        private UnitOfWork _unitOfWork;

        private TestRepository _testRepository;

        private readonly int expectedRetryCount = 10;

        [TestInitialize]
        public async Task Arrange()
        {
            _testRepository = new TestRepository();
            _unitOfWork = new UnitOfWork(new WaitAndRetrySchedule{ RetryCount = expectedRetryCount, SleepDurationProvider = i => TimeSpan.FromSeconds(0) });

            await DbHelper.ClearDown();
        }

        [TestMethod]
        public async Task ThenTheUnitOfWorkIsRetried()
        {
            //Arrange
            var retries = 0;

            //Act
            await _unitOfWork
                    .OnRetry((retryCount => { retries++; })) //ignore the count provided; let the test count
                    .ExecuteAsync(async () =>
                    {
                        await _testRepository.InsertTable1Value("TEST1");
                        await _testRepository.TransientFailure();
                    });

            //Assert
            Assert.AreEqual(expectedRetryCount, retries);
        }
    }
}
