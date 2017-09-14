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

                BLL.BLLBase bll = new BLL.BLLBase();

                DataParameter[] param = new DataParameter[] { new DataParameter("@PalletBarcode", PalletBarcode), new DataParameter("@State", state) };
                bll.ExecNonQueryTran("WCS.UpdateTaskStateByBarcode", param);

                Logger.Info("托盘/箱号：" + PalletBarcode + "到达入库站台：" + StationNo);
            }
            catch (Exception ex)
            {
                Logger.Error("InOutLocationProcess出错，原因：" + ex.Message);
            }
        }
    }
}
