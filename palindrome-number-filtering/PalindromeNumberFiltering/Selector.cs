namespace PalindromeNumberFiltering;

/// <summary>
/// A static class containing methods for filtering palindrome numbers from a collection of integers.
/// </summary>
public static class Selector
{
    /// <summary>
    /// Retrieves a collection of palindrome numbers from the given list of integers using sequential filtering.
    /// </summary>
    /// <param name="numbers">The list of integers to filter.</param>
    /// <returns>A collection of palindrome numbers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
    public static IList<int> GetPalindromeInSequence(IList<int>? numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers, nameof(numbers));

        List<int> list = new List<int>();
        foreach (var number in numbers)
        {
            if (IsPalindrome(number))
            {
                list.Add(number);
            }
        }

        return list;
    }

    /// <summary>
    /// Retrieves a collection of palindrome numbers from the given list of integers using parallel filtering.
    /// </summary>
    /// <param name="numbers">The list of integers to filter.</param>
    /// <returns>A collection of palindrome numbers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
    public static IList<int> GetPalindromeInParallel(IList<int> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers, nameof(numbers));

        List<int> list = new List<int>();
        Parallel.ForEach(numbers, number =>
        {
            if (IsPalindrome(number))
            {
                list.Add(number);
            }
        });

        return list;
    }

    /// <summary>
    /// Checks whether the given integer is a palindrome number.
    /// </summary>
    /// <param name="number">The integer to check.</param>
    /// <returns>True if the number is a palindrome, otherwise false.</returns>
    private static bool IsPalindrome(int number)
    {
        if (number < 0)
        {
            return false;
        }

        int divider = (int)Math.Pow(10, GetLength(number) - 1);
        return IsPositiveNumberPalindrome(number, divider);
    }

    /// <summary>
    /// Recursively checks whether a positive number is a palindrome.
    /// </summary>
    /// <param name="number">The positive number to check.</param>
    /// <param name="divider">The divider used in the recursive check.</param>
    /// <returns>True if the positive number is a palindrome, otherwise false.</returns>
    private static bool IsPositiveNumberPalindrome(int number, int divider)
    {
        if (number == 0 || number < divider)
        {
            return true;
        }

        int leftMostDigit = number / divider;
        int rightMostDigit = number % 10;

        if (leftMostDigit != rightMostDigit)
        {
            return false;
        }

        number = (number % divider) / 10;
        divider /= 100;

        return IsPositiveNumberPalindrome(number, divider);
    }

    /// <summary>
    /// Gets the number of digits in the given integer.
    /// </summary>
    /// <param name="number">The integer to count digits for.</param>
    /// <returns>The number of digits in the integer.</returns>
    private static byte GetLength(int number)
    {
        byte length = 0;
        while (number > 0)
        {
            number /= 10;
            length++;
        }

        return length;
    }
}
