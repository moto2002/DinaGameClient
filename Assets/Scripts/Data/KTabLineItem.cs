using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KTabLineItem : AKTabFileObject
	{
        public int ID;              //typeID
        public string Name;         //物品名
        public int Genre;           //物品大类
        public int SubType;         //物品子类
        public int Quality;         //物品品质
        public int OverdueTime;     //过期时间
        public string OverduePoint; //过期时间点
        public string Icon;         //图标
        public string Tips;         //物品描述
        public int CanTrade;        //是否可交易
        public int CanDestroy;      //是否可销毁
        public int CanStack;        //是否可堆叠
        public int MaxStackNum;     //最大堆叠数
        public int BuyPrice;        //购买价格
        public int SellPrice;       //贩卖价格
        public int MallPrice;       //商城价格       
        public int MallBindPrice;   //商城绑定价格
        public int BindType;        //绑定类型        
        public int RequireLevel;    //等级限制
        public int RequireGender;   //性别限制
        public int CoolDownID;      //冷却类型ID
        public int DestroyAfterUse; //使用后就删除
        public int SellNotify;      //卖出是否提示
        public int RequireJob;      //职业限制
        public int CanUse;          //是否可以使用
        public int CanDrop;         //是否可以丢弃
        public int IsBroadcast;     //是否广播
        public int UIID;            //打开面板ID
		public int ScriptName;

        public override string getKey()
        {
            return ID.ToString();
        }
	}
}
