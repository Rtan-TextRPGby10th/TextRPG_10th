using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace TextRPG_by_10th
{

    public class Inventory
    {


        private static bool isInitialized = false;           //최초 실행시 인벤토리에 기본아이템 추가.

        static List<Equipment> equipmentList = new List<Equipment>();
        static List<ConsumableItem> consumableList = new List<ConsumableItem>();
        static List<MiscItem> miscList = new List<MiscItem>();
        static Dictionary<string, bool> equippedItems = new Dictionary<string, bool>(); // 장착 상태 관리
        public Player player; // ✅ 플레이어 인스턴스 저장

        public float plusAtk { get; set; } = 0;  // 장착한 장비의 공격력 합
        public float plusDef { get; set; } = 0;  // 장착한 장비의 방어력 합


        static Dictionary<string, string> equippedSlots = new Dictionary<string, string>()
        {
            {"머리", "-" },
            {"몸통", "-" },
            {"손", "-" },
            {"다리", "-" },
            {"무기", "-" },
        };

        public List<Equipment> GetEquipmentList()
        {
            return equipmentList;
        }

        public List<ConsumableItem> GetConsumableList()
        {
            return consumableList;
        }

        public List<MiscItem> GetMiscList()
        {
            return miscList;
        }

        public Inventory()
        {
            foreach (var item in equipmentList)
            {
                // 기존 장착 상태 유지
                if (equippedSlots.ContainsValue(item.Name))
                {
                    item.IsEquipped = true;
                    equippedItems[item.Name] = true;
                }
                else
                {
                    item.IsEquipped = false;
                    equippedItems[item.Name] = false;
                }
            }
        }


        public void AddInventory(int id, int amount)            //인벤토리에 아이템 추가할시, id에따라 종류별 리스트로 분류
        {
            if (id >= 100 && id < 1000) // 아이템넘버 100~999는 장비 (Equipment)
            {
                Equipment item = Equipment.GetEquipmentCatalog().Find(e => e.Id == id);     //장비 도감에서 해당 넘버의 아이템을 검색. 
                if (item != null && !equipmentList.Contains(item))                          //미보유시 해당 넘버의 아이템을 장비 인벤토리에 추가.
                {
                    equipmentList.Add(item);
                    equippedItems[item.Name] = false;
                    Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
                }
            }
            else if (id >= 1000 && id < 10000) // 아이템넘버 1000~9999는 소모품 (ConsumableItem)
            {
                ConsumableItem item = ConsumableItem.GetItemCatalog().Find(c => c.Id == id);    //소모품 도감에서 해당 넘버의 아이템을 검색. 
                if (item != null)
                {
                    ConsumableItem existingItem = consumableList.Find(c => c.Id == id);
                    if (existingItem != null)
                    {
                        existingItem.Amount += amount;
                        Console.WriteLine($"{item.Name} 수량이 증가했습니다. (x{existingItem.Amount})");     //소모품 인벤토리에 이미 해당 아이템이 있을경우
                    }
                    else
                    {
                        item.Amount = amount;
                        consumableList.Add(item);
                        Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");                    //소모품 인벤토리에 해당 아이템이 없을경우
                    }
                }
            }
            else if (id >= 10000 && id < 100000) // 아이템넘버 10000~99999는 기타 아이템 (MiscItem)
            {
                MiscItem item = MiscItem.GetMiscCatalog().Find(m => m.Id == id);
                if (item != null)
                {
                    MiscItem existingItem = miscList.Find(m => m.Id == id);
                    if (existingItem != null)
                    {
                        existingItem.Amount += amount;
                        Console.WriteLine($"{item.Name} 수량이 증가했습니다. (x{existingItem.Amount})");
                    }
                    else
                    {
                        item.Amount = amount;
                        miscList.Add(item);
                        Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
                    }
                }
            }
            else
            {
                Console.WriteLine("잘못된 아이템 ID입니다.");
            }
        }


        public void RemoveInventory(int id, int amount)                     // 아이템 제거 (id 기반으로 아이템을 분류)
        {
            if (id >= 100 && id < 1000) // 장비 (Equipment)
            {
                Equipment item = equipmentList.Find(e => e.Id == id);
                if (item != null)
                {
                    equipmentList.Remove(item);
                    Console.WriteLine($"{item.Name}을(를) 인벤토리에서 제거했습니다.");
                }
                else
                {
                    Console.WriteLine("제거할 장비가 인벤토리에 없습니다.");
                }
            }
            else if (id >= 1000 && id < 10000) // 소모품 (ConsumableItem)
            {
                ConsumableItem item = consumableList.Find(c => c.Id == id);
                if (item != null)
                {
                    if (item.Amount > amount)
                    {
                        item.Amount -= amount;
                        Console.WriteLine($"{item.Name} {amount}개 제거됨. 남은 개수: {item.Amount}");
                    }
                    else
                    {
                        consumableList.Remove(item);
                        Console.WriteLine($"{item.Name}을(를) 인벤토리에서 제거했습니다.");
                    }
                }
                else
                {
                    Console.WriteLine("제거할 소모품이 인벤토리에 없습니다.");
                }
            }
            else if (id >= 10000 && id < 100000) // 기타 아이템 (MiscItem)
            {
                MiscItem item = miscList.Find(m => m.Id == id);
                if (item != null)
                {
                    if (item.Amount > amount)
                    {
                        item.Amount -= amount;
                        Console.WriteLine($"{item.Name} {amount}개 제거됨. 남은 개수: {item.Amount}");
                    }
                    else
                    {
                        miscList.Remove(item);
                        Console.WriteLine($"{item.Name}을(를) 인벤토리에서 제거했습니다.");
                    }
                }
                else
                {
                    Console.WriteLine("제거할 기타 아이템이 인벤토리에 없습니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 아이템 ID입니다.");
            }
        }


        public void ShowInventory()                                         //상태 보기(스테이터스+인벤토리+장착관리 통합) 씬 
        {
            player = SceneManager.instance.player;  // ✅ SceneManager에서 player 가져오기
            
            if (!isInitialized)
            {
                switch (player.playerJob)
                {
                    case Job.전사:
                        AddInventory(101, 1); // 전사용 무기
                        break;
                    case Job.도적:
                        AddInventory(102, 1); // 도적용 무기
                        break;
                    case Job.궁수:
                        AddInventory(103, 1); // 궁수용 무기
                        break;
                    default:
                        Console.WriteLine("잘못된 직업입니다.");
                        break;
                }

                AddInventory(1001, 3);
                AddInventory(1004, 3);
                isInitialized = true; // 기본아이템은 최초 1회만 지급
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 상태 보기 =====");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                foreach (var slot in equippedSlots)                     // 🔹 현재 장착 상태 출력
                {
                    Console.WriteLine($"{slot.Key} : {slot.Value}");
                }

                Console.WriteLine("\n<소모품>\n");                        //소모품 출력
                if (consumableList.Count == 0)
                    Console.WriteLine("  -");
                else
                    foreach (var item in consumableList)
                        Console.WriteLine($"{item.Name} {item.Description} | {item.Amount}개");

                Console.WriteLine("\n<기타>\n");                         //기타 출력
                if (miscList.Count == 0)
                    Console.WriteLine("  -");
                else
                    foreach (var item in miscList)
                        Console.WriteLine($"        {item.Name} {item.Description} | {item.Amount}개");

                Console.WriteLine("\n<장비>\n");
                for (int i = 0; i < equipmentList.Count; i++)           //장비 출력
                {
                    Equipment item = equipmentList[i];
                    string equipStatus = equippedItems[item.Name] ? "[E]" : ""; // 장착 여부 표시
                    Console.WriteLine($"{i + 1}. {equipStatus} {item.Name} {item.Description}");
                }
                

                Console.WriteLine("0.  나가기");
                Console.Write(">> ");
                (int x, int y) = Console.GetCursorPosition();       // 현재의 커서 포지션 기억
                Status();                                           // 스테이터스 출력
                Console.SetCursorPosition(x, y);                    // 기억해둔 좌표로 이동
                string input2 = Console.ReadLine();


                // 🔹 입력값 검증 및 장비 선택
                if (int.TryParse(input2, out int index) && index > 0 && index <= equipmentList.Count)
                {
                    EquipItem(index - 1); // 선택한 장비를 장착/해제
                }
                else
                {
                }
                if (input2 == "0")                  // 나가기
                {
                    SceneManager.instance.currentScene = SceneManager.Scene.Start;
                    SceneManager.instance.GameScecne(SceneManager.Scene.Start);
                    return;
                }
            }
        }

        private void EquipItem(int index)
        {
            Equipment item = equipmentList[index]; // 선택한 장비
            string equipSlot = item.Slot;          // 장비의 착용 슬롯

            // 🔹 이미 장착 중이면 해제
            if (equippedItems[item.Name])
            {
                equippedItems[item.Name] = false;
                equippedSlots[equipSlot] = "-"; // 슬롯 초기화

                // 🔹 공격력 & 방어력 감소
                player.AttackPower -= plusAtk;
                player.Defense -= plusDef;
                plusAtk -= item.Atk;
                plusDef -= item.Def;
            }
            else
            {
                // 🔹 같은 슬롯에 다른 장비가 장착되어 있으면 기존 장비 해제
                foreach (var key in equippedSlots.Keys)
                {
                    if (key == equipSlot && equippedSlots[key] != "-")
                    {
                        string equippedItem = equippedSlots[key];
                        Equipment unequippedItem = equipmentList.Find(e => e.Name == equippedItem);

                        if (unequippedItem != null)
                        {
                            equippedItems[unequippedItem.Name] = false;
                            equippedSlots[key] = "-";

                            // 🔹 기존 장비의 공격력 & 방어력 제거
                            player.AttackPower -= plusAtk;
                            player.Defense -= plusDef;
                            plusAtk -= unequippedItem.Atk;
                            plusDef -= unequippedItem.Def;
                            
                        }
                    }
                }

                // 🔹 새로운 장비 장착
                equippedItems[item.Name] = true;
                equippedSlots[equipSlot] = item.Name;

                // 🔹 공격력 & 방어력 추가
                plusAtk += item.Atk;
                plusDef += item.Def;
            }
        }




        public void Status()
        {
            player.AttackPower += plusAtk;
            player.Defense += plusDef;

            Console.SetCursorPosition(30, 3);
            Console.WriteLine($"Lv : {player.Lv}");
            Console.SetCursorPosition(30, 4);
            Console.WriteLine($"{player.Name} ( {player.playerJob} )");
            Console.SetCursorPosition(30, 5);

            if (plusAtk == 0)
            Console.WriteLine($"공격력 : {player.AttackPower}");
            else
            Console.WriteLine($"공격력 : {player.AttackPower} (+{plusAtk})");

            Console.SetCursorPosition(30, 6);

            if (plusDef == 0)
            Console.WriteLine($"방어력 : {player.Defense}");
            else
            Console.WriteLine($"방어력 : {player.Defense} (+{plusDef})");

            Console.SetCursorPosition(30, 7);

            if (player.Health == player.MaxHealth)
            Console.WriteLine($"체력 : {player.Health}");
            else
            Console.WriteLine($"체력 : {player.Health}/{player.MaxHealth}");

            Console.SetCursorPosition(30, 8);
            Console.WriteLine($"Gold : {player.Gold}");
            Console.WriteLine("");
        }





        ///------------------------------------------------↓↓호출하시면 돼요↓↓------------------------------------------------------///




        public (int tier, int specialEffect) GetEquippedWeaponEffect()                          //착용중인 무기의 티어와 특수효과를 반환
        {                                                                                       //tier는 1~5  specialEffect는 0~4 0없음 1독(지속데미지) 2빙결(턴넘김) 3감전(피격시 추가 데미지) 4화상(공격시 데미지)
            // "무기" 슬롯에 장착된 장비가 있는지 확인
            if (equippedSlots.ContainsKey("무기") && equippedSlots["무기"] != "-")
            {
                string equippedWeaponName = equippedSlots["무기"];
                Equipment equippedWeapon = equipmentList.Find(e => e.Name == equippedWeaponName);

                if (equippedWeapon != null)
                {
                    return (equippedWeapon.Tier, equippedWeapon.SpecialEffect);
                }
            }

            // 무기를 착용하지 않았을 경우 (-1, -1) 반환
            return (-1, -1);
        }


        // AddInventory(10001, 1);  인벤에 전리품 추가하기(도감id, 수량);    슬라임 점액 1개가 인벤에 추가됨


        public string UseConsumableScene()          //전투중 소모품 사용하기 (힐링포션선택시 사용. case2 맹독포션선택시 "poison" 반환.  case3 나가기선택시 "" 반환.)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 소모품 사용 =====\n");

                // 보유 중인 소모품 출력
                if (consumableList.Count == 0)
                {
                    Console.WriteLine("보유 중인 소모품이 없습니다.\n");
                    Console.WriteLine("0. 나가기");
                    Console.Write(">> ");
                    Console.ReadLine();
                    return "";
                }

                Dictionary<int, ConsumableItem> itemMap = new Dictionary<int, ConsumableItem>();
                int index = 1;

                foreach (var item in consumableList)
                {
                    Console.WriteLine($"{index}. {item.Name} | {item.Description} | 보유량 {item.Amount}개");
                    itemMap[index++] = item;
                }

                Console.WriteLine("\n0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0") return ""; // 나가기 선택 시 빈 문자열 반환

                if (int.TryParse(input, out int itemIndex) && itemMap.ContainsKey(itemIndex))
                {
                    ConsumableItem selectedItem = itemMap[itemIndex];

                    // 힐링 포션 사용 (체력 회복)
                    if (selectedItem.Value > 0)
                    {
                        player.Health += selectedItem.Value;
                        if (player.Health > player.MaxHealth) player.Health = player.MaxHealth;
                        Console.WriteLine($"{selectedItem.Name}을 사용하여 체력을 {selectedItem.Value} 회복했습니다!");
                        RemoveInventory(selectedItem.Id, 1);
                        Console.ReadKey();
                        return "";
                    }

                    // 맹독포션 사용 (전투 중 효과 적용)
                    if (selectedItem.Name.Contains("맹독포션"))
                    {
                        Console.WriteLine("맹독포션을 사용하여 무기에 독을 바릅니다!");
                        RemoveInventory(selectedItem.Id, 1);
                        Console.ReadKey();
                        return "poison"; // Battle.cs에서 상태이상 적용 가능
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                }
            }
        }       

    }
}