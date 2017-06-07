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
        TaskRtn transWCSTask(List<Task> list);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "transSRMTaskAisle", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        TaskAisleRtn transWCSTaskAisle(List<TaskAisle> list);

    }
    [DataContract]
    public class Task
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string warehouseCode { get; set; }
        [DataMember]
        public string taskNo { get; set; }
        [DataMember]
        public string taskType { get; set; }
        [DataMember]
        public int taskLevel { get; set; }
        [DataMember]
        public string taskFlag { get; set; }
        [DataMember]
        public string palletBarcode { get; set; }
        [DataMember]
        public string fromAddress { get; set; }
        [DataMember]
        public string toAddress { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string sendDate { get; set; }
        [DataMember]
        public string sender { get; set; }
        [DataMember]
        public string field1 { get; set; }
        [DataMember]
        public string field2 { get; set; }
        [DataMember]
        public string field3 { get; set; }
    }
    [DataContract]
    public class TaskRtn
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string returnCode { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string finishDate { get; set; }
        [DataMember]
        public string field1 { get; set; }        
    }
    [DataContract]
    public class TaskAisle
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string warehouseCode { get; set; }
        [DataMember]
        public string taskNo { get; set; }
        [DataMember]
        public string sendDate { get; set; }
        [DataMember]
        public string sender { get; set; }
        [DataMember]
        public string field1 { get; set; }
        [DataMember]
        public string field2 { get; set; }
        [DataMember]
        public string field3 { get; set; }
    }
    [DataContract]
    public class TaskAisleRtn
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string taskNo { get; set; }
        [DataMember]
        public string aisleNo { get; set; }
        [DataMember]
        public string finishDate { get; set; }
        [DataMember]
        public string field1 { get; set; }
    }
}
