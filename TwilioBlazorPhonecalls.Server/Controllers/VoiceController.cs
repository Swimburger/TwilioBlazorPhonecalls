using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace TwilioBlazorPhonecalls.Server.Controllers
{
    [ApiController]
    public class VoiceController : TwilioController
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<VoiceController> logger;

        public VoiceController(IConfiguration configuration, ILogger<VoiceController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpPost]
        [Route("voice/incoming")]
        public TwiMLResult Incoming([FromForm] string from)
        {
            var response = new VoiceResponse();
            var dial = new Dial();
            logger.LogInformation($"Calling blazor_client");
            dial.CallerId = from;
            dial.Client("blazor_client");
            response.Append(dial);
            return TwiML(response);
        }

        
        [HttpPost]
        [Route("voice/outgoing")]
        public TwiMLResult Outgoing([FromForm] string to)
        {
            string twilioPhoneNumber =  configuration["TwilioPhoneNumber"];
            var response = new VoiceResponse();
            var dial = new Dial();
            logger.LogInformation($"Calling {to}");
            dial.CallerId = twilioPhoneNumber;
            dial.Number(to);
            response.Append(dial);
            return TwiML(response);
        }
    }
}
