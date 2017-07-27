using NUnit.Framework;
using QuickUnity.Core.Miscs;
using System.Collections.Generic;
using UnityEngine;

namespace QuickUnity.Data
{
    /// <summary>
    /// Unit test cases for class BoxDBAdapter.
    /// </summary>
    internal class BoxDBAdapterTestVO
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public float Value
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxDBAdapterTestVO"/> class.
        /// </summary>
        public BoxDBAdapterTestVO()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxDBAdapterTestVO"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public BoxDBAdapterTestVO(int id, string name, float value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Id={0}, Name={1}, Value={2}", Id, Name, Value);
        }
    }

    /// <summary>
    /// Unit test cases for class BoxDBAdapter.
    /// </summary>
    [TestFixture]
    internal class BoxDBAdapterTests
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        private const string tableName = "BoxDBAdapterTestTable";

        /// <summary>
        /// Test for the method BoxDBAdapterTest.Insert.
        /// </summary>
        [Test]
        public void InsertTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            BoxDBAdapterTestVO[] voList = new BoxDBAdapterTestVO[3]
            {
                new BoxDBAdapterTestVO((int)db.MakeNewId(), "test1", 1f),
                new BoxDBAdapterTestVO((int)db.MakeNewId(), "测试2", 2f),
                new BoxDBAdapterTestVO((int)db.MakeNewId(), "test3", 3f)
            };
            bool success = db.Insert(tableName, voList);
            db.Dispose();
            Assert.IsTrue(success);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.SelectCount.
        /// </summary>
        [Test]
        public void SelectCountTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            long count = db.SelectCount(tableName);
            db.Dispose();
            Assert.AreEqual(3, count);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.Select when on multi-conditions query.
        /// </summary>
        [Test]
        public void MuiltiConditionsSelectCountTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            List<BoxDBQueryCondition> list = new List<BoxDBQueryCondition>()
            {
                new BoxDBQueryCondition("Id", 1),
                new BoxDBQueryCondition("Id", 2)
            };

            long count = db.SelectCount(tableName, list,
                new List<BoxDBMultiConditionOperator>(new BoxDBMultiConditionOperator[1] { BoxDBMultiConditionOperator.Or }));
            db.Dispose();
            Assert.AreEqual(2, count);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.Select when on multi-conditions query.
        /// </summary>
        [Test]
        public void MuiltiConditionsSelectTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            List<BoxDBQueryCondition> list = new List<BoxDBQueryCondition>()
            {
                new BoxDBQueryCondition("Id", 1),
                new BoxDBQueryCondition("Id", 2)
            };

            List<BoxDBAdapterTestVO> result = db.Select<BoxDBAdapterTestVO>(tableName, list,
                new List<BoxDBMultiConditionOperator>(new BoxDBMultiConditionOperator[1] { BoxDBMultiConditionOperator.Or }));
            db.Dispose();
            Assert.AreEqual(2f, result[0].Value);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.Select.
        /// </summary>
        [Test]
        public void SelectTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            BoxDBAdapterTestVO vo = db.Select<BoxDBAdapterTestVO>(tableName, 1);
            DebugLogger.Log(vo.ToString());
            db.Dispose();
            Assert.IsNotNull(vo);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.SelectAll.
        /// </summary>
        [Test]
        public void SelectAllTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            List<BoxDBAdapterTestVO> list = db.SelectAll<BoxDBAdapterTestVO>(tableName);

            foreach (BoxDBAdapterTestVO vo in list)
            {
                DebugLogger.Log(vo.ToString());
            }

            db.Dispose();
            Assert.AreEqual(3, list.Count);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.Update.
        /// </summary>
        [Test]
        public void UpdateTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            BoxDBAdapterTestVO[] voList = new BoxDBAdapterTestVO[3]
            {
                new BoxDBAdapterTestVO(1, "test1_update", 1f),
                new BoxDBAdapterTestVO(2, "测试2_update", 2f),
                new BoxDBAdapterTestVO(4, "test3_update", 3f)
            };
            db.Update(tableName, voList);

            BoxDBAdapterTestVO vo = db.Select<BoxDBAdapterTestVO>(tableName, 1);
            db.Dispose();

            Assert.AreEqual("test1_update", vo.Name);
        }

        /// <summary>
        /// Test for the method BoxDBAdapterTest.Delete.
        /// </summary>
        [Test]
        public void DeleteTest()
        {
            BoxDBAdapter db = GetBoxDBAdapter();
            BoxDBAdapterTestVO[] voList = new BoxDBAdapterTestVO[]
            {
                new BoxDBAdapterTestVO(9999, "test9999", 9999f)
            };

            db.Insert(tableName, voList);
            bool success = db.Delete(tableName, 9999);
            db.Dispose();
            Assert.IsTrue(success);
        }

        /// <summary>
        /// Gets the box database adapter.
        /// </summary>
        /// <returns>The BoxDBAdapter object.</returns>
        private BoxDBAdapter GetBoxDBAdapter()
        {
            BoxDBAdapter db = new BoxDBAdapter(Application.persistentDataPath);
            db.EnsureTable<BoxDBAdapterTestVO>(tableName, "Id");
            db.Open();
            return db;
        }
    }
}