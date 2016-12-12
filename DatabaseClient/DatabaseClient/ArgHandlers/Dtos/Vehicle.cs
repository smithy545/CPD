using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class Vehicle
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Make { get; set; }

        public string StateProvince { get; set; }

        public Suspect Suspect { get; set; }
    }
}
