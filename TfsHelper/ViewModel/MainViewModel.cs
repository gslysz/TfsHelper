using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsHelper.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseVm
    {
        private Uri _tfsUri;
        private Project _selectedProject;
        private ObservableCollection<WorkItem> _workItems;
        private TfsTeamProjectCollection _tpc;
        private WorkItemStore _workItemStore;
        private WorkItem _selectedWorkItem;
        private string _statusText;
        private ObservableCollection<Project> _projects;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

            TfsUri = new Uri(@"http://softdevtfs:8080/tfs/cadwell");

            DeleteSelectedWorkItemCommand = new RelayCommand(DeleteSelectedWorkItem);
            RefreshUriCommand = new RelayCommand(RefreshUri);

            RefreshUri();
        }


        public Uri TfsUri
        {
            get { return _tfsUri; }
            set
            {
                _tfsUri = value;
                RaisePropertyChanged();
            }
        }


        private List<Project> GetProjects()
        {
            var projectNameList = new List<Project>();

            foreach (Project project in _workItemStore.Projects)
            {
                projectNameList.Add(project);
            }

            //var uri = TfsUri;
            //TfsConfigurationServer server = TfsConfigurationServerFactory.GetConfigurationServer(uri);
            //var collectionNodes = server.CatalogNode.QueryChildren(new[] {CatalogResourceTypes.ProjectCollection}, false,
            //    CatalogQueryOptions.None);

            //foreach (var collectionNode in collectionNodes)
            //{

            //    // Get a catalog of team projects for the collection
            //    var projectNodes = collectionNode.QueryChildren(
            //        new[] { CatalogResourceTypes.TeamProject },
            //        false, CatalogQueryOptions.None);

            //    // List the team projects in the collection
            //    foreach (CatalogNode projectNode in projectNodes)
            //    {
            //        projectNameList.Add(projectNode);


            //    }
            //}

            return projectNameList.OrderBy(p => p.Name).ToList();
        }

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value; 
                RaisePropertyChanged();
            }
        }


        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;

                LoadWorkItems();

                RaisePropertyChanged();
            }
        }

        private void LoadWorkItems()
        {
            WorkItems.Clear();
            StatusText = "";

            if (SelectedProject != null)
            {
                WorkItemCollection workItemCollection = _workItemStore.Query(
     " SELECT [System.Id], [System.WorkItemType]," +
     " [System.State], [System.AssignedTo], [System.Title] " +
     " FROM WorkItems " +
     " WHERE [System.TeamProject] = '" + SelectedProject.Name +
    "' ORDER BY [System.Id]");

                foreach (WorkItem workItem in workItemCollection)
                {
                    WorkItems.Add(workItem);
                }

            }

        }

        public ObservableCollection<WorkItem> WorkItems
        {
            get
            {
                if (_workItems == null) _workItems = new ObservableCollection<WorkItem>();
                return _workItems;
            }
            set { _workItems = value; }
        }

        public WorkItem SelectedWorkItem
        {
            get { return _selectedWorkItem; }
            set
            {
                _selectedWorkItem = value;
                StatusText = "";

                RaisePropertyChanged();
            }
        }

        private void DeleteSelectedWorkItem()
        {
            if (SelectedWorkItem != null)
            {
                var toDeletes = new List<int>(new int[]{ SelectedWorkItem.Id});
                var errors = _workItemStore.DestroyWorkItems(toDeletes);
                _workItemStore.RefreshCache();
                _workItemStore.SyncToCache();


                bool errorOccurred = false;
                foreach (var workItemOperationError in errors)
                {
                    errorOccurred = true;
                    StatusText += workItemOperationError.Exception.Message + Environment.NewLine;
                }

                if (!errorOccurred)
                {
                    StatusText = string.Format("Work Item {0} was deleted.", SelectedWorkItem.Id);
                }
                    
                

            }
        }

        private void RefreshUri()
        {

            _tpc = new TfsTeamProjectCollection(TfsUri);

            try
            {
                _workItemStore = _tpc.GetService<WorkItemStore>();
                Projects = new ObservableCollection<Project>(GetProjects());
            }
            catch (Exception exception)
            {
                StatusText = exception.GetBaseException().Message;
                
            }

            
        }

        public ICommand GetProjectsCommand { get; set; }

        public ICommand DeleteSelectedWorkItemCommand { get; set; }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RefreshUriCommand { get; set; }
    }
}