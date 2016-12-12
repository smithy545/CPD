using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class PoliceDivision
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public Officer PoliceChief { get; set; }
    }
}
