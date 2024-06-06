using Netychords.Modules.CustomRows;
using Netychords.Settings;
using Netychords.Surface;
using NITHdmis.Modules.Keyboard;
using NITHdmis.Modules.MIDI;
using NITHdmis.Modules.Mouse;
using NITHdmis.Music;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Tools.Ports;
using System.Windows.Media;

namespace Netychords.Modules
{
    internal static class Rack
    {
        public const int MAX_BPM = 300;
        public const int MAXDISTANCE = 600;
        public const int MIN_BPM = 10;
        public const int MINDISTANCE = 200;
        public const bool TEST_SWITCH = false;
        public static SavingSystem SavingSystem = new();
        public static AutoScroller AutoScroller { get; set; }
        public static NithSensorBehavior_GazeToMouse BehaviorGazeToMouse { get; set; }
        public static CustomRowsManager CustomRowsManager { get; set; } = new();
        public static KeyboardModuleWPF KeyboardModule { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static MappingModule MappingModule { get; set; } = new();
        public static IMidiModule MidiModule { get; set; }
        public static MouseModule MouseModule { get; set; }
        public static NithModule NithModuleEyeTracker { get; set; }
        public static NithModule NithModuleSensor { get; set; }
        public static NithPreprocessor_HeadTrackerCalibrator? PreprocessorHeadTrackerCalibrator { get; set; }
        public static bool RaiseClickEvent { get; internal set; } = false;
        public static NetychordsSurface Surface { get; set; }
        public static UDPreceiver UDPreceiverEyeTracker { get; set; }
        public static USBreceiver USBreceiverHeadTracker { get; set; }
        public static NetychordsSettings UserSettings { get; set; } = new DefaultSettings();
        public static string TestString { get; set; } = "";

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
                    return new SolidColorBrush(Color.FromArgb(128, 0xFF, 0x99, 0x00));

                case AbsNotes.E:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0x00));

                case AbsNotes.F:
                    return new SolidColorBrush(Color.FromArgb(255, 0x99, 0xFF, 0x66));

                case AbsNotes.sF:
                    return new SolidColorBrush(Color.FromArgb(128, 0x99, 0xFF, 0x66));

                case AbsNotes.G:
                    return new SolidColorBrush(Color.FromArgb(255, 0x00, 0x00, 0xFF));

                case AbsNotes.sG:
                    return new SolidColorBrush(Color.FromArgb(128, 0x99, 0xFF, 0x66));

                case AbsNotes.A:
                    return new SolidColorBrush(Color.FromArgb(255, 0x66, 0x00, 0xFF));

                case AbsNotes.sA:
                    return new SolidColorBrush(Color.FromArgb(128, 0x66, 0x00, 0xFF));

                case AbsNotes.B:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xCC, 0x99));

                default:
                    return new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0xFF));
            }
        }
    }
}