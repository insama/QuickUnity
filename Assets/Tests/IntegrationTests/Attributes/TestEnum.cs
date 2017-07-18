using System;

namespace QuickUnity.Tests.IntegrationTests
{
    [Flags]
    public enum TestEnum
    {
        TestA = 0x01,
        TestB = 0x02,
        TestC = 0x04
    }
}