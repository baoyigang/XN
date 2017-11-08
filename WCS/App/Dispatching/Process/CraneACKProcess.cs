using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;
using Util;

namespace App.Dispatching.Process
{
    public class CraneACKProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                switch (stateItem.ItemName)
                {

                    case "ACK":
                        object obj = ObjectUtil.GetObject(stateItem.State);
                        if (obj == null)
                            return;
                        string ack = obj.ToString();

                        Logger.Debug(stateItem.Name + " Receive ACK:" + ack);
                        if (ack.Equals("True") || ack.Equals("1"))
                        {
                            WriteToService(stateItem.Name, "STB", 0);
                            Logger.Debug(stateItem.Name + " Receive ACK 1");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CraneACKProcess StateChanged方法出错，原因：" + ex.Message);
            }
        }
    }
}