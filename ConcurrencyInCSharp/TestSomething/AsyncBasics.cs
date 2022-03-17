using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestSomething {
    class AsyncBasics {
        public AsyncBasics() {
            
        }

        static async Task<string> DownloadStringWithRetries(string uri) {
            using(var client = new HttpClient()) {
                var nextDelay = TimeSpan.FromSeconds(1);
                for(int i = 0; i != 3; ++i) {
                    try {
                        return await client.GetStringAsync(uri);
                    }
                    catch {
                    }

                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }

                return await client.GetStringAsync(uri);
            }
        }

        static async Task<T> DelayResult<T>(T result, TimeSpan delay) {
            await Task.Delay(delay);
            return result;
        }

        static async Task<string> DownloadStringWithTimeout(string uri) {
            using(var client = new HttpClient()) {
                var downloadTask = client.GetStringAsync(uri);
                var timeoutTask = Task.Delay(3000);

                var completedTask = await Task.WhenAny(downloadTask, timeoutTask);
                if(completedTask == timeoutTask)
                    return null;

                return await downloadTask;
            }
        }

        static Task<T> NotImplementedAsync<T>() {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(new NotImplementedException());
            return tcs.Task;
        }
    }
}
