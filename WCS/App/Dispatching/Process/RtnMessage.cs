using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace App.Dispatching.Process
{
    public class RtnMessage
    {
        public string id { get; set; }
        public string returnCode { get; set; }
        public string message { get; set; }
        public string finishDate { get; set; }
        public string field1 { get; set; }
    }
    
}
