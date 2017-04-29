using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Lib.Resource
{
    public class KTabLineResourceVersion : AKTabFileObject
    {
        public string url = null;
        public int version = 0;
        public int size = 0;

        public override string getKey()
        {
            return url;
        }
    }

    public class KResourceVersion
    {
        public const int INVALID_VERSION = -1;
        private static KResourceVersion m_resourceVersion = null;
        private const string versionFilename = "ClientSettings/versions";
        private KTabFile<KTabLineResourceVersion> tabResourceVersion = null;
        private bool bIsCompleted = false;

        public KResourceVersion(
            KConfigFileManager.LoadAllComplete LoadFileComplete
        )
        {
            tabResourceVersion = new KTabFile<KTabLineResourceVersion>(
                versionFilename,
                new KTabFile<KTabLineResourceVersion>.LoadDataCompleteDelegate(LoadComplete),
                new KTabFile<KTabLineResourceVersion>.LoadDataCompleteDelegate(LoadFileComplete)
            );
        }

        public KResourceVersion()
            : this(null)
        {
        }

        private void LoadComplete()
        {
            bIsCompleted = true;
        }

        public int GetVersion(string url)
        {
            if (!bIsCompleted)
                return INVALID_VERSION;
            KTabLineResourceVersion tabLineResourceVersion = tabResourceVersion.getData(url);
            if (tabLineResourceVersion == null)
                return INVALID_VERSION;

            return tabLineResourceVersion.version;
        }

        public Dictionary<string, KTabLineResourceVersion> GetAllVersion()
        {
            return tabResourceVersion.getAllData();
        }
    }
}
