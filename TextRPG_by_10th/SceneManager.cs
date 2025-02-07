using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    public class SceneManager
    {
        public static SceneManager instance = new SceneManager();

        public Inventory inventory = new Inventory();
        Shop shop;

        SceneManager()
        {
            instance = this;
            inventory = new Inventory();
            shop = new Shop(inventory);
        }

        public enum Scene
        {
            None = 0,
            //시작화면-게임 시작 및 직업 선택 + 저장 및 불러오기
            Start,
            //마을-상태창 확인, 인벤토리, 상점, 던전, 여관 등으로 이동
            Town,
            //상점-장비, 아이템 구매 페이지
            Shop,
            //인벤토리-장비 장착 및 탈착
            Inventory,
            //던전-몬스터와의 전투
            Dungeon,
        }

        public Scene currentScene = Scene.Start;

        public void GameScecne(Scene scene)
        {
            switch(scene)
            {
                case Scene.Start:
                    //게임 시작 기능 실행
                    StartScene();
                    break;
                case Scene.Town:
                    //마을에서 필요한 기능 실행
                    TownScene();
                    break;
                case Scene.Shop:
                    //상점 기능 실행
                    ShopScene();
                    break;
                case Scene.Inventory:
                    //인벤토리 확인
                    InventoryScene();
                    break;
                case Scene.Dungeon:
                    //던전 입장 및 몬스터와의 전투
                    break;
            }
        }

        //각 Scene에서 실행할 기능을 수행하는 함수
        void StartScene()
        {
            Console.WriteLine("게임 시작");
            Inventory playerInventory = new Inventory();

            while (currentScene == Scene.Start)
            {
                Console.Clear();
                Console.WriteLine("===== Sparta Dungeon =====");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("플레이어 상태 보기...");
                        Console.WriteLine("아무 키나 누르면 돌아갑니다...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("인벤토리 실행");
                        currentScene = Scene.Inventory;
                        break;
                    case "3":
                        Console.WriteLine("상점 실행");
                        currentScene = Scene.Shop;
                        break;
                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                        break;
                }
            }
        }

        void TownScene()
        {
            Console.WriteLine("마을에 진입했다.");
        }

        void ShopScene()
        {
            shop.OpenShop(); // ✅ 인스턴스를 통해 호출
        }

        void InventoryScene()
        {
            inventory.ShowInventory();
        }

    }

    
}
