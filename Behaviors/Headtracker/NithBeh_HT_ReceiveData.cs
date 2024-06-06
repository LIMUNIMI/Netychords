using System.Globalization;
using Netychords.Modules;
using NITHlibrary.Nith.Behaviors;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Types;

namespace Netychords.Behaviors.Headtracker
{
    public class NithBeh_HT_ReceiveData : INithSensorBehavior
    {
        private readonly List<NithParameters> accRequiredArgs = new List<NithParameters>
        {
            NithParameters.head_acc_pitch, NithParameters.head_acc_yaw, NithParameters.head_acc_roll
        };

        private readonly List<NithParameters> posRequiredArgs = new List<NithParameters>
        {
            NithParameters.head_pos_pitch, NithParameters.head_pos_yaw, NithParameters.head_pos_roll
        };

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsParameters(accRequiredArgs))
            {
                Rack.MappingModule.HeadAccelerationData = new Polar3DData
                {
                    Yaw = nithData.GetParameter(NithParameters.head_acc_yaw).Value.Base_AsDouble,
                    Pitch = nithData.GetParameter(NithParameters.head_acc_pitch).Value.Base_AsDouble,
                    Roll = nithData.GetParameter(NithParameters.head_acc_roll).Value.Base_AsDouble
                };
            }

            if (nithData.ContainsParameters(posRequiredArgs))
            {
                Rack.MappingModule.HeadPositionData = new Polar3DData
                {
                    Yaw = double.Parse(nithData.GetParameter(NithParameters.head_pos_yaw).Value.Base, CultureInfo.InvariantCulture),
                    Pitch = double.Parse(nithData.GetParameter(NithParameters.head_pos_pitch).Value.Base, CultureInfo.InvariantCulture),
                    Roll = double.Parse(nithData.GetParameter(NithParameters.head_pos_roll).Value.Base, CultureInfo.InvariantCulture)
                };
            }
        }
    }
}
