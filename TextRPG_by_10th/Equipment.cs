using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class Equipment      //장비 아이템 클래스
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public string Slot { get; set; }
    public string Description { get; set; }
    public bool IsEquipped { get; set; }
    public bool IsForSale { get; set; }

    public int Amount { get; set; }
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
        Amount = 1;
        Price = 0;
    }

    public Equipment(int id, string name, int atk, int def, string slot, string description, bool isEquipped, bool isForSale, int amount, int price)
    {
        Id = id;
        Name = name;
        Atk = atk;
        Def = def;
        Slot = slot;
        Description = $"| {(atk > 0 ? $"공격력+{atk}" : $"방어력+{def}")} | {description}";
        IsEquipped = isEquipped;
        IsForSale = isForSale;
        Amount = amount;
        Price = price;
    }

    public static List<Equipment> GetEquipmentCatalog()                 //장비 도감
    {
        return new List<Equipment>                                      //장비 아이템 데이터 추가하려면 여기에 수정
        {
            new Equipment(101, "초심자의 목검", 3, 0, "무기1", "가벼워 다루기 좋은 검", false, false, 1, 100),
            new Equipment(102, "롱소드", 5, 0, "무기1", "기초적인 형태의 검", false, false, 1, 200),
            new Equipment(103, "화염의 검", 7, 0, "무기1", "불꽃이 타오르는 검", false, false, 1, 300),
            new Equipment(104, "흑기사의 검", 10, 0, "무기1", "새까만 흑철로 이루어진 고급스러운 검", false, false, 1, 400),

            new Equipment(201, "널빤지", 0, 1, "무기2", "없는 것보단 나은 방패", false, false, 1, 100),
            new Equipment(202, "나무 방패", 0, 2, "무기2", "단조롭지만 생각보다 단단한 방패", false, false, 1,200),
            new Equipment(203, "카이트 쉴드", 0, 3, "무기2", "숙련된 대장장이가 만든 방패", false, false, 1, 300),
            new Equipment(204, "흑기사의 방패", 0, 4, "무기2", "어마어마하게 단단한 방패", false, false, 1,400),

            new Equipment(301, "하얀 두건", 0, 1, "머리", "얇은 천으로 만든 두건", false, false, 1,100),
            new Equipment(302, "가죽 두건", 0, 2, "머리", "질긴 가죽으로 만든 두건", false, false, 1,200),
            new Equipment(303, "사슬 투구", 0, 3, "머리", "숙련된 대장장이가 만든 투구", false, false, 1,300),
            new Equipment(304, "흑기사의 투구", 0, 4, "머리", "매우 고급스럽고 단단한 투구", false, false, 1,400),

            new Equipment(401, "천 옷", 0, 1, "몸통", "천으로 만든 방어구", false, false, 1,100),
            new Equipment(402, "가죽 갑옷", 0, 2, "몸통", "가죽으로 만든 방어구", false, false, 1,200),
            new Equipment(403, "사슬 갑옷", 0, 3, "몸통", "사슬로 만든 방어구", false, false, 1,300),
            new Equipment(404, "흑기사의 갑옷", 0, 4, "몸통", "흑기사가 애용하는 방어구", false, false, 1,400),

            new Equipment(501, "천 장갑", 0, 1, "손", "천으로 만든 장갑", false, false, 1,100),
            new Equipment(502, "가죽 장갑", 0, 2, "손", "가죽으로 만든 장갑", false, false, 1,200),
            new Equipment(503, "사슬 장갑", 0, 3, "손", "사슬로 만든 장갑", false, false, 1,300),
            new Equipment(504, "흑기사의 장갑", 0, 4, "손", "흑기사가 쓰던 장갑", false, false, 1,400),

            new Equipment(601, "천 신발", 0, 1, "다리", "조잡한 신발", false, false, 1,100),
            new Equipment(602, "가죽 신발", 0, 2, "다리", "가죽으로 만든 신발", false, false, 1,200),
            new Equipment(603, "사슬 신발", 0, 3, "다리", "사슬로 만든 신발", false, false, 1,300),
            new Equipment(604, "흑기사의 신발", 0, 4, "다리", "두껍고 단단한 신발", false, false, 1,400),

        };
    }
}
