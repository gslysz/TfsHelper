namespace TfsHelper.Model
{
    public class TfsSettings
    {
        public TfsSettings()
        {
            TfsServer = "http://softdevtfs:8080/tfs/cadwell";
            ProjectName = "EEG";
            TestExplorerSettings = new TestExplorerSettings();
        }

        public string TfsServer { get; set; }

        public TestExplorerSettings TestExplorerSettings { get; set; }

        public string ProjectName { get; set; }
    }
}