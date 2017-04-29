using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Manager
{
    class TagManager
    {
        public string NavMeshTag = "NavMesh";

        public string SceneObjectTag = "SceneObject";

        private static TagManager instance;
        public static TagManager GetInstance()
        {
            if (instance == null)
            {
                instance = new TagManager();
            }
            return instance;
        }
    }
}
