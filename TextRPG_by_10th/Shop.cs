using System;
using System.Collections.Generic;

namespace TextRPG_by_10th
{
    public class Shop
    {
        private int gold = 15000000; // 플레이어 보유 골드
        private Inventory inventory;
        private List<Equipment> shopEquipments;
        private List<ConsumableItem> shopConsumables;
        private HashSet<string> soldOutItems = new HashSet<string>();

        public Shop(Inventory playerInventory)
        {
            inventory = playerInventory;
            shopEquipments = Equipment.GetEquipmentCatalog();
            shopConsumables = ConsumableItem.GetItemCatalog();
        }

        public void OpenShop()
        {
            while (SceneManager.instance.currentScene == SceneManager.Scene.Shop)
            {
                Console.Clear();
                Console.WriteLine("===== 상점 =====");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드] {gold}G\n");
                Console.WriteLine("[아이템 목록]");

                int index = 1;
                Dictionary<int, object> itemMap = new Dictionary<int, object>();

                foreach (var item in shopEquipments)
                {
                    string priceText = soldOutItems.Contains(item.Name) ? "구매완료" : $"{item.Price}G";
                    Console.WriteLine($"- {item.Name} {item.Description} | {priceText}");
                    if (!soldOutItems.Contains(item.Name))
                        itemMap[index++] = item;
                }

                foreach (var item in shopConsumables)
                {
                    Console.WriteLine($"- {item.Name} | {item.Description} | {item.Price}G");
                    itemMap[index++] = item;
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "1") BuyItem(itemMap);
                else if (input == "2") SellItem();
                else if (input == "0") SceneManager.instance.currentScene = SceneManager.Scene.Start;
                else Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
            }
        }

        private void BuyItem(Dictionary<int, object> itemMap)
        {
            Console.Clear();
            Console.WriteLine("===== 아이템 구매 =====\n");
            foreach (var pair in itemMap)
            {
                if (pair.Value is Equipment item)
                    Console.WriteLine($"{pair.Key}. {item.Name} {item.Description} | {item.Price}G");
                else if (pair.Value is ConsumableItem consumableItem)
                    Console.WriteLine($"{pair.Key}. {consumableItem.Name} | {consumableItem.Description} | {consumableItem.Price}G");
            }

            Console.Write(">> ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int itemIndex) && itemMap.ContainsKey(itemIndex))
            {
                if (itemMap[itemIndex] is Equipment equipment)
                {
                    if (gold >= equipment.Price)
                    {
                        gold -= equipment.Price;
                        inventory.AddEquipment(equipment);
                        soldOutItems.Add(equipment.Name);
                        Console.WriteLine($"{equipment.Name} 구매완료!");
                    }
                    else Console.WriteLine("골드가 부족합니다.");
                }
                else if (itemMap[itemIndex] is ConsumableItem consumable)
                {
                    Console.Write("얼마나 구매하시겠습니까? (최대 99개) >> ");
                    string amountInput = Console.ReadLine();
                    if (int.TryParse(amountInput, out int amount) && amount > 0 && amount <= 99)
                    {
                        int totalPrice = consumable.Price * amount;
                        if (gold >= totalPrice)
                        {
                            gold -= totalPrice;
                            for (int i = 0; i < amount; i++)
                                inventory.AddConsumable(consumable);
                            Console.WriteLine($"{consumable.Name} {amount}개 구매완료!");
                        }
                        else Console.WriteLine("골드가 부족합니다.");
                    }
                    else Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else Console.WriteLine("잘못된 입력입니다.");
            Console.ReadKey();
        }

        private void SellItem()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 아이템 판매 =====\n");
                Console.WriteLine("[보유 골드] " + gold + " G\n");
                List<Equipment> playerEquipments = inventory.GetEquipmentList();

                if (playerEquipments.Count == 0)
                {
                    Console.WriteLine("판매할 장비가 없습니다.");
                    Console.ReadKey();
                    return;
                }

                Dictionary<int, Equipment> sellableItems = new Dictionary<int, Equipment>();
                int index = 1;
                foreach (var item in playerEquipments)
                {
                    int sellPrice = item.Price / 2;
                    Console.WriteLine($"{index}. {item.Name} {item.Description} | 판매가 {sellPrice}G");
                    sellableItems[index++] = item;
                }

                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0") return;
                if (int.TryParse(input, out int itemIndex) && sellableItems.ContainsKey(itemIndex))
                {
                    Equipment item = sellableItems[itemIndex];
                    gold += item.Price / 2;
                    inventory.RemoveEquipment(item);
                    Console.WriteLine($"{item.Name} 판매완료!");
                }
                else Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }
        }
    }
}
