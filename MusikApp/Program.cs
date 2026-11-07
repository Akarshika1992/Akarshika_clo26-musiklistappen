using Microsoft.Extensions.Configuration;
using MusikApp;

// Läser inställningar från appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
                       ?? throw new InvalidOperationException("Hittar ingen connection string 'Default' i appsettings.json.");

// Skapa DbHandler en gång, innan menyn
var db = new DbHandler(connectionString);

bool running = true;
while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Music List ===");
    Console.WriteLine("1. Show all songs");
    Console.WriteLine("2. Add a song");
    Console.WriteLine("3. Delete a song");
    Console.WriteLine("4. Show songs longer than average");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            foreach (var song in db.GetAllSongs())
            {
                Console.WriteLine(song);
            }
            break;
        case "2":
            var newSong = new Song();

            Console.Write("Title: ");
            newSong.Title = Console.ReadLine() ?? "";

            Console.Write("Album ID (see the albums table): ");
            int.TryParse(Console.ReadLine(), out int albumId);
            newSong.AlbumId = albumId;

            Console.Write("Track number: ");
            int.TryParse(Console.ReadLine(), out int trackNumber);
            newSong.TrackNumber = trackNumber;

            Console.Write("Length in seconds: ");
            int.TryParse(Console.ReadLine(), out int length);
            newSong.LengthSeconds = length;

            db.AddSong(newSong);
            Console.WriteLine("Song added!");
            break;
        case "3":
            Console.Write("Enter the ID of the song to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int deleteId))
            {
                Console.WriteLine("That is not a valid number.");
                break;
            }

            Console.Write("Are you sure? y/n: ");
            string? answer = Console.ReadLine();

            if (answer?.ToLower() == "y")
            {
                if (db.DeleteSong(deleteId))
                    Console.WriteLine("Song deleted.");
                else
                    Console.WriteLine("No song with that ID.");
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
            break;
        case "4":
            foreach (var song in db.GetSongsLongerThanAverage())
            {
                Console.WriteLine(song);
            }
            break;
        case "0":
            running = false;
            Console.WriteLine("Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice, try again.");
            break;
    }
}