using System.Collections.Generic;
using System.Linq;
using WillsCalculator.Server.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WillsCalculator.Server.Services
{
    public class NumberStore : INumberStore
    {
        private readonly Dictionary<string, List<(int position, double number)>> _store = new();
        private readonly Dictionary<string, List<string>> _history = new();

        public void StoreNumber(string user, int position, double number)
        {
            if (!_store.ContainsKey(user))
            {
                _store[user] = new List<(int, double)>();
                _history[user] = new List<string>();
            }

            var existing = _store[user].FirstOrDefault(x => x.position == position);
            if (existing != default)
            {
                _store[user].Remove(existing);
              //  AddHistory(user, $"Replaced existing number for position {position}");
            }

            _store[user].Add((position, number));
           // AddHistory(user, $"Number {number} for position {position} has been added");
        }

        public void ClearNumbers(string user)
        {
            if (_store.ContainsKey(user))
            {
                _store[user].Clear();
              //  AddHistory(user, $"Store has been cleared");
            }
        }

        public List<(int position, double number)> RetrieveNumbers(string user)
        {
            if (_store.ContainsKey(user))
            {
                return _store[user];
            }

            return new List<(int, double)>();
        }

        public List<string> RetrieveHistory(string user)
        {
            if (_history.ContainsKey(user))
            {
                return _history[user];
            }

            return new List<string>();
        }

        public void AddHistory(string user, string message)
        {
            if (!_history.ContainsKey(user))
            {
                _history[user] = new List<string>();
            }

            _history[user].Add(message);
        }
    }
}
