using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class ConsumableItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Turn { get; set; } // 지속 턴 (힐링포션은 0, 맹독포션은 지속됨)
    public int Value { get; set; } // 회복량 또는 공격력 증가량
    public string Description { get; set; }
    public int Amount { get; set; } // 수량
    public int Price { get; set; }

    public ConsumableItem()
    {
        Id = 0;
        Name = "기본";
        Turn = 0;
        Value = 0;
        Description = "기본";
        Amount = 0;
        Price = 0;
    }

    public ConsumableItem(int id, string name, int turn, int value, string description, int amount, int price)
    {
        Id = id;
        Name = name;
        Turn = turn;
        Value = value;
        Description = $"| {(value > 0 ? $"체력+{value}" : "")} {(turn > 0 ? $"(지속 {turn}턴)" : "")} | {description}";
        Amount = amount;
        Price = price;
    }

    public static List<ConsumableItem> GetItemCatalog()
    {
        return new List<ConsumableItem>
        {
            new ConsumableItem(1, "힐링포션(소)", 0, 50, "소량의 체력을 회복하는 포션", 1, 100),
            new ConsumableItem(2, "힐링포션(중)", 0, 100, "중간 정도의 체력을 회복하는 포션", 1, 250),
            new ConsumableItem(3, "힐링포션(대)", 0, 200, "대량의 체력을 회복하는 포션", 1, 500),
            new ConsumableItem(4, "맹독포션", 5, 10, "5턴 동안 무기에 발라 공격력을 증가시킨다.", 1, 400)
        };
    }
}
