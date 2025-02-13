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
            inven = SceneManager.instance.inventory;
            player = SceneManager.instance.player;
            RefreshQuest();


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
            AudioManager.Instance.PlaySFX("click");
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
            foreach (var item in quest.Items)
            {
                MiscItem misc = MiscItem.GetMiscCatalog().Where(x => x.Id == item.Item1).First();

                Console.Write($"{misc.Name} {item.Item2}개 \t");
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
                    Console.WriteLine($"{i++}. {item.name}\t");
                }
                Console.WriteLine("\n0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();
                AudioManager.Instance.PlaySFX("click");
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
            // 원래 아이템(equip) 해제 및 삭제
            // 그에 맞는 재료 삭제, 돈 감소
            // 업그레이드 아이템 획득 및 장착 
            // 퀘스트 리스트에서 삭제
            // 새로운 퀘스트 화면 보여주기
            Console.Clear();
            if(inven.IsEquipped(q.baseEquip.Id))
            {
                inven.UnequipItemById(q.baseEquip.Id);
            }

            inven.RemoveInventory(q.baseEquip.Id, 1);
            foreach (var item in q.Items)
            {
                MiscItem misc = MiscItem.GetMiscCatalog().Where(x => x.Id == item.Item1).First();

                inven.RemoveInventory(misc.Id, item.Item2);
            }
            player.Gold -= q.gold;

            inven.AddInventory(q.resultEquip.Id, 1);
            inven.AutoEquip(q.resultEquip);
            AudioManager.Instance.PlaySFX("upgrade");
            myQuest.Remove(q);

            DataLoad.SaveAllData();

            Console.WriteLine("\n0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();
            AudioManager.Instance.PlaySFX("click");
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
                i += 100000;

                AddQuest(i);
            }

        }


        // 저장기능 넣을거면 json으로 가져오는 내용으로 교체 필요
        public void SetBasicQuest()
        {
            //SetOriginQuest();
        }

        private bool CheckQuestClear(Quest q)
        {
            if (q == null)
                return false;

            List<(int, int)> itemList = q.Items;

            foreach (var item in itemList)
            {
                bool b = inven.GetMiscList().Where(x => x.Id == item.Item1).Select(y => y.Amount >= item.Item2).FirstOrDefault();

                b = b && player.Gold > q.gold;

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

        void SetOriginQuest()
        {
            Quest quest101 = new Quest()
            {
                index = 100101,
                name = "초심자의 목검 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 101).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 102).First(),
                Items = { (10001, 2), (10002, 3) }
            };

            Quest quest102 = new Quest()
            {
                index = 100102,
                name = "그레이트 클럽 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 102).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 103).First(),
                Items = { (10002, 2), (10004, 3) }
            };

            Quest quest103 = new Quest()
            {
                index = 100103,
                name = "리치 본 소드 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 103).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 104).First(),
                Items = { (10008, 2), (10010, 3) }
            };

            Quest quest104 = new Quest()
            {
                index = 100104,
                name = "크라켄 슬래셔 업그레이드",
                canClear = false,
                gold = 400,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 104).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 105).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };

            Quest quest105 = new Quest()
            {
                index = 100105,
                name = "아이스 브레이커 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 105).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 106).First(),
                Items = { (10013, 2), (10015, 3), (10051, 7) }
            };

            Quest quest111 = new Quest()
            {
                index = 100111,
                name = "초심자의 단검 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 111).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 112).First(),
                Items = { (10001, 2), (10002, 3) }
            };

            Quest quest112 = new Quest()
            {
                index = 100112,
                name = "더블핑거 대거 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 112).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 113).First(),
                Items = { (10002, 2), (10004, 3) }
            };

            Quest quest113 = new Quest()
            {
                index = 100113,
                name = "사령의 단검 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 113).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 114).First(),
                Items = { (10008, 2), (10010, 3) }
            };

            Quest quest114 = new Quest()
            {
                index = 100114,
                name = "촉수 대거 업그레이드",
                canClear = false,
                gold = 400,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 114).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 115).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };

            Quest quest115 = new Quest()
            {
                index = 100115,
                name = "아이스 클로 대거 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 115).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 116).First(),
                Items = { (10013, 2), (10015, 3), (10051, 7) }
            };

            Quest quest121 = new Quest()
            {
                index = 100121,
                name = "초심자의 활 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 121).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 122).First(),
                Items = { (10001, 2), (10002, 3) }
            };

            Quest quest122 = new Quest()
            {
                index = 100122,
                name = "쉘 그레이트 보우 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 122).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 123).First(),
                Items = { (10002, 2), (10004, 3) }
            };

            Quest quest123 = new Quest()
            {
                index = 100123,
                name = "저주의 장궁 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 123).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 124).First(),
                Items = { (10008, 2), (10010, 3) }
            };

            Quest quest124 = new Quest()
            {
                index = 100124,
                name = "심연의 활 업그레이드",
                canClear = false,
                gold = 400,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 124).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 125).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };

            Quest quest125 = new Quest()
            {
                index = 100125,
                name = "프로스트 윙 보우 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 125).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 126).First(),
                Items = { (10013, 2), (10015, 3), (10051, 7) }
            };

            Quest quest301 = new Quest()
            {
                index = 100301,
                name = "천 두건 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 301).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 302).First(),
                Items = { (10001, 3) }
            };
            Quest quest302 = new Quest()
            {
                index = 100302,
                name = "촉촉한 머리띠 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 302).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 303).First(),
                Items = { (10001, 3) }
            };
            Quest quest303 = new Quest()
            {
                index = 100303,
                name = "해골 투구 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 303).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 304).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };
            Quest quest304 = new Quest()
            {
                index = 100304,
                name = "전갈의 투구 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 304).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 305).First(),
                Items = { (10010, 2), (10011, 3), (10051, 5) }
            };
            Quest quest305 = new Quest()
            {
                index = 100305,
                name = "설인의 모피 투구 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 305).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 306).First(),
                Items = { (10010, 2), (10011, 3), (10052, 10) }
            };


            Quest quest401 = new Quest()
            {
                index = 100401,
                name = "천 옷 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 401).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 402).First(),
                Items = { (10001, 3) }
            };
            Quest quest402 = new Quest()
            {
                index = 100402,
                name = "나무 갑옷 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 402).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 403).First(),
                Items = { (10001, 3) }
            };
            Quest quest403 = new Quest()
            {
                index = 100403,
                name = "거짓된 갑옷 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 403).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 404).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };
            Quest quest404 = new Quest()
            {
                index = 100404,
                name = "촉수 갑옷 업그레이드",
                canClear = false,
                gold = 400,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 404).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 405).First(),
                Items = { (10010, 2), (10011, 3), (10051, 5) }
            };
            Quest quest405 = new Quest()
            {
                index = 100405,
                name = "얼음 정령의 결정 갑옷 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 405).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 406).First(),
                Items = { (10010, 2), (10011, 3), (10052, 10) }
            };


            Quest quest501 = new Quest()
            {
                index = 100501,
                name = "천 장갑 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 501).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 502).First(),
                Items = { (10001, 3) }
            };
            Quest quest502 = new Quest()
            {
                index = 100502,
                name = "그린 건틀릿 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 502).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 503).First(),
                Items = { (10001, 3) }
            };
            Quest quest503 = new Quest()
            {
                index = 100503,
                name = "임프의 손장갑 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 503).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 504).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };
            Quest quest504 = new Quest()
            {
                index = 100504,
                name = "머맨의 수중 장갑 업그레이드",
                canClear = false,
                gold = 400,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 504).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 505).First(),
                Items = { (10010, 2), (10011, 3), (10051, 5) }
            };
            Quest quest505 = new Quest()
            {
                index = 100505,
                name = "프로스트 울프의 발톱 장갑 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 505).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 506).First(),
                Items = { (10010, 2), (10011, 3), (10052, 10) }
            };



            Quest quest601 = new Quest()
            {
                index = 100601,
                name = "천 신발 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 601).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 602).First(),
                Items = { (10001, 3) }
            };
            Quest quest602 = new Quest()
            {
                index = 100602,
                name = "미끈거리는 신발 업그레이드",
                canClear = false,
                gold = 200,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 602).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 603).First(),
                Items = { (10001, 3) }
            };
            Quest quest603 = new Quest()
            {
                index = 100603,
                name = "유령신발 업그레이드",
                canClear = false,
                gold = 300,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 603).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 604).First(),
                Items = { (10010, 2), (10011, 3), (10050, 5) }
            };
            Quest quest604 = new Quest()
            {
                index = 100604,
                name = "해적의 망령 신발 업그레이드",
                canClear = false,
                gold = 400,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 604).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 605).First(),
                Items = { (10010, 2), (10011, 3), (10051, 5) }
            };
            Quest quest605 = new Quest()
            {
                index = 100605,
                name = "눈보라의 신발 업그레이드",
                canClear = false,
                gold = 500,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 605).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 606).First(),
                Items = { (10010, 2), (10011, 3), (10052, 10) }
            };

            allQuest.Add(quest101);
            allQuest.Add(quest102);
            allQuest.Add(quest103);
            allQuest.Add(quest104);
            allQuest.Add(quest105);

            allQuest.Add(quest111);
            allQuest.Add(quest112);
            allQuest.Add(quest113);
            allQuest.Add(quest114);
            allQuest.Add(quest115);

            allQuest.Add(quest121);
            allQuest.Add(quest122);
            allQuest.Add(quest123);
            allQuest.Add(quest124);
            allQuest.Add(quest125);

            allQuest.Add(quest301);
            allQuest.Add(quest302);
            allQuest.Add(quest303);
            allQuest.Add(quest304);
            allQuest.Add(quest305);

            allQuest.Add(quest401);
            allQuest.Add(quest402);
            allQuest.Add(quest403);
            allQuest.Add(quest404);
            allQuest.Add(quest405);

            allQuest.Add(quest501);
            allQuest.Add(quest502);
            allQuest.Add(quest503);
            allQuest.Add(quest504);
            allQuest.Add(quest505);

            allQuest.Add(quest601);
            allQuest.Add(quest602);
            allQuest.Add(quest603);
            allQuest.Add(quest604);
            allQuest.Add(quest605);

            DataLoad.SaveQuestData();
        }

        public List<Quest> GetQuestList()
        {
            return allQuest;
        }

        public void SetQuestList(List<Quest> list)
        {
            allQuest = list;
        }
    }

    public class Quest
    {
        public int index { get; set; }
        public string name { get; set; }
        public bool canClear { get; set; }

        // 클리어 조건 데이터 필요
        // 보상 관련 데이터 필요


        // 임시 퀘스트클리어조건
        public List<(int,int)> Items { get; set; } = new List<(int,int)>();

        // 기반 아이템
        public Equipment baseEquip { get; set; }

        // 소모 골드
        public int gold { get; set; }

        // 업그레이드 보상 아이템
        public Equipment resultEquip { get; set; }
    }
}
