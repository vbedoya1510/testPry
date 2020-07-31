using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallCenterProyect.Models
{
    public class Answer
    {
        public object errors { get; set; }

        public int result { get; set; }

        public bool successful { get; set; }
    }
}