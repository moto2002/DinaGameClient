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
using Assets.Scripts.Model.Skill;
using Assets.Scripts.Model.Player;

class NumTipItem
{
	public NumTip.OFFSET_TYPE type;
	public Vector3 Position;
	public Vector3 delta;
	public string num;
	public string fontName;
	public  string animName;
}
namespace Assets.Scripts.Logic.Scene.SceneObject.Compont
{
	public class TipFontData
	{
		public UIFont font;
		public UILabel label;
		public Material material;
		
		public void Build()
		{
			GameObject go = new GameObject();
			label = go.AddComponent<UILabel>();
			label.pivot = UIWidget.Pivot.Bottom;
			label.font = font;
			go.hideFlags = HideFlags.HideAndDontSave;
			GameObject.DontDestroyOnLoad(go);
			material  = new Material(ShaderManager.GetInstance().KingSoftNumTip);
			material.mainTexture =  font.material.mainTexture;
			material.renderQueue = 5000;
			
		}
	}
	public class NumTipComponent   : BaseComponent{
		
		static GameObject root = null;
		Ticker ticker = new Ticker(200);
		
		
		//waittingMap
		List<NumTipItem> waittingitem = new List<NumTipItem>();
		public delegate void LoadComplete();
	    public static event LoadComplete OnLoadComplete;
		static int loadCount;
		static string animPath ;
		static Dictionary<string,UnityEngine.Object> animDict = new Dictionary<string, UnityEngine.Object>();
		static  Queue<string> resources = new Queue<string>();
		
		
		static void   AddResource(string resourceName)
	    {
	        resources.Enqueue(resourceName);
	    }
		
		public  UIDrawCallForShaderMat GetDrawCall(UILabel uilabel)
		{
			GameObject obj = new GameObject();
			obj.transform.localScale = new Vector3(0.004f,0.004f,0.004f);
			UIDrawCallForShaderMat dc = obj.AddComponent<UIDrawCallForShaderMat>();
			
			//obj.layer = LayerMask.NameToLayer("2D");
			BetterList<Vector3> verts = new BetterList<Vector3>();
			BetterList<Vector2> uvs = new BetterList<Vector2>();
			BetterList<Color32> cols = new BetterList<Color32>();
			dc.material = uilabel.material;
			uilabel.Update();
			uilabel.OnFill(verts,uvs,cols);
			dc.Set(verts,null,null,uvs,cols);
			
			return dc;
		}
		
		public override void OnAttachToEntity(SceneEntity ety)
        {
            BaseInit(ety);
        }
		
	    public static void Load()
	    {
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/AttackFont.font", LoadFontCompleteHandler, AssetType.SCENE);
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/CritFont.font", LoadFontCompleteHandler, AssetType.SCENE);
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/MpFont.font", LoadFontCompleteHandler, AssetType.SCENE);
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/ExpFont.font", LoadFontCompleteHandler, AssetType.SCENE);
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/HurtFont.font", LoadFontCompleteHandler, AssetType.SCENE);
			AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "UI/FightFont.font", LoadFontCompleteHandler, AssetType.SCENE);
			
			AddResource("effect_ui_shuzitanchu_putong.res");
			AddResource("effect_ui_shuzitanchu_baoji2.res");
			animPath = URLUtil.GetResourceLibPath() + "Effect/";
	        loadCount = resources.Count;
	        while (resources.Count > 0)
	        {
	            string resource = resources.Dequeue();
	            AssetLoader.GetInstance().Load(animPath + resource , OnAnimLoadComplete, AssetType.BUNDLER);
	        }
	    }
		
		private static void LoadFontCompleteHandler(AssetInfo info)
	    {
			GameObject fontObj = GameObject.Instantiate(info.bundle.mainAsset) as GameObject;
			fontObj.hideFlags = HideFlags.HideInHierarchy; 
			GameObject.DontDestroyOnLoad(fontObj);
			UIFont font = fontObj.GetComponent<UIFont>();
			string fontName = info.bundle.mainAsset.name.ToLower();
			
			TipFontData data = new TipFontData();
			data.font = font;
			data.Build();
			fontDatas[fontName] = data;
			
			loadCount--;
	        if (loadCount == 0 && OnLoadComplete != null)
	        {
	            OnLoadComplete();
	        }
		}
		
	    public static void OnAnimLoadComplete(AssetInfo info)
	    {
			int _len = animPath.Length;
			string key = info.url.Substring(_len);
	        animDict [key] = info.bundle.mainAsset;
	        loadCount--;
	        if (loadCount == 0 && OnLoadComplete != null)
	        {
	            OnLoadComplete();
	        }
	    }
	
		static Dictionary<string,TipFontData> fontDatas = new Dictionary<string, TipFontData>();
		/*static Dictionary<string,UIFont> fonts = new Dictionary<string,UIFont>();
		static Dictionary<UIFont,UILabel> labels = new Dictionary<UIFont,UILabel>();
		static Dictionary<UIFont,Material> materials = new Dictionary<UIFont,Material>();*/
		
		
		static GameObject effectRoot = null;
		
		public  void CreateTipEnum(GameObject _root,Vector3 position,Vector3 delta,string num,TipFontData data,string animName ,NumTip.OFFSET_TYPE type)
		{
			if (null == effectRoot)
				effectRoot = new GameObject("EffectRoot");
			GameObject eff = GameObject.Instantiate(animDict[animName]) as GameObject;
			eff.transform.parent = effectRoot.transform;
			eff.transform.localScale = new Vector3(1f,1f,1f);
			if (null != Owner)
				eff.transform.position = Owner.transform.position;
			eff.layer = 9;
			GameObject go = new GameObject();
			go.transform.parent = _root.transform;
			go.name = "Tips";
			UILabel label = data.label;
			label.text = num;
			NumTip tip = go.AddComponent<NumTip>();
			tip.aim = eff.transform.GetChild(0).gameObject;
			//tip.label = label;
			tip.root = eff;
			tip.position = position;
			tip.delta = delta;
			tip.owner = Owner.gameObject;
			UIDrawCallForShaderMat dc = GetDrawCall(label);
			dc.gameObject.name = "dc";
			tip.dc = dc;
			dc.transform.parent = tip.transform;
			dc.gameObject.renderer.sharedMaterial = data.material;
			dc.transform.localPosition = Vector3.zero;
			dc.transform.rotation = Quaternion.identity;
			//dc.gameObject.transform.position = tip.position + Vector3.up;
		}
		
		
		public override string GetName()
	    {
	        return GetType().Name;
	    }
		public override void DoUpdate()
	    {
	        if (ticker.IsEnable())
			{
				if (waittingitem.Count>0)
				{
					NumTipItem item = waittingitem[0];
					waittingitem.RemoveAt(0);
					CreateTipEnum(root,item.Position,item.delta,item.num,fontDatas[item.fontName],item.animName,item.type);
				}
			}
	    }
		public void CreateTip(Vector3 position,string num,string fontName, string animName ,NumTip.OFFSET_TYPE type = NumTip.OFFSET_TYPE.NONE)
		{
			fontName = fontName.ToLower();
			if(null== root)
			{
				root = new GameObject("3d Tips");
			}
			if (fontDatas.ContainsKey(fontName))
			{
				NumTipItem item = new NumTipItem();
				item.type = type;
				item.animName = animName;
				item.delta = position;
				item.num = num;
				item.Position = Owner.Position;
				item.fontName = fontName;
				waittingitem.Add(item);
			}
			
		}
	}
}
