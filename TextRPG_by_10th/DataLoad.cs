using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TextRPG_by_10th
{
    internal class DataLoad
    {

        private static string miscItemPath = "../../../../TextRPG_by_10th/MiscItemData.json";
        private static string equipItemPath = "../../../../TextRPG_by_10th/EquipItemData.json";
        private static string consumeItemPath = "../../../../TextRPG_by_10th/ConsumeItemData.json";
        private static string questPath = "../../../../TextRPG_by_10th/QuestData.json";
        private static string playerPath = Path.GetFullPath("PlayerData.json");
        private static string equipPath = Path.GetFullPath("equipData.json");
        private static string equippedPath = Path.GetFullPath("equippedData.json");
        private static string consumePath = Path.GetFullPath("consumeData.json");
        private static string miscPath = Path.GetFullPath("miscData.json");
        private static string shopMiscPath = "../../../../TextRPG_by_10th/shopMiscData.json";
        private static string shopConsumePath = "../../../../TextRPG_by_10th/shopConsumeData.json";



        #region SaveData
        public static void SaveMiscItemData()
        {
            // 소재 데이터 생성(1회용)
            var data= MiscItem.GetMiscCatalog();

            string path = miscItemPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveEquipItemData()
        {
            // 장비 데이터 생성(1회용)
            var data = Equipment.GetEquipmentCatalog();

            string path = equipItemPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveConsumeItemData()
        {
            // 소재 데이터 생성(1회용)
            var data = ConsumableItem.GetItemCatalog();

            string path = consumeItemPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveQuestData()
        {
            var data = SceneManager.instance.questManager.GetQuestList();

            string path = questPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SavePlayerData()
        {
            var data = SceneManager.instance.player;

            string path = playerPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveEquipData()
        {
            var data = SceneManager.instance.inventory.GetEquipmentList();

            string path = equipPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveConsumeData()
        {
            var data = SceneManager.instance.inventory.GetConsumableList();

            string path = consumePath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveMiscData()
        {
            var data = SceneManager.instance.inventory.GetMiscList();

            string path = miscPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }

        public static void SaveShopConsumeData()
        {
            var data = SceneManager.instance.shop.GetConsumableItemList();

            string path = shopConsumePath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }
        public static void SaveShopMiscData()
        {
            var data = SceneManager.instance.shop.GetMiscItemList();

            string path = shopMiscPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }

        public static void SaveEquippedData()
        {
            var data = SceneManager.instance.inventory.GetEquippedDic();

            string path = equippedPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);

        }

        public static void SaveAllData()
        {
            SavePlayerData();

            SaveEquippedData();
            SaveEquipData();
            SaveMiscData();
            SaveConsumeData();
        }

        #endregion



        #region LoadData

        public static void LoadMiscItemData()
        {
            // 소재아이템 데이터 로드
            string path = miscItemPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                MiscItem.SetMiscCatalog(JsonConvert.DeserializeObject<List<MiscItem>>(json));

            }
        }

        public static void LoadConsumeItemData()
        {
            // 소비아이템 데이터 로드
            string path = consumeItemPath;

            if (File.Exists(path))
            {
                string json= File.ReadAllText(path);

                ConsumableItem.SetConsumeItem(JsonConvert.DeserializeObject<List<ConsumableItem>>(json));

            }
        }
        public static void LoadEquipItemData()
        {
            // 장비아이템 데이터 로드
            string path = equipItemPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                Equipment.SetEquipmentCatalog(JsonConvert.DeserializeObject<List<Equipment>>(json));

            }
        }
        
        public static void LoadQuestData()
        {
            // 퀘스트 데이터 로드
            string path = questPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.questManager.SetQuestList(JsonConvert.DeserializeObject<List<Quest>>(json));

            }
        }
        public static void LoadPlayerData()
        {
            // 플레이어 데이터 로드
            string path = playerPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.SetPlayerData(JsonConvert.DeserializeObject<Player>(json));
            }
        }
        private static void LoadEquipData()
        {
            // 장비 인벤토리 데이터 로드
            string path = equipPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.inventory.SetEquipList(JsonConvert.DeserializeObject<List<Equipment>>(json));
            }
        }
        private static void LoadEquippedData()
        {
            // 장비 착용 인벤토리 데이터 로드
            string path = equippedPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.inventory.SetEquippedDic(JsonConvert.DeserializeObject<Dictionary<string,bool>>(json));
            }
        }

        private static void LoadConsumeData()
        {
            // 장비 인벤토리 데이터 로드
            string path = consumePath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.inventory.SetConsumeList(JsonConvert.DeserializeObject<List<ConsumableItem>>(json));
            }
        }
        private static void LoadMiscData()
        {
            // 장비 인벤토리 데이터 로드
            string path = miscPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.inventory.SetMiscList(JsonConvert.DeserializeObject<List<MiscItem>>(json));
            }
        }

        private static void LoadShopMiscData()
        {
            // 상점 소재 데이터 로드
            string path = shopMiscPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.shop.SetMiscItemList(JsonConvert.DeserializeObject<List<MiscItem>>(json));
            }
        }
        private static void LoadShopConsumeData()
        {
            // 상점 소비 데이터 로드
            string path = shopConsumePath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.shop.SetConsumeableItemList(JsonConvert.DeserializeObject<List<ConsumableItem>>(json));
            }
        }

        public static void LoadShopData()
        {
            LoadShopMiscData();
            LoadShopConsumeData();
        }

        public static void LoadInvenData()
        {
            LoadEquipData();
            LoadConsumeData();
            LoadMiscData();
        }

        public static void LoadPlayerItemData()
        {
            LoadShopData();
            LoadInvenData();
        }
        public static void LoadPlayerEquippedData()
        {
            LoadEquippedData();

        }
    }



    #endregion
}
