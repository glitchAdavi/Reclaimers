using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclaimers.Extensions;

namespace Reclaimers.UpdateSystem
{
    public class CustomList<T> : IReadOnlyList<T>
    {
        private List<T> _list = new List<T>();
        private Dictionary<T, int> _dict = new Dictionary<T, int>();
        private int _loopIndex;

        public int Count => _list.Count;

        public T this[int index] => index >= 0 && index < Count ? _list[index] : default;

        public CustomList() { }
        public CustomList(List<T> _toCopy)
        {
            _list = new List<T>(_toCopy);
        }
        public CustomList(CustomList<T> _toCopy)
        {
            _list = new List<T>(_toCopy);
        }

        public bool Add(T value)
        {
            if (_dict.ContainsKey(value)) return false;

            _list.Add(value);
            _dict.Add(value, _list.Count - 1);
            return true;
        }

        public bool Remove(T value)
        {
            if (!_dict.TryGetValue(value, out int indexToRemove)) return false;

            _dict.Remove(value);

            if (indexToRemove == _loopIndex) _loopIndex--;
            else if (indexToRemove < _loopIndex)
            {
                _list.Swap(_loopIndex, indexToRemove, out T swappedValue);
                _dict[swappedValue] = indexToRemove;
                indexToRemove = _loopIndex;
                _loopIndex--;
            }

            _list.RemoveAtSwapBack(indexToRemove, out T swappedBack);
            if (swappedBack != null) _dict[swappedBack] = indexToRemove;

            return true;
        }

        public void Clear()
        {
            _list.Clear();
            _dict.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private CustomList<T> _list;

            public Enumerator(CustomList<T> list)
            {
                _list = list;
                Reset();
            }

            public T Current => _list[_list._loopIndex];
            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (_list._loopIndex < _list.Count - 1)
                {
                    _list._loopIndex++;
                    return true;
                }
                else return false;
            }


            public void Reset()
            {
                _list._loopIndex = -1;
            }
        }
    }
}

