using System.Windows.Media;

namespace Netychords.Surface.ColorCodes
{
    class ColorCodeStandard : IColorCode
    {
        public List<Color> KeysColorCode { get; } = new List<Color>()
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
        public SolidColorBrush NotInScaleBrush { get; } = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));
        public SolidColorBrush MinorBrush { get; } = new SolidColorBrush(Colors.Blue);
        public SolidColorBrush MajorBrush { get; } = new SolidColorBrush(Colors.Red);
        public SolidColorBrush HighlightBrush { get; } = new SolidColorBrush(Colors.Black);
        public SolidColorBrush TransparentBrush { get; } = new SolidColorBrush(Colors.Transparent);
    }
}
