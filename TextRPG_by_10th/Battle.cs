using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TextRPG_by_10th
{
    public class Battle
    {
        Random random = new Random();

        Player player;
        Monster[] monsters;

        int deadCount = 0;
        bool battleEnd = false;

        //전체 전투 과정
        public void BattleProcess(Player player)
        {
            Console.Clear();
            Console.WriteLine("Battle!\n");

            //StartScene에서 생성한 Player를 입력 받음
            this.player = player;

            //전투할 몬스터 배열이 비어있을 경우 몬스터를 소환
            if(monsters == null)
            {
                SummonMonsters();
            }

            //전투가 끝날 때까지 아래 과정을 반복
            while (!battleEnd)
            {
                //플레이어 공격 차례
                PlayerTurn();
                //몬스터 사망 체크
                DeathCheck();
                //몬스터 공격 차례
                MonsterTurn();
                Thread.Sleep(1000);
            }

            //전투 종료시 마을로 복귀
            Console.WriteLine("전투 종료");
            battleEnd = false;
            monsters = null;
            SceneManager.instance.currentScene = SceneManager.Scene.Town;
            
        }

        //몬스터 소환
        void SummonMonsters()
        {
            //1~4마리의 몬스터가 소환됨
            int monsterCount = random.Next(1,5);
            //소환될 몬스터 숫자에 따라 배열 크기를 설정
            monsters = new Monster[monsterCount];
            //해당 배열에 Monster 클래스에서 몬스터를 불러와 소환
            for(int i = 0; i < monsterCount; i++)
            {
                int monsterIndex = random.Next(0, 2);

                monsters[i] = Monster.LoadMonster[monsterIndex]();
            }
        }
        //전투 상황을 보여줌
        void ShowBattleInfo()
        {
            Console.Clear();
            //몬스터별 레벨, 이름, 현재 체력 표시
            for (int i = 0; i < monsters.Length; i++)
            {
                Console.Write($"Lv.{monsters[i].Lv} {monsters[i].Name} ");
                string monsterStatus = monsters[i].isDie ? "[Dead]" : $"HP {monsters[i].Health}";
                Console.WriteLine(monsterStatus);
            }

            //Player 정보 표시
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Lv} {player.Name} ({player.playerJob.ToString()}) ");
            string playerStatus = player.isDie ? "[Dead]" : $"HP {player.Health}";
            Console.WriteLine(playerStatus+ "\n");
        }

        void PlayerTurn()
        {           
            //전투 상황을 보여줌
            ShowBattleInfo();
            //선택지 제공
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

        void MonsterTurn()
        {
            for (int i = 0; i<monsters.Length; i++)
            {
                if (!monsters[i].isDie)
                    Attack(monsters[i], player);
                else
                    Console.WriteLine($"{monsters[i].Name}은(는) 죽어있다.");
                
                if (player.isDie)
                {
                    battleEnd = true;
                    break;
                }
            }
        }

        void SelectTarget(ref Monster targetMonster)
        {
            //선택할 수 있는 몬스터 목록을 보여줌
            ShowBattleInfo();

            Console.WriteLine("공격할 대상을 선택하세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();

            if (int.Parse(input) <= monsters.Length)
            {
                targetMonster = monsters[int.Parse(input)-1];

                if(targetMonster.isDie)
                {
                    Console.WriteLine("잘못된 입력입니다");
                    targetMonster = null;
                }
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
            float damage = attacker.AttackPower;
            target.TakeDamage(attacker.AttackPower);
            Console.WriteLine($"{attacker.Name}의 공격!");
            Console.WriteLine($"Lv.{target.Lv} {target.Name}을(를) 맞췄다. [데미지 : {damage}]");
            Console.WriteLine($"Lv.{target.Lv} {target.Name}");
            Console.WriteLine($"HP {previousHp}->{target.Health}");
            Thread.Sleep(1000);
        }

        void DeathCheck()
        {
            foreach (Monster monster in monsters)
            {
                if (monster.isDie)
                {
                    deadCount++;
                }
            }

            if (deadCount == monsters.Count()) battleEnd = true;

            deadCount = 0;
        }

        //test

    }
}
