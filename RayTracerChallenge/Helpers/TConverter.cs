using System;

namespace RayTracerChallenge.Helpers
{
    public static class TConverter
    {
        public static T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }

        public static object ChangeType(Type t, object value)
        {
            if (t == value.GetType()) return value;
            return Convert.ChangeType(value, t);
        }
    }
}
