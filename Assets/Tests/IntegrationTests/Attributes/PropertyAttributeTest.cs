using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("PropertyAttributeTests")]
    [IntegrationTest.SucceedWithAssertions]
    internal class PropertyAttributeTest : MonoBehaviour
    {
        private PropertyAttributeTestCase testCase;

        private void Start()
        {
            testCase = FindObjectOfType<PropertyAttributeTestCase>();

            if (testCase)
            {
                if (testCase.ReadOnlyIntVal == 1 &&
                    testCase.TestEnumVal == (TestEnum.TestA | TestEnum.TestB))
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