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

                if (_version == localVec._version && ((uint)_index < (uint)localVec.Count))
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
}