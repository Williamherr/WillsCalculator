using System.Collections.Generic;

namespace WillsCalculator.Server.Interfaces
{
    public interface INumberStore
    {
        void StoreNumber(string user, int position, double number);
        void ClearNumbers(string user);
        List<(int position, double number)> RetrieveNumbers(string user);

        void AddHistory(string user, string message);

        List<string> RetrieveHistory(string user);


    }
}
