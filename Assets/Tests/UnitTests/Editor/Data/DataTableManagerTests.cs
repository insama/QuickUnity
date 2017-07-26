using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using QuickUnity.Extensions;
using Tests.UnitTests.Editor.Data;

namespace QuickUnity.Data
{
    /// <summary>
    /// Unit test cases for class <see cref="QuickUnity.Data.DataTableManager"/>.
    /// </summary>
    [TestFixture]
    internal class DataTableManagerTests
    {
        /// <summary>
        /// Test for the method DataTableManager.GetDataTableRow.
        /// </summary>
        [Test]
        public void GetDataTableRowTest()
        {
            TestData testData = DataTableManager.Instance.GetDataTableRow<TestData>(1L);
            DataTableManager.Instance.Dispose();

            if (testData != null)
            {
                if (testData.TestVector2.ToVector2() == new Vector2(1, 2) &&
                    testData.TestVector3.ToVector3() == new Vector3(1, 2, 3) &&
                    testData.TestQuaternion.ToQuaternion() == new Quaternion(1, 2, 3, 4) &&
                    testData.TestInt == 2147483647)
                {
                    Assert.Pass();
                }
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Test for the method DataTableManager.GetDataTableRows.
        /// </summary>
        [Test]
        public void GetDataTableRowsTest()
        {
            List<BoxDBQueryCondition> conditions = new List<BoxDBQueryCondition>()
            {
                new BoxDBQueryCondition("TestUShort", (ushort)0),
                new BoxDBQueryCondition("TestBoolean", false)
            };

            List<BoxDBMultiConditionOperator> multiConditionOps = new List<BoxDBMultiConditionOperator>()
            {
                BoxDBMultiConditionOperator.Or
            };

            TestData[] results = DataTableManager.Instance.GetDataTableRows<TestData>(conditions, multiConditionOps);
            DataTableManager.Instance.Dispose();

            if (results != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Test for the method DataTableManager.GetAllDataTableRow.
        /// </summary>
        [Test]
        public void GetAllDataTableRowsTest()
        {
            TestData[] array = DataTableManager.Instance.GetAllDataTableRows<TestData>();
            DataTableManager.Instance.Dispose();
            Assert.IsNotNull(array);
        }

        /// <summary>
        /// Test for the method DataTableManager.GetAllDataTableRowsCount.
        /// </summary>
        [Test]
        public void GetAllDataTableRowsCountTest()
        {
            long count = DataTableManager.Instance.GetAllDataTableRowsCount<TestData>();
            DataTableManager.Instance.Dispose();
            Assert.AreEqual(3L, count);
        }

        /// <summary>
        /// Test for the method DataTableManager.GetDataTableRowsCount.
        /// </summary>
        [Test]
        public void GetDataTableRowsCountTest()
        {
            List<BoxDBQueryCondition> conditions = new List<BoxDBQueryCondition>()
            {
                new BoxDBQueryCondition("TestInt", 2147483647),
                new BoxDBQueryCondition("TestBoolean", true),
                new BoxDBQueryCondition("TestUInt", (uint)0)
            };

            List<BoxDBMultiConditionOperator> multiConditionOps = new List<BoxDBMultiConditionOperator>()
            {
                BoxDBMultiConditionOperator.Or,
                BoxDBMultiConditionOperator.And
            };

            long count = DataTableManager.Instance.GetDataTableRowsCount<TestData>(conditions, multiConditionOps);
            DataTableManager.Instance.Dispose();
            Assert.Greater(count, 0L);
        }
    }
}