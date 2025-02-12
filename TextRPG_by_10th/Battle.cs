


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TextRPG_by_10th
{
    public class Battle
    {
        //상태이상 목록
        public enum DebuffType
        {
            NONE,
            POISON,
            FROST,
            PARALYZE
        }
        //추가 효과 목록
        public enum BuffType
        {
            NONE,
            TOXIC_WEAPON,
        }

        //디버프 적용받는 객체 정보를 저장하는 리스트
        List<DebuffData> debuffDatas = new List<DebuffData>();
        List<BuffData> buffDatas = new List<BuffData>();

        Random random = new Random();

        Player player;
        Inventory inventory;
        //소환된 몬스터 배열
        Monster[] monsters;
        //죽은 몬스터 수를 저장
        int deadCount = 0;
        //전투 종료 판단 변수
        bool battleEnd = false;
        //현재 던전 레벨
        int stageLevel = 0;

        //전체 전투 과정
        public void BattleProcess(Player player, Inventory inventory)
        {
            Console.Clear();
            Console.WriteLine("Battle!\n");

            //StartScene에서 생성한 Player를 입력 받음
            this.player = player;
            this.inventory = inventory;

            //전투할 몬스터 배열이 비어있을 경우 몬스터를 소환
            if (monsters == null)
            {

                SummonMonsters(SelectedStage());

            }

            //전투가 끝날 때까지 아래 과정을 반복
            while (!battleEnd)
            {
                //플레이어 공격 차례
                PlayerTurn();
                //몬스터 공격 차례
                MonsterTurn();
                //상태이상효과 지속상태 확인
                DebuffCheck();
                //추가 효과 지속 상태 확인
                BuffCheck();
                //승리조건 확인
                //VictoryCondition();

                Thread.Sleep(1000);
            }

            //전투 종료시 마을로 복귀
            Console.WriteLine("전투 종료");
            //상태이상에 속해있던 객체들의 상태이상을 해제
            foreach (DebuffData debuffData in debuffDatas)
            {
                debuffData.statusTarget.debuffType = DebuffType.NONE;
            }
            //상태이상 정보 목록을 초기화
            debuffDatas.Clear();
            //이후 진입시 전투가 시작되도록 함
            battleEnd = false;
            //몬스터 소환 배열을 초기화
            monsters = null;
            //Scene을 마을로 변경
            SceneManager.instance.currentScene = SceneManager.Scene.Town;

        }

        //몬스터 소환

        void SummonMonsters(int stage)

        {
            //1~4마리의 몬스터가 소환됨
            int monsterCount = random.Next(1, 5);
            //소환될 몬스터 숫자에 따라 배열 크기를 설정
            monsters = new Monster[monsterCount];
            //해당 배열에 Monster 클래스에서 몬스터를 불러와 소환
            for (int i = 0; i < monsterCount; i++)
            {
                // 선택된 스테이지에 해당하는 몬스터 리스트를 가져옴
                List<MonsterType> stageMonsterTypes = Monster.StageMonsters[stage];
                // 해당 스테이지의 랜덤한 몬스터 인덱스 선택
                int monsterIndex = random.Next(0, stageMonsterTypes.Count);

                MonsterType selectedType = stageMonsterTypes[monsterIndex];

                monsters[i] = Monster.LoadMonster[stage - 1](selectedType);
            }
        }

        int SelectedStage()
        {
            Console.Clear();
            int stage;
            string[] stageStr = new string[] { "술", "던전", "심해", "설산", "화산" };
            Console.WriteLine("스테이지를 선택해주세요");
            string[] stageNames =
            { player.Dungeon_Level >= 1 ? "스테이지 1 : 숲" : "스테이지 1 : 숲".ColorText(ConsoleColor.Red),
              player.Dungeon_Level >= 2 ? "스테이지 2 : 던전" : "스테이지 2 : 던전".ColorText(ConsoleColor.Red),
              player.Dungeon_Level >= 3 ? "스테이지 3 : 심해" : "스테이지 3 : 심해".ColorText(ConsoleColor.Red),
              player.Dungeon_Level >= 4 ? "스테이지 4 : 설산" : "스테이지 4 : 설산".ColorText(ConsoleColor.Red),
              player.Dungeon_Level >= 4 ? "스테이지 5 : 화산" : "스테이지 5 : 화산".ColorText(ConsoleColor.Red),
            };
            foreach (string stageName in stageNames)
            {
                Console.WriteLine(stageName);
            }

            while (true)
            {
                string input = Console.ReadLine();
                AudioManager.Instance.PlaySFX("click");
                if (int.TryParse(input, out stage) && stage >= 1 && stage <= 5)
                {
                    //플레이어 던전 레벨에 맞지 않는 던전 입장 불가
                    if(stage <= player.Dungeon_Level)
                    {
                        stageLevel = stage;
                        return stage;
                    }  
                    else
                    {
                        Console.WriteLine("이 던전은 너무 어려워서 갈 수 없다.");
                    }
                        
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 1부터 5까지의 스테이지 번호를 입력해주세요.");
                }
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
                Console.Write(monsterStatus);
                //몬스터가 상태이상에 속해있을 경우 해당 상태이상을 표시
                if (monsters[i].debuffType != DebuffType.NONE && !monsters[i].isDie)
                {
                    Console.Write($" [{monsters[i].debuffType.ToString()}]");
                }
                Console.WriteLine();
            }

            //Player 정보 표시
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Lv} {player.Name} ({player.playerJob.ToString()}) ");
            string playerStatus = player.isDie ? "[Dead]" : $"HP {player.Health}";
            Console.WriteLine(playerStatus + "\n");
        }
        //플레이어 턴에서 이루어지는 과정
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
            AudioManager.Instance.PlaySFX("click");

            switch (input)
            {
                case "1":
                    //대상 선택으로 넘어감
                    Monster targetMonster = null;
                    SelectTarget(ref targetMonster);
                    //타겟이 선정되면 공격
                    if (targetMonster != null)
                    {
                        //몬스터에게 일반공격
                        bool hasHit = Attack(player, targetMonster);

                        if (!hasHit)
                            break;

                        //독 바름 상태일 경우 상대에게 독 적용
                        ToxicWeapon(player, targetMonster);

                        //무기 속성에 따라 상태이상 효과 적용
                        switch (Inventory.GetEquippedWeaponEffect().specialEffect)
                        {
                            case 1:
                                ApplyDebuff(player, targetMonster, DebuffType.POISON);
                                break;
                            case 2:
                                ApplyDebuff(player, targetMonster, DebuffType.FROST);
                                break; ;
                            case 3:
                                ApplyDebuff(player, targetMonster, DebuffType.PARALYZE);
                                break;

                        }
                        //타겟이 PARALYZE 상태인 경우 추가 데미지 부여
                        ParalyzeDamage(targetMonster);
                        //공격한 몬스터의 사망 여부 체크
                        DeathCheck(targetMonster);
                    }
                    break;

                case "2":
                    //스킬 선택 창으로 넘어감
                    //대상 선택으로 넘어감
                    targetMonster = null;
                    SelectTarget(ref targetMonster);
                    //타겟이 선정되면 공격
                    if (targetMonster != null)
                    {
                        AudioManager.Instance.PlaySFX("skill");
                        //상태이상 테스트를 위해 임시로 사용
                        //ApplyDebuff(player, targetMonster, DebuffType.FROST);
                        ApplyDebuff(player, targetMonster, DebuffType.PARALYZE);
                        //ApllyBuff(player, BuffType.TOXIC_WEAPON);
                        DeathCheck(targetMonster);
                    }
                    break;
                case "3":
                    //아이템 사용 창으로 넘어감
                    //ApllyBuff(player, BuffType.TOXIC_WEAPON);
                    string useItem = inventory.UseConsumableScene();
                    if (useItem == "poison")
                        ApllyBuff(player, BuffType.TOXIC_WEAPON);
                        
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                    PlayerTurn();
                    break;
            }

            VictoryCondition();
        }
        //몬스터 턴에서 이루어지는 과정
        void MonsterTurn()
        {
            //소환된 몬스터들마다 각자 공격을 실행
            for (int i = 0; i < monsters.Length; i++)
            {
                //몬스터가 살아있고, FROST 상태이상이 아닐 경우 공격을 실행
                if (!monsters[i].isDie && monsters[i].debuffType != DebuffType.FROST)
                {
                    Attack(monsters[i], player);
                }
                //앞선 조건에 해당할 경우 공격 불가
                else
                {
                    Console.Clear();
                    ShowBattleInfo();
                    Console.WriteLine($"{monsters[i].Name}은(는) 공격할 수 없다.");
                    Thread.Sleep(1000);
                }

                //공격으로 플레이어가 사망하면 전투가 종료됨
                if (player.isDie)
                {
                    AudioManager.Instance.PlaySFX("game_over");
                    battleEnd = true;
                    Console.WriteLine($"Lv.{player.Lv} {player.Name}이(가) 패배했습니다.");
                    break;
                }
            }
        }
        //공격할 타겟을 선택
        void SelectTarget(ref Monster targetMonster)
        {
            //선택할 수 있는 몬스터 목록을 보여줌
            ShowBattleInfo();

            Console.WriteLine("공격할 대상을 선택하세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();
            AudioManager.Instance.PlaySFX("click");
            int int_input;
            bool isRightInput = int.TryParse(input, out int_input);

            if (int_input <= monsters.Length || !isRightInput)
            {
                targetMonster = monsters[int.Parse(input) - 1];

                if (targetMonster.isDie)
                {
                    Console.WriteLine("잘못된 입력입니다");
                    targetMonster = null;
                    SelectTarget(ref targetMonster);
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                Thread.Sleep(500);
                SelectTarget(ref targetMonster);
            }
        }
        //일반 공격 수행
        bool Attack(Creature attacker, Creature target)
        {
            ShowBattleInfo();
            Console.WriteLine($"{attacker.Name}의 {target.Name} 공격");

            bool hasHit = false;

            Random random = new Random();
            float hitRoll = (float)random.NextDouble();  // 0.0 ~ 1.0 사이 랜덤 값

            // 공격자의 명중률과 대상의 회피율을 비교하여 공격 성공 여부 결정
            if (hitRoll < attacker.HitChance - target.DodgeChance)
            {
                float damage = CalculateDamage(attacker, target);
                float previousHp = target.Health;
                target.TakeDamage(damage);

                AudioManager.Instance.PlaySFX("hit");
                Console.WriteLine($"명중! {attacker.Name}이(가) {target.Name}에게 {damage} 데미지를 입혔다.");
                Console.WriteLine($"Lv.{target.Lv} {target.Name} | HP {previousHp} → {target.Health}");
                hasHit = true;
            }
            else
            {
                Console.WriteLine($"{attacker.Name}의 공격이 빗나갔다!");
                hasHit = false;
            }

            Thread.Sleep(1000);
            return hasHit;
        }
        //공격 데미지 계산(치명타, 10% 오차 적용)
        float CalculateDamage(Creature attacker, Creature target)
        {
            Random random = new Random();
            float critRoll = (float)random.NextDouble();  // 0.0 ~ 1.0 사이 랜덤 값
            bool isCritical = critRoll < attacker.CritChance;

            float baseDamage = MathF.Max(0, attacker.AttackPower - target.Defense);
            float finalDamage = isCritical ? baseDamage * 2.0f : baseDamage; // 크리티컬 시 2배 피해

            if (isCritical)
            {
                AudioManager.Instance.PlaySFX("critical");
                Console.WriteLine($"크리티컬 히트! {attacker.Name}이(가) {"치명적인 일격".ColorText(ConsoleColor.Red)}을 가했다!");
            }

            //10%의 데미지 오차 추가
            float damageVariance = MathF.Ceiling(finalDamage * 0.1f);
            finalDamage = (float)random.Next((int)(finalDamage - damageVariance), (int)(finalDamage + damageVariance));

            return finalDamage;
        }

        //공격한 몬스터의 사망 확인
        void DeathCheck(Monster targetMonster)
        {
            if (targetMonster.isDie)
            {
                AudioManager.Instance.PlaySFX("money");
                player.AddGold(targetMonster.GetClrearGold());
                deadCount++;
            }

            VictoryCondition();
        }

        //몬스터 처치 검사
        void VictoryCondition()
        {
            if (deadCount == monsters.Count())
            {
                battleEnd = true;
                AudioManager.Instance.PlaySFX("win");
                Console.WriteLine($"Lv.{player.Lv} {player.Name}이(가) 승리했습니다.");
                Console.WriteLine($"{deadCount}마리의 몬스터를 처치했다.");
                //최대 클리어 던전 레벨과 현재 던전 레벨이 같으면 던전 레벨을 증가
                if(player.Dungeon_Level == stageLevel && player.Dungeon_Level < 5)
                {
                    int previousDungeonLevel = player.Dungeon_Level;
                    player.Dungeon_Level++;
                    Console.WriteLine($"이제 더 어려운 던전을 이용할 수 있다 ({previousDungeonLevel} → {player.Dungeon_Level})");
                }

                deadCount = 0;
                Thread.Sleep(1000);
            }
        }

        void DebuffCheck()
        {
            //전투가 종료된 상황에서는 확인하지 않음
            if (battleEnd)
                return;
            //상태이상인 객체가 없을 때는 확인하지 않음
            if (debuffDatas.Count == 0)
                return;

            ShowBattleInfo();
            Console.WriteLine("<상태이상 정보>\n");

            //상태 이상 적용이 끝난 객체를 저장
            List<DebuffData> endDebuffs = new List<DebuffData>();

            foreach (DebuffData debuffData in debuffDatas)
            {
                //아직 지속기간이 남았고, 대상이 살아있는 상태이상을 적용
                if (debuffData.turns > 0 && !debuffData.statusTarget.isDie)
                {
                    switch (debuffData.debuff)
                    {
                        case DebuffType.POISON:
                            Poision(debuffData.statusSource, debuffData.statusTarget);
                            break;
                        case DebuffType.FROST:
                            Frost(debuffData.statusTarget);
                            break;
                        case DebuffType.PARALYZE:
                            Paralyze(debuffData.statusTarget);
                            break;
                    }
                    Console.WriteLine($"남은 지속 기간 : {debuffData.turns}\n");
                    //효과를 적용하고 지속 기간을 1 감소
                    debuffData.turns--;
                }
                else
                {
                    //상태이상 지속이 끝난 객체의 상태를 NONE으로 변경
                    debuffData.statusTarget.debuffType = DebuffType.NONE;
                    //상태이상 지속이 끝난 목록에 추가
                    endDebuffs.Add(debuffData);
                }
            }
            //지속이 끝난 객체를 상태이상 목록에서 제거
            foreach (DebuffData endDebuff in endDebuffs)
            {
                debuffDatas.Remove(endDebuff);
            }
            endDebuffs.Clear();

        }

        //입력을 다르게 하여 상태이상을 적용할 수 있음
        void ApplyDebuff(Creature source, Creature target, DebuffType debuffType)
        {
            if (target.debuffType == DebuffType.NONE)
            {
                debuffDatas.Add(new DebuffData(source, target, debuffType, 3));
                Console.WriteLine($"{target.Name}을(를) {target.debuffType.ToString()} 상태로 만들었다.");
            }
            //여러 상태이상에 중복 적용은 불가능
            else
                Console.WriteLine($"{target.Name}은(는) 이미 {target.debuffType.ToString()} 상태이다.");

            Thread.Sleep(1000);

        }
        //독 상태이상 데미지 적용
        void Poision(Creature attacker, Creature target)
        {
            float previousHp = target.Health;
            Console.WriteLine("독 데미지 적용");
            target.TakeDamage(MathF.Ceiling(attacker.AttackPower * 0.1f));
            Console.WriteLine($"Lv.{target.Lv} {target.Name}");
            Console.WriteLine($"HP {previousHp}->{target.Health}");

            //독 데미지에 의해 사망할 경우 사망 처리
            if (target.creatureType == Creature.CreatureType.Monster)
            {
                DeathCheck((Monster)(target));
            }

            Thread.Sleep(1000);
        }
        //빙결 상태이상 표시
        void Frost(Creature target)
        {
            Console.WriteLine($"{target.Name}은(는) 빙결상태라 행동할 수 없다.");

            Thread.Sleep(1000);
        }
        //감전 상태이상 표시
        void Paralyze(Creature target)
        {
            Console.WriteLine($"{target.Name}은(는) 감전 데미지를 받고 있다.");

            Thread.Sleep(1000);
        }
        //피격시 감전 상태이상 데미지 부여
        void ParalyzeDamage(Creature target)
        {
            //대상이 감전 상태이면서 살아있을 때 부여
            if (target.debuffType != DebuffType.PARALYZE || target.isDie)
                return;

            Creature attacker = null;
            //감전 상태이상을 건 객체의 공격력에 따른 데미지를 부여
            foreach (DebuffData findingData in debuffDatas)
            {
                if (findingData.statusTarget == target)
                    attacker = findingData.statusSource;
            }

            float previousHp = target.Health;
            Console.WriteLine("감전 데미지 적용");
            //감전 데미지 적용
            target.TakeDamage(MathF.Ceiling(attacker.AttackPower * 0.2f));
            Console.WriteLine($"Lv.{target.Lv} {target.Name}");
            Console.WriteLine($"HP {previousHp}->{target.Health}");

            Thread.Sleep(1000);
        }

        //부가 효과를 적용
        void ApllyBuff(Creature target, BuffType buffType)
        {
            foreach (BuffData findingData in buffDatas)
            {
                if (findingData.statusTarget == target)
                {
                    Console.WriteLine($"{target.Name}은(는) 이미 {findingData.buff.ToString()} 상태이다.");
                    return;
                }

            }
            buffDatas.Add(new BuffData(target, buffType, 5));
            Console.WriteLine($"{target.Name}에 {buffType.ToString()} 을(를) 적용했다.");

        }

        void BuffCheck()
        {
            //전투가 종료된 상황에서는 확인하지 않음
            if (battleEnd)
                return;

            //상태이상인 객체가 없을 때는 확인하지 않음
            if (buffDatas.Count == 0)
                return;

            Console.WriteLine("\n<부가효과 정보>");

            //상태 이상 적용이 끝난 객체를 저장하는 배열
            List<BuffData> endBuffs = new List<BuffData>();

            foreach (BuffData buffData in buffDatas)
            {
                //아직 지속기간이 남았고, 대상이 살아있는 상태이상을 적용
                if (buffData.turns >= 0 && !buffData.statusTarget.isDie)
                {
                    Console.WriteLine($"{buffData.statusTarget.Name}의 {buffData.buff.ToString()} 효과");
                    Console.WriteLine($"남은 지속 기간 : {buffData.turns}\n");
                    //효과를 적용하고 지속 기간을 1 감소
                    buffData.turns--;
                }
                else
                {
                    //상태이상 지속이 끝난 목록에 추가
                    endBuffs.Add(buffData);
                }
            }
            //지속이 끝난 객체를 상태이상 목록에서 제거
            foreach (BuffData endBuff in endBuffs)
            {
                buffDatas.Remove(endBuff);
            }
            endBuffs.Clear();

            Thread.Sleep(3000);
        }
        //독 무기 버프 상태일 경우 공격시 상대에게 독 상태이상 적용
        void ToxicWeapon(Creature attacker, Creature target)
        {
            foreach (BuffData findingData in buffDatas)
            {
                if (findingData.statusTarget == attacker && findingData.buff == BuffType.TOXIC_WEAPON)
                    ApplyDebuff(attacker, target, DebuffType.POISON);

            }
        }


        //상태이상 정보를 저장하는 형식
        class DebuffData : Battle
        {
            public Creature statusSource;
            public Creature statusTarget;
            public DebuffType debuff;
            public int turns;

            public DebuffData(Creature statusSource, Creature statusTarget, DebuffType debuff, int turns)
            {
                this.statusSource = statusSource;
                this.statusTarget = statusTarget;
                this.debuff = debuff;
                this.turns = turns;

                statusTarget.debuffType = debuff;
            }
        }

        class BuffData : Battle
        {
            public Creature statusTarget;
            public BuffType buff;
            public int turns;

            public BuffData(Creature statusTarget, BuffType buff, int turns)
            {
                this.statusTarget = statusTarget;
                this.buff = buff;
                this.turns = turns;
            }
        }
    }
}

