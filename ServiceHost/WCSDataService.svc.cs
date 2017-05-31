using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.IO;
using System.Data;
using Util;
using System.Runtime.Serialization.Json;

namespace ServiceHost
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“WCSDataService”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class WCSDataService : IWCSDataService
    {
        public void DoWork()
        {
        }
    }
}
