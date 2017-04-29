using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Data;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	class BuffInfor
	{
		public FxAsset assert;
		public string [] bindPoints;
		public KBuff infor ;
	}
    class BuffComponent : BaseComponent
    {
        static Dictionary<ushort, BuffInfor> globalBuffsTemps = new Dictionary<ushort, BuffInfor>();
        System.Collections.Generic.Dictionary<ushort, SkillBuff> buffs = new System.Collections.Generic.Dictionary<ushort, SkillBuff>();

        public static void InitBuff(ushort wBuffID)
        {
            if (globalBuffsTemps.ContainsKey(wBuffID))
                return;
            KBuff buffInfor = KConfigFileManager.GetInstance().buffs.getData(wBuffID.ToString());
            FxAsset assert = new FxAsset();
            assert.init(URLUtil.GetResourceLibPath() + buffInfor.BuffPath);
			BuffInfor infor = new BuffInfor();
			infor.infor = buffInfor;
			infor.assert = assert;
			infor.bindPoints = buffInfor.BindPoint.Split(';');
            globalBuffsTemps[wBuffID] = infor;
        }


        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            Regist(ControllerCommand.CLEAR_BUFF, OnClearBuff);
            Regist(ControllerCommand.ADD_BUFF, OnAddBuff);
            Regist(ControllerCommand.REMOVE_BUFF, OnRemoveBuff);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.CLEAR_BUFF, OnClearBuff);
            UnRegist(ControllerCommand.ADD_BUFF, OnAddBuff);
            UnRegist(ControllerCommand.REMOVE_BUFF, OnRemoveBuff);

            base.OnDetachFromEntity(ety);
        }

        public object OnClearBuff(params object[] objs)
        {
            foreach (ushort k in buffs.Keys)
            {
                SkillBuff buff = buffs[k];
				foreach ( GameObject obj in buff.buffs)
				{
					obj.AddComponent<DestoryObject>();
				}
                
            }
			Owner.property.lockOpera = false;
            buffs.Clear();
            return null;
        }

        public object OnAddBuff(params object[] objs)
        {
            ushort wBuffID = Convert.ToUInt16(objs[0]);
            InitBuff(wBuffID);
			BuffInfor infor;
            FxAsset assert;
			OnRemoveBuff(wBuffID);
            if (globalBuffsTemps.TryGetValue(wBuffID, out infor))
            {
				if(infor.infor.AttributeKey1.CompareTo("atForbitMove") == 0)
					Owner.property.lockOpera = true;
				assert = infor.assert;
				int len = infor.bindPoints.Length;
				SkillBuff buff = new SkillBuff();
				
				buffs[wBuffID] = buff;
				buff.wBuffID = wBuffID;
				buff.infor = infor.infor;
				buff.buffs = new GameObject[len];
				for (int i=0;i<len;i++)
				{
					GameObject g = assert.CloneAndAddToParent(Owner.transform, Vector3.up * 1.5f);
					if (null == g)
                    	return null;
					buff.buffs[i] = g;
					ObjectUtil.SetTagWithAllChildren(g, CameraLayerManager.GetInstance().GetMissionSignName());
					Transform t = Owner.GetChildTransform(infor.bindPoints[i]);
					if (null != t)
						g.transform.parent = t;
						g.transform.localPosition = Vector3.zero;
						g.transform.localScale = Vector3.one;
				}
                
            }
            return null;
        }
        public object OnRemoveBuff(params object[] objs)
        {
            ushort wBuffID = Convert.ToUInt16(objs[0]);
			
			OnRemoveBuff(wBuffID);
			
			Dictionary<ushort, BuffInfor> globalBuffsTemps = new Dictionary<ushort, BuffInfor>();
			
			if (Owner.property.lockOpera)
			{
				Owner.property.lockOpera = false;
				foreach( KeyValuePair<ushort, SkillBuff> kvp in buffs)
				{
					SkillBuff sb = kvp.Value;
					if(sb.infor.AttributeKey1.IndexOf("atForbitMove") == 0)
					{
						Owner.property.lockOpera = true;
						break;
					}
				}
			}
			return null;
           
        }
		
		private object OnRemoveBuff(ushort wBuffID)
        {
            SkillBuff b;
            if (buffs.TryGetValue(wBuffID, out b))
            {
                if (null != b)
				{
					foreach (GameObject o in b.buffs)
					{
						o.AddComponent<DestoryObject>();
					}
				}   
                buffs.Remove(wBuffID);
            }
            return null;
        }
    }
}
