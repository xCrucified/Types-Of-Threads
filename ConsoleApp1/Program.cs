namespace ConsoleApp1
{
    internal class Program
    {
        public static void GenerateArray(object obj)
        {
            var a = obj as Semaphore;

            bool stop = false;
            while (!stop)
            {
                if (a.WaitOne())
                {
                    try
                    {
                        int[] arr = new int[new Random().Next(1, 20)];
                        for (int i = 0; i < arr.Length; i++) { arr[i] = new Random().Next(1, 100); }
                        string tmp = $"Thread {Thread.CurrentThread.ManagedThreadId} generated the next list of number : ";
                        for(int i = 0; i < arr.Length; i++) { tmp += ($" {arr[i]}"); }
                        Console.WriteLine(tmp);
                        Thread.Sleep(4000);
                    }
                    finally
                    {
                        stop = true;
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} remove a lock");
                        a.Release();
                    }
                }
                else
                {
                    Console.WriteLine($"Timeout for thread {Thread.CurrentThread.ManagedThreadId} expired. Re-waiting...");
                }
            }
        }

        static void Main(string[] args)
        {
            Semaphore s = new Semaphore(3, 3);
            for(int i = 0; i < 15; ++i)
            {
                ThreadPool.QueueUserWorkItem(GenerateArray, s);
            }

            Console.ReadKey();
        }

    }
}