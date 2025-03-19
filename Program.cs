var regional1Teams = new Team[] {
    new Team("EAST", 1),
    new Team("EAST", 2),
    new Team("EAST", 3),
    new Team("EAST", 4),
    new Team("EAST", 5),
    new Team("EAST", 6),
    new Team("EAST", 7),
    new Team("EAST", 8),
    new Team("EAST", 9),
    new Team("EAST", 10),
    new Team("EAST", 11),
    new Team("EAST", 12),
    new Team("EAST", 13),
    new Team("EAST", 14),
    new Team("EAST", 15),
    new Team("EAST", 16),
};
var regional2Teams = new Team[] {
    new Team("SOUTH", 1),
    new Team("SOUTH", 2),
    new Team("SOUTH", 3),
    new Team("SOUTH", 4),
    new Team("SOUTH", 5),
    new Team("SOUTH", 6),
    new Team("SOUTH", 7),
    new Team("SOUTH", 8),
    new Team("SOUTH", 9),
    new Team("SOUTH", 10),
    new Team("SOUTH", 11),
    new Team("SOUTH", 12),
    new Team("SOUTH", 13),
    new Team("SOUTH", 14),
    new Team("SOUTH", 15),
    new Team("SOUTH", 16),
};
var regional3Teams = new Team[] {
    new Team("MIDWEST", 1),
    new Team("MIDWEST", 2),
    new Team("MIDWEST", 3),
    new Team("MIDWEST", 4),
    new Team("MIDWEST", 5),
    new Team("MIDWEST", 6),
    new Team("MIDWEST", 7),
    new Team("MIDWEST", 8),
    new Team("MIDWEST", 9),
    new Team("MIDWEST", 10),
    new Team("MIDWEST", 11),
    new Team("MIDWEST", 12),
    new Team("MIDWEST", 13),
    new Team("MIDWEST", 14),
    new Team("MIDWEST", 15),
    new Team("MIDWEST", 16),
};
var regional4Teams = new Team[] {
    new Team("WEST", 1),
    new Team("WEST", 2),
    new Team("WEST", 3),
    new Team("WEST", 4),
    new Team("WEST", 5),
    new Team("WEST", 6),
    new Team("WEST", 7),
    new Team("WEST", 8),
    new Team("WEST", 9),
    new Team("WEST", 10),
    new Team("WEST", 11),
    new Team("WEST", 12),
    new Team("WEST", 13),
    new Team("WEST", 14),
    new Team("WEST", 15),
    new Team("WEST", 16),
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

