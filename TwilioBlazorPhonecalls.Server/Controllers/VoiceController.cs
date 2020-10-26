using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace TwilioBlazorPhonecalls.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public TwiMLResult Post([FromForm] string to, [FromForm] string from)
        {
            string twilioPhoneNumber =  configuration["TwilioPhoneNumber"];
            
            var response = new VoiceResponse();
            var dial = new Dial();
            if (to == twilioPhoneNumber)
            {
                logger.LogInformation($"Calling blazor_client");
                dial.CallerId = from;
                dial.Client("blazor_client");
            }
            else
            {
                logger.LogInformation($"Calling {to}");
                dial.CallerId = twilioPhoneNumber;
                dial.Number(to);
            }

            response.Append(dial);

            return TwiML(response);
        }
    }
}
