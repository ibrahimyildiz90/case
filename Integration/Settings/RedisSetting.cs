using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Settings
{
    public static class RedisSetting
    {
        //Theese fields must be get from config file in real life.
        private static string host = "localhost";
        private static int port = 6379;
        public static string Host { get => host; }
        public static int Port { get => port; }
    }
}
