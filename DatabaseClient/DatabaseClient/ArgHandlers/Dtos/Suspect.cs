using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class Suspect
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Mugshot { get; set; }

        public string DNA { get; set; }

        public string Race { get; set; }

        public decimal Height { get; set; }

        public int Weight { get; set; }
    }
}
