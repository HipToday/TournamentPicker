
var regional1Teams = new Team[] {
    new Team("South Carolina", 1),
    new Team("Notre Dame", 2),
    new Team("Oregon St", 3),
    new Team("Indiana", 4),
    new Team("Oklahoma", 5),
    new Team("Nebraska", 6),
    new Team("Ole Miss", 7),
    new Team("North Carolina", 8),
    new Team("Michigan St", 9),
    new Team("Marquette", 10),
    new Team("Texas A&M", 11),
    new Team("FGCU", 12),
    new Team("Fairfield", 13),
    new Team("E Washington", 14),
    new Team("Kent State", 15),
    new Team("SHU/PRES", 16),
};
var regional1Winner = TournamentPicker.BracketWinner(regional1Teams)[0];
Console.WriteLine($"Regional One Winner: {regional1Winner}");

var regional2Teams = new Team[] {
    new Team("Iowa", 1),
    new Team("UCLA", 2),
    new Team("LSU", 3),
    new Team("Kansas St", 4),
    new Team("Colorado", 5),
    new Team("Louisville", 6),
    new Team("Creighton", 7),
    new Team("West Virginia", 8),
    new Team("Princeton", 9),
    new Team("UNLV", 10),
    new Team("MTSU", 11),
    new Team("Drake", 12),
    new Team("Portland", 13),
    new Team("Rice", 14),
    new Team("CA Baptist", 15),
    new Team("HC/UTM", 16),
};
var regional2Winner = TournamentPicker.BracketWinner(regional2Teams)[0];
Console.WriteLine($"Regional Two Winner: {regional2Winner}");

var regional3Teams = new Team[] {
    new Team("USC", 1),
    new Team("Ohio State", 2),
    new Team("UConn", 3),
    new Team("Virginia Tech", 4),
    new Team("Baylor", 5),
    new Team("Syracuse", 6),
    new Team("Duke", 7),
    new Team("Kansas", 8),
    new Team("Michigan", 9),
    new Team("Richmond", 10),
    new Team("AUB/ARIZ", 11),
    new Team("Van/COLU", 12),
    new Team("Marshall", 13),
    new Team("Jackson St", 14),
    new Team("Maine", 15),
    new Team("Texas A&M-CC", 16),
};
var regional3Winner = TournamentPicker.BracketWinner(regional3Teams)[0];
Console.WriteLine($"Regional Three Winner: {regional3Winner}");

var regional4Teams = new Team[] {
    new Team("Texas", 1),
    new Team("Stanford", 2),
    new Team("NC State", 3),
    new Team("Gonzaga", 4),
    new Team("Utah", 5),
    new Team("Tennessee", 6),
    new Team("Iowa State", 7),
    new Team("Alabama", 8),
    new Team("Florida St", 9),
    new Team("Maryland", 10),
    new Team("Green Bay", 11),
    new Team("S Dakota St", 12),
    new Team("UC Irvine", 13),
    new Team("Chattanooga", 14),
    new Team("Norfolk St", 15),
    new Team("Drexel", 16),
};
var regional4Winner = TournamentPicker.BracketWinner(regional4Teams)[0];
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

