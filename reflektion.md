# Reflektion — Musiklistappen

**Namn:** Akarshika Srivastava
**Datum:** 07-11-2026

---

## Vad var svårast?

Det svåraste var att komma igång, eftersom jag inte riktigt visste hur jag skulle koppla C# till databasen. Jag hade också problem med att Rider inte visade mina tabeller.

---

## Vad lärde du dig?

Jag lärde mig hur ett C#-program kan prata med en databas. Jag lärde mig också hur man använder ADO.NET för att koppla ett C#-program till en databas. Man öppnar en anslutning med `SqliteConnection`, skapar ett kommando med `CreateCommand()`, skriver SQL:en i `CommandText`, skickar in värden med parametrar och kör kommandot. Jag lärde mig också att varje metod ska öppna en egen anslutning, och att `using var` stänger anslutningen automatiskt när metoden är klar. Dessutom lärde jag mig att man använder en klass, som `Song`, för att spara en rad från databasen som ett objekt.

---

## Hur använde du ADO.NET i praktiken — vad kändes naturligt och vad var förvirrande?

Det som kändes naturligt var själva SQL:en, för den hade jag redan skrivit i diskografin. Jag skrev den bara i `command.CommandText` istället. När `GetAllSongs()` fungerade var det lättare med resten, eftersom `AddSong` och `DeleteSong` följer samma mönster: öppna anslutning, skapa kommando, skicka in parametrar och köra. Det var också skönt att `using var` stänger anslutningen automatiskt.

Det som var förvirrande var först hur allt hänger ihop, till exempel att connection string kommer från `appsettings.json` via `Program.cs` till `DbHandler`. Jag förstod inte heller direkt skillnaden mellan `ExecuteReader()` och `ExecuteNonQuery()`. Nu vet jag att `ExecuteReader()` används när man hämtar rader med SELECT, och `ExecuteNonQuery()` när man ändrar data med INSERT och DELETE. 
---

## Varför ska man använda parameteriserade queries?

Vad kan hända om man klistrar ihop SQL med strängar i stället? (Tänk på Bobby Tables.)

Om man klistrar ihop SQL med strängar kan en användare skriva in SQL-kod i stället för en vanlig titel, och då körs koden i databasen. Det kallas SQL injection, som i Bobby Tables, där någon kunde radera en hel tabell. Med parametrar som `@title` skickas värdet in separat, så databasen ser det bara som text och aldrig som kod. Det är också säkrare och lättare att läsa.


---

## Om du hade mer tid — vad hade du lagt till eller gjort annorlunda?

Jag hade visat albumlistan när man lägger till en låt, så man slipper titta i databasen för att hitta album-ID. 


---

## (VG) Varför delade du upp koden som du gjorde?

Resonera kring ditt designval: varför ligger databaskoden i `DbHandler` och inte i `Program.cs`? Vad hade blivit svårare om allt låg i samma fil?

Jag la all databaskod i `DbHandler` så att `Program.cs` bara sköter menyn och det användaren ser. Då har varje fil en egen uppgift, och det blir lättare att hitta fel. När jag fick ett fel i `DbHandler.cs` visste jag direkt var jag skulle leta. Om allt låg i `Program.cs` hade koden blivit rörig och svårare att ändra.
