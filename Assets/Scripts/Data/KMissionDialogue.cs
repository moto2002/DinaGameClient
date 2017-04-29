using System;
using System.Collections.Generic;
using Assets.Scripts.Lib.Resource;

namespace Assets.Scripts.Data
{
    public class KMissionDialogue : AKTabFileObject
    {
        public int DialogueID;
        public string Content;

        public override string getKey()
        {
            return DialogueID.ToString();
        }
    }
}
