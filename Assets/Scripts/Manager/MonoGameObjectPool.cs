using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class MonoGameObjectPool<T> where T : MonoBehaviour
    {
        private Queue<T> uiGameObjectQueue = new Queue<T>();

        public T GetUIGameObject()
        {
            if (uiGameObjectQueue.Count == 0)
            {
                return CreateUIGameObject();
            }
            return uiGameObjectQueue.Dequeue();
        }

        private T CreateUIGameObject()
        {
            T uiGameObject;
            GameObject go = new GameObject("UIGameObject");
            go.hideFlags = HideFlags.HideAndDontSave;
            uiGameObject = go.AddComponent<T>();
            return uiGameObject;
        }

        public void Destory(T sprite)
        {
            uiGameObjectQueue.Enqueue(sprite);
        }

        public static readonly MonoGameObjectPool<T> Instance = new MonoGameObjectPool<T>();
    }
}
