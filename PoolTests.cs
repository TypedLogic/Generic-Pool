using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResourceManagment.Tests
{
    [TestClass()]
    public class PoolTests
    {
        [TestMethod()]
        public void PoolTest()
        {
            var pool = new Pool<string>();
            Assert.IsNotNull(pool);
        }

        [TestMethod()]
        public void PoolTestWithCreateObjectFunc()
        {
            var pool = new Pool<string>(() => "Test");
            var result = pool.Get();

            Assert.IsNotNull(pool);
            Assert.AreEqual("Test", result);
        }

        [TestMethod()]
        public void PoolTestWithCreateObjectFynctionAndPreload()
        {
            var pool = new Pool<string>(() => "Test", 10);

            Assert.IsNotNull(pool);
            Assert.AreEqual(pool.Count, 10);
        }


        [TestMethod()]
        public void PutTest()
        {
            var pool = new Pool<string>();
            pool.Put("Test1");
            pool.Put("Test2");

            Assert.AreEqual(2, pool.Count);
        }

        [TestMethod(), ExpectedException(typeof(ArgumentNullException))]
        public void PutTestThrowsException()
        {
            var pool = new Pool<string>();
            pool.Put(null);
        }

        [TestMethod()]
        public void GetTest()
        {
            var pool = new Pool<string>();
            pool.Put("Test1");
            pool.Put("Test2");

            var result = pool.Get();
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(1, pool.Count);
        }

        [TestMethod(), ExpectedException(typeof(IndexOutOfRangeException))]
        public void GetTestThrowsError()
        {
            var pool = new Pool<string>();
            pool.Get();
        }
    }
}