using Microsoft.AspNetCore.Mvc;
using WillsCalculator.Server.Interfaces;

namespace WillsCalculator.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculator _calculator;
        private readonly INumberStore _numberStore;

        public CalculatorController(ICalculator calculator, INumberStore numberStore)
        {
            _calculator = calculator;
            _numberStore = numberStore;
        }

        [HttpGet("GetHistory")]
        public ActionResult<List<string>> GetHistory(string user)
        {
            return Ok(_numberStore.RetrieveHistory(user));
        }

        [HttpGet("ShowNumbers")]
        public ActionResult<double[]> ShowNumbers(string user)
        {
            var numbers = _numberStore.RetrieveNumbers(user).Select(m => m.number).ToArray();

            return Ok(numbers);
        }

        [HttpPost("SetFirstNumber")]
        public ActionResult<List<string>> SetFirstNumber(string user, double number)
        {
            _numberStore.StoreNumber(user, 1, number);
            return Ok(_calculator.GetAvailableOperations(user));
        }

        [HttpPost("SetSecondNumber")]
        public ActionResult<List<string>> SetSecondNumber(string user, double number)
        {
            _numberStore.StoreNumber(user, 2, number);
            return Ok(_calculator.GetAvailableOperations(user));
        }

        [HttpPost("DoMath")]
        public ActionResult<double> DoMath(string user, string operation)
        {
            return Ok(_calculator.PerformCalculation(user, operation));
        }

        [HttpPost("ClearNumbers")]
        public IActionResult ClearNumbers(string user)
        {
            _numberStore.ClearNumbers(user);
            return Ok();
        }

     

    }
}
