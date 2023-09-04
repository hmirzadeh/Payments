using AcquiringBankSimulator.Domain.Entities;
using AcquiringBankSimulator.Domain.Enum;
using AcquiringBankSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using PaymentDetails = AcquiringBankSimulator.Models.PaymentDetails;

namespace AcquiringBankSimulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Simulator : ControllerBase
    {

        [HttpPost("processPayment")]
        public IActionResult Post([FromBody] Payment payout)
        {
 
            return Ok(PaymentResult.Successful);
        }

        [HttpGet("getPayment/{paymentId?}")]
        public IActionResult Get()
        {
            
            var card = new CardDetails("John Smith", "494", "123456789012", 2030,11);
            var money = new Money(15.50m, "GBP");
            var paymentDetails = new CompletedPayment()
            {
                Card = card,
                CreateDateTime = DateTime.Now.AddDays(-5),
                Id = Guid.NewGuid(),
                MerchantId = "12345678",
                Money = money,
                Result = PaymentResult.Successful

            };

            return Ok(paymentDetails);
        }
    }
}