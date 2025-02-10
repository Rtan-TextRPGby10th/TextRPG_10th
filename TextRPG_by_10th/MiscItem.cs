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
            //1티어
            new MiscItem(10001, "슬라임의 점액", "미끌미끌. 만지면 기분이 좋다.", 1, 100),
            new MiscItem(10002, "고블린의 가죽", "더럽고 냄새가 심하다.", 1, 100),
            new MiscItem(10003, "거대 애벌레의 솜털", "부드러운게 장점.", 1, 100),
            new MiscItem(10004, "뿌리 정령의 나뭇가지","마력이 깃든 작은 나뭇가지. 생명이 느껴진다.", 1,100),
            new MiscItem(10005, "오우거의 등껍질","단단한 피부 조각. 충격을 흡수하는 성질이 있다.", 1,100),
            new MiscItem(10006, "오우거의 수염","거칠고 질긴 털. 강한 마력을 머금고 있다.", 1,100),
            new MiscItem(10007, "오우거의 손가락","크고 두꺼운 손가락. 힘이 느껴진다.", 1,100),
            //2티어
            new MiscItem(10008, "스켈레톤의 두개골", "오랜 세월이 지나도 단단함을 유지하는 해골.", 1,200),
            new MiscItem(10009, "임프의 가죽", "마력이 흐르는 얇고 질긴 가죽.", 1,200),
            new MiscItem(10010, "미믹의 껍질", "보물상자로 위장하던 단단한 외피. 닿으면 소름이 돋는다.", 1,200),
            new MiscItem(10011, "망령의 그림자", "손으로 잡을 수 없지만 장비 제작에 사용할 수 있는 영적인 잔재.", 1,200),
            new MiscItem(10012, "리치의 척추", "강력한 마력을 품고 있는 해골 마법사의 척추뼈.", 1,200),
            new MiscItem(10013, "리치의 영혼 조각", "생과 사의 경계를 넘나드는 언데드의 영혼 일부.", 1,200),
            new MiscItem(10014, "리치의 손가락", "바싹 마른 뼈지만 여전히 저주가 깃들어 있다.", 1,200),
            //3티어
            new MiscItem(10015, "심연의 전갈 껍질", "강한 수압을 견디는 단단한 외골격.", 1,300),
            new MiscItem(10016, "머맨의 비늘", "물속에서 빛을 반사하는 매끄러운 비늘.", 1,300),
            new MiscItem(10017, "크라켄의 촉수 조각", "미끈거리며 스스로 움직이는 것 같은 느낌이 든다.", 1,300),
            new MiscItem(10018, "유령선 선원의 해골", "저주받은 바다에서 떠도는 유령 해적의 잔해.", 1,300),
            new MiscItem(10019, "크라켄의 발톱", "거대한 크라켄의 발톱, 수중에서도 강한 파괴력을 가진다.", 1,300),
            new MiscItem(10020, "크라켄의 심장", "심해의 마력을 담고 있는 붉은 심장.", 1,300),
            new MiscItem(10021, "크라켄의 미세 촉수", "잘라내도 계속 꿈틀거리는 작은 촉수 조각.", 1,300),
            //4티어
            new MiscItem(10022, "설인의 모피", "두껍고 따뜻한 모피. 추위 속에서 생존하는 데 유용하다.", 1,400),
            new MiscItem(10023, "프로스트 울프의 발톱", "얼음처럼 날카로운 발톱. 베이면 서서히 얼어붙는다.", 1,400),
            new MiscItem(10024, "얼음 정령의 결정", "차가운 마력을 품은 푸른 결정체.", 1,400),
            new MiscItem(10025, "눈보라 망령의 영혼", "차가운 기운을 품고 있는 유령의 흔적.", 1,400),
            new MiscItem(10026, "프로스트 드래곤의 송곳니", "극한의 추위를 담고 있는 예리한 송곳니.", 1,400),
            new MiscItem(10027, "프로스트 드래곤의 날개 조각", "가볍고 유연하지만 매우 단단한 날개 일부.", 1,400),
            new MiscItem(10028, "프로스트 드래곤의 발톱", "한 번 맞으면 뼛속까지 얼어붙는 드래곤의 발톱.", 1,400),
            //5티어
            new MiscItem(10029, "마그마 골렘의 암석", "뜨겁게 이글거리는 용암이 식어 굳어진 암석.", 1,500),
            new MiscItem(10030, "불의 정령의 불꽃", "절대 꺼지지 않는 작은 불꽃.", 1,500),
            new MiscItem(10031, "살아있는 불꽃의 심장", "생명력을 지닌 불꽃의 코어. 만지면 화상을 입을 수도 있다.", 1,500),
            new MiscItem(10032, "지옥의 사냥개의 불꽃", "불길 속에서 살아가는 사냥개의 타오르는 흔적.", 1,500),
            new MiscItem(10033, "발록의 검은 불꽃", "불이 아니라 마력으로 타오르는 지옥불의 정수.", 1,500),
            new MiscItem(10034, "발록의 지옥불 조각", "모든 것을 불태우는 검붉은 불꽃의 파편.", 1,500),
            new MiscItem(10035, "발록의 불꽃 송곳니", "뜨겁게 타오르는 발록의 강력한 이빨.", 1,500),
        };
    }
}
