using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class MiscItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; } // 수량
    public int Price { get; set; }

    public MiscItem()
    {
        Id = 0;
        Name = "기본";
        Description = "기본";
        Amount = 0;
        Price = 0;
    }

    public MiscItem(int id, string name, string description, int amount, int price)
    {
        Id = id;
        Name = name;
        Description = description;
        Amount = amount;
        Price = price;
    }

    public static List<MiscItem> GetMiscCatalog()
    {
        return new List<MiscItem>
        {
            new MiscItem(1, "구울의 머리", "구울을 처치하면 간혹 떨어지는 기괴한 머리", 1, 3000)
        };
    }
}
