using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
    public class LevelTemplate : ScriptableObject
    {
        //Level Properties
        public string LevelDescription = string.Empty;
        public int LevelNumber = -1;
        public CardType Type = CardType.None;
        public int randomReward = 0;
        public int odinReward = 0;
        public int mickiReward = 0;
        public int anubisReward = 0;
        public int reaperReward = 0;
        public int fatesReward = 0;

        public List<EnemyTemplate> Enemies = new List<EnemyTemplate>();
    }
}
