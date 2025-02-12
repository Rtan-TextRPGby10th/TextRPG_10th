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
            { "click", "click.mp3" },
            { "hit", "hit.mp3" },
            { "hit_bow", "hit_bow.mp3" },
            { "levelup", "levelup.mp3" },
            { "money", "money.mp3" },
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

            Console.WriteLine($"🎵 재생 중: {filePath}");

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

        public void PlaySFX(string soundName)
        {
            if (sfxFiles.ContainsKey(soundName))
            {
                string fullPath = Path.Combine(sfxPath, sfxFiles[soundName]);

                if (File.Exists(fullPath))
                {
                    var reader = new AudioFileReader(fullPath);
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
                }
                else
                {
                    Console.WriteLine($"[오류] 효과음 파일 없음: {soundName}");
                }
            }
            else
            {
                Console.WriteLine($"[오류] 등록되지 않은 효과음: {soundName}");
            }
        }

        public void ShowSFXList()
        {
            Console.WriteLine("🎵 [사용 가능한 효과음 리스트]");
            foreach (var sound in sfxFiles)
            {
                Console.WriteLine($"- {sound.Key} ({sound.Value})");
            }
        }
    }
}
