using System.Collections.Generic;
using Assets.Scripts.Lib.View;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.View.UIDetail;

namespace Assets.Scripts.View.Tooltip
{
    public class BaseTipsView : ToolTipsUIDetail
    {
        public ItemInfo mItem;
        private bool bInited = false;

        public BaseTipsView()
            : base(50, 50)
        {

        }

        protected override void Init()
        {
            base.Init();
            bInited = true;
            if (mItem != null)
                ShowTips(mItem);
        }

        public void ShowTips(ItemInfo itemVo)
        {
            mItem = itemVo;
            if (!bInited)
                return;
            string str = "";
            str += itemVo.Name + "\n";
            str += itemVo.Tips;
            UITooltip.ShowText(str);
            Front();
        }

        public void Hide()
        {
            UITooltip.ShowText(null);
        }
    }
}
