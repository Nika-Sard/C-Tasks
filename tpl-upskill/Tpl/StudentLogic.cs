namespace Tpl
{
    public static class StudentLogic
    {
        public static Task TaskCreated()
        {
            Task task = new Task(() =>
            {
            });

            return task;
        }

        public static Task WaitingForActivation()
        {
            return Foo(5);
        }

        public static Task WaitingToRun()
        {
            return Task.Run(() =>
            {
                Task.Delay(1000);
            });
        }

        public static Task Running()
        {
            using (Semaphore sem = new Semaphore(0, 1))
            {
                Task task = Task.Run(() =>
                {
                    sem.Release();
                    for (int i = 0; i < 5; i++)
                    {
                        Task.Delay(500).Wait();
                    }
                });

                sem.WaitOne();
                return task;
            }
        }

        public static Task RanToCompletion()
        {
            Task task = new Task(() =>
            {
                Task.Delay(1000);
            });

            task.Start();

            return task;
        }

        public static Task WaitingForChildrenToComplete()
        {
            Task parent = Task.Factory.StartNew(() =>
            {
                Task child = Task.Factory.StartNew(() => Thread.Sleep(200),
                    TaskCreationOptions.AttachedToParent);
            });

            Thread.Sleep(100);
            return parent;
        }

        public static Task IsCompleted()
        {
            var tcs = new TaskCompletionSource<bool>();
            Task task = tcs.Task;
            tcs.SetResult(true);
            return task;
        }

        public static Task IsCancelled()
        {
            throw new NotImplementedException();
        }

        public static Task IsFaulted()
        {
            throw new NotImplementedException();
        }

        public static List<int> ForceParallelismPlinq()
        {
            var testList = Enumerable.Range(1, 300).ToList();
            var result = testList.AsParallel()
                                 .Select(x => x * 2)
                                 .ToList();
            return result;
        }

        private static async Task<string> Foo(int seconds)
        {
            return await Task.Run(() =>
            {
                for (int i = 0; i < seconds; i++)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }

                return "Foo Completed";
            });
        }
    }
}
