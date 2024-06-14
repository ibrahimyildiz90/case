using Integration.Locks;
using Integration.Service;

namespace Integration;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var service = new ItemIntegrationService();
        ExecuteMultipleTaskAsync(service).Wait();


        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("a"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("b"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("c"));

        //Thread.Sleep(500);

        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("a"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("b"));
        //ThreadPool.QueueUserWorkItem(_ => service.SaveItem("c"));

        //Thread.Sleep(5000);

        //Console.WriteLine("Everything recorded:");

        //service.GetAllItems().ForEach(Console.WriteLine);

        //Console.ReadLine();
    }

    public static async Task ExecuteMultipleTaskAsync(ItemIntegrationService service)    {
        
        Task task1 = service.ExcAsync(lockObject: LockAsync.LockOnValue("lock_a"), itemContent: "a");
        Task task2 = service.ExcAsync(lockObject: LockAsync.LockOnValue("lock_b"), itemContent: "b");
        Task task3 = service.ExcAsync(lockObject: LockAsync.LockOnValue("lock_c"), itemContent: "c");

        Task task4 = service.ExcAsync(lockObject: LockAsync.LockOnValue("lock_a"), itemContent: "a");
        Task task5 = service.ExcAsync(lockObject: LockAsync.LockOnValue("lock_b"), itemContent: "b");
        Task task6 = service.ExcAsync(lockObject: LockAsync.LockOnValue("lock_c"), itemContent: "c");

        Task allTasks = Task.WhenAll(task1, task2, task3,task4,task5,task6);

        try
        {
            await allTasks;

            Console.WriteLine("Everything recorded:");
            service.GetAllItems().ForEach(Console.WriteLine);

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
            Console.WriteLine("Task IsFaulted: " + allTasks.IsFaulted);

            foreach (var inEx in allTasks.Exception.InnerExceptions)
            {
                Console.WriteLine("Task Inner Exception: " + inEx.Message);
            }
            Console.ReadLine();
        }
    }
}