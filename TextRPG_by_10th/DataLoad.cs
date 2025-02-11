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
        private static string QuestPath = "../../../../TextRPG_by_10th/QuestData.json";


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

            string path = QuestPath;

            string json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }




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
            string path = QuestPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                SceneManager.instance.questManager.SetQuestList(JsonConvert.DeserializeObject<List<Quest>>(json));

                SceneManager.instance.questManager.GetQuestList();
            }
        }
    }
}
