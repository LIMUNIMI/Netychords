namespace Netychords
{
    public static class GlobalValues
    {
        public const long BarWidth = 1000;

        /// <summary>
        /// Indicates when the selection timer will declare a fail
        /// </summary>
        public static int HeadYawRange { get; set; } = 20;
        public static int HeadPitchRange { get; set; } = 20;
        public static int HeadRollRange { get; set; } = 20;
        public static long VelocityRange { get; set; } = 6000;
    }
}
