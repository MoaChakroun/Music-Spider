using System;

public class MainProgram
{

    static void Checkup(){
        PlaylistsSystem.Start();
        QueueController.InitializeQueue();
    }

    public static void Main()
    {
        Checkup();
        int state = 0;
        while (state == 0)
        {
            Console.WriteLine("Welcome to Music Spider!");
            Console.WriteLine("1. Play a song");
            Console.WriteLine("2. Exit");
            Console.WriteLine("3. Show Playlists, and create a new one");
            Console.Write("Enter your choice: ");
            string? input = Console.ReadLine();

            if (input == "1")
            {
                string[] MusicFiles = GetMusic.Files();
                if (MusicFiles.Length == 0)
                {
                    Console.WriteLine("No audio files found");
                    return;
                }
                else
                {
                    Console.WriteLine("Available audio files:");
                    for (int i = 0; i < MusicFiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(MusicFiles[i])}");
                    }

                    Console.WriteLine("Enter the number of the song you want to play:");
                    if (int.TryParse(Console.ReadLine(), out int songNumber) && songNumber > 0 && songNumber <= MusicFiles.Length)
                    {
                        MusicPlayer.Play(MusicFiles[songNumber - 1]);
                        while (MusicPlayer.IsPlaying)
                        {
                            Console.WriteLine("Enter 'Pause', 'Resume', 'Stop', 'Next', 'Previous', 'RandomizeQueue', or 'ShowQueue':");
                            string? command = Console.ReadLine()?.ToLower();

                            switch (command)
                            {
                                case "pause":
                                    MusicPlayer.Pause();
                                    break;
                                case "resume":
                                    MusicPlayer.Resume();
                                    break;
                                case "stop":
                                    MusicPlayer.Stop();
                                    break;
                                case "next":
                                    QueueController.NextSong();
                                    break;
                                case "previous":
                                    QueueController.PreviousSong();
                                    break;
                                case "randomizequeue":
                                    QueueController.RandomQueue();
                                    break;
                                case "showqueue":
                                    QueueController.ShowQueue();
                                    break;

                                default:
                                    Console.WriteLine("Invalid command.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

            }
            
            
            else if (input == "2")
            {
                state = 1;
                Console.WriteLine("Exiting Music Spider. Goodbye!");
            }
            
            else if (input == "3")
            {
                var playlists = PlaylistsSystem.GetPlaylists();
                Console.WriteLine("Available Playlists:");
                string[] playlistNames = new string[playlists.Length];
                foreach (var playlist in playlists)
                {
                    playlistNames[Array.IndexOf(playlists, playlist)] = Path.GetFileNameWithoutExtension(playlist);
                    Console.WriteLine($"{Array.IndexOf(playlists, playlist) + 1}. {Path.GetFileNameWithoutExtension(playlist)}");
                }
                Console.WriteLine("Enter the number of the playlist you want to view or type 'new' to create a new playlist:");
                int? playlistInput = int.Parse(Console.ReadLine());
                if (playlistInput.HasValue && playlistInput.Value > 0 && playlistInput.Value <= playlists.Length)
                {
                    string selectedPlaylistPath = playlists[playlistInput.Value - 1];
                    var playlistContents = PlaylistsSystem.GetPlaylistContents(selectedPlaylistPath);
                    Console.WriteLine($"Playlist: {playlistContents[0]}");
                    Console.WriteLine($"Description: {playlistContents[1]}");
                    Console.WriteLine($"Image: {playlistContents[2]}");
                    string[]? songs = playlistContents[3].ToString().Split(',');
                    Console.WriteLine("Songs:");
                    for (int i = 0; i < songs.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(songs[i])}");
                    }
                }
                else if (playlistInput == 0)
                {
                    Console.WriteLine("Enter the name of the new playlist:");
                    string newPlaylistName = Console.ReadLine();
                    Console.WriteLine("Enter a description for the new playlist:");
                    string newPlaylistDescription = Console.ReadLine();
                    Console.WriteLine("Enter the path to an image for the new playlist (or leave blank):");
                    string newPlaylistImage = Console.ReadLine();

                    Console.WriteLine("Available audio files:");
                    string[] musicFiles = GetMusic.Files();
                    for (int i = 0; i < musicFiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(musicFiles[i])}");
                    }
                    Console.WriteLine("Enter the numbers of the songs you want to add to the playlist, separated by commas:");
                    string[] songNumbersInput = Console.ReadLine().Split(',');
                    string[] newSongs = songNumbersInput
                        .Select(num => int.TryParse(num.Trim(), out int songNum) && songNum > 0 && songNum <= musicFiles.Length ? musicFiles[songNum - 1] : null)
                        .Where(songPath => songPath != null)
                        .ToArray();

                    PlaylistsSystem.CreatePlaylist(newPlaylistName, newPlaylistDescription, newPlaylistImage, newSongs);
                    Console.WriteLine($"Playlist '{newPlaylistName}' created successfully!");
                }
            }


            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        
    }


}