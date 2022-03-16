using System;
using System.Collections.Generic;

namespace TestSomething {
    class Program {
        static void Main(string[] args) {
            Func<int, Action<string>> makeAction =
                (n) => new Action<string>(
                    (s) => Console.WriteLine($"Observer {n} : {s}"));
            Observer<string>[] observers = new Observer<string>[10];
            DataSupplier<string> msgSupplier = new DataSupplier<string>();
            for(int i = 0; i < 10; i++) {
                observers[i] = new Observer<string>(makeAction(i));
                observers[i].Subscribe(msgSupplier);
            }

            msgSupplier.Data = "hello world";
            msgSupplier.Notify();

            for(int i = 0; i < 5; i++) {
                observers[2 * i].Unsubscribe();
            }

            msgSupplier.Data = "foobar";
            msgSupplier.Notify();
        }
    }

    public class DataSupplier<T> : IObservable<T> {
        private IList<IObserver<T>> observerList;
        private T data;

        public T Data {
            get { return data; }
            set {
                data = value;
                Update(data);
            }
        }

        public DataSupplier() {
            observerList = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            if(!observerList.Contains(observer)) {
                observerList.Add(observer);
                return new Unsubscriber<T>(observerList, observer);
            }
            else {
                return null;
            }
        }

        protected void Update(T data) {
            foreach(var observer in observerList) {
                observer.OnNext(data);
            }
        }

        public void Notify() {
            foreach(var observer in observerList) {
                observer.OnCompleted();
            }
        }
    }

    public class Observer<T> : IObserver<T> {
        private Action<T> action;
        private T data;
        private IDisposable unsubscriber;

        public Observer(Action<T> action) {
            this.action = action;
        }

        public void Subscribe(IObservable<T> observable) {
            IDisposable unsubscriber = observable.Subscribe(this);
            if(unsubscriber != null)
                this.unsubscriber = unsubscriber;
        }

        public void Unsubscribe() {
            unsubscriber?.Dispose();
            unsubscriber = null;
        }

        public void OnNext(T value) {
            data = value;
        }
        public void OnError(Exception error) {
            Console.WriteLine(error.StackTrace);
        }

        public void OnCompleted() {
            action?.Invoke(data);
        }
    }

    public class Unsubscriber<T> : IDisposable {
        private IList<IObserver<T>> observerList;
        private IObserver<T> observer;

        public Unsubscriber(IList<IObserver<T>> observerList, IObserver<T> observer) {
            this.observerList = observerList;
            this.observer = observer;
        }

        public void Dispose() {
            if(observerList.Contains(observer))
                observerList.Remove(observer);
        }
    }
}
