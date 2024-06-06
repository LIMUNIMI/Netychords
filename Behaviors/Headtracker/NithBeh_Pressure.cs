using System.Globalization;
using Netychords.Modules;
using Netychords.Settings;
using NITHlibrary.Nith.Behaviors;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Filters.ValueFilters;
using NITHlibrary.Tools.Mappers;

namespace Netychords.Behaviors.Headtracker
{
    internal class NithBeh_Pressure : INithSensorBehavior
    {
        private const NetychordsInteractionMethod AssociatedMethod = NetychordsInteractionMethod.PressureBlink;
        private readonly ValueMapperDouble _velocityMapper = new(100, 127);
        private readonly float _pressureMultiplier;
        private readonly IDoubleFilter _velocityFilter;
        private double _velocity;

        /// <summary>
        /// Creates a new pressure based behavior
        /// </summary>
        /// <param name="filterAlpha">Sets the alpha for filtering the data through an Exponentially Decaying MA Filter (set to 1 to avoid filtering)</param>
        public NithBeh_Pressure(float filterAlpha, float pressureMultiplier)
        {
            _velocityFilter = new DoubleFilterMAExpDecaying(filterAlpha);
            this._pressureMultiplier = pressureMultiplier;
        }

        public void HandleData(NithSensorData nithData)
        {
            if (Rack.UserSettings.InteractionMethod == AssociatedMethod)
            {
                if (nithData.ContainsParameter(NithParameters.breath_press) || nithData.ContainsParameter(NithParameters.teeth_press))
                {
                    if (nithData.ContainsParameter(NithParameters.breath_press))
                    {
                        var nithValue = nithData.GetParameter(NithParameters.breath_press).Value;
                        ProcessValue(nithValue);
                    }
                    else if (nithData.ContainsParameter(NithParameters.teeth_press))
                    {
                        var nithValue = nithData.GetParameter(NithParameters.teeth_press).Value;
                        ProcessValue(nithValue);
                    }
                }
            }
        }

        private void ProcessValue(NithArgumentValue nithValue)
        {
            if (nithValue.Type == NithDataTypes.BaseAndMax)
            {
                _velocityFilter.Push(nithValue.Proportional * _pressureMultiplier);
                _velocity = _velocityMapper.Map(_velocityFilter.Pull());
            }
            else if (nithValue.Type == NithDataTypes.OnlyBase)
            {
                // The best we can do without any further indication about Max is to feed the Base...
                _velocityFilter.Push(double.Parse(nithValue.Base, CultureInfo.InvariantCulture) * _pressureMultiplier);
                _velocity = _velocityFilter.Pull();
            }

            // Send Pressure to mapping
            Rack.MappingModule.Velocity = (int)_velocity;
        }
    }
}