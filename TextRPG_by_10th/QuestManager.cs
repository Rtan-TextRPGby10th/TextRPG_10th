﻿using System;
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
            foreach (var item in q.Items)
            {
                MiscItem misc = MiscItem.GetMiscCatalog().Where(x => x.Id == item.Item1).First();

                inven.RemoveInventory(misc.Id, item.Item2);
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
                i += 100000;

                AddQuest(i);
            }

        }


        // 저장기능 넣을거면 json으로 가져오는 내용으로 교체 필요
        public void SetBasicQuest()
        {
            a();
            inven = SceneManager.instance.inventory;
            player = SceneManager.instance.player;
            RefreshQuest();
        }

        private bool CheckQuestClear(Quest q)
        {
            if (q == null)
                return false;

            List<(int, int)> itemList = q.Items;

            foreach (var item in itemList)
            {
                bool b = inven.GetMiscList().Where(x => x.Id == item.Item1).Select(y => y.Amount >= item.Item2).FirstOrDefault();

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


        void a()
        {
            Quest quest1 = new Quest()
            {
                index = 99101,
                name = "초심자의 목검 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 101).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 107).First(),
                Items = { Tuple.Create(10001, 2), Tuple.Create(10002, 3) }
            };

            allQuest.Add(quest1);


        }


        void a()
        {
            Quest quest1 = new Quest()
            {
                index = 100101,
                name = "초심자의 목검 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 101).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 107).First(),
                Items = { (10001, 2), (10002, 3) }
            };

            Quest quest2 = new Quest()
            {
                index = 100301,
                name = "천 두건 업그레이드",
                canClear = false,
                gold = 100,
                baseEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 301).First(),
                resultEquip = Equipment.GetEquipmentCatalog().Where(item => item.Id == 302).First(),
                Items = { (10001, 3) }
            };

            allQuest.Add(quest1);
            allQuest.Add(quest2);
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
