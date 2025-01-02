using static System.Console;

namespace GameOfLife.ConsoleClient;

public static class Program
{
    public static async Task Main()
    {
        bool[,] grid =
        {
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, true, true, true, false, false, false, false },
            { false, false, false, false, true, false, true, false, false, false, false },
            { false, false, false, false, true, true, true, false, false, false, false },
            { false, false, false, false, true, true, true, false, false, false, false },
            { false, false, false, false, true, true, true, false, false, false, false },
            { false, false, false, false, true, true, true, false, false, false, false },
            { false, false, false, false, true, false, true, false, false, false, false },
            { false, false, false, false, true, true, true, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false },
        };

        var game = new GameOfLifeSequentialVersion(grid);
        game.Simulate(15, Out, '\u25A0', ' ');
        var game1 = new GameOfLifeParallelVersion(grid);
        await using var writer = new StreamWriter(File.Create("test.txt"));
        await game1.SimulateAsync(15, writer, '●', '○');
        _ = ReadKey();
    }
}
