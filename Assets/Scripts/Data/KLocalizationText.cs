using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KLocalizationText : AKTabFileObject
    {
        public string key;
        public string value;
        public string desc;


		
		public override string getKey()
		{
            return key;
		}
		
		public void onComplete()
		{
		}

        public void onAllComplete()
		{
		}

        public static string getValueByKey(string key)
        {
            if (KConfigFileManager.GetInstance().localizationTexts == null)
                return "";

            KLocalizationText localizationText = KConfigFileManager.GetInstance().localizationTexts.getData(key) as KLocalizationText;
            if (localizationText == null)
                return "";

            return localizationText.value;
        }

        public static string getFormatText(string key, params string[] paramList)
        {
            string text = getValueByKey(key);
            return String.Format(text, paramList);
        }
    }
}
