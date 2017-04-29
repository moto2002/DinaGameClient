using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/************************************************************************/
/* 技能与物品的冷却管理                                                                     */
/************************************************************************/
namespace Assets.Scripts.Model.Skill
{
    public enum CooldownType
    {
        Skill,
        Item,
    }
    public class Cooldown : MonoBehaviour
    {
        public CooldownType type;
        public uint id;
        private float cdTime;
		private float allTime;
		private float startTime = 0;

        private void Awake()
        {
            Invoke("OnTimeOver", GetLastTime());
        }

        public float GetLastTime()
        {
            return allTime - (Time.realtimeSinceStartup - startTime);
        }

        private void OnDestroy()
        {
            CancelInvoke();
        }

        private void OnTimeOver()
        {
            CooldownManager.Instance.DeleteCooldown(type, id); 
        }

        public void Dispose()
        {
            GameObject.DestroyImmediate(this.gameObject);
        }

        public static Cooldown Create(CooldownType type, uint id, float cdTime)
        {
            GameObject go = new GameObject("Cooldown");
            go.hideFlags = HideFlags.HideAndDontSave;
            Cooldown cooldown = go.AddComponent<Cooldown>();
            cooldown.type = type;
            cooldown.id = id;
            cooldown.cdTime = cdTime;
            cooldown.startTime = Time.realtimeSinceStartup;
            return cooldown;
        }
    }

    public class CooldownManager
    {
        public delegate void CooldownChange(Cooldown cool);
        public event CooldownChange OnCooldownChange;

        private Dictionary<string, Cooldown> cooldownDict = new Dictionary<string, Cooldown>();

        public void AddCooldown(CooldownType type, uint id, float cdTime)
        {
            Cooldown cool = GetCooldown(type, id);
            if (cool != null)
            {
                cool.Dispose();
            }
            cool = Cooldown.Create(type, id, cdTime);
            string key = type.ToString() + id;
            cooldownDict[key] = cool ;
            if (OnCooldownChange != null)
            {
                OnCooldownChange(cool);
            }
        }

        public void DeleteCooldown(CooldownType type, uint id)
        {
            string key = type.ToString() + id;
            Cooldown cool;
            cooldownDict.TryGetValue(key, out cool);
            if (cool != null)
            {
                cool.Dispose();
            }
            cooldownDict.Remove(key);
        }

        public Cooldown GetCooldown(CooldownType type, uint id)
        {
            string key = type.ToString() + id;
            Cooldown cool;
            cooldownDict.TryGetValue(key, out cool);
            return cool;
        }

        public static readonly CooldownManager Instance = new CooldownManager();
    }
}
