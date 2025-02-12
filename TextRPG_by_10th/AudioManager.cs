using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace TextRPG_by_10th
{
    public class AudioManager
    {
        private static AudioManager _instance;
        public static AudioManager Instance => _instance ?? (_instance = new AudioManager());

        private string bgmPath = "../../../../TextRPG_by_10th/Audio/BGM/";
        private string sfxPath = "../../../../TextRPG_by_10th/Audio/SFX/";

        private List<string> townBGM = new List<string>();
        private List<string> dungeonBGM = new List<string>();

        private WaveOutEvent bgmPlayer;
        private AudioFileReader currentBGM;

        private List<WaveOutEvent> sfxPlayers = new List<WaveOutEvent>();

        private string currentScene = "Town";

        private Random random = new Random();

        private Dictionary<string, string> sfxFiles = new Dictionary<string, string>
        {
            { "click", "click.mp3" },           // 효과음 호출하기 AudioManager.Instance.PlaySFX("game_over");
            { "hit1", "hit1.mp3" },
            { "hit2", "hit2.mp3" },
            { "hit3", "hit3.mp3" },
            { "critical1", "critical1.mp3" },
            { "critical2", "critical2.mp3" },
            { "equip_armor", "equip_armor.mp3" },
            { "game_over", "game_over.mp3" },
            { "heal_potion", "heal_potion.mp3" },
            { "levelUp", "levelUp.mp3" },
            { "money", "money.mp3" },
            { "poison_potion", "poison_potion.mp3" },
            { "skill", "skill.mp3" },
            { "buff", "buff.mp3" },
            { "upgrade", "upgrade.mp3" },
            { "win", "win.mp3" }

        };

        private AudioManager()
        {
            LoadBGMFiles();
            PlayRandomTownBGM();
        }

        private void LoadBGMFiles()
        {
            if (Directory.Exists(bgmPath))
            {
                townBGM.AddRange(Directory.GetFiles(bgmPath, "town*.mp3"));
                dungeonBGM.AddRange(Directory.GetFiles(bgmPath, "battle*.mp3"));
            }
            else
            {
                Console.WriteLine($"[오류] BGM 폴더 없음: {bgmPath}");
            }
        }

        private void PlayRandomTownBGM()
        {
            if (townBGM.Count > 0)
            {
                PlayBGM(townBGM[random.Next(townBGM.Count)]);
            }
        }

        private void PlayRandomDungeonBGM()
        {
            if (dungeonBGM.Count > 0)
            {
                PlayBGM(dungeonBGM[random.Next(dungeonBGM.Count)]);
            }
        }

        public void PlayBGM(string filePath)
        {
            StopBGM();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[오류] BGM 파일 없음: {filePath}");
                return;
            }

            // Console.WriteLine($"재생 중: {filePath}");

            currentBGM = new AudioFileReader(filePath);
            bgmPlayer = new WaveOutEvent();
            bgmPlayer.Init(currentBGM);
            bgmPlayer.Play();
        }

        public void StopBGM()
        {
            if (bgmPlayer != null)
            {
                bgmPlayer.Stop();
                bgmPlayer.Dispose();
                bgmPlayer = null;
            }

            if (currentBGM != null)
            {
                currentBGM.Dispose();
                currentBGM = null;
            }
        }

        public void ChangeScene(string sceneName)
        {
            if (sceneName == currentScene) return;

            currentScene = sceneName;

            if (sceneName == "Dungeon")
                PlayRandomDungeonBGM();
            else if (sceneName == "Town")
                PlayRandomTownBGM();
        }
        private void PlaySFXFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var reader = new AudioFileReader(filePath);
                var sfxPlayer = new WaveOutEvent();
                sfxPlayer.Init(reader);
                sfxPlayer.Play();
                sfxPlayers.Add(sfxPlayer);

                Task.Run(() =>
                {
                    Thread.Sleep((int)reader.TotalTime.TotalMilliseconds);
                    sfxPlayer.Dispose();
                    sfxPlayers.Remove(sfxPlayer);
                });

                // Console.WriteLine($"🔊 재생 중: {filePath}");
            }
        }

        public void PlaySFX(string soundPattern)
        {
            string[] matchingFiles = Directory.GetFiles(sfxPath, soundPattern + "*.mp3");

            if (matchingFiles.Length > 0)
            {
                string randomFile = matchingFiles[random.Next(matchingFiles.Length)];
                PlaySFXFile(randomFile);
            }
            else
            {
                Console.WriteLine($"[오류] 효과음 파일 없음: {soundPattern}*.mp3");
            }
        }

        public void ShowSFXList()
        {
            Console.WriteLine("[사용 가능한 효과음 리스트]");
            foreach (var sound in sfxFiles)
            {
                Console.WriteLine($"- {sound.Key} ({sound.Value})");
            }
        }
    }
}
