using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    public class CacheService : ICacheService
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        public T GetData<T>(string key) => (T) _cache.Get(key);
        

        public bool RemoveData(string key)
        {
            bool result = false;
            if (key is not null) 
            {
                _cache.Remove(key);
                result = true;
            }
            return result;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expiration)
        {
            bool result = false;
            if(value is not null)
            {
                _cache.Set(key, value, expiration);
                result = true;
            }
            return result;
            
        }
    }
}
