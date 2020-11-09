using System.Collections.Generic;

namespace VideoMetaInfo.common
{
    public static class InMemory
    {
        private static IDictionary<string, object> container = new Dictionary<string, object>();

        public static bool Can(string key)
        {
            return container.ContainsKey(key);
        }

        public static object Get(string key)
        {
            return container[key];
        }

        public static void Set(string key, object val)
        {
            container[key] = val;
        }
    }
}
