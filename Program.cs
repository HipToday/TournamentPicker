Random random = new();

Team WhoWins(Team home, Team away) {
    var seeds = new int[home.Seed + away.Seed];

    Array.Fill(seeds, away.Seed, 0, home.Seed);
    Array.Fill(seeds, home.Seed, home.Seed, away.Seed);

//    Console.WriteLine($"seeds[{seeds.Length}]: " + string.Join(",", seeds));
    var winningSeed = seeds[random.Next(seeds.Length)];
    return (winningSeed == home.Seed) ? home : away;
}

Team[] RoundWinners(Team[] teams) {
    var winners = new Team[teams.Length / 2];

    for (int i = 0; i < teams.Length / 2; i++) {
        winners[i] = WhoWins(teams[i], teams[^(i + 1)]);
        Console.WriteLine($"Seed: {teams[i]}");
        Console.WriteLine($" vs.\tWinner: {winners[i]}");
        Console.WriteLine($"Seed: {teams[^(i + 1)]}");
    }

    return winners;
}

Team[] BracketWinner(Team[] teams) {
    Console.WriteLine($"\nRound of {teams.Length * 4}");
    Console.WriteLine("-----------");
    var winner = RoundWinners(teams);

    // If we're down to 1 we've found our winner
    if (winner.Length == 1) {
        return teams;
    }

    return BracketWinner(winner);
}

var regional1Teams = new Team[] {
    new Team("Regional One", 1),
    new Team("Regional One", 2),
    new Team("Regional One", 3),
    new Team("Regional One", 4),
    new Team("Regional One", 5),
    new Team("Regional One", 6),
    new Team("Regional One", 7),
    new Team("Regional One", 8),
    new Team("Regional One", 9),
    new Team("Regional One", 10),
    new Team("Regional One", 11),
    new Team("Regional One", 12),
    new Team("Regional One", 13),
    new Team("Regional One", 14),
    new Team("Regional One", 15),
    new Team("Regional One", 16),
};
var regional1Winner = BracketWinner(regional1Teams)[0];
Console.WriteLine($"Regional One Winner: {regional1Winner}");

var regional2Teams = new Team[] {
    new Team("Regional Two", 1),
    new Team("Regional Two", 2),
    new Team("Regional Two", 3),
    new Team("Regional Two", 4),
    new Team("Regional Two", 5),
    new Team("Regional Two", 6),
    new Team("Regional Two", 7),
    new Team("Regional Two", 8),
    new Team("Regional Two", 9),
    new Team("Regional Two", 10),
    new Team("Regional Two", 11),
    new Team("Regional Two", 12),
    new Team("Regional Two", 13),
    new Team("Regional Two", 14),
    new Team("Regional Two", 15),
    new Team("Regional Two", 16),
};
var regional2Winner = BracketWinner(regional2Teams)[0];
Console.WriteLine($"Regional Two Winner: {regional2Winner}");

var regional3Teams = new Team[] {
    new Team("Regional Three", 1),
    new Team("Regional Three", 2),
    new Team("Regional Three", 3),
    new Team("Regional Three", 4),
    new Team("Regional Three", 5),
    new Team("Regional Three", 6),
    new Team("Regional Three", 7),
    new Team("Regional Three", 8),
    new Team("Regional Three", 9),
    new Team("Regional Three", 10),
    new Team("Regional Three", 11),
    new Team("Regional Three", 12),
    new Team("Regional Three", 13),
    new Team("Regional Three", 14),
    new Team("Regional Three", 15),
    new Team("Regional Three", 16),
};
var regional3Winner = BracketWinner(regional3Teams)[0];
Console.WriteLine($"Regional Three Winner: {regional3Winner}");

var regional4Teams = new Team[] {
    new Team("Regional Four", 1),
    new Team("Regional Four", 2),
    new Team("Regional Four", 3),
    new Team("Regional Four", 4),
    new Team("Regional Four", 5),
    new Team("Regional Four", 6),
    new Team("Regional Four", 7),
    new Team("Regional Four", 8),
    new Team("Regional Four", 9),
    new Team("Regional Four", 10),
    new Team("Regional Four", 11),
    new Team("Regional Four", 12),
    new Team("Regional Four", 13),
    new Team("Regional Four", 14),
    new Team("Regional Four", 15),
    new Team("Regional Four", 16),
};
var regional4Winner = BracketWinner(regional4Teams)[0];
Console.WriteLine($"Regional Four Winner: {regional4Winner}");

var finalFour = new Team[] {
    regional1Winner,
    regional2Winner,
    regional3Winner,
    regional4Winner,
};
var champion = BracketWinner(finalFour)[0];
Console.WriteLine($"Champion: {champion}");

public class Team(string name, int seed)
{
    public string Name { get; } = name;
    public int Seed { get; } = seed;

    override public string ToString() {
        return $"{Seed} {Name}";
    }
};

