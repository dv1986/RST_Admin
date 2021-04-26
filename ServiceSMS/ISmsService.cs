using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSMS
{
    public interface ISmsService
    {
        IRestResponse GetBalance();
        IRestResponse SendOTP(string MobileNumber, string OTP, string TemplateName);
        IRestResponse VerifyOTP(string SessionId, string OTP);
    }
}
