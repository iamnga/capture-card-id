using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sdb_eid_be.Models;

namespace sdb_eid_be.Controllers
{

    [ApiController]
    public class AioController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AioController));

        [Route("[controller]/upload-image")]
        [HttpPost]
        public async Task<object> UploadImage(AllInOneRequest<UploadImage> req)
        {
            try { 
            var reqJSON = JsonConvert.SerializeObject(req);
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient(clientHandler); var response = await client.PostAsync("https://172.20.18.5/digizone/upload-image", new StringContent(reqJSON, Encoding.UTF8, "application/json"));
            var contents = await response.Content.ReadAsStringAsync();
            logger.Info(contents);
            return contents;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new
                {
                    respCode = "01",
                    respDescription = ex
                };
            }
        }

        [Route("[controller]/verify-sessionId")]
        [HttpPost]
        public async Task<object> VerifySessionId(AllInOneRequest<string> req)
        {
            try
            {
                req.data = null;
                var reqJSON = JsonConvert.SerializeObject(req);


                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                var client = new HttpClient(clientHandler);

                //var response = await client.PostAsync("http://istio-ingressgateway-istio-system.apps.ocptest.sacombank.local/digizone/verify-sessionId", new StringContent(reqJSON, Encoding.UTF8, "application/json"));
                var response = await client.PostAsync("https://172.20.18.5/digizone/verify-sessionId", new StringContent(reqJSON, Encoding.UTF8, "application/json"));
                var contents = await response.Content.ReadAsStringAsync();
                logger.Info(contents);

                return contents;
            }
            catch (Exception ex)
            {

                logger.Error(ex);
                return new
                {
                    respCode = "01",
                    respDescription = ex
                };
            }

        }
    }
}
