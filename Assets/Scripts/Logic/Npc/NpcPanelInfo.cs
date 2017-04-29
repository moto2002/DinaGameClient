using System;
using System.Collections.Generic;

namespace Assets.Scripts.Logic.Npc
{
    public class NpcPanelInfo
    {
        public int npcID;
        public string npcName;
        public List<NpcLinkInfo> actionLinks;
        public List<NpcLinkInfo> missionLinks;
        public string content;
    }
}
