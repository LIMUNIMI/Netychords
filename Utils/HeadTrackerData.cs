using NeeqDMIs.Utils;
using System.Collections.Generic;

namespace HANDMIs_TestSuite.Utils
{
    public class HeadTrackerData
    {
        private AngleBaseChanger pitchTransf;
        private AngleBaseChanger yawTransf;
        private AngleBaseChanger calibrationYawTransf;
        private AngleBaseChanger rollTransf;
        public double Pitch { get; set; }
        public double CalibrationYaw { get; set; }
        public double Yaw { get; set; }
        public double Roll { get; set; }
        public double TranspPitch { get { return pitchTransf.Transform(Pitch); } }
        public double TranspYaw { get { return yawTransf.Transform(Yaw); } }
        public double TranspCalibrationYaw { get { return calibrationYawTransf.Transform(CalibrationYaw); } }
        public double TranspRoll { get { return rollTransf.Transform(Roll); } }
        public double Velocity { get; set; }

        public HeadTrackerData()
        {
            pitchTransf = new AngleBaseChanger();
            yawTransf = new AngleBaseChanger();
            calibrationYawTransf = new AngleBaseChanger();
            rollTransf = new AngleBaseChanger();
        }

        public double GetYawDeltaBar()
        {
            return yawTransf.getDeltaBar();
        }

        public void SetDeltaForAll()
        {
            pitchTransf.Delta = Pitch;
            yawTransf.Delta = Yaw;
            calibrationYawTransf.Delta = CalibrationYaw;
            rollTransf.Delta = Roll;
        }

        public void SetPitchDelta()
        {
            pitchTransf.Delta = Pitch;
        }

        public void SetYawDelta()
        {
            yawTransf.Delta = Yaw;
        }

        public void SetRollDelta()
        {
            rollTransf.Delta = Roll;
        }
    }
}
