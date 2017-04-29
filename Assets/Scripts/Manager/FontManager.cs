using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class FontManager
    {
        public delegate void LoadComplete();
        public event LoadComplete OnLoadComplete; 
        public static string DefaultFontName = "msyh";

        private string fontName;

        public UIFont font;

        public void LoadDefault()
        {
            fontName = DefaultFontName;
            Load(fontName);
        }

        public void Load(string fontName)
        {
            AssetLoader.GetInstance().Load(URLUtil.GetResourceLibPath() + "Font/" + fontName + ".res", Font_OnLoadComplete, AssetType.BUNDLER);
        }

        public void Font_OnLoadComplete(AssetInfo info)
        {
            GameObject go = (GameObject)Object.Instantiate(info.bundle.mainAsset);
            Object.DontDestroyOnLoad(go);
            font = go.GetComponent<UIFont>();
            if (OnLoadComplete != null)
            {
                OnLoadComplete();
            }
        }

        public UIFont Font
        {
            get
            {
                return font;
            }
        }

        public string CurrentFont
        {
            set
            {
                if (fontName != value)
                {
                    fontName = value;
                    Load(fontName);
                }
            }
            get
            {
                return fontName;
            }
        }

        public static FontManager instance;
        public static FontManager GetInstance()
        {
            if (instance == null)
            {
                instance = new FontManager();
            }
            return instance;
        }
    }
}
