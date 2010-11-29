using System.Collections.Generic;

namespace App_Code.UnitTests.TestingFramework
{
    public interface IUnitTestsView
    {
        void Display(IList<TestSuite> results);
    }
} 