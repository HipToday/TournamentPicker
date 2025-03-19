using GoogleSheets;

// Get the TournamentPickerApiKey and the TournamentPickerSpreadsheetIdfrom the environment variables
var apiKey = Environment.GetEnvironmentVariable("TournamentPickerApiKey", EnvironmentVariableTarget.User);
var spreadsheetId = Environment.GetEnvironmentVariable("TournamentPickerSpreadsheetId", EnvironmentVariableTarget.User);

// Check if the API key or spreadsheet ID is missing
if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(spreadsheetId))
{
    Console.WriteLine("API key or spreadsheet ID is missing.");
    return;
}

var googleSheetsService = new GoogleSheetsService(apiKey);

var sheetTitles = await googleSheetsService.GetSheetTitlesAsync(spreadsheetId);
Console.WriteLine("Sheet Titles:");
foreach (var title in sheetTitles)
{
    Console.WriteLine(title);
}

var values = await googleSheetsService.GetValuesAsync(spreadsheetId, sheetTitles[0], "A2:B17");
Console.WriteLine("Values:");
foreach (var row in values)
{
    Console.WriteLine(string.Join(", ", row));
}

var regional1Teams = await GetRegionalTeamsAsync(googleSheetsService, spreadsheetId, "South");
var regional2Teams = await GetRegionalTeamsAsync(googleSheetsService, spreadsheetId, "East");
var regional3Teams = await GetRegionalTeamsAsync(googleSheetsService, spreadsheetId, "Midwest");
var regional4Teams = await GetRegionalTeamsAsync(googleSheetsService, spreadsheetId, "West");

// Check if any of the regions failed to load
if (regional1Teams.Length == 0 || regional2Teams.Length == 0 || regional3Teams.Length == 0 || regional4Teams.Length == 0)
{
    Console.WriteLine("Failed to load bracket data for one or more regions.");
    return;
}

var regional1Winner = TournamentPicker.BracketWinner(regional1Teams)[0];
var regional2Winner = TournamentPicker.BracketWinner(regional2Teams)[0];
var regional3Winner = TournamentPicker.BracketWinner(regional3Teams)[0];
var regional4Winner = TournamentPicker.BracketWinner(regional4Teams)[0];

Console.WriteLine();
Console.WriteLine($"Regional One Winner: {regional1Winner}");
Console.WriteLine($"Regional Two Winner: {regional2Winner}");
Console.WriteLine($"Regional Three Winner: {regional3Winner}");
Console.WriteLine($"Regional Four Winner: {regional4Winner}");

var finalFour = new Team[] {
    regional1Winner,
    regional2Winner,
    regional3Winner,
    regional4Winner,
};

var champion = TournamentPicker.BracketWinner(finalFour, true)[0];
Console.WriteLine($"Champion: {champion}");

/// <summary>
/// Get the regional teams for the given region.
/// </summary>
/// <param name="googleSheetsService">The Google Sheets service.</param>
/// <param name="spreadsheetId">The ID of the spreadsheet.</param>
/// <param name="region">The region to get the teams for.</param>
/// <returns>The regional teams.</returns>
/// <exception cref="InvalidOperationException">Thrown when the regional teams cannot be loaded.</exception>
/// <exception cref="ArgumentException">Thrown when the region is invalid.</exception>
/// <exception cref="ArgumentNullException">Thrown when the Google Sheets service is null.</exception>
static async Task<Team[]> GetRegionalTeamsAsync(GoogleSheetsService googleSheetsService, string spreadsheetId, string region)
{
    if (googleSheetsService == null)
    {
        throw new ArgumentNullException(nameof(googleSheetsService));
    }

    if (string.IsNullOrEmpty(region))
    {
        throw new ArgumentException("Region cannot be null or empty.", nameof(region));
    }

    var regionTeams = (await googleSheetsService.GetValuesAsync(spreadsheetId, $"{region} Regional", "A2:B17"))
        .Select((row, i) => {
            var seedValue = row[0]?.ToString()?
                .Replace("*", string.Empty);
            if (!int.TryParse(seedValue, out int seed)) {
                seed = i + 1;
            }
            var name = row[1]?.ToString() ?? $"{region} {seed}";

            return new Team(name, seed);
        })
        .ToArray();

    if (regionTeams.Length == 0)
    {
        throw new InvalidOperationException($"Failed to load bracket data for the {region} region.");
    }

    return regionTeams;
}

class TournamentPicker {
    private static readonly Random random = new();

    public static Team WhoWins(Team home, Team away) {
        var seeds = new int[home.Seed + away.Seed];

        Array.Fill(seeds, away.Seed, 0, home.Seed);
        Array.Fill(seeds, home.Seed, home.Seed, away.Seed);

        // Console.WriteLine($"seeds[{seeds.Length}]: " + string.Join(",", seeds));
        var winningSeed = seeds[random.Next(seeds.Length)];
        return (winningSeed == home.Seed) ? home : away;
    }

    public static Team[] RoundWinners(Team[] teams) {
        var winners = new Team[teams.Length / 2];

        for (int i = 0; i < teams.Length / 2; i++) {
            // upsets are defined as a difference of 5 or more in seed
            var potentialUpset = Math.Abs(teams[i].Seed - teams[^(i + 1)].Seed) >= 5;
            var underdog = potentialUpset ? new[] { teams[i], teams[^(i + 1)] }.OrderBy(t => t.Seed).Last() : null;
            winners[i] = WhoWins(teams[i], teams[^(i + 1)]);
            Console.WriteLine($"{teams[i]}");
            Console.WriteLine($"  vs.\tWinner: {winners[i]}{(underdog == winners[i] ? " (upset)" : "")}");
            Console.WriteLine($"{teams[^(i + 1)]}");
        }

        return winners;
    }

    public static Team[] BracketWinner(Team[] teams, bool finalFour = false) {
        switch (teams.Length) {
            case 4:
                if (finalFour) {
                    Console.WriteLine($"\nFinal Four:");
                } else {
                    Console.WriteLine($"\nSweet Sixteen:");
                }
                break;

            case 2:
                if (finalFour) {
                    Console.WriteLine($"\nChampionship:");
                } else {
                    Console.WriteLine($"\nElite Eight:");
                }
                break;

            default:
                Console.WriteLine($"\nRound of {teams.Length * 4}");
                break;
        }

        Console.WriteLine("-----------");
        
        var winner = RoundWinners(teams);

        // If we're down to 1 we've found our winner
        if (winner.Length == 1) {
            return winner;
        }

        return BracketWinner(winner, finalFour);
    }

}

public class Team(string name, int seed)
{
    public string Name { get; } = name;
    public int Seed { get; } = seed;

    override public string ToString() {
        return $"{Seed} {Name}";
    }
};

