using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace FunctionParseContactInfo
{
    public static class FunctionParseContactInfo
    {
        [FunctionName("FunctionParseContactInfo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();
            OCRData ocr = JsonConvert.DeserializeObject<OCRData>(data.ToString());
            var ci = new ContactInfo();
            
            foreach (Region r in ocr.regions)
            {
                ci.ParseJSON(r);
            }
            return req.Content == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Unable able to parse JSON")
                : req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(ci));

        }
    }
}