using System.Collections.Generic;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.Tooltip;
using Assets.Scripts.View.Intensify;

namespace Assets.Scripts.Manager
{
    public class TooltipsManager
    {
        private BaseTipsView tips;
		private IntensifyTipsView euqipTip;
		private EquipSuitTipView suitTip;

        private static TooltipsManager instance;
        public static TooltipsManager GetInstance()
        {
            if (instance == null)
                instance = new TooltipsManager();
            return instance;
        }

        public void Show(ItemInfo itemVO)
        {
            if (tips == null)
                tips = new BaseTipsView();
            tips.ShowTips(itemVO);
        }
		
		/// <summary>
		/// 显示装备强化时的TIPS  2014.3.12 by linfeng
		/// </summary>
		public void ShowEquipTip(EquipInfo itemVO)
		{
			if(euqipTip == null)
				euqipTip = new IntensifyTipsView();
			else
				euqipTip.Show();
			euqipTip.showTips(itemVO);
		}
		
		public void ShowEquipSuitTip(List<ItemInfo> equip)
		{
			if(suitTip == null)
				suitTip = new EquipSuitTipView();
			else
				suitTip.Show();
			suitTip.showTips(equip);
		}

        public void Hide()
        {
            if (tips != null)
                tips.Hide();
			if (euqipTip != null)
				euqipTip.Close();
			if(suitTip!= null)
				suitTip.Close();
        }
    }
}
