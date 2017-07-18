using QuickUnity.Attributes;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    public class PropertyAttributeTestCase : MonoBehaviour
    {
        [SerializeField]
        [ReadOnlyField]
        private int m_readOnlyIntVal;

        public int readOnlyIntVal
        {
            get { return m_readOnlyIntVal; }
        }

        [SerializeField]
        [EnumFlags]
        private TestEnum m_testEnumVal;

        public TestEnum testEnumVal
        {
            get { return m_testEnumVal; }
        }

        private void Awake()
        {
            m_readOnlyIntVal = 1;
        }
    }
}