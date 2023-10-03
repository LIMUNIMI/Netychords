using NITHdmis.Headtracking;
using NITHdmis.Headtracking.NeeqHT;
using NITHdmis.NithSensors;
using System.Collections.Generic;
using System.Globalization;

namespace Netychords.Behaviors.HeadSensor
{
    public class NithBeh_HT_ReceiveData : INithSensorBehavior
    {
        private readonly List<NithArguments> accRequiredArgs = new List<NithArguments>
        {
            NithArguments.acc_pitch, NithArguments.acc_yaw, NithArguments.acc_roll
        };

        private readonly List<NithArguments> posRequiredArgs = new List<NithArguments>
        {
            NithArguments.pos_pitch, NithArguments.pos_yaw, NithArguments.pos_roll
        };

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsArguments(accRequiredArgs))
            {
                R.NDB.HeadData.Acceleration = new Polar3DData
                {
                    Yaw = double.Parse(nithData.GetArgument(NithArguments.acc_yaw).Value.Base, CultureInfo.InvariantCulture),
                    Pitch = double.Parse(nithData.GetArgument(NithArguments.acc_pitch).Value.Base, CultureInfo.InvariantCulture),
                    Roll = double.Parse(nithData.GetArgument(NithArguments.acc_roll).Value.Base, CultureInfo.InvariantCulture)
                };
            }

            if (nithData.ContainsArguments(posRequiredArgs))
            {
                R.NDB.HeadData.Position = new Polar3DData
                {
                    Yaw = double.Parse(nithData.GetArgument(NithArguments.pos_yaw).Value.Base, CultureInfo.InvariantCulture),
                    Pitch = double.Parse(nithData.GetArgument(NithArguments.pos_pitch).Value.Base, CultureInfo.InvariantCulture),
                    Roll = double.Parse(nithData.GetArgument(NithArguments.pos_roll).Value.Base, CultureInfo.InvariantCulture)
                };
            }
        }
    }
}
