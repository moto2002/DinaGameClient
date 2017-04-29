using System;
using System.Collections.Generic;
using Assets.Scripts.Proto;
using Assets.Scripts.Data;

namespace Assets.Scripts.Logic.Item
{
    public class ItemInfo : IDragInfo
    {
        protected int _typeId;
        public int ID;              //对应物品栏ID
        public int ServerID;        //服务器生成ID
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
        public int CurNum;          //当前数量
        public int Position;        //在背包中的位置
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
        public int CanDrop = 1;         //是否可以丢弃
        public int IsBroadcast;     //是否广播
        public int UIID;            //打开面板ID

        public virtual int typeId
        {
            get
            {
                return _typeId;
            }
            set
            {
                if (_typeId != value)
                {
                    _typeId = value;
                    KTabLineItem item = ItemLocator.GetInstance().GetOtherItem(typeId);
                    ID = item.ID;
                    Name = item.Name;
                    Genre = item.Genre;
                    SubType = item.SubType;
                    Quality = item.Quality;
                    OverdueTime = item.OverdueTime;
                    OverduePoint = item.OverduePoint;
                    Icon = item.Icon;
                    Tips = item.Tips;
                    CanTrade = item.CanTrade;
                    CanDestroy = item.CanDestroy;
                    CanStack = item.CanStack;
                    MaxStackNum = item.MaxStackNum;
                    BuyPrice = item.BuyPrice;
                    SellPrice = item.SellPrice;
                    MallPrice = item.MallPrice;
                    MallBindPrice = item.MallBindPrice;
                    RequireLevel = item.RequireLevel;
                    RequireGender = item.RequireGender;
                    CoolDownID = item.CoolDownID;
                    DestroyAfterUse = item.DestroyAfterUse;
                    SellNotify = item.SellNotify;
                    RequireJob = item.RequireJob;
                    CanUse = item.CanUse;
                    IsBroadcast = item.IsBroadcast;
                    UIID = item.UIID;
                }
            }
        }

        public string DragIcon { get { return Icon; } }

        public virtual void Copy(S2C_SYNC_ITEM vo)
        {
            this.typeId = (int)vo.dwTabIndex;
            this.Genre = (int)vo.byTabType;
            this.Position = (int)vo.byPos;
            //this.Quality = (int)vo.quality;
            this.BindType = (int)vo.byBind;
            this.CurNum = (int)vo.uStackNum;
        }
    }
}
