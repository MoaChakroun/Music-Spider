using System;
using System.IO;
using NetCoreAudio;

public static class MusicPlayer
{
    private static readonly Player Player = new Player(); //why is this readonly? and what does Player Player new Player mean ?
    static string[] musicFiles = GetMusic.Files();

    public static bool IsPlaying => Player.Playing;
    public static bool IsPaused => Player.Paused;
	public static string? NameChecker(string songName)
    {

        string? foundPath = musicFiles.FirstOrDefault(path =>
            Path.GetFileName(path).Equals(songName, StringComparison.OrdinalIgnoreCase) ||
            path.Equals(songName, StringComparison.OrdinalIgnoreCase));

        if (foundPath != null)
        {
            return foundPath;
        }
        else
        {
            Console.WriteLine($"[Error] Song '{songName}' not found.");
            return null;
        }
    }

        public static void Play(string song)
        {
            string songPath = NameChecker(song);
            if (songPath != null)
            {
                try {
                if (Player.Playing || Player.Paused)
                {
                    Player.Stop().Wait();
                    System.Threading.Thread.Sleep(200);
                }
                if (QueueController.Queue == null)
                {
                    QueueController.InitializeQueue();
                }

                Player.Play(songPath).Wait();
                QueueController.CurrentSong = songPath;
                Console.WriteLine($"▶ Playing: {Path.GetFileName(songPath)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] An error occurred while trying to play the song: {ex.Message}");
                }
            }
        }
        public static void Pause()
        {
            if (Player.Playing)
            {
                Player.Pause().Wait();
                Console.WriteLine("⏸ Playback paused.");
            }
        }

        public static void Resume()
        {
            if (Player.Paused)
            {
                Player.Resume().Wait();
                Console.WriteLine("▶ Playback resumed.");
            }
        }

        public static void Stop()
        {
            if (Player.Playing || Player.Paused)
            {
                Player.Stop().Wait();
                System.Threading.Thread.Sleep(200);
                Console.WriteLine("⏹ Playback stopped.");
                QueueController.CurrentSong = null;
            }
        }
}




public class QueueController
{
    public static string[]? Queue;
    public static string? CurrentSong;
    public static string[] MusicFiles = GetMusic.Files();

    public static void InitializeQueue()
    {
        Queue = MusicFiles;
    }
    public static void NextSong()
    {
        if (Queue == null || Queue.Length == 0)
        {
            Console.WriteLine("The queue is empty.");
            return;
        }

        int currentIndex = Array.IndexOf(Queue, CurrentSong ?? "");
        int nextIndex = (currentIndex + 1) % Queue.Length; // Loop back to the start if at the end
        string nextSong = Queue[nextIndex];

        MusicPlayer.Play(nextSong);
    }
    public static void PreviousSong()
    {
        if (Queue == null || Queue.Length == 0)
        {
            Console.WriteLine("The queue is empty.");
            return;
        }

        int currentIndex = Array.IndexOf(Queue, CurrentSong ?? "");
        int previousIndex = (currentIndex - 1 + Queue.Length) % Queue.Length; // Loop back to the end if at the start
        string previousSong = Queue[previousIndex];

        MusicPlayer.Play(previousSong);
    }
    public static void AddToQueue(string songName)
    {
        string foundPath = MusicPlayer.NameChecker(songName);

        if (foundPath != null)
        {

            if (Queue == null)
            {
                InitializeQueue();
            }
            var queueList = Queue.ToList();
            queueList.Add(foundPath);
            Queue = queueList.ToArray();
            Console.WriteLine($"Added '{songName}' to the queue.");
        }
    }
    public static void SkipToSongQueue(string songName)
    {
        string? foundPath = MusicPlayer.NameChecker(songName);

        if (foundPath != null && Queue != null)
        {

            int index = Array.IndexOf(Queue, foundPath);
            if (index != -1)
            {
                CurrentSong = foundPath;
                MusicPlayer.Play(foundPath);
                Console.WriteLine($"Skipped to '{songName}' in the queue.");
            }
            else
            {
                Console.WriteLine($"'{songName}' is not in the queue.");
            }
        }
    }
    public static void RandomQueue()
    {
        if (Queue == null || Queue.Length == 0)
        {
            InitializeQueue();
        }

        string[]CurrentQueue = Queue;
        int CurrentQueueLength = Queue.Length;

        // --- Fisher-Yates Shuffle Starts Here ---
        Random rand = new Random();
        
        // Loop backwards from the last item to the second item
        for (int i = Queue.Length - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i (inclusive)
            int randomIndex = rand.Next(i + 1);

            // Swap the elements at index i and randomIndex
            string temp = Queue[i];
            Queue[i] = Queue[randomIndex];
            Queue[randomIndex] = temp;
        }
    }

    public static void ShowQueue()
    {
        if (Queue == null || Queue.Length == 0)
        {
            Console.WriteLine("The queue is empty.");
            return;
        }

        Console.WriteLine("Current Queue:");
        for (int i = 0; i < Queue.Length; i++)
        {
            string songName = Path.GetFileName(Queue[i]);
            if (Queue[i] == CurrentSong)
            {
                Console.WriteLine($"{i + 1}. {songName} (Currently Playing)");
            }
            else
            {
                Console.WriteLine($"{i + 1}. {songName}");
            }
        }
    }


}