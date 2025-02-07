using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public string Slot { get; set; }
    public string Description { get; set; }
    public bool IsEquipped { get; set; }
    public bool IsForSale { get; set; }
    public int Price { get; set; }

    public Equipment()
    {
        Id = 0;
        Name = "기본 장비";
        Atk = 0;
        Def = 0;
        Slot = "기본";
        Description = "기본 장비입니다.";
        IsEquipped = false;
        IsForSale = false;
        Price = 0;
    }

    public Equipment(int id, string name, int atk, int def, string slot, string description, bool isEquipped, bool isForSale, int price)
    {
        Id = id;
        Name = name;
        Atk = atk;
        Def = def;
        Slot = slot;
        Description = $"| {(atk > 0 ? $"공격력+{atk}" : $"방어력+{def}")} | {description}";
        IsEquipped = isEquipped;
        IsForSale = isForSale;
        Price = price;
    }

    public static List<Equipment> GetEquipmentCatalog()
    {
        return new List<Equipment>
        {
            new Equipment(1, "천둥검", 12, 0, "무기1", "번개의 힘을 담은 검", false, false, 500),
            new Equipment(2, "무쇠 갑옷", 0, 7, "몸통", "무거운 철로 만든 튼튼한 갑옷", false, false, 700),
            new Equipment(3, "화염의 검", 15, 0, "무기1", "불꽃이 타오르는 강력한 검", false, false, 800),
            new Equipment(4, "고대의 방패", 0, 10, "무기2", "고대 문명의 수호자가 쓰던 방패", false, false, 600),
            new Equipment(5, "그리폰 헬름", 0, 3, "머리", "그리폰의 깃털로 만든 신비한 투구", false, false, 400)
        };
    }
}
