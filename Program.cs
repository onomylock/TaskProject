using System.Threading;

namespace TaskProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread myThread = new(new ThreadStart(Print));
                myThread.Name = $"Thread {i + 1}";
                myThread.Start();
            }
        }
        static void Print()
        {
            try
            {
                int count = 0;

                while (Console.KeyAvailable == false)
                {
                    count++;
                    Console.WriteLine($"{Thread.CurrentThread.Name}: {count}");
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException ex)
            {
                Console.WriteLine(ex.ExceptionState);
            }
        }
    }
}
