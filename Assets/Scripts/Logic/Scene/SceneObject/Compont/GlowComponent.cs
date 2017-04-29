using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Manager;
using Assets.Scripts.Utils;
using Assets.Scripts.Define;
using Assets.Scripts;

namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
    class GlowComponent : BaseComponent
    {
        protected static Material glowMaterial;
		protected static bool isLoadingGlowMaterial = false;
        protected static GameObject glowGameObject;
        protected static List<Material> glow_material_pool = new List<Material>();
        public static GameObject globalSelectGameObject;
		
		
        public static GameObject globalPlayerSelectGameObject;
		

        public override string GetName()
        {
            return GetType().Name;
        }

        public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);

            // 注册事件响应函数
            Regist(ControllerCommand.SET_GLOW, OnSetGlow);
        }

        public override void OnDetachFromEntity(SceneEntity ety)
        {
            UnRegist(ControllerCommand.SET_GLOW, OnSetGlow);
            base.OnDetachFromEntity(ety);
        }

        

        

        //设置发光
        public object OnSetGlow(params object[] objs)
        {
            bool isGlow = Convert.ToBoolean(objs[0]);
            if (glowMaterial == null)
            {
				glowMaterial = ShaderManager.GetInstance().KingSoftSelected;
                /*if (!isLoadingGlowMaterial)
                {
                    AssetLoader.GetInstance().Load(URLUtil.GetEffectPath("effect_hero_light"), GlowMaterial_LoadComplete, AssetType.BUNDLER);
                    isLoadingGlowMaterial = true;
                }*/
            }
            else
            {
                SetGlowImpl(isGlow);
            }
            //SetGlowImpl(isGlow);
            return null;
        }

        private void GlowMaterial_LoadComplete(AssetInfo info)
        {
            glowGameObject = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
            glowGameObject.name = "effect_hero_light";
            //glowGameObject.hideFlags = HideFlags.HideAndDontSave;
            glowGameObject.transform.localScale = Vector3.one;
            GameObject.DontDestroyOnLoad(glowGameObject);
            glowGameObject.SetActive(false);
            glowGameObject.hideFlags = HideFlags.HideAndDontSave;
            glowMaterial = glowGameObject.renderer.material;
            isLoadingGlowMaterial = false;
        }

        protected void SetGlowImpl(bool isGlow)
        {
            if (!Owner.property.isInteractive)
                return;
			if (null == Owner.property.bodyGo)
				return;
            if (isGlow)
            {
                if (Owner.property.saveMaterialMap.Count == 0)
                {
                    foreach (Renderer render in Owner.property.bodyGo.GetComponentsInChildren<Renderer>())
                    {
                        int _len = render.materials.Length;
                        Material[] newMats = new Material[_len];
                        if (render.gameObject.tag.CompareTo(CameraLayerManager.GetInstance().GetMissionSignName()) == 0
							|| render.gameObject.layer == 11
							)
                            continue;
                        for (int i = 0; i < _len; i++)
                        {
                            Material oldMaterial = render.materials[i];
							oldMaterial.SetColor("_Emission",new Color32(0,0,0,0));
                            if (!oldMaterial.HasProperty("_MainTex"))
                            {
                                continue;
                            }
							
                            Material staticMaterial = null;
                            if (glow_material_pool.Count > 0)
                            {
                                staticMaterial = glow_material_pool[0];
                                staticMaterial.color = Owner.property.selectMainColor;
                                staticMaterial.SetColor("_RimColor", Owner.property.rimColor);
                                staticMaterial.SetFloat("_RimPower", Owner.property.rimPower);
								//staticMaterial.SetColor("_Emission",new Color32(0,0,0,0));
                                glow_material_pool.RemoveAt(0);
                            }
                            else
                            {
                                Owner.property.SelectShaderName = glowMaterial.shader.name;
                                staticMaterial = new Material(glowMaterial.shader);
                                //staticMaterial = new Material(Shader.Find(SelectShaderName));
                                staticMaterial.color = Owner.property.selectMainColor;
                                staticMaterial.SetFloat("_RimPower", Owner.property.rimPower);
                                staticMaterial.SetColor("_RimColor", Owner.property.rimColor);
								staticMaterial.SetColor("_Emission",new Color32(0,0,0,0));
                            }
                            staticMaterial.mainTexture = oldMaterial.mainTexture;
                            newMats[i] = staticMaterial;
                        }
                        Owner.property.saveMaterialMap[render] = render.materials;
                        render.materials = newMats;
                        if (_len == 1)
                        {
                            render.material = newMats[0];
                        }
                    }
                }
            }
            else
            {
                if (Owner.property.saveMaterialMap.Count > 0)
                {
                    foreach (Renderer renderer in Owner.property.saveMaterialMap.Keys)
                    {
                        if (renderer == null)
                            continue;
                        foreach (Material mat in renderer.materials)
                        {
                            if (mat.shader.name.CompareTo(Owner.property.SelectShaderName) == 0)
                            {
                                glow_material_pool.Add(mat);
                            }
                        }
                        renderer.materials = Owner.property.saveMaterialMap[renderer];
                    }
                    Owner.property.saveMaterialMap.Clear();
                }
            }
        }
    }
}
