namespace TextRPG_by_10th
{
    class Program
    {
        static void Main(string[] args)
        {
            //외부에서도 SceneManager.currentScene을 변경해 Scene 상태를 바꿀 수 있음
            SceneManager.instance.GameScecne(SceneManager.instance.currentScene);
        }
    }
}

