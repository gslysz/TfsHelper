using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client.Wiql;

namespace TfsHelper
{
    [Serializable]
    public class TestCase
    {
        [NonSerialized]
        private TeamFoundationIdentityName teamFoundationIdentityName;
        private int id;
        private string title;
        private string area;
        private string createdBy;
        private DateTime dateCreated;
        private DateTime dateModified;
        private Priority priority;
        protected bool isInitialized;
        private ITestCase currentTestCase;
        private ITestSuiteBase parentTestSuite;
        private ITestPlan testPlan;
        private bool initializeTestCaseStatus;

        public TestCase(ITestCase currentTestCase, ITestSuiteBase parentTestSuite, ITestPlan testPlan, bool initializeTestCaseStatus)
        {
            this.currentTestCase = currentTestCase;
            this.parentTestSuite = parentTestSuite;
            this.testPlan = testPlan;
            this.initializeTestCaseStatus = initializeTestCaseStatus;
        }

        public string OwnerDisplayName { get; set; }

        public Guid TeamFoundationId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateModified { get; set; }

        public string Area { get; set; }

        public TeamFoundationIdentityName TeamFoundationIdentityName { get; set; }

        public Priority Priority { get; set; }
    }
}
