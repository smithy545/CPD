using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class Investigation
    {
        public int Id { get; set; }

        public DateTime CourtDate { get; set; }

        public string Judge { get; set; }

        public Nullable<bool> Verdict { get; set; }
    }
}
