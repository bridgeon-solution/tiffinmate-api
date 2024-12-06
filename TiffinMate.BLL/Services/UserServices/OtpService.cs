using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces.AuthInterface;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

namespace TiffinMate.BLL.Services.AuthService
{
    public class OtpService:IOtpService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _verifySid;

        public OtpService(string accountSid,string authToken,string verifySid)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _verifySid = verifySid;         
        }

        public async Task<string> SendSmsAsync(string mobileNumber)
        {

            TwilioClient.Init(_accountSid,_authToken);

            var message = await VerificationResource.CreateAsync(
                 to: $"+91{mobileNumber}",
                    channel: "sms",
                    pathServiceSid:_verifySid
            );
            return message.Sid;
        }
       public async Task<bool> VerifyOtpAsync(string mobileNumber, string otp)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var verificationCheck = await VerificationCheckResource.CreateAsync(
                to: $"+91{mobileNumber}",
                code: otp,
                pathServiceSid: _verifySid
            );

            return verificationCheck.Status == "approved";
        }
    
       
    
    }
}
