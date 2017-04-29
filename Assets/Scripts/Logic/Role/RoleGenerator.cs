using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using Assets.Scripts.Manager;
using Assets.Scripts.Logic.Scene.SceneObject.Compont;

namespace Assets.Scripts.Logic.Role
{
    public class RoleGenerator
    {
        public delegate void LoadConfigCompleteEvent();
        public event LoadConfigCompleteEvent LoadConfigComplete;

        private static Dictionary<string, Dictionary<string, List<CharacterElement>>> sortedElements;
        private static List<string> availableRoles = new List<string>();
        private Dictionary<string, AssetBundleRequest> roleBaseRequests = new Dictionary<string, AssetBundleRequest>();
        private string curRole;
        private Dictionary<string, CharacterElement> curConfiguration = new Dictionary<string, CharacterElement>();
        private float progressValue;



        public RoleGenerator()
        {

        }

        public static RoleGenerator Create()
        {
            RoleGenerator gen = new RoleGenerator();
            return gen;
        }

        public void PrepareRandomConfig()
        {
            PrepareRandomConfig(availableRoles[Random.Range(0, availableRoles.Count)]);
        }

        public void PrepareRandomConfig(string character)
        {
            curConfiguration.Clear();
            curRole = character.ToLower();
            foreach (KeyValuePair<string, List<CharacterElement>> category in sortedElements[curRole])
            {
                curConfiguration.Add(category.Key, category.Value[Random.Range(0, category.Value.Count)]);
            }
        }

        public void PrepareConfig(string config)
        {
            config = config.ToLower();
            string[] settings = config.Split('|');
            curRole = settings[0];
            curConfiguration = new Dictionary<string, CharacterElement>();
            for (int i = 1; i < settings.Length; )
            {
                string categoryName = settings[i++];
                string elementName = settings[i++];
                CharacterElement element = null;
				
				if (ConfigManager.GetInstance().DebugMode)
				{
					foreach (CharacterElement e in sortedElements[curRole][categoryName])
                    {
                        if (e.name != elementName) continue;
                        element = e;
                        break;
                    }
				}
				else
				{
					try
	                {
	                    foreach (CharacterElement e in sortedElements[curRole][categoryName])
	                    {
	                        if (e.name != elementName) continue;
	                        element = e;
	                        break;
	                    }
	                }
	                catch (Exception ex)
	                {
						foreach ( string c in sortedElements.Keys)
						{
							Debug.LogWarning(c);
							foreach( string c1 in sortedElements[c].Keys)
							{
								Debug.LogWarning("\t"+c1);
							}
						}
	                    Debug.LogError("item is not exists, categoryName=" + categoryName + ", elementName=" + elementName);
	                }
					
				}
                
                if (element == null)
                {
                    Debug.Log("Element not found: " + elementName);
                    continue;
                }
                curConfiguration.Add(categoryName, element);
            }
        }

        public string GetConfig()
        {
            string s = curRole;
            foreach (KeyValuePair<string, CharacterElement> category in curConfiguration)
                s += "|" + category.Key + "|" + category.Value.name;
            return s;
        }

        public void ChangeCharacter(bool next)
        {
            string character = null;
            for (int i = 0; i < availableRoles.Count; i++)
            {
                if (availableRoles[i] != curRole) continue;
                if (next)
                    character = i < availableRoles.Count - 1 ? availableRoles[i + 1] : availableRoles[0];
                else
                    character = i > 0 ? availableRoles[i - 1] : availableRoles[availableRoles.Count - 1];
                break;
            }
            PrepareRandomConfig(character);
        }

        public void ChangeElement(string catagory, bool next)
        {
            List<CharacterElement> available = sortedElements[curRole][catagory];
            CharacterElement element = null;
            for (int i = 0; i < available.Count; i++)
            {
                if (available[i] != curConfiguration[catagory]) continue;
                if (next)
                    element = i < available.Count - 1 ? available[i + 1] : available[0];
                else
                    element = i > 0 ? available[i - 1] : available[available.Count - 1];
                break;
            }
            curConfiguration[catagory] = element;
        }

        public static void LoadRoleBaseData()
        {
            AssetLoader.GetInstance().Load(URLUtil.url("/ResourceLib/Actor/RoleElementDatabase.assetbundle")
                                            , LoadCompleteHandler, AssetType.BUNDLER);
        }

        private static void LoadCompleteHandler(AssetInfo info)
        {
			CharacterElementHolder ceh;
			try
			{
				ceh = info.bundle.mainAsset as CharacterElementHolder;
            	sortedElements = new Dictionary<string, Dictionary<string, List<CharacterElement>>>();
			}
			catch(System.Exception e)
			{
				//游戏对象已经被释放.
				return;
			}

            
            foreach (CharacterElement element in ceh.content)
            {
                string[] a = element.bundleName.Replace(".actorSkin", "").Split('_');
                string character = a[0]  ;
                string category = a[2];
				//Debug.LogWarning("character = "+character+" category = "+category);
                if (!availableRoles.Contains(character))
                    availableRoles.Add(character);

                if (!sortedElements.ContainsKey(character))
                    sortedElements.Add(character, new Dictionary<string, List<CharacterElement>>());

                if (!sortedElements[character].ContainsKey(category))
                    sortedElements[character].Add(category, new List<CharacterElement>());

                sortedElements[character][category].Add(element);
            }
            EventDispatcher.GameWorld.Dispath(ControllerCommand.ROLE_DATA_BASE_LOADED, new object());
        }
		
        private int configNum;
        public void LoadConfig(int num ,LoadAssetComponent loader)
        {
			loader.Release();
			configNum = num+1;//加一个基础模型.
            loader.Load(URLUtil.url("/ResourceLib/Actor/" + curRole + "/rolebase.model")
                                            , LoadConfigCompleteHandler, AssetType.BUNDLER);
            foreach (CharacterElement c in curConfiguration.Values)
            {
                string[] a = c.bundleName.Split('_');
                loader.Load(URLUtil.url("/ResourceLib/Actor/" + curRole + "/" + a[2] + "/" + a[1] + ".actorSkin")
                                                , LoadConfigCompleteHandler, AssetType.BUNDLER);
            }
        }

        private void LoadConfigCompleteHandler(AssetInfo info)
        {
			AssetBundleRequest request = null;
            if (info.url.Contains(".model"))
            {
                if (!roleBaseRequests.ContainsKey(curRole))
                {
					request = info.bundle.LoadAsync("rolebase", typeof(GameObject));
                    roleBaseRequests.Add(curRole, request);
					
                }
            }
            else
            {
                string bundleName = info.url.Substring(URLUtil.url("/ResourceLib/Actor/").Length);
                string[] str = bundleName.Replace(".actorSkin", "").Split('/');
                bundleName = str[0] + "_" + str[2] + "_" + str[1];
                foreach (CharacterElement c in curConfiguration.Values)
                {
                    if (c.bundleName == bundleName)
                    {
                        c.FillInfo(info.bundle);
                        break;
                    }
                }
            }
            configNum--;
            if (configNum == 0 && LoadConfigComplete != null)
                LoadConfigComplete();
        }

        public GameObject Generate()
        {
            GameObject root = (GameObject)Object.Instantiate(roleBaseRequests[curRole].asset);
            root.name = curRole;
            root.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            if (root.animation == null)
            {
                root.AddComponent<Animation>();
            }
            return Generate(root);
        }

        public GameObject Generate(GameObject root)
        {
            float startTime = Time.realtimeSinceStartup;

            List<CombineInstance> combineInstances = new List<CombineInstance>();
            List<Material> materials = new List<Material>();
            List<Transform> bones = new List<Transform>();
            Transform[] transforms = root.GetComponentsInChildren<Transform>();
			
			foreach (KeyValuePair<string, CharacterElement> kvp in curConfiguration)
            {
				
				CharacterElement element = kvp.Value;
                SkinnedMeshRenderer smr = element.GetSkinnedMeshRenderer();
                materials.AddRange(smr.materials);
                for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = smr.sharedMesh;
                    ci.subMeshIndex = sub;
					//Debug.LogWarning(smr.sharedMesh.bindposes.Length);
                    combineInstances.Add(ci);
                }
				
				int boneCount = element.GetBoneNames().Length;
                foreach (string bone in element.GetBoneNames())
                {
                    foreach (Transform transform in transforms)
                    {
                        if (transform.name != bone)
						{
							continue;
						}
                        bones.Add(transform);
                        break;
                    }
                }
                Object.Destroy(smr.gameObject);
				
            }

            SkinnedMeshRenderer r = root.GetComponent<SkinnedMeshRenderer>();
            r.sharedMesh = new Mesh();
            r.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
            r.bones = bones.ToArray();
			
			int bones_count = r.bones.Length;
			int bindposes_count = r.sharedMesh.bindposes.Length;
			//Debug.LogWarning("bones.Count = "+bones_count + " bindposes.Count = "+ bindposes_count + " totalCount ="+totalCount );
			
            r.materials = materials.ToArray();
            Debug.Log("Generating character took: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
            return root;
        }
    }
}
