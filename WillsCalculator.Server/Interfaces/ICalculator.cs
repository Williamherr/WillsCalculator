using System.Collections.Generic;

namespace WillsCalculator.Server.Interfaces
{
    public interface ICalculator
    {
        List<string> GetAvailableOperations(string user);
        double PerformCalculation(string user, string operation);
    }
}
