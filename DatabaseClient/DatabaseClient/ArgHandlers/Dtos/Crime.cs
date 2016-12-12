using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class Crime
    {
        public int Id { get; set; }

        public Investigation Investigation { get; set; }

        public DateTime TimeDate { get; set; }

        public string Location { get; set; }

        public string CrimeType { get; set; }

        public int Severity { get; set; }
    }
}
