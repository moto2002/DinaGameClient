using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Manager;
using Assets.Scripts.Lib.Loader;

namespace Assets.Scripts.Data
{
    public class KPassiveSkill : AKTabFileObject
    {
        public uint SkillID;
        public uint Level;
        public string Text;
        public string Desc;
        public string Type;
        public int LearnLevel;
        public int CostMoney;
        public int CostItem1;
        public int CostItemNum1;
        public int CostItem2;
        public int CostItemNum2;
        public int CostItem3;
        public int CostItemNum3;
        public int Job;
        public int Index;
        public int SkillType;
        public uint NeedSkillID1;
        public uint NeedSkillID2;
        public uint NeedSkillLevel1;
        public uint NeedSkillLevel2;

        private List<ItemInfo> reqItemList = null;
        private List<KPassiveSkill> reqSkillIDList = null;

        public override string getKey()
        {
            return SkillID + "_" + Level;
        }


        public List<ItemInfo> GetReqItemList()
        {
            if (reqItemList == null)
            {
                reqItemList = new List<ItemInfo>();
                ItemInfo vo = null;
                if (CostItem1 > 0)
                {
                    vo = ItemLocator.GetInstance().GetItemVO(CostItem1, ItemConstant.TYPE_OTHER);
                    vo.MaxStackNum = CostItemNum1;
                    reqItemList.Add(vo);
                }
                if (CostItem2 > 0)
                {
                    vo = ItemLocator.GetInstance().GetItemVO(CostItem2, ItemConstant.TYPE_OTHER);
                    vo.MaxStackNum = CostItemNum2;
                    reqItemList.Add(vo);
                }
                if (CostItem3 > 0)
                {
                    vo = ItemLocator.GetInstance().GetItemVO(CostItem3, ItemConstant.TYPE_OTHER);
                    vo.MaxStackNum = CostItemNum3;
                    reqItemList.Add(vo);
                }
            }
            return reqItemList;
        }

        public List<KPassiveSkill> GetReqSkillList()
        {
            if (reqSkillIDList == null)
            {
                reqSkillIDList = new List<KPassiveSkill>();
                if (NeedSkillID1 > 0)
                {
                    reqSkillIDList.Add(KConfigFileManager.GetInstance().GetPassiveSkill(NeedSkillID1, NeedSkillLevel1));
                }
                if (NeedSkillID2 > 0)
                {
                    reqSkillIDList.Add(KConfigFileManager.GetInstance().GetPassiveSkill(NeedSkillID2, NeedSkillLevel2));
                }
            }
            return reqSkillIDList;
        }

    }
}
