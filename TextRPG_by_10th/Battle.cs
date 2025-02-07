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

        List<ConsumableItem> consumableItems = Inventory.consumableList;

        void SummonMonsters()
        {
            int monsterCount = random.Next(1,5);
            monsters = new Monster[monsterCount];

            for(int i = 0; i < monsterCount; i++)
            {
                int monsterIndex = random.Next(0,Monster.monsterList.Length);
                monsters[i] = Monster.monsterList[monsterIndex];
            }
        }

        void PlayerTurn()
        {
            Console.WriteLine("Battle!\n");
            for(int i = 0;i < monsters.Length;i++)
            {
                Console.WriteLine($"Lv.{monsters[i].Lv} {monsters[i].Name} HP {monsters[i].Health}");
            }

            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Lv} {player.Name} (직업)");
            //향후에 직업 정보 추가되면 수정
            Console.WriteLine("무엇을 할까?");
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬 사용");
            Console.WriteLine("3. 아이템 사용");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //대상 선택으로 넘어감
                    Monster targetMonster = SelectTarget();
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

        Monster SelectTarget()
        {
            Monster targetMonster = null;
            string input = Console.ReadLine();

            if (int.Parse(input) < monsters.Length)
            {
                targetMonster = monsters[int.Parse(input)];
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                SelectTarget();
            }

            return targetMonster;
        }

        void Attack(Creature attacker, Creature target)
        {
            target.TakeDamage(attacker.AttackPower);
        }

    }
}
