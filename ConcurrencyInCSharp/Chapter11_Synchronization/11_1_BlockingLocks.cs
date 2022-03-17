using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter11_Synchronization {
    class _11_1_BlockingLocks {
        private readonly object _mutext = new object();
        private int _value;

        public void Increment() {
            lock(_mutext) {
                _value = _value + 1;
            }
        }
    }
}
