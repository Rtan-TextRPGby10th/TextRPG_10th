using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

public class Equipment      //장비 아이템 클래스
{
    public int Id { get; set; }
    public int Tier { get; set; }
    public string Name { get; set; }
    public int SpecialEffect { get; set; }              //상태이상 거는무기     0 = 없음       1 = 독       2 = 빙결      3 = 감전      4 = 화상   
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
        Tier = 1;
        Name = "기본 장비";
        SpecialEffect = 0;                              
        Atk = 0;
        Def = 0;
        Slot = "기본";
        Description = "기본 장비입니다.";
        IsEquipped = false;
        IsForSale = false;
        Amount = 1;
        Price = 0;
    }


    public Equipment(int id, int tier, string name, int specialEffect, int atk, int def, string slot, string description, bool isEquipped, bool isForSale, int amount, int price)
    {
        Id = id;
        Tier = tier;
        Name = name;
        SpecialEffect = specialEffect;
        Atk = atk;
        Def = def;
        Slot = slot;
        Description = $"| {(atk > 0 ? $"공격력+{atk}" : $"방어력+{def}")} | {description}";
        IsEquipped = isEquipped;
        IsForSale = isForSale;
        Amount = amount;
        Price = price;
    }

    private static List<Equipment> _equipmentCatalog=new List<Equipment>()
    //public static void SetEquipItem(List<Equipment> list)
    {
        //도감id,티어, 이름 상태이상,공,방,   슬롯  ,   설명               ,장착중?,판매중?,수량,가격
        //    무기
        new Equipment(101, 0, "초심자의 목검 [평범]",0, 5, 0, "무기", "가벼워 다루기 좋은 검.", false, false, 1, 200),
        new Equipment(102, 1, "그레이트 클럽 [고급]".ColorText(ConsoleColor.Green), 0, 10, 0, "무기", "오우거의 소재로 만든 몽둥이. 외의로 강하다.", false, false, 1, 1000),
        new Equipment(103, 2, "리치 본 소드 [희귀]".ColorText(ConsoleColor.Blue), 2, 20, 0, "무기", "리치의 뼈로 만들어진 대검. 차가운 기운이 감돈다. 공격 시 일정 확률로 빙결 부여.", false, false, 1, 2000),
        new Equipment(104, 3, "크라켄 슬래셔 [영웅]".ColorText(ConsoleColor.Magenta), 3, 30, 0, "무기", "크라켄의 거대한 발톱을 이용한 대검. 맞은 적을 마비시킨다. 공격 시 일정 확률로 감전 부여.", false, false, 1, 3000),
        new Equipment(105, 4, "아이스 브레이커 [전설]".ColorText(ConsoleColor.Yellow), 2, 40, 0, "무기", "프로스트 드래곤의 송곳니를 깎아 만든 대검. 공격 시 일정 확률로 빙결 부여.", false, false, 1, 4000),
        new Equipment(106, 5, "발록 헬파이어 블레이드 [유물]".ColorText(ConsoleColor.Red), 4, 50, 0, "무기", "발록의 불길을 담은 대검. 맞은 적을 불태운다. 공격 시 일정 확률로 화상 부여.", false, false, 1, 5000),
        
        new Equipment(111, 0, "초심자의 단검 [평범]", 0, 5, 0, "무기", "가벼워 다루기 좋은 단검.", false, false, 1, 200),
        new Equipment(112, 1, "더블핑거 대거 [고급]".ColorText(ConsoleColor.Green), 0, 10, 0, "무기", "오우거의 소재로 만든 단검. 가벼워 다루기 좋다.", false, false, 1, 1000),
        new Equipment(113, 2, "사령의 단검 [희귀]".ColorText(ConsoleColor.Blue), 2, 20, 0, "무기", "리치의 손가락을 이용해 만든 단검. 영혼을 갉아먹는다. 공격 시 일정 확률로 빙결 부여.", false, false, 1, 2000),
        new Equipment(114, 3, "촉수 대거 [영웅]".ColorText(ConsoleColor.Magenta), 1, 30, 0, "무기", "크라켄의 작은 촉수를 이용해 만든 단검. 적을 감아올린다. 공격 시 일정 확률로 독 부여.", false, false, 1, 3000),
        new Equipment(115, 4, "아이스 클로 대거 [전설]".ColorText(ConsoleColor.Yellow), 2, 40, 0, "무기", "프로스트 드래곤의 발톱을 활용한 단검. 공격 시 일정 확률로 빙결 부여.", false, false, 1, 4000),
        new Equipment(116, 5, "불타는 단검 [유물]".ColorText(ConsoleColor.Red), 4, 50, 0, "무기", "발록의 불꽃이 깃든 단검. 베인 상처에서 불길이 솟는다. 공격 시 일정 확률로 화상 부여.", false, false, 1, 5000),
        
        new Equipment(121, 0, "초심자의 활 [평범]", 0, 5, 0, "무기", "가벼워 다루기 좋은 활.", false, false, 1, 200),
        new Equipment(122, 1, "쉘 그레이트 보우 [고급]".ColorText(ConsoleColor.Green), 0, 10, 0, "무기", "오우거의 소재로 만든 활. 신축성이 강하다.", false, false, 1, 1000),
        new Equipment(123, 2, "저주의 장궁 [희귀]".ColorText(ConsoleColor.Blue), 1, 20, 0, "무기", "리치의 마력을 담은 활. 맞은 적은 서서히 체력을 잃는다. 공격 시 일정 확률로 독 부여.", false, false, 1, 2000),
        new Equipment(124, 3, "심연의 활 [영웅]".ColorText(ConsoleColor.Magenta), 1, 30, 0, "무기", "크라켄의 심해 마력을 응축한 활. 화살이 깊이 스며든다. 공격 시 일정 확률로 독 부여.", false, false, 1, 3000),
        new Equipment(125, 4, "프로스트 윙 보우 [전설]".ColorText(ConsoleColor.Yellow), 2, 40, 0, "무기", "프로스트 드래곤의 날개 막으로 제작된 활. 맞은 적은 동작이 둔해진다. 공격 시 일정 확률로 빙결 부여.", false, false, 1, 4000),
        new Equipment(126, 5, "지옥의 장궁 [유물]".ColorText(ConsoleColor.Red), 4, 50, 0, "무기", "발록의 마력을 담은 활. 맞은 적은 불길 속에서 괴로워한다. 공격 시 일정 확률로 화상 부여.", false, false, 1, 5000),
        

        //투구
        new Equipment(301, 0, "천 두건 [평범]", 0, 0, 5, "머리", "얇은 천으로 만든 두건.", false, false, 1,200),
        new Equipment(302, 1, "촉촉한 머리띠 [고급]".ColorText(ConsoleColor.Green), 0, 0, 10, "머리", "슬라임의 소재로 만든 투구. 물컹해서 기분 나쁘다.", false, false, 1,1000),
        new Equipment(303, 2, "해골 투구 [희귀]".ColorText(ConsoleColor.Blue), 0, 0, 20, "머리", "스켈레톤 워리어의 머리를 장식한 투구. 쓰기엔 조금 찜찜하다.", false, false, 1,2000),
        new Equipment(304, 3, "전갈의 투구 [영웅]".ColorText(ConsoleColor.Magenta), 0, 0, 30, "머리", "심연의 전갈의 껍질로 만든 단단한 투구. 독에 강한 방어 효과가 있다.", false, false, 1,3000),
        new Equipment(305, 4, "설인의 모피 투구 [전설]".ColorText(ConsoleColor.Yellow), 0, 0, 40, "머리", "설인의 두꺼운 모피로 만든 방한용 투구. 따뜻하다.", false, false, 1,4000),
        new Equipment(306, 5, "마그마 투구 [유물]".ColorText(ConsoleColor.Red), 0, 0, 50, "머리", "마그마 골렘의 몸을 일부 떼어 만든 투구. 뜨겁다.", false, false, 1,5000),

        //갑옷
        new Equipment(401, 0, "천 옷 [평범]", 0, 0, 5, "몸통", "천으로 만든 방어구.", false, false, 1,200),
        new Equipment(402, 1, "나무 갑옷 [고급]".ColorText(ConsoleColor.Green), 0, 0, 10, "몸통", "뿌리 정령의 소재로 만든 갑옷. 단단하지만 화염에 취약하다.", false, false, 1,1000),
        new Equipment(403, 2, "거짓된 갑옷 [희귀]".ColorText(ConsoleColor.Blue), 0, 0, 20, "몸통", "미믹의 몸 일부를 이용해 만든 갑옷. 가끔 미세하게 움직인다.", false, false, 1,2000),
        new Equipment(404, 3, "촉수 갑옷 [영웅]".ColorText(ConsoleColor.Magenta), 0, 0, 30, "몸통", "크라켄의 촉수 일부로 만든 갑옷. 마치 살아있는 듯 꿈틀거린다.", false, false, 1,3000),
        new Equipment(405, 4, "얼음 정령의 결정 갑옷 [전설]".ColorText(ConsoleColor.Yellow), 0, 0, 40, "몸통", "얼음 정령의 몸에서 떨어진 조각으로 만든 갑옷. 차가운 기운이 돈다.", false, false, 1,4000),
        new Equipment(406, 5, "살아있는 불꽃 갑옷 [유물]".ColorText(ConsoleColor.Red), 0, 0, 50, "몸통", "살아있는 불꽃이 깃든 갑옷. 전투 중에도 불길이 어른거린다.", false, false, 1,5000),

        //장갑
        new Equipment(501, 0, "천 장갑 [평범]", 0, 0, 5, "손", "천으로 만든 장갑.", false, false, 1,200),
        new Equipment(502, 1, "그린 건틀릿 [고급]".ColorText(ConsoleColor.Green), 0, 0, 10, "손", "고블린의 소재로 만든 장갑. 의외로 튼튼하다.", false, false, 1,1000),
        new Equipment(503, 2, "임프의 손장갑 [희귀]".ColorText(ConsoleColor.Blue), 0, 0, 20, "손", "임프의 가죽으로 만든 장갑. 마력이 미세하게 흐른다.", false, false, 1,2000),
        new Equipment(504, 3, "머맨의 수중 장갑 [영웅]".ColorText(ConsoleColor.Magenta), 0, 0, 30, "손", "머맨의 비늘로 만든 장갑. 물속에서도 유연하게 움직일 수 있다.", false, false, 1,3000),
        new Equipment(505, 4, "프로스트 울프의 발톱 장갑 [전설]".ColorText(ConsoleColor.Yellow), 0, 0, 40, "손", "날카로운 얼음 발톱이 붙어 있는 장갑. 강한 공격력을 제공한다.", false, false, 1,4000),
        new Equipment(506, 5, "불의 정령 장갑 [유물]".ColorText(ConsoleColor.Red), 0, 0, 50, "손", "불의 정령의 정수를 담은 장갑. 닿는 것마다 불타오른다.", false, false, 1,5000),

        //신발
        new Equipment(601, 0, "천 신발 [평범]", 0, 0, 5, "다리", "조잡한 신발.", false, false, 1,200),
        new Equipment(602, 1, "미끈거리는 신발 [고급]".ColorText(ConsoleColor.Green), 0, 0, 10, "다리", "거대 애벌레의 소재로 만든 신발. 오래 신으면 찝찝하다.", false, false, 1,1000),
        new Equipment(603, 2, "유령신발 [희귀]".ColorText(ConsoleColor.Blue), 0, 0, 20, "다리", "망령의 기운이 서려 있는 신발. 바닥에서 미끄러지듯 움직인다.", false, false, 1,2000),
        new Equipment(604, 3, "해적의 망령 신발 [영웅]".ColorText(ConsoleColor.Magenta), 0, 0, 30, "다리", "바다 유령선 선원의 유령 조각이 깃든 신발. 착용 시 가벼운 움직임을 보장한다.", false, false, 1,3000),
        new Equipment(605, 4, "눈보라의 신발 [전설]".ColorText(ConsoleColor.Yellow), 0, 0, 40, "다리", "눈보라 망령의 마력을 머금은 신발. 빠르게 움직일 수 있다.", false, false, 1,4000),
        new Equipment(606, 5, "지옥의 불길 신발 [유물]".ColorText(ConsoleColor.Red), 0, 0, 50, "다리", "지옥의 사냥개가 남긴 불길이 깃든 신발. 빠른 이동이 가능하다.", false, false, 1,5000),
    };

    public static void SetEquipmentCatalog(List<Equipment> list)
    {
        _equipmentCatalog = list;
    }


    // 캐싱된 리스트 반환 (매번 새로 만들지 않음)
    public static List<Equipment> GetEquipmentCatalog()
        {
            return _equipmentCatalog;
        }
}

public static class ConsoleExtensions
{
    public static string ColorText(this string text, ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Green => $"\u001b[92m{text}\u001b[0m",   // 밝은 초록 - 고급(1티어)
            ConsoleColor.Blue => $"\u001b[94m{text}\u001b[0m",    // 밝은 파랑 - 희귀(2티어)
            ConsoleColor.Magenta => $"\u001b[95m{text}\u001b[0m", // 밝은 보라 - 영웅(3티어)
            ConsoleColor.Yellow => $"\u001b[93m{text}\u001b[0m",  // 밝은 노랑 - 전설(4티어)
            ConsoleColor.Red => $"\u001b[91m{text}\u001b[0m",     // 밝은 빨강 - 유물(5티어)

            ConsoleColor.Gray => $"\u001b[37m{text}\u001b[0m",    // 밝은 회색
            ConsoleColor.Cyan => $"\u001b[96m{text}\u001b[0m",    // 밝은 청록
            _ => text
        };
    }
}

