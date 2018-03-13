using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WindowsFormsApplication1
{
    public class channelTest<TChannel>
    {
       // private static ChannelFactory<TChannel> channelFactory = new ChannelFactory<TChannel>(typeof(TChannel).Name);

        private static TChannel channel;

        public static string a;

        private static object _lock = new object();

        public static string GetInstance() 
        {
            if (channel==null)
            {
                lock (_lock)
                {
                    if (channel==null)
                    {
                       a = typeof(TChannel).Name;
                       // channel = channelFactory.CreateChannel();
                    }
                }
            }
            return a;
        }
    }
}
