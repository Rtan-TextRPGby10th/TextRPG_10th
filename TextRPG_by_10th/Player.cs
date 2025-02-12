using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_by_10th;

namespace TextRPG_by_10th
{
    // 플레이어의 입력 1, 2, 3 으로 직업 선택
    public enum Job
    {
        전사 = 1,
        도적 = 2,
        궁수 = 3
    }

    public class Player : Creature
    {
        public Job playerJob { get; set; }
        public int Gold { get; set; } // 캐릭터의 보유 골드

        public int experience = 0; // 경험치
        public int[] maxexperience = { 10, 35, 65, 100 }; // 레벨업시 필요한 경험치량

        public float addItemPower;
        public float addItemDefense;
        public float addItemHealth;

        // 캐릭터 생성자, 크리처 생성자를 가져옴
        public Player(string name, float health, float maxHealth, float attackPower, float defense,  int gold, int lv, Job job, float hitChance, float dodgeChance, float critChance) 
                        : base(name, health, attackPower, defense, lv, hitChance, dodgeChance, critChance)
        {
            Gold = gold;
            playerJob = job;
            MaxHealth = maxHealth;
        }

        public void AddGold(int addGold) // Monster 클리어 골드를 가져와 캐릭터 보유 골드 증가 함수
        {

            int previousGold = Gold;
            Gold += addGold;
            Console.WriteLine($"{addGold}G를 획득했다! ({previousGold}->{Gold})");
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
        // 아이템을 착용 및 해제를 통한 방어력 증가
        // Defense : 기본 능력치면서 통합 방어력을 표시
        public float ItemDefense(float itemValue) // itemValue = Invetory.GetTotalDef() 메서드의 반환값을 가져옴
        {
            Defense -= addItemDefense; // 호출되면 합쳐진 값에서 현재 착용중인 장비 능력치를 빼준다.
            addItemDefense = 0.0f;  // 호출 되면 장비 능력치를 0으로 초기화
            addItemDefense += itemValue; 
            return Defense += addItemDefense;
        }
        // 아이템을 착용하면 공격력 증가

        public float ItemAtkPower(float itemValue)
        {
            AttackPower -= addItemPower;
            addItemPower = 0.0f;
            addItemPower += itemValue;
            return AttackPower += addItemPower;
        }
        // 아이템을 착용하면 체력 증가
        public float ItemHealth(float itemValue)
        {
            MaxHealth -= addItemHealth;
            addItemHealth = 0.0f;
            addItemHealth += itemValue;
            MaxHealth += addItemPower;
            return MaxHealth;
        }
    }

    // 직업 클래스
    public class Warrior : Player 
    {
        public Warrior(string name)
                        : base(name, 100.0f, 100.0f, 10.0f, 20.0f, 0, 1, Job.전사, 0.9f, 0.05f, 0.2f) // 명중 90%, 회피 5% 치명타율 20%
        {

        }
    }
    public class Assassin : Player
    {
        public Assassin(string name)
                        : base(name, 75.0f, 75.0f, 15.0f, 15.0f, 0, 1, Job.도적, 0.85f, 0.2f, 0.25f) // 명중 85%, 회피 20% 치명타율 25%
        {

        }
    }
    public class Archer : Player
    {
        public Archer(string name)
                        : base(name, 50.0f, 50.0f, 20.0f, 10.0f, 0, 1, Job.궁수, 0.8f, 0.1f, 0.3f) // 명중 80%, 회피 10% 치명타율 30%
        {

        }
    }
 }

//// 이름 및 직업 선택 예시
//Console.WriteLine("이름을 정해주세요");
//string nameInput = Console.ReadLine();
//Console.WriteLine("직업을 선택해 주세요");
//Console.WriteLine("1. 전사, 2. 도적, 3. 궁수");
//Console.Write(">> ");
//int jobInput;
//Player player = null;
//if (int.TryParse(Console.ReadLine(), out jobInput) && 0 < jobInput && jobInput < 4)
//{
//    Job selectedJob = (Job)(jobInput);
//    if (jobInput > 0 && jobInput < 4)
//    {
//        if (jobInput == 1)
//        {
//            player = new Warrior(nameInput, 100, 50, 20, 1000, 1, selectedJob);
//            Console.WriteLine($"{player.Name},  {player.playerJob}");
//        }
//        else if (jobInput == 2)
//        {
//            player = new Assassin(nameInput, 100, 50, 20, 1000, 1, selectedJob);
//            Console.WriteLine($"{player.Name},  {player.playerJob}");
//        }
//        else if (jobInput == 3)
//        {
//            player = new Archer(nameInput, 100, 50, 20, 1000, 1, selectedJob);
//            Console.WriteLine($"{player.Name},  {player.playerJob}");
//        }
//    }
//}

//// 장비 장착 예시
//float itemvalue = 10.0f;
//player.ItemAtkPower(itemvalue);
//Console.WriteLine($"{player.Name},  {player.playerJob}, {player.AttackPower}");
