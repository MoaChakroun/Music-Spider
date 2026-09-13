using System;

public static class PlaylistsSystem
{
    public class DefaultPlaylist
    {
        public string PlaylistName { get; set; }= string.Empty;
        public string PlaylistDescription { get; set; } = string.Empty;
        public string PlaylistImage { get; set; } = string.Empty;
        public List<string> Songs { get; set; } = new List<string>();
    }    
    public static void Start(){
        {
            if (!Directory.Exists("Playlists"))
            {
                Directory.CreateDirectory("Playlists");
            }
        }
        }
    
    public static void CreatePlaylist(string playlistName, string playlistDescription, string playlistImage, string[] songs)
    {
        var NewPlaylist = new DefaultPlaylist
        {
            PlaylistName = playlistName,
            PlaylistDescription = playlistDescription,
            PlaylistImage = playlistImage,
            Songs = songs.ToList()
        };

        string json = System.Text.Json.JsonSerializer.Serialize(NewPlaylist);
        string filePath = Path.Combine("Playlists", $"{playlistName}.json");
        File.WriteAllText(filePath, json);
    }

    public static string[] GetPlaylists()
    {
        if (!Directory.Exists("Playlists"))
        {
            return Array.Empty<string>();
        }

        return Directory.GetFiles("Playlists", "*.json");
    }
    
    public static object[] GetPlaylistContents(string playlistFilePath)
    {
        if (!File.Exists(playlistFilePath))
        {
            return Array.Empty<string>();
        }

        string json = File.ReadAllText(playlistFilePath);
        var playlist = System.Text.Json.JsonSerializer.Deserialize<DefaultPlaylist>(json);

#pragma warning disable CS8601 // Possible null reference assignment.
        object[] Info =
        {
            playlist?.PlaylistName,
            playlist?.PlaylistDescription,
            playlist?.PlaylistImage,
            playlist?.Songs,
        };
#pragma warning restore CS8601 // Possible null reference assignment.

        return Info;
    }
}