namespace Netychords
{
    public enum ControlModes
    {
        BreathIntensity,
        GazePointXRaw,
        GazePointXSmooth,
        GazePointY,
        HeadPitch,
        HeadYaw,
        HeadRoll,
        HeadVelocity,
        None
    }

    static class ControlModesMethods
    {
        public static ControlDirection GetDirection(this ControlModes controlMode)
        {
            switch (controlMode)
            {
                case ControlModes.BreathIntensity:
                    return ControlDirection.X;
                case ControlModes.GazePointXRaw:
                    return ControlDirection.X;
                case ControlModes.GazePointXSmooth:
                    return ControlDirection.X;
                case ControlModes.GazePointY:
                    return ControlDirection.Y;
                case ControlModes.HeadPitch:
                    return ControlDirection.X;
                case ControlModes.HeadYaw:
                    return ControlDirection.X;
                case ControlModes.HeadRoll:
                    return ControlDirection.X;
                case ControlModes.HeadVelocity:
                    return ControlDirection.X;
                default:
                    return ControlDirection.X;
            }
        }
    }

    public enum ControlDirection
    {
        X,
        Y
    }
}
