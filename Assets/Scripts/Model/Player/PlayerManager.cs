using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model.Scene;
using Assets.Scripts.Lib.Loader;
using Assets.Scripts.Data;
using Assets.Scripts.Manager;
using Assets.Scripts.Define;
using Assets.Scripts.Utils;
using Assets.Scripts.Logic.Item;
using Assets.Scripts.Logic.Bag;
using Assets.Scripts.View.Scene;
using Assets.Scripts.Logic.Scene;
using Assets.Scripts.Logic.Scene.SceneObject;

namespace Assets.Scripts.Model.Player
{
    class PlayerManager
    {
        private MajorPlayer majorPlayer;

        private bool firstLoginSign = true;//第一次登陆标识

        private Dictionary<ulong, Player> players = new Dictionary<ulong, Player>();

        public PlayerManager()
        {
            majorPlayer = new MajorPlayer();
        }

        public void AddPlayer(ulong playerID, string szPlayerName, byte byGender)
        {
            if (players.ContainsKey(playerID))
                return;

            Player newPlayerInfo = new Player();
            newPlayerInfo.PlayerID = playerID;
            newPlayerInfo.PlayerName = szPlayerName;
            newPlayerInfo.Gender = (KGender)byGender;
            players.Add(newPlayerInfo.PlayerID, newPlayerInfo);
        }

        public void AddPlayer(ulong id, Player player)
        {
            players.Add(id, player);
        }

        public Player GetPlayer(ulong id)
        {
            if (id == majorPlayer.PlayerID)
            {
                return majorPlayer;
            }
            Player player;
            players.TryGetValue(id, out player);
            return player;
        }

        public MajorPlayer MajorPlayer { get { return majorPlayer; } }

        private static PlayerManager instance;
        public static PlayerManager GetInstance()
        {
            if (instance == null)
            {
                instance = new PlayerManager();
            }
            return instance;
        }
    }
}
