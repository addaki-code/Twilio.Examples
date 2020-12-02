using System;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace Twilio.Examples.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoiceController : TwilioController
    {

        [HttpPost]
        [Route("")]
        public IActionResult Index()
        {
            

			Gather gather = new Gather(numDigits: 1, action: new Uri("/voice/gather"));
			gather.Append(new Say("For Sales, Press 1. For Support, Press 2"));

			var response = new VoiceResponse();
			response.Append(gather);

            // If the user doesn't enter input, loop
            response.Redirect(new Uri("/voice"));

            return Content(response.ToString(), "text/xml");
        }

		[HttpPost]
		[Route("Gather")]
		public IActionResult Gather([FromForm] string digits)
		{
			var response = new VoiceResponse();

			// If the user entered digits, process their request
			if (!string.IsNullOrEmpty(digits))
			{
				switch (digits)
				{
					case "1":
						response.Say("You selected sales. Good for you!");
						break;
					case "2":
						response.Say("You need support. We will help!");
						break;
					default:
						response.Say("Sorry, I don't understand that choice.").Pause();
						response.Redirect(new Uri("/voice"));
						break;
				}
			}
			else
			{
				// If no input was sent, redirect to the /voice route
				response.Redirect(new Uri("/voice"));
			}

			return TwiML(response);
		}
	}
}
