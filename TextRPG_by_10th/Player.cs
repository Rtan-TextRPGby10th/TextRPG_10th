using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    // 플레이어의 입력 1, 2, 3 으로 직업 선택
    enum Job
    {
        전사 = 1,
        도적 = 2,
        궁수 = 3
    }

    internal class Player : Creature
    {
        public Job playerJob { get; set; }
        public int Gold { get; set; } // 캐릭터의 보유 골드

        public int experience = 0; // 경험치
        public int[] maxexperience = { 10, 35, 65, 100 }; // 레벨업시 필요한 경험치량

        // 캐릭터 생성자, 크리처 생성자를 가져옴
        public Player(string name, float health, float attackPower, float defense,  int gold, int lv, Job job) 
                        : base(name, health, attackPower, defense, lv)
        {
            playerJob = job;
            
        }

        public void AddGold(int addGold) // Monster 클리어 골드를 가져와 캐릭터 보유 골드 증가 함수
        {
            Gold += addGold;
        }

        // 몬스터의 레벨, 몬스터의 등장 숫자(등장 랜덤 값)를 가져와 경험치를 증가시키는 함수
        public void AddExperience(Monster[] monster, int lenght)
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

        // 레벨업 증가 함수
        public void LevelUp() 
        {
            Lv++;
            experience = 0;

            AttackPower += 0.5f;
            Defense += 1.0f;
            Console.WriteLine("레벨이 증가하였습니다");
        }
    }

    // 직업 클래스
    class Warrior : Player 
    {
        public Warrior(string name, float health, float attackPower, float defense, int gold, int lv, Job job)
                        : base(name, health, attackPower, defense, gold, lv, job)
        {

        }
    }
    class Assassin : Player
    {
        public Assassin(string name, float health, float attackPower, float defense, int gold, int lv, Job job)
                        : base(name, health, attackPower, defense, gold, lv, job)
        {

        }
    }
    class Archer : Player
    {
        public Archer(string name, float health, float attackPower, float defense, int gold, int lv, Job job)
                        : base(name, health, attackPower, defense, gold, lv, job)
        {

        }
    }
        //    static void Main(string[] args)
        //{
        //    Console.WriteLine("이름을 정해주세요");
        //    string nameInput = Console.ReadLine();
        //    Console.WriteLine("직업을 선택해 주세요");
        //    Console.WriteLine("1. 전사, 2. 도적, 3. 궁수");
        //    Console.Write(">> ");
        //    int jobInput;
        //    if (int.TryParse(Console.ReadLine(), out jobInput) && 0 < jobInput && jobInput < 4)
        //    {
        //        Job selectedJob = (Job)(jobInput);
        //        if (jobInput > 0 && jobInput < 4)
        //        {
        //            if (jobInput == 1)
        //            {
        //                Warrior warrior = new Warrior(nameInput, 100, 50, 20, 1000, 1, selectedJob);
        //                Console.WriteLine($"{warrior.Name},  {warrior.playerJob}");
        //            }
        //            else if (jobInput == 2)
        //            {
        //                Assassin assassin = new Assassin(nameInput, 100, 50, 20, 1000, 1, selectedJob);
        //                Console.WriteLine($"{assassin.Name},  {assassin.playerJob}");
        //            }
        //            else if (jobInput == 3)
        //            {
        //                Archer archer = new Archer(nameInput, 100, 50, 20, 1000, 1, selectedJob);
        //                Console.WriteLine($"{archer.Name},  {archer.playerJob}");
        //            }
        //        }
        //    }
        }
