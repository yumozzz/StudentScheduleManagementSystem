using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace StudentScheduleManagementSystem.DataStructure
{
    public class Vector<T> : IEnumerable<T>, ICollection<T>
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

            public void Dispose() { }

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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

    public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        private struct Entry
        {
            public int hashCode; // Lower 31 bits of hash code, -1 if unused
            public int next; // Index of next entry, -1 if last
            public TKey key; // Key of entry
            public TValue value; // Value of entry
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            private readonly HashTable<TKey, TValue> _hashTable;
            private readonly int _version;
            private int _index;
            private KeyValuePair<TKey, TValue> _current;

            internal Enumerator(HashTable<TKey, TValue> hashTable)
            {
                this._hashTable = hashTable;
                _version = hashTable._version;
                _index = 0;
                _current = new();
            }

            public bool MoveNext()
            {
                if (_version != _hashTable._version)
                {
                    throw new InvalidOperationException();
                }

                while (_index < _hashTable._count)
                {
                    if (_hashTable._entries[_index].hashCode >= 0)
                    {
                        _current = new KeyValuePair<TKey, TValue>(_hashTable._entries[_index].key,
                                                                  _hashTable._entries[_index].value);
                        _index++;
                        return true;
                    }
                    _index++;
                }

                _index = _hashTable._count + 1;
                _current = new KeyValuePair<TKey, TValue>();
                return false;
            }

            public KeyValuePair<TKey, TValue> Current => _current;

            public void Dispose() { }

            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || (_index == _hashTable._count + 1))
                    {
                        throw new InvalidOperationException();
                    }

                    return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
                }
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    if (_index == 0 || (_index == _hashTable._count + 1))
                    {
                        throw new InvalidOperationException();
                    }

                    return new DictionaryEntry(_current.Key, _current.Value);
                }
            }

            void IEnumerator.Reset()
            {
                if (_version != _hashTable._version)
                {
                    throw new InvalidOperationException();
                }

                _index = 0;
                _current = new KeyValuePair<TKey, TValue>();
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    if (_index == 0 || (_index == _hashTable._count + 1))
                    {
                        throw new InvalidOperationException();
                    }

                    return _current.Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    if (_index == 0 || (_index == _hashTable._count + 1))
                    {
                        throw new InvalidOperationException();
                    }

                    return _current.Value;
                }
            }
        }

        private readonly int[] _primes = new[]
        {
            11,
            23,
            47,
            97,
            197,
            397,
            797,
            1597,
            3191,
            6389,
            12791,
            25589,
            51197,
            102407,
            204821,
            409609,
            819241,
            1638487,
            3276967,
            6553949,
            13107893,
            26215771,
            52431647,
            104863301,
            209726639
        };

        private int[] _buckets;
        private Entry[] _entries;
        private int _count;
        private int _version;
        private int _freeList;
        private int _freeCount;
        private readonly EqualityComparer<TKey> _comparer = EqualityComparer<TKey>.Default;

        public bool IsReadOnly => false;
        public int Count => _count - _freeCount;

        public HashTable()
        {
            int size = 11;
            _buckets = new int[size];
            Array.Fill(_buckets, -1);
            _entries = new Entry[size];
            _freeList = -1;
        }

        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new(Count);
                foreach (var bucket in _buckets)
                {
                    if (bucket == -1)
                    {
                        continue;
                    }
                    Entry entry = _entries[bucket];
                    while (true)
                    {
                        keys.Add(entry.key);
                        if (entry.next == -1)
                        {
                            break;
                        }
                        entry = _entries[entry.next];
                    }
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new(Count);
                foreach (var bucket in _buckets)
                {
                    Entry entry = _entries[bucket];
                    if (bucket == -1)
                    {
                        continue;
                    }
                    while (true)
                    {
                        values.Add(entry.value);
                        if (entry.next == -1)
                        {
                            break;
                        }
                        entry = _entries[entry.next];
                    }
                }
                return values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                int i = FindEntry(key);
                return i >= 0 ? _entries[i].value : default;
            }
            set => Insert(key, value, false);
        }

        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public void Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int i = FindEntry(keyValuePair.Key);
            if (i >= 0 && EqualityComparer<TValue>.Default.Equals(_entries[i].value, keyValuePair.Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int i = FindEntry(keyValuePair.Key);
            if (i >= 0 && EqualityComparer<TValue>.Default.Equals(_entries[i].value, keyValuePair.Value))
            {
                Remove(keyValuePair.Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            if (_count == 0)
            {
                return;
            }
            for (int i = 0; i < _buckets.Length; i++)
            {
                _buckets[i] = -1;
            }
            Array.Clear(_entries, 0, _count);
            _freeList = -1;
            _count = 0;
            _freeCount = 0;
            _version++;
        }

        public bool ContainsKey(TKey key) => FindEntry(key) >= 0;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int FindEntry(TKey key)
        {
            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            for (int i = _buckets[hashCode % _buckets.Length]; i >= 0; i = _entries[i].next)
            {
                if (_entries[i].hashCode == hashCode && _comparer.Equals(_entries[i].key, key))
                {
                    return i;
                }
            }
            return -1;
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            int targetBucket = hashCode % _buckets.Length;

            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].next)
            {
                if (_entries[i].hashCode == hashCode && _comparer.Equals(_entries[i].key, key))
                {
                    if (add)
                    {
                        throw new ArgumentException("item already existed");
                    }
                    _entries[i].value = value;
                    _version++;
                    return;
                }
            }

            int index;
            if (_freeCount > 0)
            {
                index = _freeList;
                _freeList = _entries[index].next;
                _freeCount--;
            }
            else
            {
                if (_count == _entries.Length)
                {
                    Resize();
                    targetBucket = hashCode % _buckets.Length;
                }
                index = _count;
                _count++;
            }

            _entries[index].hashCode = hashCode;
            _entries[index].next = _buckets[targetBucket];
            _entries[index].key = key;
            _entries[index].value = value;
            _buckets[targetBucket] = index;
            _version++;
        }

        private void Resize()
        {
            int size = _primes.FirstOrDefault(size => size > _buckets.Length);
            if (size == _buckets.Length)
            {
                throw new Exception("Hashtable too big");
            }
            Resize(size);
        }

        private void Resize(int newSize)
        {
            int[] newBuckets = new int[newSize];
            Array.Fill(newBuckets, -1);
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, 0, newEntries, 0, _count);
            for (int i = 0; i < _count; i++)
            {
                if (newEntries[i].hashCode >= 0)
                {
                    int bucket = newEntries[i].hashCode % newSize;
                    newEntries[i].next = newBuckets[bucket];
                    newBuckets[bucket] = i;
                }
            }
            _buckets = newBuckets;
            _entries = newEntries;
        }

        public bool Remove(TKey key)
        {
            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
            int bucket = hashCode % _buckets.Length;
            int last = -1;
            for (int i = _buckets[bucket]; i >= 0; last = i, i = _entries[i].next)
            {
                if (_entries[i].hashCode == hashCode && _comparer.Equals(_entries[i].key, key))
                {
                    if (last < 0)
                    {
                        _buckets[bucket] = _entries[i].next;
                    }
                    else
                    {
                        _entries[last].next = _entries[i].next;
                    }
                    _entries[i].hashCode = -1;
                    _entries[i].next = _freeList;
                    _entries[i].key = default;
                    _entries[i].value = default;
                    _freeList = i;
                    _freeCount++;
                    _version++;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int i = FindEntry(key);
            if (i >= 0)
            {
                value = _entries[i].value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            Entry[] entries = this._entries;
            for (int i = 0; i < _count; i++)
            {
                if (entries[i].hashCode >= 0)
                {
                    array[index++] = new(entries[i].key, entries[i].value);
                }
            }
        }
    }
}