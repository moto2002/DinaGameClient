using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    class EnumUtils
    {
        public static T GetEnumIns<T>(int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T GetEnumIns<T>(System.Object obj)
        {
            return (T)Enum.ToObject(typeof(T), obj);
        }
    }
}
