using System.Collections;

namespace StudentScheduleManagementSystem.DataStructure
{
    internal class Vector<T> : IEnumerable<T>, ICollection<T>
    {
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly Vector<T> _vec;
            private int _index;
            private readonly int _version;
            private T? _current;

            internal Enumerator(Vector<T> vec)
            {
                _vec = vec;
                _index = 0;
                _version = vec._version;
                _current = default;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                Vector<T> localVec = _vec;

                if (_version == localVec._version && _index < localVec.Count)
                {
                    _current = localVec._array[_index];
                    _index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (_version != _vec._version)
                {
                    throw new InvalidOperationException();
                }

                _index = _vec.Count + 1;
                _current = default;
                return false;
            }

            public T Current => _current!;

            object? IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index == _vec.Count + 1)
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (_version != _vec._version)
                {
                    throw new InvalidOperationException();
                }

                _index = 0;
                _current = default;
            }
        }

        private T[] _array;
        private int _version;
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public Vector()
        {
            _array = new T[10];
            Count = 0;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return _array[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                _array[index] = value;
                _version++;
            }
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (Count == _array.Length)
            {
                T[] newArray = new T[_array.Length * 2];
                Array.Copy(_array, newArray, _array.Length);
                _array = newArray;
            }
            _array[Count] = item;
            Count++;
            _version++;
        }

        public void Clear()
        {
            _array = new T[10];
            Count = 0;
            _version++;
        }

        public bool Contains(T element)
        {
            bool found = false;
            foreach (var item in _array)
            {
                if (element!.Equals(item))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public void CopyTo(T[] array, int index)
        {
            Array.Copy(_array, 0, array, index, Count);
        }

        public bool Remove(T element)
        {
            bool found = false;
            for (int i = 0; i < Count; i++)
            {
                if (element!.Equals(_array[i]))
                {
                    found = true;
                    Count--;
                    _version++;
                    Array.Copy(_array, i + 1, _array, i, Count - i);
                    break;
                }
            }
            return found;
        }
    }

    public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private const int InitialCapacity = 10;
        private const float LoadFactor = 0.75f;

        private Entry<TKey, TValue>[] table;
        private int count;

        public MyDictionary()
        {
            table = new Entry<TKey, TValue>[InitialCapacity];
            count = 0;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                int index = FindEntryIndex(key);
                if (index >= 0)
                {
                    table[index].Value = value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public int Count => count;

        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>(count);
                foreach (Entry<TKey, TValue> entry in table)
                {
                    if (entry != null)
                    {
                        keys.Add(entry.Key);
                    }
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>(count);
                foreach (Entry<TKey, TValue> entry in table)
                {
                    if (entry != null)
                    {
                        values.Add(entry.Value);
                    }
                }
                return values;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (count >= table.Length * LoadFactor)
            {
                ResizeTable();
            }

            int index = GetBucketIndex(key);
            Entry<TKey, TValue> entry = table[index];
            while (entry != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    throw new ArgumentException("An element with the same key already exists in the dictionary.");
                }
                entry = entry.Next;
            }

            entry = new Entry<TKey, TValue>(key, value);
            entry.Next = table[index];
            table[index] = entry;
            count++;
        }

        public bool ContainsKey(TKey key)
        {
            int index = GetBucketIndex(key);
            Entry<TKey, TValue> entry = table[index];
            while (entry != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    return true;
                }
                entry = entry.Next;
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            int index = GetBucketIndex(key);
            Entry<TKey, TValue> entry = table[index];
            Entry<TKey, TValue> previous = null;
            while (entry != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    if (previous == null)
                    {
                        table[index] = entry.Next;
                    }
                    else
                    {
                        previous.Next = entry.Next;
                    }
                    count--;
                    return true;
                }
                previous = entry;
                entry = entry.Next;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetBucketIndex(key);
            Entry<TKey, TValue> entry = table[index];
            while (entry != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    value = entry.Value;
                    return true;
                }
                entry = entry.Next;
            }
            value = default(TValue);
            return false;
        }

        private int GetBucketIndex(TKey key)
        {
            int hashCode = key.GetHashCode() & 0x7FFFFFFF; // Ensure positive hash code
            return hashCode % table.Length;
        }
        private int FindEntryIndex(TKey key)
        {
            int index = GetBucketIndex(key);
            Entry<TKey, TValue> entry = table[index];
            int i = 0;
            while (entry != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(entry.Key, key))
                {
                    return index + i;
                }
                entry = entry.Next;
                i++;
            }
            return -1;
        }

        private void ResizeTable()
        {
            int newCapacity = table.Length * 2;
            Entry<TKey, TValue>[] newTable = new Entry<TKey, TValue>[newCapacity];

            foreach (Entry<TKey, TValue> entry in table)
            {
                Entry<TKey, TValue> current = entry;
                while (current != null)
                {
                    int newIndex = GetBucketIndex(current.Key);
                    Entry<TKey, TValue> next = current.Next;
                    current.Next = newTable[newIndex];
                    newTable[newIndex] = current;
                    current = next;
                }
            }

            table = newTable;
        }

        private class Entry<TKey, TValue>
        {
            public TKey Key { get; }
            public TValue Value { get; set; }
            public Entry<TKey, TValue> Next { get; set; }

            public Entry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (Entry<TKey, TValue> entry in table)
            {
                Entry<TKey, TValue> current = entry;
                while (current != null)
                {
                    yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                    current = current.Next;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class SimpleHashSet<T>
    {
        private const int DefaultCapacity = 16;
        private const double LoadFactor = 0.75;

        private Entry<T>[] buckets;
        private int count;

        public SimpleHashSet()
        {
            buckets = new Entry<T>[DefaultCapacity];
            count = 0;
        }

        public bool Add(T item)
        {
            EnsureCapacity();

            int bucketIndex = GetBucketIndex(item);

            if (Contains(item, bucketIndex))
            {
                return false;
            }

            Entry<T> entry = new Entry<T>(item, bucketIndex, buckets[bucketIndex]);
            buckets[bucketIndex] = entry;
            count++;
            return true;
        }

        public bool Remove(T item)
        {
            int bucketIndex = GetBucketIndex(item);

            if (buckets[bucketIndex] == null)
            {
                return false;
            }

            if (buckets[bucketIndex].Item.Equals(item))
            {
                buckets[bucketIndex] = buckets[bucketIndex].Next;
                count--;
                return true;
            }

            Entry<T> current = buckets[bucketIndex].Next;
            Entry<T> previous = buckets[bucketIndex];

            while (current != null)
            {
                if (current.Item.Equals(item))
                {
                    previous.Next = current.Next;
                    count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }

        public bool Contains(T item)
        {
            int bucketIndex = GetBucketIndex(item);

            return Contains(item, bucketIndex);
        }

        public void Clear()
        {
            Array.Clear(buckets, 0, buckets.Length);
            count = 0;
        }

        public int Count
        {
            get { return count; }
        }

        private void EnsureCapacity()
        {
            if (count >= buckets.Length * LoadFactor)
            {
                Resize();
            }
        }

        private void Resize()
        {
            int newCapacity = buckets.Length * 2;
            Entry<T>[] newBuckets = new Entry<T>[newCapacity];

            foreach (Entry<T> entry in buckets)
            {
                Entry<T> current = entry;

                while (current != null)
                {
                    int newBucketIndex = GetBucketIndex(current.Item, newCapacity);
                    Entry<T> next = current.Next;
                    current.Next = newBuckets[newBucketIndex];
                    newBuckets[newBucketIndex] = current;
                    current = next;
                }
            }

            buckets = newBuckets;
        }

        private bool Contains(T item, int bucketIndex)
        {
            Entry<T> current = buckets[bucketIndex];

            while (current != null)
            {
                if (current.Item.Equals(item))
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        private int GetBucketIndex(T item)
        {
            return GetBucketIndex(item, buckets.Length);
        }

        private int GetBucketIndex(T item, int capacity)
        {
            int hashCode = item.GetHashCode();
            int bucketIndex = hashCode & (capacity - 1);  // equivalent to hashCode % capacity

            return bucketIndex;
        }

        private class Entry<TEntry>
        {
            public TEntry Item { get; }
            public Entry<TEntry> Next { get; set; }

            public Entry(TEntry item, int bucketIndex, Entry<TEntry> next)
            {
                Item = item;
                Next = next;
            }
        }
    }
}