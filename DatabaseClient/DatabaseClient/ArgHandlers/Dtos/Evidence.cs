using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class Evidence
    {
        public int Id { get; set; }

        public Crime Crime { get; set; }

        public DateTime DateFound { get; set; }

        public string Description { get; set; }

        public decimal EvidenceWeight { get; set; }
    }
}
