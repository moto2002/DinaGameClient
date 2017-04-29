using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;
using Assets.Scripts.Define;

namespace Assets.Scripts.Data
{
    /************************************************************************/
    /* 礼包数据实体
     * 表格 dino-products\trunk\resource\settings\gift
     * 实例化位置 KConfigFileManager
     * author@wuheyang*/
    /************************************************************************/
    public class KGiftData : AKTabFileObject
    {
        public int nID;
        public KGiftType eType;
        public string Name;
        public int nLevelLimit;//等级需求
        public int nCombatLimit;//战斗力需求
        public int nOnlineTime;//在线时间长
        public int nIsRepeat;//是否可重复领取，0不可重复，1可重复
        public int nAwardID;//dino-products\trunk\resource\settings\award\award.tab nID

        public override string getKey()
        {
            return nID.ToString();
        }
    }


}
