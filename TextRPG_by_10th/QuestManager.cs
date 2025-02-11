using System;
using System.Collections;
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

        private Player player;

        // 퀘스트 -> 장비 업그레이드로 변경
        public void ShowQuestList()
        {
            // 진행중인 모든 퀘스트 나열하기
            Console.Clear();
            Console.WriteLine("===== 장비 업그레이드 =====");
            Console.WriteLine("현재 착용중인 장비를 업그레이드 할 수 있습니다.\n");
            Console.WriteLine($"소지 골드 : {player.Gold}\n");
            foreach (var item in myQuest)
            {
                ShowQuestDetail(item);
            }

            Console.WriteLine("1. 업그레이드하기");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "1")                       // 업그레이드 화면으로 이동
            {
                ShowUpgradeList();
            }
            else if (input == "0")                  // 메인 씬
            {
                SceneManager.instance.currentScene = SceneManager.Scene.Start;
                SceneManager.instance.GameScecne(SceneManager.Scene.Start);
                return;
            }


        }

        private void ShowUpgradeList()
        {
            Console.Clear();
            Console.WriteLine("===== 장비 업그레이드 =====");
            Console.WriteLine("현재 착용중인 장비를 업그레이드 할 수 있습니다.\n");
            Console.WriteLine($"소지 골드 : {SceneManager.instance.player.Gold}\n");


            ShowUpgradeDetail();
        }

        private void ShowQuestDetail(Quest quest)
        {
            if (quest == null)
                return;

            string str = CheckQuestClear(quest)? "업그레이드 가능":"재료 부족";

            Console.WriteLine($" - {quest.name}\t {str}");

            Console.WriteLine("업그레이드 재료 : ");
            foreach (var item in quest.miscItems)
            {
                Console.Write($"{item.Name} {item.Amount}개 \t");
            }
            Console.Write($"{quest.gold}G");

            Console.WriteLine();
            Console.WriteLine();
        }

        private void ShowUpgradeDetail()
        {
            while(true) 
            {
                int i = 1;

                foreach (var item in myQuest)
                {
                    Console.WriteLine($"{i++}. {item.name}\t{item.des}");
                }
                Console.WriteLine("\n0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") return;

                int index = int.Parse(input);
                index--;

                Quest q = myQuest.ElementAt(index);

                if (CheckQuestClear(q))
                {
                    Console.WriteLine($"장비 강화");
                    UpgradeEquip(q);
                    return;
                }
                else
                {
                    Console.WriteLine("재료가 부족합니다");
                }
            }
        }

        private void UpgradeEquip(Quest q)
        {
            // 원래 아이템(equip 삭제)
            // 그에 맞는 재료 삭제, 돈 감소
            // 업그레이드 아이템 획득 및 장착 
            // 퀘스트 리스트에서 삭제
            // 새로운 퀘스트 화면 보여주기
            Console.Clear();
            inven.RemoveInventory(q.baseEquip.Id, 1);
            foreach (var item in q.miscItems)
            {
                inven.RemoveInventory(item.Id, item.Amount);
            }
            player.Gold -= q.gold;

            inven.AddInventory(q.resultEquip.Id, 1);
            myQuest.Remove(q);

            Console.WriteLine("\n0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();
            if (input == "0") return;


        }

        public void AddQuest(int index)
        {
            Quest q = allQuest.FirstOrDefault(x => x.index == index);

            if (myQuest.Contains(q))
            {
                Console.WriteLine("이미 가지고 있는 퀘스트입니다.");
                return;
            }
            else if(q==null)
            {
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
                miscItems = new List<MiscItem>(),
                gold = 100,
                baseEquip=Equipment.GetEquipmentCatalog().Where(item=>item.Id==101).First(),
                resultEquip=Equipment.GetEquipmentCatalog().Where(item=>item.Id==107).First()
            };

            MiscItem i = new MiscItem()
            {
                Id = 10001,
                Name = "슬라임의 점액",
                Amount = 2
            };
            MiscItem i2 = new MiscItem()
            {
                Id = 10002,
                Name = "고블린의 가죽",
                Amount = 3
            };
            quest1.miscItems.Add(i);
            quest1.miscItems.Add(i2);


            Quest quest2 = new Quest()
            {
                index = 99301,
                name = "천 두건 업그레이드",
                des = "설명2",
                canClear = false,
                miscItems = new List<MiscItem>(),
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 301).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 302).First()
            };

            MiscItem i3 = new MiscItem()
            {
                Id = 10001,
                Name = "슬라임의 점액",
                Amount = 7
            };
            quest2.miscItems.Add(i3);

            Quest quest3 = new Quest()
            {
                index = 99401,
                name = "천 옷 업그레이드",
                des = "설명3",
                canClear = false,
                miscItems = new List<MiscItem>(),
                gold = 6000000,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 401).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 402).First()
            };
            MiscItem i4 = new MiscItem()
            {
                Id = 10001,
                Name = "10052",
                Amount = 1
            };
            quest3.miscItems.Add(i4);


            allQuest.Add(quest1);
            allQuest.Add(quest2);
            allQuest.Add(quest3);

            inven = SceneManager.instance.inventory;
            player = SceneManager.instance.player;
            RefreshQuest();
        }

        private bool CheckQuestClear(Quest q)
        {
            if (q == null)
                return false;

            List<MiscItem> list = q.miscItems;

            foreach (MiscItem item in list)
            {
                bool b = inven.GetMiscList().Where(x => x.Id == item.Id).Select(y => y.Amount >= item.Amount).FirstOrDefault();

                b = b && inven.player.Gold > q.gold;

                if (b)
                {
                    continue;
                }
                else
                {
                    return false; 
                }
            }

            return true; 
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
        public Equipment baseEquip { get; set; }

        // 소모 골드
        public int gold { get; set; }

        // 업그레이드 보상 아이템
        public Equipment resultEquip { get; set; }
    }
}
