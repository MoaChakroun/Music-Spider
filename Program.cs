using System;

public class MainProgram
{
    public static void Main()
    {
        int state = 0;
        while (state == 0)
        {
            Console.WriteLine("Welcome to Music Spider!");
            Console.WriteLine("1. Play a song");
            Console.WriteLine("2. Exit");
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
                            Console.WriteLine("Enter 'pause', 'resume', 'stop', 'next', or 'previous':");
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
            
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        
    }


}