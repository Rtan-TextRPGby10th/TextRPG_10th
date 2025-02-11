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

        public static void SaveMiscItemData()
        {
            // 소재 데이터 생성(1회용)
            var ab= MiscItem.GetMiscCatalog();

            string path = miscItemPath;

            string json = JsonConvert.SerializeObject(ab);

            File.WriteAllText(path, json);
        }

        public static void LoadMiscItemData()
        {
            // 소재아이템 데이터 로드

            string path = miscItemPath;

            if (File.Exists(path))
            {
                string json= File.ReadAllText(path);

                MiscItem.SetMiscItem(JsonConvert.DeserializeObject<List<MiscItem>>(json));


                MiscItem.GetMiscCatalog();
            }


        }
    }
}
