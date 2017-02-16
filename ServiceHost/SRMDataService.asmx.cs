using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using Util;
using System.Configuration;
using System.Data.SqlClient;

namespace ServiceHost
{
    /// <summary>
    /// SRMDataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SRMDataService : System.Web.Services.WebService
    {
      
        //总控WCS提供给立库WCS任务数据
        [WebMethod]
        public string transSRMTask(string wcsProductObject)
        {
            DataTable dt = Util.JsonHelper.Json2Dtb(wcsProductObject);
            string json = "[{\"id\":" + dt.Rows[0]["id"] + ",\"returnCode\":000" + ",\"message\":\"成功\"";
            BLL.BLLBase bll = new BLL.BLLBase();
            if (dt.Rows.Count == 0)
            {
                return json = "[{\"id\":" + dt.Rows[0]["id"] + ",\"returnCode\":001" + ",\"message\":\"失败\"" + ",\"finishDate\":" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "}]";
            }

            bll.BatchInsertTable(dt, "WCS_TaskTemp");

          
            bll.ExecNonQuery("WCS.Sp_InsertWCSTask");
            #region
            //List<string> list = new List<string>();
            //List<DataParameter[]> paras = new List<DataParameter[]>();
            //DataParameter[] para;

            //foreach (DataRow row in dt.Rows)
            //{
            //    string billTypeCode;
            //    string CellCode = row["fromAddress"].ToString();
            //    string ToCellCode = "";
            //    string AisleNo = "";
            //    string AreaCode = "B01";
            //    switch (row["taskType"].ToString())
            //    {
            //        case "11":
            //            billTypeCode = "001";
            //            CellCode = row["toAddress"].ToString();
            //            break;
            //        case "12":
            //            billTypeCode = "020";
            //            break;
            //        case "13":
            //            billTypeCode = "030";
            //            ToCellCode = row["toAddress"].ToString();
            //            break;
            //        case "15":
            //            billTypeCode = "050";
            //            break;
            //        case "16":
            //            billTypeCode = "070";
            //            break;
            //        default:
            //            billTypeCode = "060";
            //            break;
            //    }
            //    string As = CellCode.Substring(2, 1);
            //    if (AreaCode == "A01")
            //    {

            //        if (As == "1" || As == "2" || As == "3")
            //        {
            //            AisleNo = "01";
            //        }
            //        else if (As == "4" || As == "5" || As == "6")
            //        {
            //            AisleNo = "02";
            //        }
            //        else if (As == "7" || As == "8" || As == "9" || As == "10")
            //        {
            //            AisleNo = "03";
            //        }
            //        else
            //        {
            //            AisleNo = "04";
            //        }
            //    }
            //    else if (AreaCode == "B01")
            //    {
            //        if (As == "1" || As == "2" || As == "3" || As == "4")
            //        {
            //            AisleNo = "01";
            //        }
            //        else if (As == "5" || As == "6" || As == "7")
            //        {
            //            AisleNo = "02";
            //        }
            //        else
            //        {
            //            AisleNo = "03";
            //        }
            //    }
            //    if (row["taskFlag"].ToString() == "1")
            //    {
            //        list.Add("WCS.InsertTestWCSTask");
            //        para = new DataParameter[] {new DataParameter("@TaskID",row["taskNo"]),
            //                                        new DataParameter("@TaskNo", row["taskNo"]),
            //                                        new DataParameter("@BillTypeCode",billTypeCode),
            //                                        new DataParameter("@TaskType",row["taskType"]),
            //                                        new DataParameter("@TaskLevel",row["taskLevel"]),
            //                                        new DataParameter("@PalletBarcode",row["palletBarcode"]),
            //                                        new DataParameter("@DeviceNo",""),
            //                                        new DataParameter("@AisleNo",AisleNo),
            //                                        new DataParameter("@CellCode",CellCode),
            //                                        new DataParameter("@ToCellCode",ToCellCode),
            //                                        new DataParameter("@State",row["status"]),
            //                                        new DataParameter("@Tasker",row["sender"]),
            //                                        new DataParameter("@TaskDate",DateTime.ParseExact(row["sendDate"].ToString(),"yyyy-MM-dd HH:mm:ss fff",null)),
            //                                        new DataParameter("@RequestDate",null),
            //                                        new DataParameter("@StartDate",null),
            //                                        new DataParameter("@FinishDate",null),
            //                                        new DataParameter("@AlarmCode","0"),
            //                                        new DataParameter("@AlarmDesc",""),
            //                                        new DataParameter("@AreaCode",AreaCode),
            //                                        new DataParameter("@StationNo","02")
            //                                        };
            //        paras.Add(para);
            //    }
            //    else if (row["taskFlag"].ToString() == "2")
            //    {
            //        list.Add("WCS.UpdateTestWCSTask");
            //        para = new DataParameter[] {new DataParameter("@BillTypeCode",billTypeCode),
            //                                        new DataParameter("@TaskType",row["taskType"]),
            //                                        new DataParameter("@TaskLevel",row["taskLevel"]),
            //                                        new DataParameter("@PalletBarcode",row["palletBarcode"]),
            //                                        new DataParameter("@AisleNo",AisleNo),
            //                                        new DataParameter("@CellCode",CellCode),
            //                                        new DataParameter("@ToCellCode",ToCellCode),
            //                                        new DataParameter("@State",row["status"]),
            //                                        new DataParameter("@Tasker",row["sender"]),
            //                                        new DataParameter("@TaskDate",DateTime.ParseExact(row["sendDate"].ToString(),"yyyy-MM-dd HH:mm:ss fff",null)),
            //                                        new DataParameter("@AreaCode",AreaCode),
            //                                        new DataParameter("@TaskNo", row["taskNo"])
            //                                        };
            //        paras.Add(para);
            //    }
            //    else
            //    {
            //        list.Add("WCS.DeleteTestWCSTask");
            //        para = new DataParameter[] { };
            //        paras.Add(para);
            //    }
            //}


            //try
            //{
            //    bll.ExecTran(list.ToArray(), paras);
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            #endregion
            json = json + ",\"finishDate\":" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "}]";
            return json;

        }





        //总控WCS入库巷道请求
        [WebMethod]
        public string transSRMTaskAisle(string wcsProductObject)
        {
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dt = Util.JsonHelper.Json2Dtb(wcsProductObject);
            string json;
            if (dt.Rows.Count == 0)
            {
                return json = "[{\"id\":" + dt.Rows[0]["id"] + ",\"returnCode\":001" + ",\"message\":\"失败\"" + ",\"finishDate\":" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "}]";
            }

            bll.BatchInsertTable(dt, "WCS_AisleTemp");

            string Aisle = "";
            string AreaCode = dt.Rows[0]["areaCode"].ToString();

            DataTable dtSelectAisle = bll.FillDataTable("Cmd.SelectAisle", new DataParameter("{0}", AreaCode));
            Aisle = dtSelectAisle.Rows[0]["AisleNo"].ToString();
            
            json = "[{\"id\":" + dt.Rows[0]["id"] + "\"taskNo\":" + dt.Rows[0]["taskNo"] + "\"aisleNo\":" + Aisle + "\"finishDate\":" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "}]";

            return json;
         }
    }
}
