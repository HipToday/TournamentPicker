var regional1Teams = new Team[] {
    new Team("Region One", 1),
    new Team("Region One", 2),
    new Team("Region One", 3),
    new Team("Region One", 4),
    new Team("Region One", 5),
    new Team("Region One", 6),
    new Team("Region One", 7),
    new Team("Region One", 8),
    new Team("Region One", 9),
    new Team("Region One", 10),
    new Team("Region One", 11),
    new Team("Region One", 12),
    new Team("Region One", 13),
    new Team("Region One", 14),
    new Team("Region One", 15),
    new Team("Region One", 16),
};
var regional2Teams = new Team[] {
    new Team("Region Two", 1),
    new Team("Region Two", 2),
    new Team("Region Two", 3),
    new Team("Region Two", 4),
    new Team("Region Two", 5),
    new Team("Region Two", 6),
    new Team("Region Two", 7),
    new Team("Region Two", 8),
    new Team("Region Two", 9),
    new Team("Region Two", 10),
    new Team("Region Two", 11),
    new Team("Region Two", 12),
    new Team("Region Two", 13),
    new Team("Region Two", 14),
    new Team("Region Two", 15),
    new Team("Region Two", 16),
};
var regional3Teams = new Team[] {
    new Team("Region Three", 1),
    new Team("Region Three", 2),
    new Team("Region Three", 3),
    new Team("Region Three", 4),
    new Team("Region Three", 5),
    new Team("Region Three", 6),
    new Team("Region Three", 7),
    new Team("Region Three", 8),
    new Team("Region Three", 9),
    new Team("Region Three", 10),
    new Team("Region Three", 11),
    new Team("Region Three", 12),
    new Team("Region Three", 13),
    new Team("Region Three", 14),
    new Team("Region Three", 15),
    new Team("Region Three", 16),
};
var regional4Teams = new Team[] {
    new Team("Region Four", 1),
    new Team("Region Four", 2),
    new Team("Region Four", 3),
    new Team("Region Four", 4),
    new Team("Region Four", 5),
    new Team("Region Four", 6),
    new Team("Region Four", 7),
    new Team("Region Four", 8),
    new Team("Region Four", 9),
    new Team("Region Four", 10),
    new Team("Region Four", 11),
    new Team("Region Four", 12),
    new Team("Region Four", 13),
    new Team("Region Four", 14),
    new Team("Region Four", 15),
    new Team("Region Four", 16),
};

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
            winners[i] = WhoWins(teams[i], teams[^(i + 1)]);
            Console.WriteLine($"{teams[i]}");
            Console.WriteLine($"  vs.\tWinner: {winners[i]}");
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

