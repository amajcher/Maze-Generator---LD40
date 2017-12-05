using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Gameplay
{
    [System.Serializable]
    public class GameSettings
    {
        public int WinPointsTotal = 5;
        public int Time = 5;

        public bool Active = false;

        public int TimeLeft = 0;
        public int WinPointsCount = 0;




    }
}
