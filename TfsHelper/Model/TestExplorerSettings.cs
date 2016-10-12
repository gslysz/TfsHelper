using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsHelper.Model
{
    public class TestExplorerSettings
    {

        public TestExplorerSettings()
        {
            TestPlanName = "Arc Test Plan";
        }

        public string TestPlanName { get; set; }
    }
}
