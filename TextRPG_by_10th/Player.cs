using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    internal class Player : Creature
    {
        public int Gold { get; set; }

        public int experience = 0;
        public int[] maxexperience = { 10, 35, 65, 100 };
        public Player(string name, float health, float attackPower, float defense,  int gold, int lv) 
                        : base(name, health, attackPower, defense, lv)
        {
            Gold = gold;
        }

        public void AddGold(int addGold) // Monster 클리어 골드
        {
            Gold += addGold;
        }

        public void AddExperience(Monster[] monster, int lenght) // 몬스터의 레벨, 몬스터의 등장 숫자(등장 랜덤 값)
        {
            for (int i = 0; i < lenght; i++)
            {
                experience += monster[i].Lv; // 여기서 Lv은 해당 몬스터의 레벨

                if(experience > maxexperience[Lv]) // 여기서 Lv은 캐릭터의 레벨
                {
                    LevelUp();
                }
            }
        }

        public void LevelUp() 
        {
            Lv++;
            experience = 0;

            AttackPower += 0.5f;
            Defense += 1.0f;
            Console.WriteLine("레벨이 증가하였습니다");
        }
    }
    class Warrior : Player 
    {
        public string Job = "전사";

        public Warrior(string name, float health, float attackPower, float defense, int gold, int lv)
                        : base(name, health, attackPower, defense, gold, lv)
        {
            
        }
    }
    class Assassin : Player
    {
        public string Job = "도적";

        public Assassin(string name, float health, float attackPower, float defense, int gold, int lv)
                        : base(name, health, attackPower, defense, gold, lv)
        {

        }
    }
    class Archer : Player
    {
        public string Job = "궁수";

        public Archer(string name, float health, float attackPower, float defense, int gold, int lv)
                        : base(name, health, attackPower, defense, gold, lv)
        {

        }
    }
}
