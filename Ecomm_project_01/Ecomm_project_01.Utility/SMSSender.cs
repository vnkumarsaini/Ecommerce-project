using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Ecomm_project_01.Utility
{
    public class SMSSender: ISMSSender
    {
        private readonly TwilioSettings _twilioSettings;
        public SMSSender(IOptions <TwilioSettings> twilioSettings)
        {
            _twilioSettings= twilioSettings.Value;
        }
        public async Task SendSMSAsync(string toPhone, string message)
        {
            TwilioClient.Init(_twilioSettings.AccountSID, _twilioSettings.AuthToken);
            await MessageResource.CreateAsync(
                to: toPhone,
                from: _twilioSettings.FromPhone,
                body: message
                );
        }

    }
}
