using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts.Model.Player;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Data;
using Assets.Scripts.Logic.Role;
using Assets.Scripts;
using Assets.Scripts.Logic.Intensify;


namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    class EquipChangeComponent : BaseComponent
    {
        private RoleGenerator generator;

        private bool newRoleType = true;


        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            generator = RoleGenerator.Create();

            // 注册事件响应函数
            Regist(ControllerCommand.EQUIP_CHANGE, OnEquipChange);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.EQUIP_CHANGE, OnEquipChange);
            base.OnDetachFromEntity(ety);
        }

        private object OnEquipChange(params object[] objs)
        {
            uint[] equipIDs = null;
            if (objs.Length >= 1)
            {
                equipIDs = (uint[])objs[0];
                Owner.EquipIDs = equipIDs;
            }
            else
            {
                equipIDs = new uint[16];
            }

            string moduleID = "01";
            string str = "";
            str += Owner.property.roleType;
            str += "|face|" + Owner.property.roleType + "_" + moduleID + "_face";
			
			int equ_num = 1;
            for (int i = 0; i < equipIDs.Length; ++i)
            {
                if (ItemConstant.ROLE_CATEGORY_NAME[i] != "")
                {
					/*if (ItemConstant.ROLE_CATEGORY_NAME[i].CompareTo("pad") == 0 &&  Owner.property.roleType.CompareTo("p3")==0)
					{
						reducing = true;
						continue;
					}*/
					equ_num++;
                    if (equipIDs[i] == 0)
                    {
                        str += "|" + ItemConstant.ROLE_CATEGORY_NAME[i] + "|" + Owner.property.roleType + "_" + moduleID + "_" + ItemConstant.ROLE_CATEGORY_NAME[i];
                    }
                    else
                    {
                        KTabLineEquip item = ItemLocator.GetInstance().GetEquip((int)equipIDs[i]);
                        KTabLineShowInfo showInfo = ItemLocator.GetInstance().GetEquipShowInfo(item.ShowID);
                        if (showInfo != null)
                        {
                            str += "|" + ItemConstant.ROLE_CATEGORY_NAME[i] + "|" + Owner.property.roleType + "_" + showInfo.OrderNumber + "_" + showInfo.Category;
                        }
                    }
                }
            }
			if (Owner.property.roleType.CompareTo("p4")==0)
			{
				equ_num++;
				str += "|lianzi|" + Owner.property.roleType + "_" + moduleID + "_lianzi";
				equ_num++;
				str += "|lianzi001|" + Owner.property.roleType + "_" + moduleID + "_lianzi001";
			}
            generator.PrepareConfig(str);
			generator.LoadConfigComplete += CheckModelIsLoaded;
            generator.LoadConfig(equ_num,Owner.Loader);
			if(IntensifyLogic.GetInstance().GetIntensifyUI() != null && IntensifyLogic.GetInstance().GetIntensifyUI().isOpen())
				IntensifyLogic.GetInstance().UpdataIntensify(0,true);
            return null;
        }

        /**
         * 检查角色配件是否加载完
         * */
        private void CheckModelIsLoaded()
        {
            if (newRoleType)
            {
				try
				{
					GameObject.Destroy(Owner.BodyGo);
                	Owner.BodyGo = generator.Generate();	
				}
				catch(System.Exception e)
				{
					//游戏对象已经被释放.
					return;
				}
                Owner.BodyGo.transform.parent = Owner.transform;
                Owner.BodyGo.transform.localPosition = Vector3.zero;
				Owner.BodyGo.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
                Owner.BodyGo.transform.rotation = Quaternion.identity;
                Owner.BodyGo.layer = CameraLayerManager.GetInstance().GetSceneObjectTag();
				BoxCollider _boxCollider = Owner.BodyGo.GetComponent<BoxCollider>();
				if (null != _boxCollider)
				{
					Owner.property.characterController.size = _boxCollider.size*Owner.BodyGo.transform.localScale.y;
					Owner.property.characterController.center = _boxCollider.center*Owner.BodyGo.transform.localScale.y;
					GameObject.Destroy(_boxCollider);
				}
				
                Owner.Anim = Owner.BodyGo.GetComponent<Animation>();
                Owner.DispatchEvent(ControllerCommand.PlayAnimation, Owner.L_Anim_Name, Owner.AnimModel);
                newRoleType = false;
            }
            else
            {
                Owner.BodyGo = generator.Generate(Owner.BodyGo);
            }
			foreach (Renderer render in Owner.BodyGo.GetComponentsInChildren<Renderer>())
            {
                int _len = render.materials.Length;
                Material[] newMats = new Material[_len];
                if (render.gameObject.tag.CompareTo(CameraLayerManager.GetInstance().GetMissionSignName()) == 0)
                    continue;
                for (int i = 0; i < _len; i++)
                {
                    Material oldMaterial = render.materials[i];
					oldMaterial.SetColor("_Emission",new Color32(0,0,0,0));   
                }
            }
            if (SceneLogic.GetInstance().backgroundType == 1)
            {
                //foreach (Material m in Owner.BodyGo.renderer.materials)
                //{
                //    m.color = new Color32(255, 255, 255, 255);
                //}
            }
            else
            {
                //foreach (Material m in Owner.BodyGo.renderer.materials)
                //{
                //    m.color = new Color32(255, 255, 255, 255);
                //}
            }
			if (!Owner.AnimCmp.pause)
				if (Owner.property.isDeadTemp)
				{
					Owner.DispatchEvent(ControllerCommand.CrossFadeAnimation, "dead");
				}
				else
				{
					
					Owner.DispatchEvent(ControllerCommand.CrossFadeAnimation, "idle1");
				}
            
            generator.LoadConfigComplete -= CheckModelIsLoaded;
            if (Owner.OwnerID == PlayerManager.GetInstance().MajorPlayer.PlayerID)
            {
                EventDispatcher.GameWorld.Dispath(ControllerCommand.UPDATE_ROLE_AVATAR, new object());
            }
			
        }
    }
}
