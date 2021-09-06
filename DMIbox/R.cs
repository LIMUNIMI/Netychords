namespace Netychords
{
    internal static class R
    {

        private static NetychordsDMIBox netychordsdmibox = new NetychordsDMIBox();
        public static NetychordsDMIBox NDB { get => netychordsdmibox; set => netychordsdmibox = value; }
        public static ISettings UserSettings { get; set; } = new DefaultSettings();
    }
}