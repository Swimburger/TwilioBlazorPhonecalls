using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio.Jwt;
using Twilio.Jwt.Client;

namespace TwilioBlazorPhonecalls.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public TokenController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        [HttpGet]
        [EnableCors("BlazorClientPolicy")]
        public string Get()
        {
            string twilioAccountSid = configuration["TwilioAccountSid"];
            string twilioAuthToken =  configuration["TwilioAuthToken"];
            string twiMLApplicationSid =  configuration["TwiMLApplicationSid"];

            HashSet<IScope> scopes = new HashSet<IScope>
            {
                new IncomingClientScope("blazor_client"),
                new OutgoingClientScope(twiMLApplicationSid)
            };

            var capability = new ClientCapability(twilioAccountSid, twilioAuthToken, scopes: scopes);

            return capability.ToJwt();
        }
    }
}
