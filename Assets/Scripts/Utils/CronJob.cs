using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utils
{
	public class CronJob : MonoBehaviour
	{
		private static CronJob instance;
		
		public delegate void Run();
		
		public Run OnRun;
	
		void Awake ()
		{
			instance = this;
		}
		
		void Update ()
		{
			if(OnRun != null)
			{
				OnRun();
			}
		}
		
		public static CronJob GetInstance()
		{
			return instance;
		}
	}
}


