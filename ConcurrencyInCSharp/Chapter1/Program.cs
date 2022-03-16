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

            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

            val *= 2;

            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            Console.WriteLine(val);
        }

        //IEnumerable<bool> PrimalityTest(IEnumerable<int> values) {
        //    return values.AsParallel().Select(val => IsPrime(val));
        //}

        static void ProcessArray(double[] array) {
            Parallel.Invoke(
                () => ProcessPartialArray(array, 0, array.Length / 2),
                () => ProcessPartialArray(array, array.Length / 2, array.Length));
        }

        static void ProcessPartialArray(double[] array, int begin, int end) {

        }
    }
}
