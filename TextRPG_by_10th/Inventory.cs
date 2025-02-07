using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace TextRPG_by_10th
{
    public class Inventory
    {
        static public List<Equipment> equipmentList = Equipment.GetEquipmentCatalog(); // 장비 리스트
        static public List<ConsumableItem> consumableList = ConsumableItem.GetItemCatalog(); // 소모품 리스트
        static public List<MiscItem> miscList = MiscItem.GetMiscCatalog(); // 기타 아이템 리스트
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

        // 생성자
        public Inventory()
        {
            foreach (var item in equipmentList)
            {
                equippedItems[item.Name] = item.IsEquipped;
            }
        }

        // 인벤토리 표시
        public void ShowInventory()
        {
            Console.Clear();
            Console.WriteLine("===== 인벤토리 =====");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("   <장비>");
            if (equipmentList.Count == 0)
                Console.WriteLine("        x");
            else
                foreach (var item in equipmentList)
                    Console.WriteLine($"        {item.Name} {item.Description}" + (equippedItems[item.Name] ? " (장착 중)" : ""));

            Console.WriteLine("\n   <소모품>");
            if (consumableList.Count == 0)
                Console.WriteLine("        x");
            else
                foreach (var item in consumableList)
                    Console.WriteLine($"        {item}");

            Console.WriteLine("\n   <기타>");
            if (miscList.Count == 0)
                Console.WriteLine("        x");
            else
                foreach (var item in miscList)
                    Console.WriteLine($"        {item}");

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "1")
                ShowEquipmentScreen();

            if (input == "0")
            {
                SceneManager.instance.currentScene = SceneManager.Scene.Start;
                return;
            }
        }

        // 장착 관리 화면
        public void ShowEquipmentScreen()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 장착 관리 =====\n");

                // 장착 상태 출력
                foreach (var slot in equippedSlots)
                {
                    Console.WriteLine($"{slot.Key} : {slot.Value}");
                }

                Console.WriteLine("\n===== 장비 목록 =====");
                for (int i = 0; i < equipmentList.Count; i++)
                {
                    Equipment item = equipmentList[i];
                    string equipStatus = equippedItems[item.Name] ? "[E]" : ""; // 장착 여부 표시

                    Console.WriteLine($"{i + 1}. {equipStatus} {item.Name} {item.Description}");
                }

                Console.WriteLine("0. 뒤로 가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0")
                { 
                    break;
                }

                if (int.TryParse(input, out int index) && index > 0 && index <= equipmentList.Count)
                {
                    EquipItem(index - 1);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }

        // 장비 착용 / 해제
        private void EquipItem(int index)
        {
            Equipment item = equipmentList[index];
            string equipSlot = item.Slot;

            if (equippedItems[item.Name])
            {
                equippedItems[item.Name] = false;
                equippedSlots[equipSlot] = "-"; // 슬롯 초기화

            }
            else
            {
                // 기존에 같은 슬롯에 장착된 아이템이 있는지 확인하고 해제
                foreach (var key in equippedSlots.Keys)
                {
                    if (key == equipSlot && equippedSlots[key] != "-")
                    {
                        string equippedItem = equippedSlots[key];
                        equippedItems[equippedItem] = false;
                        equippedSlots[key] = "-";
                    }
                }

                equippedItems[item.Name] = true;
                equippedSlots[equipSlot] = item.Name; // 슬롯에 아이템 이름 표시

            }


        }

        public void AddEquipment(Equipment item)
        {
            equipmentList.Add(item);
            Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
        }

        public void RemoveEquipment(Equipment item)
        {
            if (equipmentList.Contains(item))
            {
                equipmentList.Remove(item);
                Console.WriteLine($"{item.Name}을(를) 인벤토리에서 제거했습니다.");
            }
        }

        public void AddConsumable(ConsumableItem item)
        {
            foreach (var existingItem in consumableList)
            {
                if (existingItem.Name == item.Name)
                {
                    existingItem.Amount += 1;
                    Console.WriteLine($"{item.Name} 수량이 증가했습니다. (x{existingItem.Amount})");
                    return;
                }
            }
            consumableList.Add(item);
            Console.WriteLine($"{item.Name}을(를) 인벤토리에 추가했습니다.");
        }

        public List<Equipment> GetEquipmentList()
        {
            return equipmentList;
        }


        //장착중인 장비 공격력 총합
        public int GetTotalAtk()
        {
            int totalAtk = 0;

            foreach (var item in equipmentList)
            {
                if (equippedItems[item.Name]) // 장착된 아이템만 계산
                {
                    totalAtk += item.Atk;
                }
            }

            return totalAtk;
        }

        //장착중인 장비 방어력 총합
        public int GetTotalDef()
        {
            int totalDef = 0;

            foreach (var item in equipmentList)
            {
                if (equippedItems[item.Name]) // 장착된 아이템만 계산
                {
                    totalDef += item.Def;
                }
            }

            return totalDef;
        }


    }
}
