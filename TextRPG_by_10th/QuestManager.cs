using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextRPG_by_10th
{
    public class QuestManager
    {
        private List<Quest> allQuest = new List<Quest>();
        private List<Quest> myQuest = new List<Quest>();

        private Inventory inven;

        public void ShowQuestList()
        {
            // 진행중인 모든 퀘스트 나열하기
            Console.Clear();
            Console.WriteLine("===== 장비 업그레이드 =====");
            Console.WriteLine("현재 착용중인 장비를 업그레이드 할 수 있습니다.\n");

            foreach (var item in myQuest)
            {
                ShowQuestDetail(item);
            }

            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "1")                       // 대장간으로 이동?
            {

            }
            else if (input == "0")                  // 메인 씬
            {
                SceneManager.instance.currentScene = SceneManager.Scene.Start;
                SceneManager.instance.GameScecne(SceneManager.Scene.Start);
                return;
            }

        }

        private void ShowQuestDetail(Quest quest)
        {
            Console.WriteLine($" - {quest.name}\t {CheckQuestClear(quest)}");

            Console.WriteLine("퀘스트 클리어 조건 : ");
            foreach (var item in quest.miscItems)
            {
                Console.Write($"{item.Name} {item.Amount}개 \t");
            }
            Console.WriteLine();
            Console.WriteLine();

        }

        public void AddQuest(int index)
        {
            Quest q = allQuest.FirstOrDefault(x => x.index == index);

            if (myQuest.Contains(q))
            {
                Console.WriteLine("이미 가지고 있는 퀘스트입니다.");
                return;
            }

            myQuest.Add(q);
        }

        public void RefreshQuest()
        {
            myQuest.Clear();
            List<Equipment> list = inven.GetEquipmentList();

            foreach (var item in list)
            {
                int i = item.Id;
                i += 99000;

                AddQuest(i);
            }

        }


        // 저장기능 넣을거면 json으로 가져오는 내용으로 교체 필요
        public void SetBasicQuest()
        {
            Quest quest1 = new Quest()
            {
                index = 99101,
                name = "초심자의 목검 업그레이드",
                des = "설명1",
                canClear = false,
                miscItems = new List<MiscItem>()
            };

            MiscItem item = MiscItem.GetMiscCatalog().First();
            MiscItem item3 = MiscItem.GetMiscCatalog().ElementAt(1);
            item.Amount = 2;
            item3.Amount = 3;
            quest1.miscItems.Add(item);
            quest1.miscItems.Add(item3);


            Quest quest2 = new Quest()
            {
                index = 99201,
                name = "널빤지 업그레이드",
                des = "설명2",
                canClear = false,
                miscItems = new List<MiscItem>()
            };


            MiscItem item2 = MiscItem.GetMiscCatalog().First();
            item2.Amount = 7;
            quest2.miscItems.Add(item2);

            Quest quest3 = new Quest()
            {
                index = 99401,
                name = "천 옷 업그레이드",
                des = "설명3",
                canClear = false,
                miscItems = new List<MiscItem>()
            };
            MiscItem item4 = MiscItem.GetMiscCatalog().LastOrDefault();
            item4.Amount = 1;
            quest3.miscItems.Add(item4);



            allQuest.Add(quest1);
            allQuest.Add(quest2);
            allQuest.Add(quest3);

            inven = SceneManager.instance.inventory;
            RefreshQuest();
        }

        private string CheckQuestClear(Quest q)
        {
            List<MiscItem> list = q.miscItems;

            foreach (MiscItem item in list)
            {
                bool b = inven.GetMiscList().Where(x => x.Id == item.Id).Select(y => y.Amount >= item.Amount).FirstOrDefault();

                if (b)
                {
                    continue;
                }
                else
                {
                    return "재료 부족";
                }
            }
            return "업그레이드 가능";
        }

    }

    public class Quest
    {
        public int index { get; set; }
        public string name { get; set; }
        public string des { get; set; }
        public bool canClear { get; set; }

        // 클리어 조건 데이터 필요
        // 보상 관련 데이터 필요


        // 임시 퀘스트클리어조건
        public List<MiscItem> miscItems { get; set; }

        // 기반 아이템
        public Equipment equipment { get; set; }

        // 소모 골드
        public int gold { get; set; }
    }
}
