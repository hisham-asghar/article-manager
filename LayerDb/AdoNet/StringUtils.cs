using System;

namespace LayerDb.AdoNet
{
    public static class StringUtils
    {
        public static object To(this string value, Type t)
        {
            return Convert.ChangeType(value, t);
        }
    }
}
