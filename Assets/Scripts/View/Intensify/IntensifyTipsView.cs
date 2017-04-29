using System.Collections;
using Assets.Scripts.View.UIDetail;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Manager;

namespace Assets.Scripts.View.Intensify
{
	public class IntensifyTipsView : IntensifyTipsUIDetail
	{
		public EquipInfo mItem;
		private bool bInited = false;
		
		public IntensifyTipsView()
            : base(50, 50)
        {

        }
		
		protected override void Init ()
		{
			base.Init ();
			bInited = true;
            if (mItem != null)
                showTips(mItem);
		}
		
		public void showTips (EquipInfo info)
		{
			mItem = info;
            if (!bInited)
                return;
			UIAtlas atlas = UIAtlasManager.GetInstance().GetUIAtlas("IconAtlas");
			ItemFrame.spriteName = info.Icon;
			ItemFrame.atlas = atlas;
			
			Name.text = info.Name;
			Description.text = info.Tips;
		}
		
		public void Close ()
		{
			clearUI();
			Hide();
		}
		
		private void clearUI ()
		{
			Name.text = "";
			Description.text = "";
		}
	}
}

