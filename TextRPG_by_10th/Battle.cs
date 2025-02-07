using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TextRPG_by_10th.SceneManager;

namespace TextRPG_by_10th
{
    public class Battle
    {
        Random random = new Random();

        Player player;
        Monster[] monsters;

        List<Monster> monsterList = new List<Monster>();

        List<ConsumableItem> consumableItems = Inventory.consumableList;

        int deadCount = 0;
        bool battleEnd = false;

        public void BattleProcess(Player player)
        {
            Console.Clear();
            Console.WriteLine("Battle!\n");

            this.player = player;

            LoadMonsterList();

            if(monsters == null)
            {
                SummonMonsters();
            }

            while (!battleEnd)
            {
                PlayerTurn();

                foreach (Monster monster in monsters)
                {
                    if (monster.isDie)
                    {
                        deadCount++;
                    }
                }

                if (deadCount == monsters.Count()) battleEnd = true;

            }

            Console.WriteLine("전투 종료");
            SceneManager.instance.currentScene = Scene.Town;
            
        }

        void LoadMonsterList()
        {
            monsterList.Add(new MonsterType1("test1", 1f, 1f, 1f, 1, 1));
            monsterList.Add(new MonsterType1("test2", 1f, 1f, 1f, 1, 1));
            monsterList.Add(new MonsterType1("test3", 1f, 1f, 1f, 1, 1));
            monsterList.Add(new MonsterType1("test4", 1f, 1f, 1f, 1, 1));

        }

        void SummonMonsters()
        {
            int monsterCount = random.Next(1,5);
            monsters = new Monster[monsterCount];

            for(int i = 0; i < monsterCount; i++)
            {
                int monsterIndex = random.Next(0,monsterList.Count);
                monsters[i] = monsterList[monsterIndex];
            }
        }

        void ShowBattleInfo()
        {
            Console.Clear();

            for (int i = 0; i < monsters.Length; i++)
            {
                Console.WriteLine($"Lv.{monsters[i].Lv} {monsters[i].Name} HP {monsters[i].Health}");
            }

            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Lv} {player.Name} ({player.playerJob.ToString()})\n");
        }

        void PlayerTurn()
        {            
            ShowBattleInfo();
            Console.WriteLine("무엇을 할까?");
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬 사용");
            Console.WriteLine("3. 아이템 사용");
            Console.Write(">> ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //대상 선택으로 넘어감
                    Monster targetMonster = null;
                    SelectTarget(ref targetMonster);
                    //타겟이 선정되면 공격
                    if(targetMonster != null)
                        Attack(player, targetMonster);
                    break;
                case "2":
                    //스킬 선택 창으로 넘어감
                    break;
                case "3":
                    //아이템 사용 창으로 넘어감
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                    break;
            }
        }

        void SelectTarget(ref Monster targetMonster)
        {
            ShowBattleInfo();

            Console.WriteLine("공격할 대상을 선택하세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();

            if (int.Parse(input)+1 < monsters.Length)
            {
                targetMonster = monsters[int.Parse(input)+1];
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
            }
        }

        void Attack(Creature attacker, Creature target)
        {
            ShowBattleInfo();

            Console.WriteLine($"{attacker.Name}의 {target.Name} 공격");
            float previousHp = target.Health;
            target.TakeDamage(attacker.AttackPower);
            Console.WriteLine($"Lv.{target.Lv} {target.Name} HP {previousHp}->{target.Health}");
        }

    }
}
