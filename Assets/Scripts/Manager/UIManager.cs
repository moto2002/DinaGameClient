using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.View.Role;
using Assets.Scripts.Lib.View;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class UIManager
    {
        private List<ViewScript> viewList = new List<ViewScript>();
        private int layerIndex = 0;
		
		public void Update()
		{
			foreach( ViewScript v in viewList )
			{
				if (v.IsNotLoad() || !v.isOpen())
					continue;
				v.Update();
			}
		}
		
        public void FixedUpdate()
        {
            foreach (ViewScript v in viewList)
            {
                if (null == v || v.IsNotLoad())
                    continue;
                v.FixedUpdate();
            }
        }

        public void AddChild(ViewScript view)
        {
            if (viewList.Contains(view))
            {
				viewList.Remove(view);
            }
            viewList.Add(view);
            view.SetLayerIndex(++layerIndex);
        }

        public void Front(ViewScript view)
        {
            if (view.GetLayerIndex() != layerIndex)
            {
                AddChild(view);
                UIPanel.SetDirty();
            }
        }

        private static UIManager instance = null;
        public static UIManager GetInstance()
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
}
