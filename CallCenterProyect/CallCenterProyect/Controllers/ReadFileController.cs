using CallCenterProyect.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CallCenterProyect.Controllers
{

    public class ReadFileController : Controller
    {
        public Process process = new Process();

        /// <summary>
        /// Receive a file in a base 64 string to process
        /// </summary>
        public ActionResult evaluateFile(string file)
        {
            var serializer = new JavaScriptSerializer();
            var respuestaCliente = new ContentResult();
            respuestaCliente.ContentType = "application/json";

            try
            {
                //convert string to byte array 
                byte[] fileByteArray = Convert.FromBase64String(file);
                //get string of byte array to process
                var str = System.Text.Encoding.Default.GetString(fileByteArray);
                respuestaCliente.Content = serializer.Serialize(process.getPoints(str));

                return respuestaCliente;

            }
            catch (Exception e)
            {
                Answer answer = new Answer();
                answer.errors = "Error en el archivo ingresado "+e.Message;
                answer.successful = false;
                answer.result = 0;
                respuestaCliente.Content = serializer.Serialize(answer);
                return respuestaCliente;
            }
        }
    }
}
