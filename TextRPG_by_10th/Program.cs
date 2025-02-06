namespace TextRPG_by_10th
{
    public class Program
    {
        
        public Inventory inventory = new Inventory();
        Shop shop;


        public Program()
        {
            inventory = new Inventory();
            shop = new Shop(inventory);
        }

        public void Run()
        {
           
            Inventory playerInventory = new Inventory();

            while (true)
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
                        inventory.ShowInventory();
                        break;
                    case "3":
                        shop.OpenShop(); // ✅ 인스턴스를 통해 호출
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

        public static void Main(string[] args)
        {
            Program program = new Program(); // ✅ Program 인스턴스 생성
            program.Run(); // ✅ 인스턴스 메서드 호출
        }
    }
}
