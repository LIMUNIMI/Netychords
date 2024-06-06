using Netychords.Surface;
using NITHdmis.Music;
using System;

namespace Netychords.Settings
{
    [Serializable]
    internal class DefaultSettings : NetychordsSettings
    {
        public DefaultSettings() : base()
        {
            BlinkLeftStop = true;
            HighlightStrokeThickness = 6;
            HighlightRadius = 65;
            VerticalSpacer = 150;
            HorizontalSpacer = 300;
            ButtonHeight = 100;
            ButtonWidth = 100;
            OccluderOffset = 50;
            EllipseStrokeDim = 15;
            EllipseStrokeSpacer = 15;
            LineThickness = 3;
            NCols = 12;
            NRows = 9;
            Spacing = 100;
            GenerativeNote = 40;
            StartPositionX = 800;
            StartPositionY = 800;
            OccluderAlpha = 10;
            FirstRoot = AbsNotes.C;
            OnlyDiatonic = false;
            Layout = Layouts.FifthCircle;
            KeyboardSustain = false;
            AutoStrum = false;
            KeyChangeMode = KeyChangeModes.Sustain;
            MIDIPort = 1;
            SensorPort = 4;
            Reed1 = true;
            Reed2 = true;
            Margins = MarginModes.Grid;
            HTFeedbackMode = HTFeedbackModule.HTFeedbackModes.None;
            Preset = Presets.Empty;
            InteractionMethod = NetychordsInteractionMethod.HeadYaw;
        }
    }
}