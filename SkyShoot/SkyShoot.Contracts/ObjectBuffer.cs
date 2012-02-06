using System.Collections.Generic;

namespace SkyShoot.Contracts
{
    public class ObjectBuffer<T>
    {
        private readonly T[] _buffer;
        private readonly int _size;

        private int _currentIndex;

        public int Count
        {
            get { return _size; }
        }

        public ObjectBuffer(int size)
        {
            _size = size;
            _buffer = new T[size];
        }

        public T this[int index]
        {
            get { return _buffer[index]; }
            set { _buffer[index] = value; }
        }

        public void Add(T value)
        {
            _buffer[_currentIndex++] = value;
            _currentIndex %= _size;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var value in collection)
            {
                Add(value);
            }
        }
    }
}
