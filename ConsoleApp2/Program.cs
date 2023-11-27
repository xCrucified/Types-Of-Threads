using Newtonsoft.Json;
using System.Text.Json;
using System.Xml;

namespace ConsoleApp2
{
    internal class Program
    {
        public static string path = "C:\\Users\\mka.STEP.000\\source\\repos\\ConsoleApp2";
        public static string path2 = "C:\\Users\\mka.STEP.000\\source\\repos\\ConsoleApp1\\sum";
        public static string path3 = "C:\\Users\\mka.STEP.000\\source\\repos\\ConsoleApp1\\mult";

        static void Main(string[] args)
        {
            AutoResetEvent are = new AutoResetEvent(true);

            ThreadPool.QueueUserWorkItem(GenerateAPair, are);

            AutoResetEvent resetEvent = new AutoResetEvent(false);

            ThreadPool.QueueUserWorkItem(Sum, resetEvent);
            ThreadPool.QueueUserWorkItem(Mult, resetEvent);

            Console.ReadKey();

        }

        public static void GenerateAPair(object obj)
        {
            var ev = obj as EventWaitHandle;

            if (ev.WaitOne(1))
            {

                var tmp = (AutoResetEvent)obj;
                ev.Reset();
                KeyValuePair<int, int>[] kvp = new KeyValuePair<int, int>[5];
                for (int i = 0; i < kvp.Length; i++)
                {
                    kvp[i] = new KeyValuePair<int, int>(new Random().Next(1, 20), new Random().Next(1, 20));
                }
                string jsonString = JsonConvert.SerializeObject(kvp, Newtonsoft.Json.Formatting.None);
                File.WriteAllText(path, jsonString);
                Console.WriteLine(jsonString);
            }
            else
            {
                Console.WriteLine("Thread {0} late", Thread.CurrentThread.ManagedThreadId);
            }

            Console.ReadKey();
        }

        public static void Sum(object obj)
        {

            var arr = JsonConvert.DeserializeObject<KeyValuePair<int, int>[]>(File.ReadAllText(path));

            int[] sumarr = new int[arr.Length];

            for(int i = 0; i < arr.Length; i++) {sumarr[i] = arr[i].Value + arr[i].Key;}

            File.WriteAllText(path2, JsonConvert.SerializeObject(sumarr, Newtonsoft.Json.Formatting.None));
            
            (obj as EventWaitHandle).WaitOne();
        }

        public static void Mult(object obj)
        {
            

            var arr = JsonConvert.DeserializeObject<KeyValuePair<int, int>[]>(File.ReadAllText(path));

            int[] multiarr = new int[arr.Length];

            for (int i = 0; i < arr.Length; i++) { multiarr[i] = arr[i].Value * arr[i].Key; }

            File.WriteAllText(path3, JsonConvert.SerializeObject(multiarr, Newtonsoft.Json.Formatting.None));

            (obj as EventWaitHandle).WaitOne();
        }


    }
}