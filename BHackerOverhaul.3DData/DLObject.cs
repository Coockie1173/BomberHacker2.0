using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHackerOverhaul._3DData
{
    public class DLObject
    {
        public int DataOffset { get; set; }
        public int Length { get; set; }

        public List<Connection> connections;

        public DLObject()
        {
            connections = new List<Connection>();
        }
    }
}
