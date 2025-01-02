using NUnit.Framework;

namespace GameOfLife.Tests;

[TestFixture]
public class GameOfLifeExtensionTests
{
    private const string FilePath = "Generations.txt";

    private readonly bool[,] grid =
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

    [Test]
    public void Simulate_Sequential_InvalidGenerations_ArgumentOutOfRangeException()
    {
        var game = new GameOfLifeSequentialVersion(5, 5);
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => game.Simulate(-1, new StringWriter(), 'O', ' '));
    }

    [Test]
    public void SimulateAsync_Parallel_InvalidGenerations_ArgumentOutOfRangeException()
    {
        var game = new GameOfLifeParallelVersion(5, 5);
        _ = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => game.SimulateAsync(-1, new StringWriter(), 'O', ' '));
    }

    [Test]
    public void Simulate_Sequential_NullWriter_ArgumentNullException()
    {
        var game = new GameOfLifeSequentialVersion(5, 5);
        _ = Assert.Throws<ArgumentNullException>(() => game.Simulate(3, null, 'O', ' '));
    }

    [Test]
    public void Simulate_Parallel_NullWriter_ArgumentNullException()
    {
        var game = new GameOfLifeParallelVersion(5, 5);
        _ = Assert.ThrowsAsync<ArgumentNullException>(() => game.SimulateAsync(-1, null, 'O', ' '));
    }

    [Test]
    public void Simulate_Sequential_NullGame_ArgumentNullException()
    {
        GameOfLifeSequentialVersion? game = null;
        _ = Assert.Throws<ArgumentNullException>(() => game.Simulate(3, new StringWriter(), '0', ' '));
    }

    [Test]
    public void SimulateAsync_Parallel_NullGame_ArgumentNullException()
    {
        GameOfLifeParallelVersion? game = null;
        _ = Assert.ThrowsAsync<ArgumentNullException>(() => game.SimulateAsync(3, new StringWriter(), '0', ' '));
    }

    [Test]
    public void Simulate_Sequential_ValidArguments_OutputCorrectResult()
    {
        var game = new GameOfLifeSequentialVersion(this.grid);
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        game.Simulate(15, stringWriter, '●', '○');
        var streamWriter = new StreamWriter(FilePath);
        game.Restart();
        game.Simulate(15, streamWriter, '●', '○');
        streamWriter.Close();
        using var streamReader = new StreamReader(FilePath);
        Assert.That(stringWriter.ToString(), Is.EqualTo(streamReader.ReadToEnd()));
    }

    [Test]
    public async Task SimulateAsync_Parallel_ValidArguments_OutputCorrectResult()
    {
        var game = new GameOfLifeParallelVersion(this.grid);
        await using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        await game.SimulateAsync(15, stringWriter, '●', '○');
        var expected = stringWriter.ToString();
        game.Restart();
        var streamWriter = new StreamWriter(FilePath);
        await game.SimulateAsync(15, streamWriter, '●', '○');
        streamWriter.Close();
        using var streamReader = new StreamReader(FilePath);
        var actual = await streamReader.ReadToEndAsync();
        Assert.That(expected, Is.EqualTo(actual));
    }
}
