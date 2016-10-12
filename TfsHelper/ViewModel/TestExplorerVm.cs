using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using TfsHelper.Services;

namespace TfsHelper.ViewModel
{
    public class TestExplorerVm
    {
        private DataService _dataService;

        public TestExplorerVm(DataService dataService)
        {
            _dataService = dataService;
            
            TfsUri = new Uri(_dataService.TfsSettings.TfsServer);

            TfsTeamProjectCollection projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(TfsUri);

            string project = _dataService.TfsSettings.ProjectName;

            TestManagementTeamProject = projectCollection.GetService<ITestManagementService>().GetTeamProject(project);


        }

        public Uri TfsUri    { get; set; }


        public ITestManagementTeamProject TestManagementTeamProject { get; set; }

        public IObservable<ITestCase> TestCases { get; set; }
        
        public ITestSuiteCollection TestSuites { get; set; }


    }
}
