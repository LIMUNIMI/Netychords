using NeeqDMIs.Music;
using Netychords.Surface;
using System.Collections.Generic;
using System.Windows.Media;

namespace Netychords
{
    public interface ISettings
    {
        int VerticalSpacer { get; set; }
        int HorizontalSpacer { get; set; }
        int ButtonHeight { get; set; }
        int ButtonWidth { get; set; }
        int OccluderOffset { get; set; }
        int EllipseStrokeDim { get; set; }
        int EllipseStrokeSpacer { get; set; }
        int LineThickness { get; set; }
        int HighlightStrokeDim { get; set; }
        int HighlightRadius { get; set; }
        List<Color> KeysColorCode { get; set; }
        SolidColorBrush NotInScaleBrush { get; set; }
        SolidColorBrush MinorBrush { get; set; }
        SolidColorBrush MajorBrush { get; set; }
        SolidColorBrush HighlightBrush { get; set; }
        SolidColorBrush TransparentBrush { get; set; }
        int NCols { get; set; }
        int NRows { get; set; }
        int Spacing { get; set; }
        int GenerativeNote { get; set; }
        int StartPositionX { get; set; }
        int StartPositionY { get; set; }
        int OccluderAlpha { get; set; }
        AbsNotes TonalCenter { get; set; }
        string FirstNote { get; set; }
        bool OnlyDiatonic { get; set; }
        Layouts Layout { get; set; }
        bool BlinkPlay { get; set; }
        bool KeyboardSustain { get; set; }
        int AutoStrumBPM { get; set; }
        bool AutoStrum { get; set; }
    }
}