using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsHelper.Services;
using TfsHelper.ViewModel;

namespace TfsHelper.Testing
{
    [TestClass]
    public class TestExplorerVmTests
    {
        [TestMethod]
        public void GetListOfActiveTests()
        {
            DataService dataService = new DataService();
            TestExplorerVm vm = new TestExplorerVm(dataService);
            Assert.IsNotNull(vm.TestManagementTeamProject);

            var plans = vm.TestManagementTeamProject.TestPlans.Query("Select * From TestPlan");
            var testPlanName = dataService.TfsSettings.TestExplorerSettings.TestPlanName;
            //testPlanName = "Arc Software Full Verification Plan - Release Baseline for v. 2.0";

            var arcTestPlan = plans.FirstOrDefault(p => p.Name==testPlanName);
            Assert.IsNotNull(arcTestPlan);

            var testPoints = arcTestPlan.QueryTestPoints("Select * from TestPoint");
            int counter = 1;

            foreach (var testPoint in testPoints.OrderBy(p => p.SuiteId))
            {
                if (testPoint.State == TestPointState.Ready && !testPoint.IsTestCaseAutomated)
                {
                    var suite = vm.TestManagementTeamProject.TestSuites.Find(testPoint.SuiteId);
                    var testCaseLocal = vm.TestManagementTeamProject.TestCases.Find(testPoint.TestCaseId);

                    bool passesFilter = testPoint.ConfigurationName.Contains("direct", StringComparison.OrdinalIgnoreCase);
                    passesFilter = true;
                    if (passesFilter)
                        Console.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", counter++, suite.Parent.Title, suite.Id, suite.Title, testCaseLocal.Id, testCaseLocal.Title, testPoint.ConfigurationName));
                }
            }

        }


        [Ignore]
        [TestMethod]
        public void OtherJunkTests1()
        {
            DataService dataService = new DataService();
            TestExplorerVm vm = new TestExplorerVm(dataService);
            Assert.IsNotNull(vm.TestManagementTeamProject);

            var plans = vm.TestManagementTeamProject.TestPlans.Query("Select * From TestPlan");
            var testPlanName = dataService.TfsSettings.TestExplorerSettings.TestPlanName;
            //testPlanName = "Arc Software Full Verification Plan - Release Baseline for v. 2.0";

            var arcTestPlan = plans.FirstOrDefault(p => p.Name == testPlanName);
            Assert.IsNotNull(arcTestPlan);

            var testPoints = arcTestPlan.QueryTestPoints("Select * from TestPoint");
            int counter = 1;

            foreach (var testPoint in testPoints.OrderBy(p => p.SuiteId))
            {
                if (testPoint.State == TestPointState.Ready && !testPoint.IsTestCaseAutomated)
                {
                    var suite = vm.TestManagementTeamProject.TestSuites.Find(testPoint.SuiteId);
                    var testCaseLocal = vm.TestManagementTeamProject.TestCases.Find(testPoint.TestCaseId);

                    bool passesFilter = testPoint.ConfigurationName.Contains("direct", StringComparison.OrdinalIgnoreCase);
                    passesFilter = true;
                    if (passesFilter)
                        Console.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", counter++, suite.Parent.Title, suite.Id, suite.Title, testCaseLocal.Id, testCaseLocal.Title, testPoint.ConfigurationName));
                }
            }

            return;
            var suites = arcTestPlan.RootSuite.Entries;

            foreach (var suite in suites)
            {
                string suiteOutput = string.Format("{0}", suite.Title);
                Console.WriteLine(suiteOutput);
            }

            var regressionSuiteEntry = suites.FirstOrDefault(p => p.Title.Contains("Regression"));
            Assert.IsNotNull(regressionSuiteEntry);

            var regressionSuite = regressionSuiteEntry.TestSuite;
            Assert.IsNotNull(regressionSuite);

            foreach (var testCase in regressionSuite.AllTestCases.Where(p => !p.IsAutomated).Take(10))
            {
                Console.WriteLine("-------------------------------------------------------------------------------");
                var testResults = vm.TestManagementTeamProject.TestResults.ByTestId(testCase.Id).Where(p => p.BuildNumber == "ArcInstallerMainLine_2.1.46.0");

                var lastResult = testResults.LastOrDefault();


                var resultString = lastResult == null ? "[null]" : lastResult.ToString();



                string testCaseOutput = string.Format("{0} - {1} - {2} - {3}", testCase.Id, resultString, testCase.Title, testCase.Reason);
                Console.WriteLine(testCaseOutput);
            }





        }


        public static List<TestCase> GetAllTestCasesInTestPlan(ITestManagementTeamProject testManagementTeamProject, ITestPlan testPlan, bool initializeTestCaseStatus = true)
        {
            testPlan.Refresh();
            List<TestCase> testCasesList;
            testCasesList = new List<TestCase>();
            string fullQuery =
                String.Format("SELECT [System.Id], [System.Title] FROM WorkItems WHERE [System.WorkItemType] = 'Test Case' AND [Team Project] = '{0}'", testManagementTeamProject.TeamProjectName);
            IEnumerable<ITestCase> allTestCases = testManagementTeamProject.TestCases.Query(fullQuery);
            foreach (var currentTestCase in allTestCases)
            {
                TestCase testCaseToAdd = new TestCase(currentTestCase, currentTestCase.TestSuiteEntry.ParentTestSuite, testPlan, initializeTestCaseStatus);
                if (!testCasesList.Contains(testCaseToAdd))
                {
                    testCasesList.Add(testCaseToAdd);
                }
            }

            return testCasesList;
        }
    }


    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
