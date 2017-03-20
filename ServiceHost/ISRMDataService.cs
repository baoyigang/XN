using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;

namespace ServiceHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISRMDataService" in both code and config file together.
    [ServiceContract]
    public interface ISRMDataService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate ="transSRMTask", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //string transWCSTask(string taskData);
        string transWCSTask(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "transSRMTaskAisle/{taskData}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string transWCSTaskAisle(string taskData);

    }
}
