using NITHdmis.Music;
using Netychords.DMIbox.CustomRows;
using System.Collections.Generic;
using System.Windows.Media;

namespace Netychords
{
    internal static class R
    {
        public const int MINDISTANCE = 200;
        public const int MAXDISTANCE = 600;
        public const int MIN_BPM = 10;
        public const int MAX_BPM = 300;
        private static NetychordsDMIBox netychordsdmibox = new NetychordsDMIBox();
        public static NetychordsDMIBox NDB { get => netychordsdmibox; set => netychordsdmibox = value; }
        public static NetychordsSettings UserSettings { get; set; } = new DefaultSettings();
        public static List<Color> KeysColorMode { get; set; } = new List<Color>
        {
            Colors.Red,
            Colors.Tomato,
            Colors.Orange,
            Colors.Gold,
            Colors.Yellow,
            Colors.LightGreen,
            Colors.DarkGreen,
            Colors.Azure,
            Colors.Blue,
            Colors.Purple,
            Colors.Pink,
            Colors.Coral
        };

        public static Brush GetNoteColor(AbsNotes v)
        {
            switch (v)
            {
                case AbsNotes.C:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x00, 0x00));

                case AbsNotes.sC:
                    return new SolidColorBrush(Color.FromArgb(128, 0xFF, 0x00, 0x00));

                case AbsNotes.D:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x99, 0x00));

                case AbsNotes.sD:
                    return new SolidColorBrush(Color.FromArgb(128, 0xFF, 0x99, 0x00));//

                case AbsNotes.E:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0x00));

                case AbsNotes.F:
                    return new SolidColorBrush(Color.FromArgb(255, 0x99, 0xFF, 0x66));//

                case AbsNotes.sF:
                    return new SolidColorBrush(Color.FromArgb(128, 0x99, 0xFF, 0x66));

                case AbsNotes.G:
                    return new SolidColorBrush(Color.FromArgb(255, 0x00, 0x00, 0xFF));

                case AbsNotes.sG:
                    return new SolidColorBrush(Color.FromArgb(128, 0x99, 0xFF, 0x66));

                case AbsNotes.A:
                    return new SolidColorBrush(Color.FromArgb(255, 0x66, 0x00, 0xFF));

                case AbsNotes.sA:
                    return new SolidColorBrush(Color.FromArgb(128, 0x66, 0x00, 0xFF));//

                case AbsNotes.B:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xCC, 0x99));

                default:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0xFF));
            }
        }
        public static SavingSystem SavingSystem = new SavingSystem();
        public static CustomRowsManager CustomRowsManager { get; set; } = new CustomRowsManager();
        public static bool RaiseClickEvent { get; internal set; } = false;

        public const bool TEST_SWITCH = false;
    }
}