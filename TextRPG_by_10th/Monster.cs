namespace TextRPG_by_10th
{
    public enum MonsterType
    {
        슬라임 = 1, 고블린, 거대애벌레, 뿌리정령, 오우거,
        스켈레톤, 임프, 미믹, 망령, 리치,
        심연의전갈, 머맨, 크라켄의촉수, 유령해적, 크라켄,
        설인, 울프, 얼음정령, 눈보라망령, 프로스트드래곤,
        마그마골렘, 불의정령, 살아있는불꽃, 지옥의사냥개, 발록
    }


    public class Monster : Creature
    {
        public static List<Equipment> equip = Equipment.GetEquipmentCatalog(); // 아이템 도감에서 장비를 가져온다.
        public static List<ConsumableItem> consum = ConsumableItem.GetItemCatalog(); // 소모품 도감에서 소모품을 가져온다
        public static List<MiscItem> miscItem = MiscItem.GetMiscCatalog(); // 기타템 도감에서 기타템을 가져온다


        // 각 스테이지에 해당하는 몬스터들 리스트
        public static readonly Dictionary<int, List<MonsterType>> StageMonsters = new Dictionary<int, List<MonsterType>>()
        {
            { 1, new List<MonsterType> { MonsterType.슬라임, MonsterType.고블린, MonsterType.거대애벌레, MonsterType.뿌리정령, MonsterType.오우거 } },
            { 2, new List<MonsterType> { MonsterType.스켈레톤, MonsterType.임프, MonsterType.미믹, MonsterType.망령, MonsterType.리치 } },
            { 3, new List<MonsterType> { MonsterType.심연의전갈, MonsterType.머맨, MonsterType.크라켄의촉수, MonsterType.유령해적, MonsterType.크라켄 } },
            { 4, new List<MonsterType> { MonsterType.설인, MonsterType.울프, MonsterType.얼음정령, MonsterType.눈보라망령, MonsterType.프로스트드래곤 } },
            { 5, new List<MonsterType> { MonsterType.마그마골렘, MonsterType.불의정령, MonsterType.살아있는불꽃, MonsterType.지옥의사냥개, MonsterType.발록 } }
        };

        //몬스터 소환을 위한 생성자를 저장하는 배열->Battle.cs에서 해당 생성자를 불러와서 몬스터를 소환
        public static Func<MonsterType, Monster>[] LoadMonster =
        {
            (type) => new ForestMonsters(type),
            (type) => new DungeonMonsters(type),
            (type) => new AbyssMonsters(type),
            (type) => new FrozenPeaksMonsters(type),
            (type) => new VolcanicCoreMonsters(type)
        };

        public MonsterType Type { get; set; }

        int ClearGold;
        public Monster(MonsterType type, string name,float health, float attackPower,float defense, int lv, int clearGold ,float hitChance = 0.8f, float dodgeChance = 0.1f)
                        : base(name, health, attackPower, defense, lv, hitChance, dodgeChance)
        {
            ClearGold = clearGold;
            Type = type;

        }

        public Equipment GetRewardEquipment() // 장비 리워드 지급
        {
            if (equip.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(equip.Count);
                return equip[randomIndex]; // 랜덤으로 장비 지급
            }
            return null;
        }
        public ConsumableItem GetRewardConsumble() // 소모품 리워드 지급
        {
            if (equip.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(consum.Count);
                return consum[randomIndex]; // 랜덤으로 소모품 지급
            }
            return null;
        }
        public ConsumableItem GetRewardMiscItem() // 기타템 리워드 지급
        {
            if (equip.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(miscItem.Count);
                return consum[randomIndex]; // 기타템으로 장비 지급
            }
            return null;
        }


        public int GetClrearGold(Player player)
        {
            return ClearGold;
        }
    }

    class ForestMonsters : Monster
    {
        public ForestMonsters(MonsterType type) : base(type, type.ToString(), GetHealth(type), GetAttack(type), GetDefense(type), GetLevel(type), GetClearGold(type), HitChance(type), DodgeChance(type))
        {

        }

        public static float GetHealth(MonsterType type)
        {
            return type switch
            {
                MonsterType.슬라임 => 10.0f,
                MonsterType.고블린 => 15.0f,
                MonsterType.거대애벌레 => 20.0f,
                MonsterType.뿌리정령 => 18.0f,
                MonsterType.오우거 => 50.0f
            };
        }

        public static float GetAttack(MonsterType type)
        {
            return type switch
            {
                MonsterType.슬라임 => 3.0f,
                MonsterType.고블린 => 5.0f,
                MonsterType.거대애벌레 => 6.0f,
                MonsterType.뿌리정령 => 4.0f,
                MonsterType.오우거 => 12.0f
            };
        }
        public static float GetDefense(MonsterType type)
        {
            return type switch
            {
                MonsterType.슬라임 => 1.0f,
                MonsterType.고블린 => 2.0f,
                MonsterType.거대애벌레 => 3.0f,
                MonsterType.뿌리정령 => 4.0f,
                MonsterType.오우거 => 6.0f
            };
        }
        public static int GetLevel(MonsterType type) // 레벨부터 회피까지 수치 레벨디자인 필요
        {
            return type switch
            {
                MonsterType.슬라임 => 1,
                MonsterType.고블린 => 2,
                MonsterType.거대애벌레 => 3,
                MonsterType.뿌리정령 => 4,
                MonsterType.오우거 => 5
            };
        }
        public static int GetClearGold(MonsterType type)
        {
            return type switch
            {
                MonsterType.슬라임 => 5,
                MonsterType.고블린 => 10,
                MonsterType.거대애벌레 => 15,
                MonsterType.뿌리정령 => 12,
                MonsterType.오우거 => 50
            };
        }
        public static float HitChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.슬라임 => 0.8f,
                MonsterType.고블린 => 0.8f,
                MonsterType.거대애벌레 => 0.8f,
                MonsterType.뿌리정령 => 0.8f,
                MonsterType.오우거 => 0.8f
            };
        }
        public static float DodgeChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.슬라임 => 0.1f,
                MonsterType.고블린 => 0.1f,
                MonsterType.거대애벌레 => 0.1f,
                MonsterType.뿌리정령 => 0.1f,
                MonsterType.오우거 => 0.1f
            };
        }

    }
    class DungeonMonsters : Monster
    {
        public DungeonMonsters(MonsterType type) : base(type, type.ToString(), GetHealth(type), GetAttack(type), GetDefense(type), GetLevel(type), GetClearGold(type), HitChance(type), DodgeChance(type))
        {

        }

        public static float GetHealth(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 25.0f,
                MonsterType.임프 => 18.0f,
                MonsterType.미믹 => 30.0f,
                MonsterType.망령 => 40.0f,
                MonsterType.리치 => 70.0f
            };
        }

        public static float GetAttack(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 7.0f,
                MonsterType.임프 => 8.0f,
                MonsterType.미믹 => 10.0f,
                MonsterType.망령 => 9.0f,
                MonsterType.리치 => 15.0f
            };
        }
        public static float GetDefense(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 5.0f,
                MonsterType.임프 => 3.0f,
                MonsterType.미믹 => 4.0f,
                MonsterType.망령 => 5.0f,
                MonsterType.리치 => 8.0f
            };
        }
        public static int GetLevel(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 4,
                MonsterType.임프 => 4,
                MonsterType.미믹 => 5,
                MonsterType.망령 => 6,
                MonsterType.리치 => 8
            };
        }
        public static int GetClearGold(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 20,
                MonsterType.임프 => 15,
                MonsterType.미믹 => 25,
                MonsterType.망령 => 30,
                MonsterType.리치 => 75
            };
        }
        public static float HitChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 0.8f,
                MonsterType.임프 => 0.8f,
                MonsterType.미믹 => 0.8f,
                MonsterType.망령 => 0.8f,
                MonsterType.리치 => 0.8f
            };
        }
        public static float DodgeChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.스켈레톤 => 0.1f,
                MonsterType.임프 => 0.1f,
                MonsterType.미믹 => 0.1f,
                MonsterType.망령 => 0.1f,
                MonsterType.리치 => 0.1f
            };
        }
    }
    class AbyssMonsters : Monster
    {
        public AbyssMonsters(MonsterType type) : base(type, type.ToString(), GetHealth(type), GetAttack(type), GetDefense(type), GetLevel(type), GetClearGold(type), HitChance(type), DodgeChance(type))
        {

        }

        public static float GetHealth(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 40.0f,
                MonsterType.머맨 => 45.0f,
                MonsterType.크라켄의촉수 => 50.0f,
                MonsterType.유령해적 => 55.0f,
                MonsterType.크라켄 => 100.0f
            };
        }

        public static float GetAttack(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 12.0f,
                MonsterType.머맨 => 10.0f,
                MonsterType.크라켄의촉수 => 15.0f,
                MonsterType.유령해적 => 14.0f,
                MonsterType.크라켄 => 20.0f
            };
        }
        public static float GetDefense(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 6.0f,
                MonsterType.머맨 => 7.0f,
                MonsterType.크라켄의촉수 => 8.0f,
                MonsterType.유령해적 => 7.0f,
                MonsterType.크라켄 => 12.0f
            };
        }
        public static int GetLevel(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 7,
                MonsterType.머맨 => 8,
                MonsterType.크라켄의촉수 => 9,
                MonsterType.유령해적 => 9,
                MonsterType.크라켄 => 12
            };
        }
        public static int GetClearGold(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 35,
                MonsterType.머맨 => 40,
                MonsterType.크라켄의촉수 => 45,
                MonsterType.유령해적 => 50,
                MonsterType.크라켄 => 100
            };
        }
        public static float HitChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 0.8f,
                MonsterType.머맨 => 0.8f,
                MonsterType.크라켄의촉수 => 0.8f,
                MonsterType.유령해적 => 0.8f,
                MonsterType.크라켄 => 0.8f
            };
        }
        public static float DodgeChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.심연의전갈 => 0.1f,
                MonsterType.머맨 => 0.1f,
                MonsterType.크라켄의촉수 => 0.1f,
                MonsterType.유령해적 => 0.1f,
                MonsterType.크라켄 => 0.1f
            };
        }
    }
    class FrozenPeaksMonsters : Monster
    {
        public FrozenPeaksMonsters(MonsterType type) : base(type, type.ToString(), GetHealth(type), GetAttack(type), GetDefense(type), GetLevel(type), GetClearGold(type), HitChance(type), DodgeChance(type))
        {

        }

        public static float GetHealth(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 60.0f,
                MonsterType.울프 => 50.0f,
                MonsterType.얼음정령 => 70.0f,
                MonsterType.눈보라망령 => 80.0f,
                MonsterType.프로스트드래곤 => 150.0f
            };
        }

        public static float GetAttack(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 16.0f,
                MonsterType.울프 => 14.0f,
                MonsterType.얼음정령 => 18.0f,
                MonsterType.눈보라망령 => 20.0f,
                MonsterType.프로스트드래곤 => 30.0f
            };
        }
        public static float GetDefense(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 10.0f,
                MonsterType.울프 => 8.0f,
                MonsterType.얼음정령 => 12.0f,
                MonsterType.눈보라망령 => 14.0f,
                MonsterType.프로스트드래곤 => 20.0f
            };
        }
        public static int GetLevel(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 10,
                MonsterType.울프 => 10,
                MonsterType.얼음정령 => 11,
                MonsterType.눈보라망령 => 12,
                MonsterType.프로스트드래곤 => 15,
            };
        }
        public static int GetClearGold(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 60,
                MonsterType.울프 => 55,
                MonsterType.얼음정령 => 65,
                MonsterType.눈보라망령 => 70,
                MonsterType.프로스트드래곤 => 150
            };
        }
        public static float HitChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 0.8f,
                MonsterType.울프 => 0.8f,
                MonsterType.얼음정령 => 0.8f,
                MonsterType.눈보라망령 => 0.8f,
                MonsterType.프로스트드래곤 => 0.8f
            };
        }
        public static float DodgeChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.설인 => 0.1f,
                MonsterType.울프 => 0.1f,
                MonsterType.얼음정령 => 0.1f,
                MonsterType.눈보라망령 => 0.1f,
                MonsterType.프로스트드래곤 => 0.1f
            };
        }
    }
    class VolcanicCoreMonsters : Monster
    {
        public VolcanicCoreMonsters(MonsterType type) : base(type, type.ToString(), GetHealth(type), GetAttack(type), GetDefense(type), GetLevel(type), GetClearGold(type), HitChance(type), DodgeChance(type))
        {

        }

        public static float GetHealth(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 90.0f,
                MonsterType.불의정령 => 80.0f,
                MonsterType.살아있는불꽃 => 70.0f,
                MonsterType.지옥의사냥개 => 85.0f,
                MonsterType.발록 => 200.0f
            };
        }

        public static float GetAttack(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 25.0f,
                MonsterType.불의정령 => 22.0f,
                MonsterType.살아있는불꽃 => 20.0f,
                MonsterType.지옥의사냥개 => 28.0f,
                MonsterType.발록 => 40.0f
            };
        }
        public static float GetDefense(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 18.0f,
                MonsterType.불의정령 => 16.0f,
                MonsterType.살아있는불꽃 => 14.0f,
                MonsterType.지옥의사냥개 => 15.0f,
                MonsterType.발록 => 25.0f
            };
        }
        public static int GetLevel(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 14,
                MonsterType.불의정령 => 14,
                MonsterType.살아있는불꽃 => 13,
                MonsterType.지옥의사냥개 => 14,
                MonsterType.발록 => 20
            };
        }
        public static int GetClearGold(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 85,
                MonsterType.불의정령 => 80,
                MonsterType.살아있는불꽃 => 75,
                MonsterType.지옥의사냥개 => 90,
                MonsterType.발록 => 200
            };
        }
        public static float HitChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 0.8f,
                MonsterType.불의정령 => 0.8f,
                MonsterType.살아있는불꽃 => 0.8f,
                MonsterType.지옥의사냥개 => 0.8f,
                MonsterType.발록 => 0.8f
            };
        }
        public static float DodgeChance(MonsterType type)
        {
            return type switch
            {
                MonsterType.마그마골렘 => 0.1f,
                MonsterType.불의정령 => 0.1f,
                MonsterType.살아있는불꽃 => 0.1f,
                MonsterType.지옥의사냥개 => 0.1f,
                MonsterType.발록 => 0.1f
            };
        }
    }
}
