using UnityEngine;
using System.Collections;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
	public class KTabServerEquip : AKTabFileObject
	{
		public int ID;  //ID
		public string Name; //名字
		public string Desc; // tip 介绍 
		public int Genre;// 类型 
		public int SubType;// 子类型
		public int DropID;// 掉落组ID
		public int Quality;// 品质
		public int Prefix;//
		public int ReqJob;//职业限制
		public int ReqSex;//性别限制
		public int BindType;//绑定类型
		public int ReqMinLevel;//使用等级
		public int UseTime;//时间
		public int UseSign;//标示
		public int DropSign;//掉落标示
		public int Broadcast;//
		public int BuyPrice;//价格
		public int SellPrice;// 卖价
		public int ShopPrice;// 商城
		public int ShopBindPrice;//
		public int UIID;//
		public int ShowID;//
		public int DestroyAfterUse;//
		public int StackNum;//
		public int ScriptID;//
		public int WashSign;//
		public int MaxStrengthen;//
		public int InitHole;//
		public int MaxHole;//
		
		public override string getKey ()
		{
			return ID.ToString();
		}
	}
}
