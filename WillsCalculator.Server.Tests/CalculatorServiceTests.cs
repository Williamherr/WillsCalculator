using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WillsCalculator.Server.Interfaces;
using WillsCalculator.Server.Services;

namespace WillsCalculator.Server.Tests
{
    [TestClass]
    public class CalculatorServiceTests
    {
        private ICalculator _calculator;
        private INumberStore _numberStore;

        [TestInitialize]
        public void Setup()
        {
            _numberStore = new NumberStore();
            _calculator = new Calculator(_numberStore);
        }

        [TestMethod]
        public void GetAvailableOperations_NoNumbers_ReturnsEmpty()
        {
            var operations = _calculator.GetAvailableOperations("testUser");
            Assert.AreEqual(0, operations.Count);
        }

        [TestMethod]
        public void GetAvailableOperations_LessThanTwoNumbers_ReturnsEmpty()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            var operations = _calculator.GetAvailableOperations("testUser");
            Assert.AreEqual(0, operations.Count);
        }

        [TestMethod]
        public void GetAvailableOperations_WithZeroForDivision_ExcludesDivide()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            _numberStore.StoreNumber("testUser", 2, 0);
            var operations = _calculator.GetAvailableOperations("testUser");
            Assert.IsFalse(operations.Contains("Divide"));
        }

        [TestMethod]
        public void GetAvailableOperations_ValidNumbers_IncludesAll()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            _numberStore.StoreNumber("testUser", 2, 5);
            var operations = _calculator.GetAvailableOperations("testUser");
            Assert.IsTrue(operations.Contains("Add") && operations.Contains("Subtract") && operations.Contains("Multiply") && operations.Contains("Divide"));
        }

        [TestMethod]
        public void PerformCalculation_Addition_ReturnsCorrectResult()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            _numberStore.StoreNumber("testUser", 2, 20);
            double result = _calculator.PerformCalculation("testUser", "Add");
            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void PerformCalculation_Subtraction_ReturnsCorrectResult()
        {
            _numberStore.StoreNumber("testUser", 1, 20);
            _numberStore.StoreNumber("testUser", 2, 10);
            double result = _calculator.PerformCalculation("testUser", "Subtract");
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void PerformCalculation_Multiplication_ReturnsCorrectResult()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            _numberStore.StoreNumber("testUser", 2, 2);
            double result = _calculator.PerformCalculation("testUser", "Multiply");
            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void PerformCalculation_Division_ReturnsCorrectResult()
        {
            _numberStore.StoreNumber("testUser", 1, 20);
            _numberStore.StoreNumber("testUser", 2, 2);
            double result = _calculator.PerformCalculation("testUser", "Divide");
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void PerformCalculation_DivisionByZero_ThrowsException()
        {
            _numberStore.StoreNumber("testUser", 1, 20);
            _numberStore.StoreNumber("testUser", 2, 0);
            _calculator.PerformCalculation("testUser", "Divide");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PerformCalculation_InvalidOperation_ThrowsException()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            _numberStore.StoreNumber("testUser", 2, 20);
            _calculator.PerformCalculation("testUser", "InvalidOperation");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PerformCalculation_InsufficientNumbers_ThrowsException()
        {
            _numberStore.StoreNumber("testUser", 1, 10);
            _calculator.PerformCalculation("testUser", "Add");
        }
    }
}
