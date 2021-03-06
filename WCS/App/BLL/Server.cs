﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using IServices;
using System.Data;

namespace BLL
{
    public class Server//<TChannel>
    {

        /// <summary>
        /// 通道字典
        /// </summary>
        private static Dictionary<string, object> Channels = new Dictionary<string, object>();
       //private static  ChannelFactory<TChannel> channelFactory = new ChannelFactory<TChannel>(typeof(TChannel).Name);
       //private static TChannel channel;
        
        /// <summary>
        /// 创建一个指定类型的通道
        /// </summary>
        /// <typeparam name="TChannel">WCF接口类型</typeparam>
        /// <returns></returns>
        public static TChannel GetChannel<TChannel>()
        {
            try
            {
                string endPointConfigName = typeof(TChannel).Name;
                if (Channels.ContainsKey(endPointConfigName))
                {
                    return (TChannel)Channels[endPointConfigName];
                }

                ChannelFactory<TChannel> channelFactory = new ChannelFactory<TChannel>(endPointConfigName);
                TChannel channel = channelFactory.CreateChannel();
                Channels.Add(endPointConfigName, channel);
                return channel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static TChannel GetChannel<TChannel>()
        //{
        //    try
        //    {
        //       
        //        //if (channel == null)
        //        //{
        //        //    channel = channelFactory.CreateChannel();
        //        //}
        //      
        //        return channel;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
