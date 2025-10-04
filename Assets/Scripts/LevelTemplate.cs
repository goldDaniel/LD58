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

        public List<EnemyTemplate> Enemies = new List<EnemyTemplate>();
    }
}
