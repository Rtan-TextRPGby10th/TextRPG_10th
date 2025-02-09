using System;
using System.Collections.Generic;
using System.Linq;
using static TextRPG_by_10th.SceneManager;

namespace TextRPG_by_10th
{
    public class Shop
    {
        private Inventory inventory;
        private List<Equipment> shopEquipments = new List<Equipment>();
        private List<ConsumableItem> shopConsumables = new List<ConsumableItem>();
        private List<MiscItem> shopMiscItems = new List<MiscItem>();
        public Player player;
        

        public Shop(Inventory playerInventory)
        {
            inventory = playerInventory;
            //상점에서 판매하는 아이템 추가하기. AddShopItem(도감의 id넘버, 수량)

            //레어도 보기위한 샘플 아이템들
            AddShopItem(303);
            AddShopItem(304);
            AddShopItem(305);
            AddShopItem(306);
            //
            AddShopItem(104);
            AddShopItem(301);                                  
            AddShopItem(401);
            AddShopItem(501);
            AddShopItem(601);
            AddShopItem(1001);
            AddShopItem(1004);

            
            Console.WriteLine("\n===== [상점 판매 목록] =====");                              // ✅ 상점 판매 목록 출력 (넘버링 없이 표시)
            foreach (var item in shopEquipments)
                Console.WriteLine($"- {item.Name} |  {item.Description} | {item.Price}G");

            foreach (var item in shopConsumables)
                Console.WriteLine($"- {item.Name} | {item.Description} | {item.Price}G");

            foreach (var item in shopMiscItems)
                Console.WriteLine($"- {item.Name} | {item.Description} | {item.Price}G");

            Console.WriteLine("============================\n");
        }
        

        private string GetStatString(Equipment item)
        {
            return item.Atk > 0 ? $"공격력+{item.Atk}" : $"방어력+{item.Def}";
        }

        
        public void AddShopItem(int id)                                                     // 🔹 **ID에 따라 자동으로 아이템을 상점 목록에 추가**
        {
            if (id >= 100 && id <= 999)  // 장비 아이템 (1회 판매시 목록에서 제거)
            {
                Equipment? item = Equipment.GetEquipmentCatalog().FirstOrDefault(e => e.Id == id);
                if (item != null) shopEquipments.Add(item);
            }
            else if (id >= 1000 && id <= 9999)  // 소모품 아이템 (매진 되지 않음)
            {
                ConsumableItem? item = ConsumableItem.GetItemCatalog().FirstOrDefault(c => c.Id == id);
                if (item != null) shopConsumables.Add(item); // ❌ 삭제되지 않도록 유지
            }
            else if (id >= 10000 && id <= 99999)  // 기타 아이템
            {
                MiscItem? item = MiscItem.GetMiscCatalog().FirstOrDefault(m => m.Id == id);
                if (item != null) shopMiscItems.Add(item);
            }
        }

        public void OpenShop()                                      //상점 씬
        {
            player = SceneManager.instance.player;  // ✅ SceneManager에서 player 가져오기
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 상점 =====");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드] {player.Gold}G\n");

                Console.WriteLine("[아이템 목록]");

                // ✅ 넘버링 없이 출력
                foreach (var item in shopEquipments)
                    Console.WriteLine($"- {item.Name} {item.Description} | {item.Price}G");

                foreach (var item in shopConsumables)
                    Console.WriteLine($"- {item.Name} {item.Description} | {item.Price}G");

                foreach (var item in shopMiscItems)
                    Console.WriteLine($"- {item.Name} {item.Description} | {item.Price}G");

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "1") BuyItem();                        //구매하기 씬
                else if (input == "2") SellItem();                  //판매하기 씬
                else if (input == "0")                              //나가기
                {
                    SceneManager.instance.currentScene = SceneManager.Scene.Start;      //메인 씬
                    SceneManager.instance.GameScecne(SceneManager.Scene.Start);
                    return;
                }
            }
        }


        private void BuyItem()                                          //구매하기 씬
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 아이템 구매 =====\n");

                Dictionary<int, object> itemMap = new Dictionary<int, object>();
                int index = 1;

                index = DisplayItemListWithNumbers(shopEquipments, itemMap, index);
                index = DisplayItemListWithNumbers(shopConsumables, itemMap, index);
                index = DisplayItemListWithNumbers(shopMiscItems, itemMap, index);

                Console.WriteLine("\n0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0") return;

                if (int.TryParse(input, out int itemIndex) && itemMap.ContainsKey(itemIndex))
                {
                    if (itemMap[itemIndex] is Equipment equipment)
                    {
                        if (player.Gold >= equipment.Price)
                        {
                            player.Gold -= equipment.Price;
                            inventory.AddInventory(equipment.Id, 1);
                            shopEquipments.Remove(equipment); // ✅ 장비는 매진
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
                            if (player.Gold >= totalPrice)
                            {
                                player.Gold -= totalPrice;
                                inventory.AddInventory(consumable.Id, amount);
                                Console.WriteLine($"{consumable.Name} {amount}개 구매완료!");
                                // ✅ 소모품은 목록에서 삭제하지 않음
                            }
                            else Console.WriteLine("골드가 부족합니다.");
                        }
                    }
                    else if (itemMap[itemIndex] is MiscItem misc)
                    {
                        if (player.Gold >= misc.Price)
                        {
                            player.Gold -= misc.Price;
                            inventory.AddInventory(misc.Id, 1);
                            shopMiscItems.Remove(misc); // ✅ 기타 아이템은 매진
                            Console.WriteLine($"{misc.Name} 구매완료!");
                        }
                        else Console.WriteLine("골드가 부족합니다.");
                    }
                }
            }
        }

        private int DisplayItemListWithNumbers<T>(List<T> items, Dictionary<int, object> itemMap, int startIndex)       //아이템의 어떤 정보를 출력할지 설정
        {
            int index = startIndex;
            foreach (var item in items)
            {
                if (item is Equipment equipment)
                {
                    Console.WriteLine($"{index}. {equipment.Name} {equipment.Description} | {equipment.Price}G");
                    itemMap[index++] = equipment;
                }
                else if (item is ConsumableItem consumable)
                {
                    Console.WriteLine($"{index}. {consumable.Name} {consumable.Description} | {consumable.Price}G");
                    itemMap[index++] = consumable;
                }
                else if (item is MiscItem misc)
                {
                    Console.WriteLine($"{index}. {misc.Name} {misc.Description} | {misc.Price}G");
                    itemMap[index++] = misc;
                }
            }
            return index;
        }

       

        private void SellItem()                                              //판매하기 씬
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== 아이템 판매 =====\n");
                Console.WriteLine("[보유 골드] " + player.Gold + " G\n");

                Dictionary<int, object> sellableItems = new Dictionary<int, object>();
                int index = 1;

                // 🔹 장비 목록 출력
                foreach (var item in inventory.GetEquipmentList())
                {
                    if (!item.IsEquipped)  // 장착 중이 아닌 아이템만 출력
                    {
                        int sellPrice = item.Price / 2;
                        Console.WriteLine($"{index}. {item.Name} {item.Description} | 판매가 {sellPrice}G");
                        sellableItems[index++] = item;
                    }
                    
                }

                // 🔹 소모품 목록 출력
                foreach (var item in inventory.GetConsumableList())
                {
                    Console.WriteLine($"{index}. {item.Name} | {item.Description} | 보유 {item.Amount}개 | 판매가 {item.Price / 2}G");
                    sellableItems[index++] = item;
                }

                // 🔹 기타 아이템 목록 출력
                foreach (var item in inventory.GetMiscList())
                {
                    Console.WriteLine($"{index}. {item.Name} | {item.Description} | 보유 {item.Amount}개 | 판매가 {item.Price / 2}G");
                    sellableItems[index++] = item;
                }

                Console.WriteLine("\n0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0") return;

                if (int.TryParse(input, out int itemIndex) && sellableItems.ContainsKey(itemIndex))
                {
                    // 🔹 장비 판매 처리
                    if (sellableItems[itemIndex] is Equipment equipment)
                    {
                        player.Gold += equipment.Price / 2;
                        inventory.RemoveInventory(equipment.Id, 1);
                        Console.WriteLine($"{equipment.Name} 판매완료! +{equipment.Price / 2}G");
                    }
                    // 🔹 소모품 판매 처리
                    else if (sellableItems[itemIndex] is ConsumableItem consumable)
                    {
                        Console.Write("몇 개를 판매하시겠습니까? >> ");
                        string amountInput = Console.ReadLine();
                        if (int.TryParse(amountInput, out int amount) && amount > 0 && amount <= consumable.Amount)
                        {
                            player.Gold += (consumable.Price / 2) * amount;
                            inventory.RemoveInventory(consumable.Id, amount);
                            Console.WriteLine($"{consumable.Name} {amount}개 판매완료! +{(consumable.Price / 2) * amount}G");
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                        }
                    }
                    // 🔹 기타 아이템 판매 처리
                    else if (sellableItems[itemIndex] is MiscItem misc)
                    {
                        Console.Write("몇 개를 판매하시겠습니까? >> ");
                        string amountInput = Console.ReadLine();
                        if (int.TryParse(amountInput, out int amount) && amount > 0 && amount <= misc.Amount)
                        {
                            player.Gold += (misc.Price / 2) * amount;
                            inventory.RemoveInventory(misc.Id, amount);
                            Console.WriteLine($"{misc.Name} {amount}개 판매완료! +{(misc.Price / 2) * amount}G");
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                        }
                    }

                    Console.ReadKey(); // 결과를 확인하기 위해 대기
                }
                else
                {
                }
            }
        }

    }
}


