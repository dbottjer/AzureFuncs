using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using TF.CognitiveServices.VisionServices.OCR;

namespace FunctionParseContactInfo
{
    public static class FunctionParseContactInfo
    {
        [FunctionName("FunctionParseContactInfo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var ci = new ContactInfo();

            try
            {
                // Get request body.  Converted to ReadAsStringAync to avoid unicode conversion error.
                string data = await req.Content.ReadAsStringAsync();

                //dynamic data = await req.Content.ReadAsAsync<object>();
                OCRData ocr = JsonConvert.DeserializeObject<OCRData>(data.ToString());
                
                Parallel.ForEach(ocr.Regions, (r) =>
                {
                    ci.Parse(r);
                });
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Unable to parse JSON");
            }
            var results = new Results(ci.Parsers.Select(p => new { p.Name, p.Value }).ToDictionary(d => d.Name, d => d.Value), ci.UnKnown);

            return req.Content == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Unable to parse JSON")
                : req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(results));

        }
    }
}