using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    class ObjectFactory<T>
    {
        public T GetInstance()
        {
            return Activator.CreateInstance<T>();
        }

        public void Push(T obj)
        {

        }
    }
}
