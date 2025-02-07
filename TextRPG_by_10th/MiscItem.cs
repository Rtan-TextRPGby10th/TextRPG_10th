using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class MiscItem       //기타 아이템 클래스
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
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

    public static List<MiscItem> GetMiscCatalog()                       //기타 도감
    {
        return new List<MiscItem>                                       //기타 아이템 데이터 추가하려면 여기에 수정
        {
            new MiscItem(10001, "구울의 머리", "구울을 처치하면 간혹 떨어지는 기괴한 머리", 1, 500),
            new MiscItem(10002, "슬라임의 점액", "미끌미끌. 만지면 기분이 좋다", 1, 200),
            new MiscItem(10003, "거대 슬라임의 점액", "매우 희귀하다. 비싼값에 거래된다", 1, 2000)
        };
    }
}
