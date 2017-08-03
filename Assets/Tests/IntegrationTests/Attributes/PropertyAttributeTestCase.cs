using QuickUnity.Attributes;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    internal class PropertyAttributeTestCase : MonoBehaviour
    {
        [SerializeField]
        [ReadOnlyField]
        private int readOnlyIntVal;

        public int ReadOnlyIntVal
        {
            get { return readOnlyIntVal; }
        }

        [SerializeField]
        [EnumFlags]
        private TestEnum testEnumVal;

        public TestEnum TestEnumVal
        {
            get { return testEnumVal; }
        }

        private void Awake()
        {
            readOnlyIntVal = 1;
        }
    }
}