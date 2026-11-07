using Microsoft.Data.Sqlite;

namespace MusikApp;

/// <summary>
/// ALL databaskod bor här. Program.cs ska aldrig innehålla SQL eller SqliteConnection.
///
/// Det här är ett skelett: metoderna finns, kommentarerna visar vad varje metod ska göra,
/// men koden är din. Alla metoder kastar NotImplementedException tills du har skrivit dem.
///
/// ADO.NET i fem steg (samma i varje metod som pratar med databasen):
///   1. Öppna en anslutning        -> new SqliteConnection(...) och connection.Open()
///   2. Skapa ett kommando         -> connection.CreateCommand()
///   3. Skriv SQL:en               -> command.CommandText = "..."
///   4. Skicka in värden säkert    -> command.Parameters.AddWithValue("@namn", värde)
///   5. Kör kommandot              -> ExecuteReader() för SELECT, ExecuteNonQuery() för INSERT/DELETE
///
/// Använd "using var" på anslutning och kommando så stängs de av sig själva.
/// Bygg SQL:en med parametrar (@titel), aldrig genom att klistra ihop strängar.
/// </summary>
public class DbHandler
{
    // Tips: spara bara connection string här. Den kommer från appsettings.json
    // via Program.cs (t.ex. "Data Source=musik.db").
    // Öppna sedan en NY anslutning inuti varje metod nedan.

    private readonly string _connectionString;

    public DbHandler(string connectionString)
    {
        _connectionString = connectionString;
    }
    // ---------------------------------------------------------------
    // G-nivå: du behöver minst tre av de fem här (välj själv vilka)
    // ---------------------------------------------------------------

   
    /// <summary>Hämtar alla låtar, sorterade på artist, album och spårnummer.</summary>
    public List<Song> GetAllSongs()
    {
        var songs = new List<Song>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT songs.song_id, songs.album_id, songs.title, songs.track_number,
               songs.length_seconds, albums.title AS album_title, artists.name AS artist_name
        FROM songs
        JOIN albums ON songs.album_id = albums.album_id
        JOIN artists ON albums.artist_id = artists.artist_id
        ORDER BY artists.name, albums.title, songs.track_number";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            songs.Add(new Song
            {
                Id = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                Title = reader.GetString(2),
                TrackNumber = reader.GetInt32(3),
                LengthSeconds = reader.GetInt32(4),
                AlbumTitle = reader.GetString(5),
                ArtistName = reader.GetString(6)
            });
        }

        return songs;
    }

    /// <summary>Hämtar en specifik låt. Returnerar null om den inte finns.</summary>
    public Song? GetSongById(int id)
    {
        // TODO: SELECT ... WHERE Id = @id (parameter!).
        // TODO: if (reader.Read()) { ... } annars returnera null.
        throw new NotImplementedException();
    }

    /// <summary>Söker låtar där titeln innehåller söktexten.</summary>
    public List<Song> SearchSongs(string searchText)
    {
        // TODO: SELECT ... WHERE Titel LIKE @sok.
        // Tips: parametervärdet ska vara "%" + searchText + "%" så att LIKE matchar delar av titeln.
        throw new NotImplementedException();
    }

    /// <summary>Lägger till en ny låt.</summary>
    public void AddSong(Song song)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO songs (album_id, title, track_number, length_seconds)
        VALUES (@albumId, @title, @trackNumber, @lengthSeconds)";
        command.Parameters.AddWithValue("@albumId", song.AlbumId);
        command.Parameters.AddWithValue("@title", song.Title);
        command.Parameters.AddWithValue("@trackNumber", song.TrackNumber);
        command.Parameters.AddWithValue("@lengthSeconds", song.LengthSeconds);
        command.ExecuteNonQuery();
    }

    /// <summary>Tar bort en låt. Returnerar true om en rad faktiskt togs bort.</summary>
    public bool DeleteSong(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM songs WHERE song_id = @id";
        command.Parameters.AddWithValue("@id", id);

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    // ---------------------------------------------------------------
    // VG-nivå: du behöver minst EN av dessa två (och reflektionen i README)
    // ---------------------------------------------------------------

    /// <summary>(VG, alternativ A) Låtar tillsammans med albumets namn. Kräver en JOIN.</summary>
    public List<(string SongTitle, string AlbumName)> GetSongsWithAlbum()
    {
        // TODO: SELECT Låtar.Titel, Album.Namn FROM Låtar JOIN Album ON ... ORDER BY ...
        throw new NotImplementedException();
    }

    /// <summary>(VG, alternativ B) Låtar som är längre än medellängden. Kräver en subquery.</summary>
    public List<Song> GetSongsLongerThanAverage()
    {
        var songs = new List<Song>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT songs.song_id, songs.album_id, songs.title, songs.track_number,
               songs.length_seconds, albums.title AS album_title, artists.name AS artist_name
        FROM songs
        JOIN albums ON songs.album_id = albums.album_id
        JOIN artists ON albums.artist_id = artists.artist_id
        WHERE songs.length_seconds > (SELECT AVG(length_seconds) FROM songs)
        ORDER BY songs.length_seconds DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            songs.Add(new Song
            {
                Id = reader.GetInt32(0),
                AlbumId = reader.GetInt32(1),
                Title = reader.GetString(2),
                TrackNumber = reader.GetInt32(3),
                LengthSeconds = reader.GetInt32(4),
                AlbumTitle = reader.GetString(5),
                ArtistName = reader.GetString(6)
            });
        }

        return songs;
    }
}
