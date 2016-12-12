using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers.Dtos
{
    class TaskForce
    {
        public int Id { get; set; }

        public Officer Officer { get; set; }

        public Crime Crime { get; set; }
    }
}
