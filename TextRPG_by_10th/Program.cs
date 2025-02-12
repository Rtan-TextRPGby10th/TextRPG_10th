namespace TextRPG_by_10th
{
    class Program
    {
        static void Main(string[] args)
        {
            DataLoad.LoadMiscItemData();
            DataLoad.LoadEquipItemData();
            DataLoad.LoadConsumeItemData();
            DataLoad.LoadQuestData();

            DataLoad.LoadPlayerItemData();

            // 게임 시작할 때 BGM 자동 재생
            AudioManager.Instance.ChangeScene("Town");

            //외부에서도 SceneManager.currentScene을 변경해 Scene 상태를 바꿀 수 있음
            while (true)
            {
                SceneManager.instance.GameScecne(SceneManager.instance.currentScene);
            }
        }
    }
}