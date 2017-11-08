using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;

namespace App.Dispatching.Process
{
    public class InOutLocationProcess : AbstractProcess
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            object[] obj = ObjectUtil.GetObjects(stateItem.State);
            if (obj == null)
                return;
            if (obj.ToString().Trim().Length <= 0)
                return;
            string PalletBarcode = Util.ConvertStringChar.BytesToString(obj);
            if (PalletBarcode.Trim().Length <= 0)
                return;
            string StationNo = "";
            int state = 1;
            string AisleNo = stateItem.Name.Substring(5, 2);
            if (stateItem.ItemName == "RequestBarCode")
            {
                int WriteFinished=2;
                int count = bll.GetRowCount("WCS_Task", string.Format("PalletBarcode='{0}' and AisleNo='{1}' and State in('0','1','2')", PalletBarcode, AisleNo));
                if (count > 0)
                    WriteFinished = 1;
                WriteToService(stateItem.Name, "RequestFinished", WriteFinished);
                if (WriteFinished == 2)
                {
                    Logger.Error("条码：" + PalletBarcode + " 分配错误巷道" + AisleNo);
                }
                return;
            }
            else
            {
                switch (stateItem.ItemName)
                {
                    case "InLocation01":
                        StationNo = "SX-" + stateItem.Name.Substring(5, 2) + "-00";
                        state = 1;
                        break;
                    case "InLocation02":
                        StationNo = "SX-" + stateItem.Name.Substring(5, 2) + "-01";
                        state = 2;
                        break;
                    case "OutLocation01":
                        StationNo = "SX-" + stateItem.Name.Substring(5, 2) + "-00";
                        state = 6;
                        break;
                    case "OutLocation02":
                        StationNo = "SX-" + stateItem.Name.Substring(5, 2) + "-02";
                        state = 7;
                        break;
                }
                try
                {
                    if (stateItem.ItemName.StartsWith("InLocation"))
                    {
                       
                        if (bll.GetRowCount("WCS_Task", string.Format("PalletBarcode='{0}' and AisleNo='{1}' and State in('0','1','2')", PalletBarcode, AisleNo)) > 0)
                        {
                            DataParameter[] param = new DataParameter[] { new DataParameter("@PalletBarcode", PalletBarcode), new DataParameter("@AisleNo", AisleNo), new DataParameter("@State", state) };
                            bll.ExecNonQueryTran("WCS.UpdateTaskStateByBarcode", param);
                        }
                        else
                        {
                            Logger.Error("托盘/箱号：" + PalletBarcode + "到达站台：" + StationNo + " 到达错误站台！");
                            return;
                        }
                    }
                    Logger.Info("托盘/箱号：" + PalletBarcode + "到达站台：" + StationNo);


                }
                catch (Exception ex)
                {
                    Logger.Error("InOutLocationProcess出错，原因：" + ex.Message);
                }
            }
        }
    }
}
