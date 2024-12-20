using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;
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

        public async Task<bool> SendSmsAsync(string mobileNumber)
        {

            TwilioClient.Init(_accountSid,_authToken);

            var message = await VerificationResource.CreateAsync(
                 to: $"+91{mobileNumber}",
                    channel: "sms",
                    pathServiceSid:_verifySid
            );
            return !string.IsNullOrEmpty(message.Sid);
        }
       public async Task<bool> VerifyOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var verificationCheck = await VerificationCheckResource.CreateAsync(
                to: $"+91{verifyOtpDto.phone}",
                code: verifyOtpDto. otp,
                pathServiceSid: _verifySid
            );

            return verificationCheck.Status == "approved";
        }
    
       
    
    }
}
