namespace TextRPG_by_10th
{
    internal class Monster : Creature
    {
        public static List<Equipment> equip = Equipment.GetEquipmentCatalog(); // 아이템 도감에서 장비를 가져온다.
        public static List<ConsumableItem> consum = ConsumableItem.GetItemCatalog(); // 소모품 도감에서 소모품을 가져온다
        public static List<MiscItem> miscItem = MiscItem.GetMiscCatalog(); // 기타템 도감에서 기타템을 가져온다

        int ClearGold;
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
    }
    class MonsterType2 : Monster
    {
        public MonsterType2(string name, float health, float attackPower, float defense, int lv, int clearGold)
                        : base(name, health, attackPower, defense,lv, clearGold)
        {

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
    }
}
