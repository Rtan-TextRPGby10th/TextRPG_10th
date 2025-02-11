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

        public static void LoadMiscItemData()
        {
            // 소재아이템 데이터 로드
            string path = miscItemPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                MiscItem.SetMiscItem(JsonConvert.DeserializeObject<List<MiscItem>>(json));

                MiscItem.GetMiscCatalog();
            }
        }

        public static void LoadConsumeItemData()
        {
            // 소재아이템 데이터 로드
            string path = consumeItemPath;

            if (File.Exists(path))
            {
                string json= File.ReadAllText(path);

                ConsumableItem.SetConsumeItem(JsonConvert.DeserializeObject<List<ConsumableItem>>(json));

                ConsumableItem.GetItemCatalog();
            }
        }
        public static void LoadEquipItemData()
        {
            // 소재아이템 데이터 로드
            string path = equipItemPath;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                Equipment.SetEquipItem(JsonConvert.DeserializeObject<List<Equipment>>(json));

                Equipment.GetEquipmentCatalog();
            }
        }
    }
}
