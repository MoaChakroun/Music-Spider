using System;
using System.IO;
using System.Linq;
public class GetMusic
{

    private static string FolderCheckerAndSender()
    {
        string pather = @"music";
        if (Directory.Exists(pather))
        {
            return pather;
        }
        else
        {
            Directory.CreateDirectory(pather);
            return pather;

        }

    }
    public static string[] Files() 
    {
        string pather = FolderCheckerAndSender();
        string[] allowedExtensions =  { ".mp3", ".wav", ".flac", ".m4a" };
        string[] music = Directory.GetFiles(pather);
        return music.Where(f => allowedExtensions.Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase))).ToArray(); 

    }

}