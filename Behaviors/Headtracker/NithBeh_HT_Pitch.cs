using Netychords.Modules;
using Netychords.Settings;
using NITHlibrary.Nith.Behaviors;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Filters.ValueFilters;
using NITHlibrary.Tools.Mappers;

namespace Netychords.Behaviors.Headtracker
{
    /// <summary>
    /// Head based strumming, on yaw movement, bidirectional.
    /// </summary>
    internal class NithBeh_HT_Pitch : INithSensorBehavior
    {
        private const double STRUMTHRESHOLD = 5f;
        private readonly NetychordsInteractionMethod associatedMethod = NetychordsInteractionMethod.HeadPitch;

        private float accelMultiplier = 1f;
        private double lastAccel = 0f;
        private ValueMapperDouble Mapper_AccToVelocity = new ValueMapperDouble(25f, 127);
        private NithParameters requiredParam = NithParameters.head_acc_pitch;
        private IDoubleFilter ThresholdFilter = new DoubleFilterMAExpDecaying(0.25f);
        private IDoubleFilter VelocityFilter = new DoubleFilterMAExpDecaying(0.1f); // OLD 0.04f

        public NithBeh_HT_Pitch(float accelMultiplier = 1f)
        {
            this.accelMultiplier = accelMultiplier;
        }

        public void HandleData(NithSensorData nithData)
        {
            if (Rack.UserSettings.InteractionMethod == associatedMethod && nithData.ContainsParameter(requiredParam))
            {
                double accel = nithData.GetParameter(requiredParam).Value.Base_AsDouble;
                VelocityFilter.Push(Math.Abs(accel * accelMultiplier));

                var filteredAccel = (VelocityFilter.Pull());
                Rack.MappingModule.Velocity = (int)Mapper_AccToVelocity.Map(filteredAccel);

                if (Math.Sign(lastAccel) != Math.Sign(accel) && Math.Abs(filteredAccel) > STRUMTHRESHOLD)
                {
                    if (Rack.MappingModule.LastChord != null)
                    {
                        Rack.MappingModule.StopNotes();
                    }
                    Rack.MappingModule.PlayChord(Rack.MappingModule.Chord);
                }

                lastAccel = accel;
            }
        }
    }
}