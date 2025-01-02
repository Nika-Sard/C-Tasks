namespace NumberTheory;

public static class SieveOfEratosthenes
{
    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a sequential approach.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersSequentialAlgorithm(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "The number must be greater than zero.");
        }

        return GetPrimeNumbers(n);
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a modified sequential approach.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersModifiedSequentialAlgorithm(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "The number must be greater than zero.");
        }

        int nSquareRoot = (int)Math.Sqrt(n);
        List<int> basePrimes = GetPrimeNumbers(nSquareRoot);
        List<int> primes = new List<int>(GetPrimeNumbersModified(nSquareRoot + 1, n, basePrimes));
        basePrimes.AddRange(primes);
        return basePrimes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a concurrent approach by data decomposition.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentDataDecomposition(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "The number must be greater than zero.");
        }

        int firstStart = (int)Math.Sqrt(n) + 1;
        int firstEnd = (n + firstStart) / 2;
        int secondStart = firstEnd + 1;
        int secondEnd = n;

        List<int> basePrimes = GetPrimeNumbers(firstStart - 1);
        List<int> primes = new List<int>(basePrimes);

        Thread thread = new Thread(() =>
        {
            List<int> firstHalfPrimes = GetPrimeNumbersModified(firstStart, firstEnd, basePrimes);
            lock (primes)
            {
                primes.AddRange(firstHalfPrimes);
            }
        });

        thread.Start();

        List<int> secondHalfPrimes = GetPrimeNumbersModified(secondStart, secondEnd, basePrimes);
        lock (primes)
        {
            primes.AddRange(secondHalfPrimes);
        }

        thread.Join();
        primes.Sort();

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a concurrent approach by "basic" primes decomposition.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentBasicPrimesDecomposition(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "The number must be greater than zero.");
        }

        int nSquareRoot = (int)Math.Sqrt(n);
        List<int> basePrimes = GetPrimeNumbers(nSquareRoot);
        List<int> primes = new List<int>(basePrimes);
        bool[] numberStatus = new bool[n - nSquareRoot + 1];

        Thread thread = new Thread(() =>
        {
            for (int i = 0; i < basePrimes.Count / 2; i++)
            {
                int basePrime = basePrimes[i];
                int startNumber = Math.Max(
                    basePrime * basePrime,
                    (nSquareRoot + basePrime - 1) / basePrime * basePrime);

                for (int j = startNumber; j <= n; j += basePrime)
                {
                    if (j >= nSquareRoot + 1)
                    {
                        numberStatus[j - (nSquareRoot + 1)] = true;
                    }
                }
            }
        });

        thread.Start();

        for (int i = basePrimes.Count / 2; i < basePrimes.Count; i++)
        {
            int basePrime = basePrimes[i];
            int startNumber = Math.Max(
                basePrime * basePrime,
                (nSquareRoot + basePrime - 1) / basePrime * basePrime);

            for (int j = startNumber; j <= n; j += basePrime)
            {
                if (j >= nSquareRoot + 1)
                {
                    numberStatus[j - (nSquareRoot + 1)] = true;
                }
            }
        }

        thread.Join();

        for (int i = nSquareRoot + 1; i <= n; i++)
        {
            if (!numberStatus[i - (nSquareRoot + 1)])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using thread pool and signaling construct.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentWithThreadPool(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "The number must be greater than zero.");
        }

        int nSquareRoot = (int)Math.Sqrt(n);
        List<int> basePrimes = GetPrimeNumbers(nSquareRoot);
        List<int> primes = new List<int>(basePrimes);
        bool[] numberStatus = new bool[n - nSquareRoot + 1];

        using (CountdownEvent countdown = new CountdownEvent(basePrimes.Count))
        {
            foreach (int basePrime in basePrimes)
            {
                int prime = basePrime;
                ThreadPool.QueueUserWorkItem(
                    _ =>
                    {
                        int startNumber = Math.Max(
                            prime * prime,
                            (nSquareRoot + prime - 1) / prime * prime);

                        for (int j = startNumber; j <= n; j += prime)
                        {
                            if (j >= nSquareRoot + 1)
                            {
                                numberStatus[j - (nSquareRoot + 1)] = true;
                            }
                        }

                        countdown.Signal();
                    },
                    basePrime);
            }

            countdown.Wait();
        }

        for (int i = nSquareRoot + 1; i <= n; i++)
        {
            if (!numberStatus[i - (nSquareRoot + 1)])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    private static List<int> GetPrimeNumbers(int n)
    {
        bool[] numberStatus = new bool[n + 1];
        for (int i = 2; i <= (int)Math.Sqrt(n) + 1; i++)
        {
            if (!numberStatus[i])
            {
                for (int j = i * i; j <= n; j += i)
                {
                    numberStatus[j] = true;
                }
            }
        }

        List<int> primeNumbers = new List<int>();
        for (int i = 2; i <= n; i++)
        {
            if (!numberStatus[i])
            {
                primeNumbers.Add(i);
            }
        }

        return primeNumbers;
    }

    private static List<int> GetPrimeNumbersModified(int start, int end, List<int> basePrimes)
    {
        List<int> primesModified = new List<int>();
        bool[] numberStatus = new bool[end - start + 1];
        foreach (int basePrime in basePrimes)
        {
            int startNumber = Math.Max(basePrime * basePrime, (start + basePrime - 1) / basePrime * basePrime);
            for (int j = startNumber; j <= end; j += basePrime)
            {
                numberStatus[j - start] = true;
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (!numberStatus[i - start])
            {
                primesModified.Add(i);
            }
        }

        return primesModified;
    }
}
