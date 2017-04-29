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

	public class SampleComponent  : BaseComponent {
	
		
		public override string GetName()
        {
            return GetType().Name;
        }
        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);	
			//Regist(ControllerCommand.LOAD_RES, LoadResource);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
			//UnRegist(ControllerCommand.EQUIP_CHANGE, OnEquipChange);
            base.OnDetachFromEntity(ety);
        }
		public override void DoUpdate()
        {
			
		}
	}
}