using System;
using System.Threading.Tasks;

namespace Chapter1 {
    class Program {
        static void Main(string[] args) {
            Task task = DoSomethingAsync();
            task.Wait();
        }

        static async Task DoSomethingAsync() {
            int val = 13;
            Console.WriteLine(val);

            await Task.Delay(TimeSpan.FromSeconds(1));

            val *= 2;

            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine(val);
        }
    }
}
