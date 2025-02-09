using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class ConsumableItem     //소모품 아이템 클래스
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Turn { get; set; } // 지속 턴 (힐링포션은 0, 맹독포션은 지속됨)
    public int Value { get; set; } // 회복량
    public string Description { get; set; }
    public int Amount { get; set; } 
    public int Price { get; set; }

    public ConsumableItem()     //기본값
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
        Description = $"| {description}";
        Amount = amount;
        Price = price;
    }

    public static List<ConsumableItem> GetItemCatalog()                                 //소모품 도감
    {
        return new List<ConsumableItem>                                                 //소모품 아이템 데이터 추가하려면 여기에 수정
        {


         //                    도감id               턴, 값                            수량,가격
            new ConsumableItem(1001, "힐링포션(소)", 0, 30, "30의 체력을 회복하는 포션.", 1, 100),
            new ConsumableItem(1002, "힐링포션(중)", 0, 50, "50의 체력을 회복하는 포션.", 1, 170),
            new ConsumableItem(1003, "힐링포션(대)", 0, 70, "70의 체력을 회복하는 포션.", 1, 220),
            new ConsumableItem(1004, "맹독포션", 5, -1, "무기에 바를 수 있는 맹독성 포션.", 1, 120)

        };
    }
}
