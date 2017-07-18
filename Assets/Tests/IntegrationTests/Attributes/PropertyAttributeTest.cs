using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("PropertyAttributeTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class PropertyAttributeTest : MonoBehaviour
    {
        private PropertyAttributeTestCase m_testCase;

        private void Start()
        {
            m_testCase = FindObjectOfType<PropertyAttributeTestCase>();

            if (m_testCase)
            {
                if (m_testCase.readOnlyIntVal == 1 &&
                    m_testCase.testEnumVal == (TestEnum.TestA | TestEnum.TestB))
                {
                    IntegrationTest.Pass();
                }
                else
                {
                    IntegrationTest.Fail();
                }
            }
            else
            {
                IntegrationTest.Fail();
            }
        }
    }
}