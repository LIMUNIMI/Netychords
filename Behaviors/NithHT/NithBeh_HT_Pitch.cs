using Netychords.Settings;
using NITHdmis.Filters.ValueFilters;
using NITHdmis.NithSensors;
using NITHdmis.Utils;
using System;

namespace Netychords.Behaviors.NithHT
{
    /// <summary>
    /// Head based strumming, based on pitch, monodirectional (only on downstrokes)
    /// </summary>
    internal class NithBeh_HT_Pitch : INithSensorBehavior
    {
        private readonly NetychordsInteractionMethod associatedMethod = NetychordsInteractionMethod.HeadPitch;

        private IDoubleFilter VelocityFilter = new DoubleFilterMAExpDecaying(0.1f); // OLD 0.04f
        private IDoubleFilter ThresholdFilter = new DoubleFilterMAExpDecaying(0.25f);
        private ValueMapperDouble Mapper_AccToVelocity = new ValueMapperDouble(25f, 127);
        private readonly float accelMultiplier = 1.1f;
        private double lastVelocity = 0f;
        private const double STRUMTHRESHOLD = 1.5f;

        public void HandleData(NithSensorData nithData)
        {
            if (R.UserSettings.InteractionMethod == associatedMethod)
            {
                VelocityFilter.Push(Math.Abs(R.NDB.HeadData.Acceleration.Pitch * accelMultiplier));

                R.NDB.FilteredVelocity = (VelocityFilter.Pull());
                R.NDB.Velocity = (int)Mapper_AccToVelocity.Map(R.NDB.FilteredVelocity);

                // Last condition for only upstrokes
                if (Math.Sign(lastVelocity) != Math.Sign(R.NDB.HeadData.Acceleration.Pitch) && Math.Abs(lastVelocity - R.NDB.HeadData.Acceleration.Pitch) > STRUMTHRESHOLD && Math.Sign(lastVelocity) < 0)
                {
                    if (R.NDB.LastChord != null)
                    {
                        R.NDB.StopNotes();
                    }
                    R.NDB.PlayChord(R.NDB.Chord);
                }

                lastVelocity = R.NDB.HeadData.Acceleration.Pitch;
            }
        }
    }
}
