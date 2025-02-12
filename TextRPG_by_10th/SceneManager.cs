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

        public Shop shop;

        Battle battle = new Battle();

        public Player player { get; private set; }  // 수정 전 - Player player 

        public QuestManager questManager = new QuestManager();

        private bool isInit=false;
        
        SceneManager()
        {
            instance = this;
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
            // 
            Quest
        }

        public Scene currentScene = Scene.Start;

        public void GameScecne(Scene scene)
        {
            switch (scene)
            {
                case Scene.Start:
                    //게임 시작 기능 실행
                    StartScene();
                    break;
                case Scene.Town:
                    //마을에서 필요한 기능 실행
                    AudioManager.Instance.ChangeScene("Town");  // 마을 BGM 유지
                    TownScene();
                    break;
                case Scene.Shop:
                    //상점 기능 실행
                    AudioManager.Instance.PlaySFX("click");
                    ShopScene();
                    break;
                case Scene.Inventory:
                    //인벤토리 확인
                    AudioManager.Instance.PlaySFX("click");
                    InventoryScene();
                    break;
                case Scene.Dungeon:
                    //던전 입장 및 몬스터와의 전투
                    AudioManager.Instance.PlaySFX("click");
                    AudioManager.Instance.ChangeScene("Dungeon");  // 던전 BGM으로 변경
                    DungeonScene();
                    break;
                case Scene.Quest:
                    //현재 진행중인 퀘스트 확인
                    AudioManager.Instance.PlaySFX("click");
                    QuestScene();
                    break;
            }
        }

        //각 Scene에서 실행할 기능을 수행하는 함수
        void TownScene()
        {
            Console.WriteLine("게임 시작");

            Inventory playerInventory = new Inventory();

            while (currentScene == Scene.Town)
            {
                Console.Clear();
                Console.WriteLine("===== Sparta Dungeon =====");
                Console.WriteLine("1. 저장하기");
                Console.WriteLine("2. 상태 보기");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전");
                Console.WriteLine("5. 장비 업그레이드");
                Console.WriteLine("0. 나가기");
                Console.Write(">> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("저장중");
                        DataLoad.SaveAllData();
                        Console.WriteLine("아무 키나 누르면 돌아갑니다...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("상태 보기");
                        currentScene = Scene.Inventory;
                        break;
                    case "3":
                        Console.WriteLine("상점 실행");
                        currentScene = Scene.Shop;
                        break;
                    case "4":
                        currentScene = Scene.Dungeon;
                        break;                    
                    case "5":
                        Console.WriteLine("장비 업그레이드");
                        currentScene = Scene.Quest;
                        break;
                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                        break;
                }
            }
        }

        void StartScene()
        {
            while (currentScene == Scene.Start)
            {
                if (player == null)
                {
                    isInit = !isInit;
                    CreatPlayer();
                }
                else
                {
                    inventory.SetPlayer(player);
                    if (isInit == false)
                    {
                        isInit = !isInit;
                        DataLoad.LoadShopData();
                        DataLoad.LoadPlayerItemData();

                        DataLoad.LoadPlayerEquippedData();
                    }
                    currentScene = Scene.Town;
                }
            }
        }

        void CreatPlayer()
        {
            Console.Clear();
            //Console.SetWindowSize(120, 40);         //인벤토리 짤려서 창 크기 변경
            Console.Write("이름을 입력하세요 : ");
            string playerName = Console.ReadLine();

            Console.WriteLine("직업을 선택하세요.");
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 도적");
            Console.WriteLine("3. 궁수");

            Console.Write(">> ");
            string input = Console.ReadLine();
            AudioManager.Instance.PlaySFX("click");
            switch (input)
            {
                case "1":
                    player = new Warrior(playerName);
                    break;
                case "2":
                    player = new Assassin(playerName);
                    break;
                case "3":
                    player = new Archer(playerName);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                    break;
            }

            if (player != null)
            {
                Console.WriteLine($"{player.playerJob.ToString()}을 선택했습니다.");
                currentScene = Scene.Town;
            }
            inventory.GiveStartpack();                //초기장비 지급

            DataLoad.SaveAllData();
            DataLoad.LoadShopData();
        }

        void ShopScene()
        {
            shop.OpenShop(); // ✅ 인스턴스를 통해 호출
        }

        void InventoryScene()
        {
            inventory.ShowInventory();
        }

        void DungeonScene()
        {
            battle.BattleProcess(player, inventory);
        }
        void QuestScene()
        {
            // 퀘스트 테스트용 코드
            //questManager.SetBasicQuest();

            questManager.ShowQuestList();
        }

        public void SetPlayerData(Player p)
        {
            player=p;
        }

    }
}
