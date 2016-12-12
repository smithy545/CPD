using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class Officer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int YearsOnForce { get; set; }

        public int Salary { get; set; }

        public string Description { get; set; }

        public PoliceDivision PoliceDivision { get; set; }
    }
}
