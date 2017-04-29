using UnityEngine;
using System.Collections;
using Assets.Scripts.View.Alert;
using Assets.Scripts.Logic.Item;

namespace Assets.Scripts.Manager
{
	public delegate void AlertCallBackDelegate();
	
	public class AlertWindowManager
	{	
		private BaseAlertView alertUI;	
		private static AlertWindowManager instance;
        public static AlertWindowManager GetInstance()
        {
            if (instance == null)
                instance = new AlertWindowManager();
            return instance;
        }
		
		public void AlertEquipWindow (EquipInfo info , int time ,AlertCallBackDelegate alertCallBack)
		{

			if(alertUI == null)
				alertUI = new BaseAlertView();
			else
				alertUI.Show();
			alertUI.AlertWindows(info ,time);
			if (null != alertCallBack)
			{
				alertUI.AlertCallBack += alertCallBack;
			}

		}
	}
}
