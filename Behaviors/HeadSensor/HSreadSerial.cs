using NeeqDMIs.ATmega;
using System.Globalization;

namespace Netychords.Behaviors.Sensor
{
    public class HSreadSerial : ISensorReaderBehavior
    {
        private string[] split;

        public void ReceiveSensorRead(string val)
        {
            if (val.Contains("%")) //Arduino need some signal to start the serial flow
            {
                R.NDB.HeadTrackerModule.Write("A");
            }
            else if (val.StartsWith("$")) //Input data is formatted as $yaw!pitch!roll ($0.00!0.00!0.00)
            {
                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                //Extracting the single data from the input string
                R.NDB.HTData.Yaw = double.Parse(split[0], CultureInfo.InvariantCulture);
                R.NDB.HTData.Pitch = double.Parse(split[1], CultureInfo.InvariantCulture);
                R.NDB.HTData.Roll = double.Parse(split[2], CultureInfo.InvariantCulture);

                //Strumming is elaborated only while the head position is centered along the pitch axis
                //if (Rack.NetychordsDMIBox.HeadTrackerData.Pitch <= Rack.NetychordsDMIBox.MainWindow.centerPitchZone.Value && Rack.NetychordsDMIBox.HeadTrackerData.Pitch >= -Rack.NetychordsDMIBox.MainWindow.centerPitchZone.Value)
                //{
                    R.NDB.ElaborateStrumming();
                //}
            }

            //Debugging variables
            R.NDB.Str_HeadTrackerRaw = R.NDB.HTData.Yaw.ToString();
            R.NDB.Str_HeadTrackerCalib = R.NDB.HTData.TranspYaw.ToString();
        }
    }
}