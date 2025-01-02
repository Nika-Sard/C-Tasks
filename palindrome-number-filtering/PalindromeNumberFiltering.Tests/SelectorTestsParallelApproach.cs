using System.Diagnostics;
using NUnit.Framework;
using static PalindromeNumberFiltering.Selector;

namespace PalindromeNumberFiltering.Tests;

[TestFixture]
public class SelectorTestsParallelApproach
{
    [TestCase(new[] { 2212332, 0, 1405644, 12345, 1, -1236674, 123321, 1111111 }, new[] { 0, 1, 123321, 1111111 })]
    [TestCase(new[] { 1111111112, 987654, -24, 1234654321, 32, 1005 }, new int[] { })]
    [TestCase(new[] { -27, 987656789, 7557, int.MaxValue, 7556, 7243, 7243427, int.MinValue }, new[] { 987656789, 7557, 7243427 })]
    [TestCase(new[] { 111, 111, 111, 11111111 }, new[] { 111, 111, 111, 11111111 })]
    [TestCase(new[] { -1, 0, 111, -11, -1 }, new[] { 0, 111 })]
    [TestCase(new[] { 0, 1, 2, 3, 4 }, new[] { 0, 1, 2, 3, 4 })]
    public void GetPalindromeInParallel_ReturnNewArray(IList<int> array, IList<int> expected)
    {
        var actual = GetPalindromeInParallel(array);
        Assert.That(expected, Is.EquivalentTo(actual));
    }

    [Test]
    public void GetPalindromeInParallel_ArrayIsNull_ThrowArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => GetPalindromeInParallel(null!), "Array can not be null.");
    }

    [Test]
    public void GetPalindromeInParallel_PerformanceTest()
    {
        int sourceLength = 10_000_000;
        int palindromic = 1234554321;
        List<int> source = Enumerable.Repeat(int.MaxValue, sourceLength).ToList();
        int count = 1_000_000, step = sourceLength / count;
        for (int i = 0; i < sourceLength; i += step)
        {
            source[i] = palindromic;
        }

        IList<int> expected = Enumerable.Repeat(palindromic, count).ToList();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var actual = GetPalindromeInParallel(source);
        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.ElapsedMilliseconds} milliseconds");
        Assert.That(expected, Is.EquivalentTo(actual));
    }
}
