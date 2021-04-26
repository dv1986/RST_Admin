using RestSharp;
using ServiceHelper;
using System;

namespace ServiceSMS
{
    public class SmsService: BaseService, ISmsService
    {
        public IRestResponse GetBalance()
        {
            var client = new RestClient("http://2factor.in/API/V1/711cc8eb-4fe0-11eb-8153-0200cd936042/BAL/SMS");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);
            return response;
        }


        public IRestResponse SendOTP(string MobileNumber, string OTP, string TemplateName)
        {
            // https://2factor.in/API/V1/{api_key}/SMS/{phone_number}/{otp}/{template_name}
            var client = new RestClient("http://2factor.in/API/V1/711cc8eb-4fe0-11eb-8153-0200cd936042/SMS/" + MobileNumber + "/"+ OTP + "/"+ TemplateName + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);
            return response;
        }

        public IRestResponse VerifyOTP(string SessionId, string OTP)
        {
            // https://2factor.in/API/V1/{api_key}/SMS/VERIFY/{session_id}/{otp_input}
            var client = new RestClient("http://2factor.in/API/V1/711cc8eb-4fe0-11eb-8153-0200cd936042/SMS/VERIFY/" + SessionId + "/"+ OTP + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
