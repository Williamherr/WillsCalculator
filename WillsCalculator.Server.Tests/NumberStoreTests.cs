using WillsCalculator.Server.Interfaces;
using WillsCalculator.Server.Services;

namespace WillsCalculator.Server.Tests
{
    [TestClass]
    public class NumberStoreTests
    {
        private INumberStore _numberStore;

        [TestInitialize]
        public void Setup()
        {
            _numberStore = new NumberStore();
        }

        [TestMethod]
        public void StoreAndRetrieveNumbers()
        {
            string user = "testUser";
            _numberStore.StoreNumber(user, 1, 10.5);
            _numberStore.StoreNumber(user, 2, 20.5);

            var numbers = _numberStore.RetrieveNumbers(user);

            Assert.AreEqual(2, numbers.Count);
            Assert.IsTrue(numbers.Any(n => n.position == 1 && n.number == 10.5));
            Assert.IsTrue(numbers.Any(n => n.position == 2 && n.number == 20.5));
        }

        [TestMethod]
        public void ClearNumbers()
        {
            string user = "testUser";
            _numberStore.StoreNumber(user, 1, 10.5);
            _numberStore.ClearNumbers(user);

            var numbers = _numberStore.RetrieveNumbers(user);

            Assert.AreEqual(0, numbers.Count);
        }
        [TestMethod]
        public void AddAndRetrieveHistory()
        {
            string user = "testUser";
            string message1 = "First message";
            string message2 = "Second message";

            _numberStore.AddHistory(user, message1);
            _numberStore.AddHistory(user, message2);

            var history = _numberStore.RetrieveHistory(user);

            Assert.AreEqual(2, history.Count);
            Assert.IsTrue(history.Contains(message1));
            Assert.IsTrue(history.Contains(message2));
        }

        [TestMethod]
        public void ReplaceExistingNumber()
        {
            string user = "testUser";
            _numberStore.StoreNumber(user, 1, 10.5);
            _numberStore.StoreNumber(user, 1, 20.5); // Replace the number at position 1

            var numbers = _numberStore.RetrieveNumbers(user);

            Assert.AreEqual(1, numbers.Count); // Ensure only one number exists for position 1
            Assert.IsTrue(numbers.Any(n => n.position == 1 && n.number == 20.5));
        }


    }


}