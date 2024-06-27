using System;
using System.Collections.Generic;
using System.Linq;
using WillsCalculator.Server.Interfaces;

namespace WillsCalculator.Server.Services
{
    public class Calculator : ICalculator
    {
        private readonly INumberStore _numberStore;
        private readonly Dictionary<string, string> operationSymbols
            = new()
            {
                { "Add", "+" },
                { "Subtract", "-" },
                { "Multiply", "*" },
                { "Divide", "/" }
            };

        public Calculator(INumberStore numberStore)
        {
            _numberStore = numberStore;
        }

        public List<string> GetAvailableOperations(string user)
        {
            var numbers = _numberStore.RetrieveNumbers(user);
            var operations = new List<string> { "Add", "Subtract", "Multiply", "Divide" };

            if (numbers == null || numbers.Count < 2)
                return new List<string>();

            if (numbers.Any(n => n.position == 2 && n.number == 0))
                operations.Remove("Divide");


            return operations;
        }



        public double PerformCalculation(string user, string operation)
        {
            var numbers = _numberStore.RetrieveNumbers(user).OrderBy(n => n.position).Select(n => n.number).ToList();
            if (numbers is null || numbers.Count < 2)
                throw new InvalidOperationException("Insufficient numbers available.");

            double result = numbers[0];

            // For-loop for incase feature implementation of more than 2 numbers
            // otherwise numbers[1] would be sufficient
            for (int i = 1; i < numbers.Count; i++)
            {
                switch (operation)
                {
                    case "Add":
                        result += numbers[i];
                        break;
                    case "Subtract":
                        result -= numbers[i];
                        break;
                    case "Multiply":
                        result *= numbers[i];
                        break;
                    case "Divide":
                        if (numbers[i] == 0)
                            throw new DivideByZeroException("Cannot divide by zero.");
                        result /= numbers[i];
                        break;
                    default:
                        throw new InvalidOperationException("Invalid operation.");
                }
            }
            _numberStore.AddHistory(user, $"{numbers[0]} {operationSymbols[operation]} {numbers[1]} = {result}");
            return result;
        }
    }
}
