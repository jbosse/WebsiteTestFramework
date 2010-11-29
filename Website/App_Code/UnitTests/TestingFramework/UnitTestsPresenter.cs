using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace App_Code.UnitTests.TestingFramework
{
    public class TestResult
    {
        public string Test { get; set; }
        public bool Pass { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }

    public class TestSuite
    {
        public string Name { get; set; }
        public IList<TestResult> Results { get; set; }

        public TestSuite()
        {
            Results = new List<TestResult>();
        }
    }

    public class UnitTestsPresenter
    {
        private readonly IUnitTestsView _view;
        private readonly IOrderedEnumerable<Type> _tests;
        private readonly IList<TestSuite> _suites;

        public UnitTestsPresenter(IUnitTestsView view)
        {
            _view = view;
            _tests = GetType().Assembly.GetTypes().Where(type => (from customAttribute in type.GetCustomAttributes(true) where customAttribute.GetType() == typeof(NUnit.Framework.TestFixtureAttribute) select customAttribute).Count() > 0).OrderBy(t => t.Name);
            _suites = new List<TestSuite>();
            PerformTests();
            _view.Display(_suites);
        }

        public void PerformTests()
        {
            foreach (var testType in _tests)
            {
                var suite = new TestSuite { Name = testType.Name };
                var testObject = Activator.CreateInstance(testType);
                var methods = testType.GetMethods();
                var tests = (from methodInfo in methods from customAttribute in methodInfo.GetCustomAttributes(true) where customAttribute.GetType() == typeof(NUnit.Framework.TestAttribute) select methodInfo);
                var setupMethod = (from methodInfo in methods from customAttribute in methodInfo.GetCustomAttributes(true) where customAttribute.GetType() == typeof(NUnit.Framework.SetUpAttribute) select methodInfo).FirstOrDefault();
                var teardownMethod = (from methodInfo in methods from customAttribute in methodInfo.GetCustomAttributes(true) where customAttribute.GetType() == typeof(NUnit.Framework.TearDownAttribute) select methodInfo).FirstOrDefault();
                foreach (var testMethod in tests)
                {
                    InvokeMethodAndLogResult(suite, testType, testMethod, testObject, setupMethod, teardownMethod);
                }
                _suites.Add(suite);
            }
        }

        private static void InvokeMethodAndLogResult(TestSuite suite, Type testClass, MethodInfo testMethod, object testObject, MethodInfo setupMethod, MethodInfo teardownMethod)
        {
            var result = new TestResult { Test = testMethod.Name };
            try
            {
                if (setupMethod != null)
                {
                    testClass.InvokeMember(setupMethod.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, testObject, null);
                }
                testClass.InvokeMember(testMethod.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, testObject, null);
                if (teardownMethod != null)
                {
                    testClass.InvokeMember(teardownMethod.Name, BindingFlags.Default | BindingFlags.InvokeMethod, null, testObject, null);
                }
                result.Pass = true;
            }
            catch (Exception e)
            {
                result.Pass = false;
                result.Exception = e.GetBaseException().Message;
                result.StackTrace = e.GetBaseException().StackTrace;
            }
            suite.Results.Add(result);
        }
    }
}