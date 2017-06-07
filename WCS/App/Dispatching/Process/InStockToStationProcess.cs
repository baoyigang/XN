using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;

namespace App.Dispatching.Process
{
    public class InStockToStationProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            object obj = ObjectUtil.GetObject(stateItem.State);
            if (obj == null)
                return;
            if (obj.ToString().Trim().Length <= 0)
                return;
            string PalletBarcode = obj.ToString();

            string StationNo = "";
            int state = 1;
            switch (stateItem.ItemName)
            {
                case "ToInStation1":
                    StationNo = "AX-" + stateItem.Name.Substring(5, 2) + "-00";
                    break;
                case "ToInStation2":
                    StationNo = "AX-" + stateItem.Name.Substring(5, 2) + "-01";
                    state = 2;
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
                Logger.Error("InStockToStationProcess出错，原因：" + ex.Message);
            }
        }
    }
}
