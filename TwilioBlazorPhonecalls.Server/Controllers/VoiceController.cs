using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio.AspNet.Core;
using Twilio.Http;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace TwilioBlazorPhonecalls.Server.Controllers
{
    [ApiController]
    public class VoiceController : TwilioController
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<VoiceController> logger;
        private bool enablePrivacyMode;

        public VoiceController(IConfiguration configuration, ILogger<VoiceController> logger)
        {
            this.configuration = configuration;
            enablePrivacyMode = configuration.GetValue<bool>("EnablePrivacyMode");
            this.logger = logger;
        }

        [HttpPost]
        [Route("voice/outgoing")]
        public TwiMLResult Outgoing([FromForm] string to)
        {
            string twilioPhoneNumber = configuration["TwilioPhoneNumber"];
            var response = new VoiceResponse();
            var dial = new Dial();
            if (enablePrivacyMode)
            {
                logger.LogInformation($"Calling {MaskPhoneNumber(to)}");
            }
            else
            {
                logger.LogInformation($"Calling {to}");
            }
            dial.CallerId = twilioPhoneNumber;
            dial.Number(to);
            response.Append(dial);
            return TwiML(response);
        }

        [HttpPost]
        [Route("voice/incoming")]
        public TwiMLResult Incoming([FromForm] string from)
        {
            if (enablePrivacyMode)
            {
                logger.LogInformation($"Receiving call from {MaskPhoneNumber(from)}");
            }
            else
            {
                logger.LogInformation($"Receiving call from {from}");
            }

            var response = new VoiceResponse();
            var dial = new Dial();
            logger.LogInformation($"Calling blazor_client");
            dial.CallerId = from;
            dial.Client("blazor_client");

            dial.Action = new Uri("/voice/incoming-action", UriKind.Relative);
            dial.Method = HttpMethod.Post;

            response.Append(dial);


            return TwiML(response);
        }

        [HttpPost]
        [Route("voice/incoming-action")]
        public TwiMLResult IncomingAction([FromForm] string dialCallStatus)
        {
            if(dialCallStatus == "completed" || dialCallStatus == "answered")
            {
                return null;
            }

            var response = new VoiceResponse();
            response.Say("The person you are trying to reach is unavailable. Leave a message after the beep.");
            response.Record();

            return TwiML(response);
        }

        private static string MaskPhoneNumber(string phoneNumber)
        {
            bool maskFully = true;
            if (maskFully)
            {
                return "**********";
            }

            Match match = Regex.Match(phoneNumber, @"(?:\+\d)?(\d*)\d{4}$");
            if (match.Groups.Count != 2)
            {
                // if regex fails, fallback to 10 *
                return "**********";
            }

            string partToMask = match.Groups[1].Value;
            string partToReplaceWith = new string('*', partToMask.Length);
            phoneNumber = phoneNumber.Replace(partToMask, partToReplaceWith);
            return phoneNumber;
        }
    }
}
