namespace TextRPG_by_10th
{
    public class Monster : Creature
    {
        public static List<Equipment> equip = Equipment.GetEquipmentCatalog(); // 아이템 도감에서 장비를 가져온다.
        public static List<ConsumableItem> consum = ConsumableItem.GetItemCatalog(); // 소모품 도감에서 소모품을 가져온다
        public static List<MiscItem> miscItem = MiscItem.GetMiscCatalog(); // 기타템 도감에서 기타템을 가져온다
        //몬스터 소환을 위한 생성자를 저장하는 배열->Battle.cs에서 해당 생성자를 불러와서 몬스터를 소환
        public static Func<Monster>[] LoadMonster =
        {
            () => new Slime(),
            () => new Mandrake(),
            () => new Spider(),
            () => new Wolf(),
            () => new Minotaur(),
            () => new Dragon(),
        };

        int ClearGold;
        public Monster(string name,float health, float attackPower,float defense, int lv, int clearGold, float hitChance = 0.8f, float dodgeChance = 0.1f)
                        : base(name, health, attackPower, defense, lv, hitChance, dodgeChance)
        {
            ClearGold = clearGold;
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

    class Slime : Monster
    {
        public Slime() : base("슬라임", 20.0f, 5.0f, 2.0f, 1, 10, 0.7f, 0.2f) // 낮은 명중, 높은 회피
        {
        }

       
    }
    class Mandrake : Monster
    {
        public Mandrake() : base("맨드레이크", 40.0f, 9.0f, 4.0f, 5, 25, 0.8f, 0.1f)
        {

        }
    }
    class Spider : Monster
    {
        public Spider() : base("거미", 70, 14, 6, 10, 50, 0.8f, 0.1f)
        {

        }
    }
    class Wolf : Monster
    {
        public Wolf() : base("늑대", 110, 18, 8, 15, 75, 0.8f, 0.1f)
        {

        }
    } 
    class Minotaur : Monster
    {
        public Minotaur() : base("미노타우르스", 150, 22, 10, 20, 100)
        {

        }
    } 
    class Dragon : Monster
    {
        public Dragon() : base("드래곤", 200, 30, 15, 25, 200, 0.95f, 0.05f) // 높은 명중, 낮은 회피
        {

        }
    }
}
