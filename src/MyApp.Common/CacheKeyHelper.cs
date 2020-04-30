using System;

namespace MyApp.Common
{
    public class CacheKeyHelper
    {
        public string DefaultPrefix { get; set; } = "MyCache.";

        public string CreateKey(Type theType, string prefix = null)
        {
            return string.Format("{0}{1}", prefix ?? DefaultPrefix, theType.FullName);
        }

        public static CacheKeyHelper Instance = new CacheKeyHelper();
    }
}
