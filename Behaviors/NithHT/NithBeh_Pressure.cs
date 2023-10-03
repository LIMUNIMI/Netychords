using Netychords.Settings;
using NITHdmis.Filters.ValueFilters;
using NITHdmis.NithSensors;
using NITHdmis.Utils;
using System.Globalization;

namespace Netychords.Behaviors.NithHT
{
    internal class NithBeh_Pressure : INithSensorBehavior
    {
        private readonly NetychordsInteractionMethod associatedMethod = NetychordsInteractionMethod.PressureBlink;
        private readonly ValueMapperDouble velocityMapper = new ValueMapperDouble(100, 127);
        private double velocity = 0f;
        private readonly IDoubleFilter velocityFilter;
        private readonly float pressureMultiplier;

        /// <summary>
        /// Creates a new pressure based behavior
        /// </summary>
        /// <param name="filterAlpha">Sets the alpha for filtering the data through an Exponentially Decaying MA Filter (set to 1 to avoid filtering)</param>
        public NithBeh_Pressure(float filterAlpha, float pressureMultiplier)
        {
            velocityFilter = new DoubleFilterMAExpDecaying(filterAlpha);
            this.pressureMultiplier = pressureMultiplier;
        }

        public void HandleData(NithSensorData nithData)
        {
            if (R.UserSettings.InteractionMethod == associatedMethod)
            {
                if (nithData.ContainsArgument(NithArguments.press))
                {
                    NithValue nithValue = nithData.GetArgument(NithArguments.press).Value;
                    if (nithValue.Type == NithDataTypes.ValueAndMax)
                    {
                        velocityFilter.Push(nithValue.Proportional * pressureMultiplier);
                        velocity = velocityMapper.Map(velocityFilter.Pull());
                    }
                    else if (nithValue.Type == NithDataTypes.OnlyValue)
                    {
                        // The best we can do without any further indication about Max is to feed the Base...
                        velocityFilter.Push(double.Parse(nithValue.Base, CultureInfo.InvariantCulture)  * pressureMultiplier);
                        velocity = velocityFilter.Pull();
                    }

                    // Send Pressure to mapping
                    R.NDB.Velocity = (int)velocity;
                }
            }
        }
    }
}