using NITHdmis.Music;
using Netychords.Surface;
using System;
using System.Collections.Generic;

namespace Netychords.Settings
{
    [Serializable]
    public class NetychordsSettings
    {
        public NetychordsSettings()
        {

        }

        public NetychordsSettings(int verticalSpacer, int horizontalSpacer, int buttonHeight, int buttonWidth, int occluderOffset, int ellipseStrokeDim, int ellipseStrokeSpacer, int lineThickness, int highlightStrokeThickness, int highlightRadius, int nCols, int nRows, int spacing, int generativeNote, int startPositionX, int startPositionY, int occluderAlpha, AbsNotes firstRoot, bool onlyDiatonic, Layouts layout, bool keyboardSustain, int autoStrumBPM, bool autoStrum, KeyChangeModes keyChangeMode, int mIDIPort, int sensorPort, bool reed0, bool reed1, bool reed2, bool reed3, bool reed4, MarginModes margins, HTFeedbackModule.HTFeedbackModes hTFeedbackMode, Presets preset, NetychordsInteractionMethod interactionMethod, bool blinkPlay)
        {
            VerticalSpacer = verticalSpacer;
            HorizontalSpacer = horizontalSpacer;
            ButtonHeight = buttonHeight;
            ButtonWidth = buttonWidth;
            OccluderOffset = occluderOffset;
            EllipseStrokeDim = ellipseStrokeDim;
            EllipseStrokeSpacer = ellipseStrokeSpacer;
            LineThickness = lineThickness;
            HighlightStrokeThickness = highlightStrokeThickness;
            HighlightRadius = highlightRadius;
            NCols = nCols;
            NRows = nRows;
            Spacing = spacing;
            GenerativeNote = generativeNote;
            StartPositionX = startPositionX;
            StartPositionY = startPositionY;
            OccluderAlpha = occluderAlpha;
            FirstRoot = firstRoot;
            OnlyDiatonic = onlyDiatonic;
            Layout = layout;
            KeyboardSustain = keyboardSustain;
            AutoStrumBPM = autoStrumBPM;
            AutoStrum = autoStrum;
            KeyChangeMode = keyChangeMode;
            MIDIPort = mIDIPort;
            SensorPort = sensorPort;
            Reed0 = reed0;
            Reed1 = reed1;
            Reed2 = reed2;
            Reed3 = reed3;
            Reed4 = reed4;
            Margins = margins;
            HTFeedbackMode = hTFeedbackMode;
            Preset = preset;
            InteractionMethod = interactionMethod;
            BlinkLeftStop = blinkPlay;
        }

        public int VerticalSpacer { get; set; }
        public int HorizontalSpacer { get; set; }
        public int ButtonHeight { get; set; }
        public int ButtonWidth { get; set; }
        public int OccluderOffset { get; set; }
        public int EllipseStrokeDim { get; set; }
        public int EllipseStrokeSpacer { get; set; }
        public int LineThickness { get; set; }
        public int HighlightStrokeThickness { get; set; }
        public int HighlightRadius { get; set; }
        public int NCols { get; set; }
        public int NRows { get; set; }
        public int Spacing { get; set; }
        public int GenerativeNote { get; set; }
        public int StartPositionX { get; set; }
        public int StartPositionY { get; set; }
        public int OccluderAlpha { get; set; }
        public AbsNotes FirstRoot { get; set; }
        public bool OnlyDiatonic { get; set; }
        public Layouts Layout { get; set; }
        public bool KeyboardSustain { get; set; }
        public int AutoStrumBPM { get; set; }
        public bool AutoStrum { get; set; }
        public KeyChangeModes KeyChangeMode { get; set; }
        public int MIDIPort { get; set; }
        public int SensorPort { get; set; }
        public bool Reed0 { get; set; } = false;
        public bool Reed1 { get; set; } = false;
        public bool Reed2 { get; set; } = false;
        public bool Reed3 { get; set; } = false;
        public bool Reed4 { get; set; } = false;
        public MarginModes Margins { get; set; }
        public HTFeedbackModule.HTFeedbackModes HTFeedbackMode { get; set; }
        public Presets Preset { get; set; }
        public List<int> Reeds()
        {
            List<int> reeds = new List<int>();
            if (Reed0) { reeds.Add(0); }
            if (Reed1) { reeds.Add(1); }
            if (Reed2) { reeds.Add(2); }
            if (Reed3) { reeds.Add(3); }
            if (Reed4) { reeds.Add(4); }
            return reeds;
        }

        public NetychordsInteractionMethod InteractionMethod { get; set; }
        public bool BlinkLeftStop { get; set; }
    }
}