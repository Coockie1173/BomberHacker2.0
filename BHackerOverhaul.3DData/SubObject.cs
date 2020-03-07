using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHackerOverhaul._3DData
{
    public class SubObject
    {
        List<Vector3> Vertices { get; set; }
        List<Connection> Connections { get; set; }
        int RoomID { get; set; }
    }
}
