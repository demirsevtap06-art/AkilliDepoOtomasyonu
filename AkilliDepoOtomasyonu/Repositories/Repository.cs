using AkilliDepoOtomasyonu.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkilliDepoOtomasyonu.Repositories
{
    public class Repository<T> where T : class, IEntity
    {
        private readonly List<T> _items = new List<T>();
        public void Add(T item)
        {
            if (_items.Any(x => x.Id == item.Id))
            {
                throw new InvalidOperationException($"Hata: {item.Id} ID'li öge zaten mevcut!");
            }
            _items.Add(item);
        }
        public bool Remove(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                return _items.Remove(item);
            }
            return false;
        }
        public T GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }
        public IReadOnlyList<T> GetAll()
        {
            return _items.AsReadOnly();
        }
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _items.Where(predicate);
        }
        public int Count => _items.Count;
    }
    }

