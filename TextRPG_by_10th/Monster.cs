using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    internal class Monster : Creature
    {
        static public Monster[] monsterList;

        int ClearGold = 0;
        
        public Monster(string name,float health, float attackPower,float defense, int lv, int clearGold)
                        : base(name, health, attackPower, defense, lv)
        {
            ClearGold = clearGold;
        }
    }

    class MonsterType1 : Monster
    {
        public MonsterType1(string name, float health, float attackPower, float defense, int lv, int clearGold)
                        : base(name, health, attackPower, defense,lv, clearGold)
        {

        }
    }
}
