using NITHdmis.NithSensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netychords.Behaviors.NithHT
{
    internal class NithBeh_HT_ElaborateStrum : INithSensorBehavior
    {
        public void HandleData(NithSensorData nithData)
        {
            R.NDB.ElaborateStrumming(1f);
        }
    }
}
