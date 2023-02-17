using System.Threading;

namespace TaskProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            List<Task> tasks = new List<Task>();
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    int num = i;

                    tasks.Add(new Task(() =>
                    {
                        int count = 0;
                        {
                            while (true)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    token.ThrowIfCancellationRequested();
                                }
                                else
                                {
                                    count++;
                                    Console.WriteLine($"{num + 1}: {count}");
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }, token));
                    tasks.Last().Start();
                }

                while (true)
                {
                    if (Console.KeyAvailable == true)
                    {
                        cts.Cancel();
                        foreach (Task task in tasks)
                        {
                            task.Wait();
                        }

                        break;
                    }
                }
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                    {
                        Console.WriteLine("Operation aborted");
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            finally
            {
                cts.Dispose();
            }

            foreach (Task task in tasks)
            {

                Console.WriteLine($"Task {tasks.IndexOf(task)} status: {task.Status}");
            }
        }
    }
}
