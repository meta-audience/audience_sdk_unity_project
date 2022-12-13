using System;
using System.Collections;

namespace AudienceSDK.Scripts {

    internal class WeakReferenceTable {
        private Hashtable _table = new Hashtable();

        public object this[object key] {
            get {
                WeakReference reference = this._table[key] as WeakReference;
                return reference.NullOrValue();
            }
        }

        public ICollection CopiedValues {
            get {
                var array = new object[this._table.Count];
                int i = 0;
                foreach (var value in this._table.Values) {

                    var reference = value as WeakReference;
                    array[i] = reference.NullOrValue();
                    i++;
                }

                return array;
            }
        }

        public void Add(object key, object value) {
            this._table.Add(key, new WeakReference(value));
        }

        public void Remove(object key) {
            this._table.Remove(key);
        }

        public void Clear() {
            this._table.Clear();
        }

        public bool ContainsKey(object key) {
            return this._table.ContainsKey(key);
        }
    }
}
