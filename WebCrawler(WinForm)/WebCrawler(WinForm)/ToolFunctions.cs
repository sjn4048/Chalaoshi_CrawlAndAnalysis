using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace WebCrawler_WinForm_
{
    class ToolFunctions
    {
        public static bool CheckNetworkStatus()
        {
            Ping objPingSender = new Ping();
            byte[] buffer = Encoding.UTF8.GetBytes("test");

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (objPingSender.Send("baidu.com", 300, buffer).Status == IPStatus.Success)
                        return true;
                }
                catch
                {

                }
            }
            return false;
        }
    }
}
