using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace Microsoft.Bot.Sample.FormBot
{
   
    public class ServiceNow
    {
        string _incidentNumber;
        string _userSysId;
        string _requestNumber;

        //postIncident method is to create an incident in service-now
        public string postIncident(string userSysID, string errorDesc, string assignmentGroup)
        {

            try

            {
                string inputData = "{\"caller_id\":\"" + userSysID + "\", \"category\": \"event\", \"subcategory\": \"alert\",  \"short_description\" : \"" + errorDesc + "\", \"assignment_group\" : \"" + assignmentGroup + "\"}";



                var webRequest = WebRequest.Create("https://dev6.service-now.com/api/now/v1/table/incident");
                webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("username" + ":" + "password"));
                webRequest.Method = "POST";
                var bytes = Encoding.UTF8.GetBytes(inputData);
                webRequest.ContentLength = bytes.Length;
                webRequest.ContentType = "application/json";
                using (var requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                HttpWebResponse resp = webRequest.GetResponse() as HttpWebResponse;


                StreamReader reader =
                 new StreamReader(resp.GetResponseStream(), Encoding.UTF8);

                string result = reader.ReadToEnd().ToString();

                dynamic data1 = JObject.Parse(result);

                //Below is the raised incident number
                _incidentNumber = data1.result.number;



            }

            catch (WebException ex)
            {
                _incidentNumber = "unable to raise an incident";
            }
            catch (Exception ex)
            {
                _incidentNumber = "unable to raise an incident"; ;
            }

            return _incidentNumber;
        }


        //getUserDetails methos is used to take user Sys_ID from user HUU ID using Service-now user Table
        public string getUserDetails(string hubid)
        {
            try
            {


                var webRequest = WebRequest.Create(@"https://dev1.service-now.com/api/now/v1/table/sys_user?user_name=" + hubid);
                webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("username" + ":" + "password"));
                webRequest.Method = "GET";

                webRequest.ContentType = "application/json";

                HttpWebResponse resp = webRequest.GetResponse() as HttpWebResponse;


                StreamReader reader =
                 new StreamReader(resp.GetResponseStream(), Encoding.UTF8);



                string resultUser = reader.ReadToEnd().ToString();

                dynamic dataUser = JObject.Parse(resultUser);

                _userSysId = dataUser.result[0].sys_id;

            }
            catch (WebException ex)
            {
                _userSysId = "An error occured retrieving UserID";
            }
            catch (Exception ex)
            {
                _userSysId = "An error occured retrieving UserID";
            }


            return _userSysId;
        }

        //raise request menthos is to create a request in service-now
        public string raiseRequest(string userSysID, string queryDesc)
        {
            string assignmentGroup = "5ddc098b2b583100bbb2dcb3e4da15dd";

            try

            {
                string inputData = "{\"v_priority\": \"3\", \"v_due_date\": \"2019-05-20 05:38:19\", \"v_service\": \"ce9bb5c13c33a100071b28f1df4e27be\"," +
                    " \"v_business_function_area\": \"1dc5123a2bf13100071b36a3e4da1501\", \"v_application\": \"56e88a6b6ffe7d400231e82fae3ee49c\"," +
                    " \"v_spl_instructions\": \"" + queryDesc + "\", \"v_app_name\": \"56e88a6b6ffe7d400231e82fae3ee49c\", \"v_record_type\": \"sc_request\",  \"v_short_description\" : \"" + queryDesc + "\",  \"v_description\" : \"" + queryDesc + "\", \"v_assignment_group\" : \"" + assignmentGroup + "\"}";



                var webRequest = WebRequest.Create("https://dev6.service-now.com/rest_catalog.do?cat_item=0c8bb14680676500bbb25a7f4b90f51f");
                webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("username" + ":" + "password"));
                webRequest.Method = "POST";
                var bytes = Encoding.UTF8.GetBytes(inputData);
                webRequest.ContentLength = bytes.Length;
                webRequest.ContentType = "application/json";
                using (var requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }


                HttpWebResponse resp = webRequest.GetResponse() as HttpWebResponse;


                StreamReader reader =
                 new StreamReader(resp.GetResponseStream(), Encoding.UTF8);

                string result = reader.ReadToEnd().ToString();

                dynamic data1 = JObject.Parse(result);
                string temp = data1.ToString();

                string stringBetweenTwoStrings = temp.Substring(temp.LastIndexOf("number"),
    temp.LastIndexOf("status") - temp.LastIndexOf("number"));
                string temp3 = stringBetweenTwoStrings.Substring(10, 10);
                //Below is the raised incident number
                _requestNumber = temp3;

                string temp2 = data1.ToString();

            }

            catch (WebException ex)
            {
                _requestNumber = "unable to raise request";
            }
            catch (Exception ex)
            {
                _requestNumber = "unable to raise request"; 
            }

            return _requestNumber;
        }
    }
}