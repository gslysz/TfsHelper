using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsHelper.Model;

namespace TfsHelper.Services
{
    public class DataService
    {

        public DataService()
        {
            TfsSettings = new TfsSettings();
        }

        public TfsSettings TfsSettings { get; set; }
        
    }
}
