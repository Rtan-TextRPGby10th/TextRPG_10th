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

        SceneManager()
        {
            instance = this;
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
                    break;
                case Scene.Inventory:
                    //인벤토리 확인
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
        }

        void TownScene()
        {
            Console.WriteLine("마을에 진입했다.");
        }

    }

    
}
