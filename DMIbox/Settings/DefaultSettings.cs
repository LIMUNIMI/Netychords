using NeeqDMIs.Music;
using Netychords.Surface;
using Netychords.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Netychords
{
    class DefaultSettings : ISettings
    {
        public int HighlightStrokeDim { get; set; } = 5;
        public int HighlightRadius { get; set; } = 65;
        public int VerticalSpacer { get; set; } = 150;
        public int HorizontalSpacer { get; set; } = 300;
        public int ButtonHeight { get; set; } = 70;
        public int ButtonWidth { get; set; } = 70;
        public int OccluderOffset { get; set; } = 28;
        public int EllipseStrokeDim { get; set; } = 15;
        public int EllipseStrokeSpacer { get; set; } = 15;
        public int LineThickness { get; set; } = 3;
        public List<Color> KeysColorCode { get; set; } = new List<Color>()
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
        public SolidColorBrush NotInScaleBrush { get; set;} = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));
        public SolidColorBrush MinorBrush { get; set; } = new SolidColorBrush(Colors.Blue);
        public SolidColorBrush MajorBrush { get; set; } = new SolidColorBrush(Colors.Red);
        public SolidColorBrush HighlightBrush {get; set; } = new SolidColorBrush(Colors.Black);
        public SolidColorBrush TransparentBrush { get; set; } = new SolidColorBrush(Colors.Transparent);
        public int NCols { get; set; } = 3;
        public int NRows { get; set; } = System.Enum.GetNames(typeof(ChordType)).Length;
        public int Spacing { get; set; } = 100;
        public int GenerativeNote { get; set; } = 40;
        public int StartPositionX { get; set; } = 800;
        public int StartPositionY { get; set; } = 800;
        public int OccluderAlpha { get; set; } = 10;
        public AbsNotes TonalCenter { get; set; } = AbsNotes.C;
        public string FirstNote { get; set; } = "F";
        public bool OnlyDiatonic { get; set; } = true;
        public Layouts Layout { get; set; } = Layouts.Diatonic_3;
        public bool BlinkPlay { get; set; } = false;
        public bool KeyboardSustain { get; set; } = false;
        public int AutoStrumBPM { get; set; } = 120;
        public bool AutoStrum { get; set; } = false;
    }
}
