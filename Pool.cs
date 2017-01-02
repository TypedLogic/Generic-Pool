using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;

namespace ResourceManagment
{
    public class Pool<T>
    {
        /// <summary>
        /// Number of items in the pool
        /// </summary>
        public int Count => _pool.Count;

        /// <summary>
        /// Boolean whether the pool can return any more objects or not.
        /// </summary>
        public bool IsObjectAvailable => _pool.Count == 0 && _createObject == null;

        readonly ConcurrentBag<T> _pool;

        readonly Func<T> _createObject;

        /// <summary>
        /// Create an empty pool. Object must be put on the pool before they can be taken out.
        /// </summary>
        public Pool()
        {
            _pool = new ConcurrentBag<T>();
        }

        /// <summary>
        /// Create an empty pool. If there are no object available a new object will be created from the function.
        /// </summary>
        /// <param name="createObjectFunc">A function to create a new object</param>
        public Pool(Func<T> createObjectFunc) : this()
        {
            _createObject = createObjectFunc;
        }

        /// <summary>
        /// Create a pool pre loaded with objects
        /// </summary>
        /// <param name="createObjectFunc">A function to create a new object</param>
        /// <param name="numberofObjectsToPreLoad">The number of objects to pre load</param>
        public Pool(Func<T> createObjectFunc, int numberofObjectsToPreLoad) : this(createObjectFunc)
        {
            for (var i = 0; i < numberofObjectsToPreLoad; i++)
                Put(createObjectFunc());
        }

        /// <summary>
        /// Push an item to the pool
        /// </summary>
        /// <param name="item"></param>
        public void Put(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _pool.Add(item);
        }

        /// <summary>
        /// Pull an item from the pool
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            if (_pool.TryTake(out var item))
                return item;
            if (_createObject != null)
                return _createObject();
            throw new IndexOutOfRangeException("There are no objects left in the pool and there is no function specified for creating a new object");
        }
    }
}
