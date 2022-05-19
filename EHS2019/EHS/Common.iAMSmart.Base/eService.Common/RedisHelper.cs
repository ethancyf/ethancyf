using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
//using eID_Common.Component.ServiceProvider;


namespace eService.Common
{
    public class RedisHelper
    {
        private LogUtils _logUtils;

        public LogUtils LogUtils
        {
            get
            {
                if (_logUtils == null)
                {
                    return new LogUtils(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }
                return _logUtils;
            }
        }

        private readonly PooledRedisClientManager pool = null;
        private readonly string[] WriteHosts = null;
        private readonly string[] ReadHosts = null;

        public RedisHelper()
        {
            if (!string.IsNullOrEmpty(Constants.REDIS_WRITE_HOST))
            {
                WriteHosts = Constants.REDIS_WRITE_HOST.Split(',');
                ReadHosts = Constants.REDIS_READ_HOST.Split(',');
                if (ReadHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(WriteHosts, ReadHosts,
                        new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = Constants.REDIS_MAX_WRITE_POOL,
                            MaxReadPoolSize = Constants.REDIS_MAX_READ_POOL,

                            AutoStart = true
                        });
                }
            }
        }

        public void Add<T>(string key, T value, DateTime expiry)
        {
            //ServiceProviderBLL udtServiceProviderBLL = new ServiceProviderBLL();
            //eIDInformation udteIDInformation = new eIDInformation();

            string final_expiry="";

            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, expiry - DateTime.Now);
                            //r.Set(key, value, DateTime.Now.AddDays(1));  //临时改成1天
                            final_expiry = (expiry - DateTime.Now).ToString();
                            
                            // store the data into the table eIDInformation
                          //  udtServiceProviderBLL.AddeIDInfo(key.ToString(), value.ToString(), final_expiry);
                            //udteIDInformation.AddeIDInfo(key.ToString(), value.ToString(), final_expiry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}。异常：{3}", "cache", "存储", key, ex.Message);
                LogUtils.Error(msg);
            }

        }

        /// <summary>
        /// 添加到缓存，默认过期时间1年
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(string key, T value)
        {
            this.Add<T>(key, value, DateTime.Now.AddDays(365));
            LogUtils.Debug(string.Format("put key:{0},value:{1} into redis", key, value));
        }

        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
          //  ServiceProviderBLL udtServiceProviderBLL = new ServiceProviderBLL();

            //eIDInformation udteIDInformation = new eIDInformation();

            string sliding_expiry = "";

            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);
                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, slidingExpiration);
                            //LogUtils.Info("2");
                            // store the data into the table eIDInformation
                            sliding_expiry = slidingExpiration.ToString();
                           //udtServiceProviderBLL.AddeIDInfo(key.ToString(), value.ToString(), sliding_expiry);
                            //udteIDInformation.AddeIDInfo(key.ToString(), value.ToString(), sliding_expiry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}。异常：{3}", "cache", "存储", key, ex.Message);
                LogUtils.Error(msg);
            }

        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = default(T);
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}。异常：{3}", "cache", "获取", key, ex.Message);
                LogUtils.Error(msg);
            }

            LogUtils.Debug(string.Format("get key:{0},value:{1} from redis", key, obj));
            return obj;
        }

        public void Remove(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}。异常：{3}", "cache", "删除", key, ex.Message);
                LogUtils.Error(msg);
            }

        }

        public bool Exists(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.ContainsKey(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}。异常：{3}", "cache", "是否存在", key, ex.Message);
                LogUtils.Error(msg);
            }

            return false;
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys) where T : class
        {
            if (keys == null)
            {
                return null;
            }

            keys = keys.Where(k => !string.IsNullOrEmpty(k));

            if (keys.Count() == 1)
            {
                T obj = Get<T>(keys.Single());

                if (obj != null)
                {
                    return new Dictionary<string, T>() { { keys.Single(), obj } };
                }

                return null;
            }

            if (!keys.Any())
            {
                return null;
            }

            try
            {
                using (var r = pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        return r.GetAll<T>(keys);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}。异常：{3}", "cache", "获取", keys.Aggregate((a, b) => a + "," + b), ex.Message);
                LogUtils.Error(msg);
            }

            return null;
        }
    }
}