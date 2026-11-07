namespace MusikApp;

// Din data-klass: ett Song-objekt = en rad i din låt-tabell.
//
// TODO: lägg till en egenskap per kolumn du vill visa i appen,
// t.ex. Id, Titel och Längd. Namn och typer ska passa din databas.
//
// Behöver du fler klasser (Album, Artist) för din app? Skapa dem i egna filer.
public class Song
{ public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Title { get; set; } = "";
    public int TrackNumber { get; set; }
    public int LengthSeconds { get; set; }

    // Filled in by the JOIN
    public string AlbumTitle { get; set; } = "";
    public string ArtistName { get; set; } = "";

    public override string ToString()
    {
        int min = LengthSeconds / 60;
        int sec = LengthSeconds % 60;
        return $"{Id}. {ArtistName} - {Title} ({AlbumTitle}) [{min}:{sec:D2}]";
    }
}
