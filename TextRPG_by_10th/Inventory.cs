using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace TextRPG_by_10th
{
    
    public class Inventory
    {
        private static bool isInitialized = false;           //최초 실행시 인벤토리에 기본아이템 추가.

        static List<Equipment> equipmentList = new List<Equipment>();
        static List<ConsumableItem> consumableList = new List<ConsumableItem>();
        static List<MiscItem> miscList = new List<MiscItem>();

        static Dictionary<string, bool> equippedItems = new Dictionary<string, bool>(); // 장착 상태 관리
        static Dictionary<string, string> equippedSlots = new Dictionary<string, string>()
        {
            {"머리", "-" },
            {"몸통", "-" },
            {"손", "-" },
            {"다리", "-" },
            {"무기1", "-" },
            {"무기2", "-" }
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
                equippedItems[item.Name] = item.IsEquipped;
            }

            if (!isInitialized)
            {
                isInitialized = true; //인벤토리에 아이템 추가하기. AddInventory(도감의 id넘버, 수량)
                AddInventory(101, 1);
                AddInventory(401, 1);
                AddInventory(402, 1);
                AddInventory(1001, 3);
                AddInventory(1004, 3);
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

        
        public void ShowInventory()                                         //인벤토리 씬 
        {
            Console.Clear();
            Console.WriteLine("===== 인벤토리 =====");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("   <장비>");
            if (equipmentList.Count == 0)
                Console.WriteLine("        -");
            else
                foreach (var item in equipmentList)
                    Console.WriteLine($"        {item.Name} {item.Description}" + (equippedItems[item.Name] ? " (장착 중)" : ""));

            Console.WriteLine("\n   <소모품>");
            if (consumableList.Count == 0)
                Console.WriteLine("        -");
            else
                foreach (var item in consumableList)
                    Console.WriteLine($"        {item.Name} {item.Description} | {item.Amount}개");

            Console.WriteLine("\n   <기타>");
            if (miscList.Count == 0)
                Console.WriteLine("        -");
            else
                foreach (var item in miscList)
                    Console.WriteLine($"        {item.Name} {item.Description} | {item.Amount}개");

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "1")                       // 장착관리 씬
            {
                ShowEquipmentScene();                   
            }
            else if (input == "0")                  // 메인 씬
            {
                SceneManager.instance.currentScene = SceneManager.Scene.Start;
                SceneManager.instance.GameScecne(SceneManager.Scene.Start);
                return;
            }
        }


        public void ShowEquipmentScene()                            // 장착관리 씬
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 장착 관리 =====\n");

                
                foreach (var slot in equippedSlots)                     // 🔹 현재 장착 상태 출력
                {
                    Console.WriteLine($"{slot.Key} : {slot.Value}");
                }

                Console.WriteLine("\n===== 장비 목록 =====");

                
                for (int i = 0; i < equipmentList.Count; i++)           // 🔹 장비 목록 출력
                {
                    Equipment item = equipmentList[i];
                    string equipStatus = equippedItems[item.Name] ? "[E]" : ""; // 장착 여부 표시
                    Console.WriteLine($"{i + 1}. {equipStatus} {item.Name} {item.Description}");
                }

                Console.WriteLine("0. 뒤로 가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                
                if (input == "0") return;                   // 나가기

                // 🔹 입력값 검증 및 장비 선택
                if (int.TryParse(input, out int index) && index > 0 && index <= equipmentList.Count)
                {
                    EquipItem(index - 1); // 선택한 장비를 장착/해제
                }
                else
                {
                }
            }
        }

        private void EquipItem(int index)
        {
            Equipment item = equipmentList[index];
            string equipSlot = item.Slot;

            // 🔹 이미 장착 중인 경우 해제
            if (equippedItems[item.Name])
            {
                equippedItems[item.Name] = false;
                equippedSlots[equipSlot] = "-"; // 슬롯 초기화
            
            }
            else
            {
                // 🔹 같은 슬롯에 장착된 아이템 해제
                foreach (var key in equippedSlots.Keys)
                {
                    if (key == equipSlot && equippedSlots[key] != "-")
                    {
                        string equippedItem = equippedSlots[key];
                        equippedItems[equippedItem] = false;
                        equippedSlots[key] = "-";
                    }
                }

                // 🔹 새로운 아이템 장착
                equippedItems[item.Name] = true;
                equippedSlots[equipSlot] = item.Name;
            
            }
        }
    }
}