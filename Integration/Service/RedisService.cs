using Integration.Settings;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service
{
    public sealed class RedisService //singleton
    {
        private static volatile RedisService instance;
        private readonly string _host;
        private readonly int _port;
        private static object syncRoot = new object();

        private ConnectionMultiplexer _connectionMultiplexer;

        private RedisService()
        {
            _host = RedisSetting.Host;
            _port = RedisSetting.Port;
        }

        public static RedisService CreateNewInstanseSingleton()
        {
            if(instance==null)
            {
                lock(syncRoot)
                {
                    if(instance==null) //added this row to thread safe mode
                        instance = new RedisService();
                }
            }
            instance.Connect();
            return instance;
        }


        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");


        public IDatabase GetDB(int db = 1) => _connectionMultiplexer.GetDatabase(db);
    }
}
