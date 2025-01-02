# Filtering Palindromes Numbers: Sequential and Parallel Approaches

Intermediate level task for practicing `Parallel.ForEach` for CPU-bound operations.

Estimated time to complete the task - 1.5h.

The task requires .NET 8 SDK installed.

## Task Description

Implement a `Selector` static class that contains methods for filtering palindrome numbers from a collection of integers. The class provides two different approaches for filtering palindrome numbers: sequential filtering and parallel filtering.

Implement the following methods in the [Selector](/PalindromeNumberFiltering/Selector.cs) class:

- `GetPalindromeInSequence`: This method retrieves a collection of palindrome numbers from the given list of integers using sequential filtering. The method utilizes the private `IsPalindrome` method to determine whether a number is a palindrome or not.

- `GetPalindromeInParallel`: This method retrieves a collection of palindrome numbers from the given list of integers using parallel filtering. The method utilizes parallel processing with [Parallel.ForEach](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-simple-parallel-foreach-loop) to filter palindrome numbers concurrently and [ConcurrentBag](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-7.0) to store the result. It also uses the private `IsPalindrome` method to check whether a number is a palindrome.

The `Selector` class contains several private helper methods:
- `IsPalindrome`: This private method checks whether the given integer is a palindrome number. It first verifies that the number is non-negative and then uses the `IsPositiveNumberPalindrome` method to perform a recursive check on positive numbers.
- `IsPositiveNumberPalindrome`: This private method recursively checks whether a positive number is a palindrome. It compares the first digit with the last digit and proceeds with the check by removing these digits from the number until the number becomes less than 10.
- `GetLength`: This private method calculates the number of digits in the given integer. It uses a switch statement to handle different ranges of numbers efficiently.

_Note_: To determine if a number is a palindrome, you must implement the solution without using arrays, collections, or the string class: use only basic numerical operations and logical comparisons.

To get insights into the benefits of parallel processing when dealing with computationally intensive tasks like filtering palindrome numbers from a large collection of integers compare the efficiency of the two approaches in terms of performance (see [GetPalindromeInParallel_PerformanceTest](PalindromeNumberFiltering.Tests/SelectorTestsParallelApproach.cs#L28) and [GetPalindromeInSequence_PerformanceTest](PalindromeNumberFiltering.Tests/SelectorTestsSequentialApproach.cs#L24)). 
