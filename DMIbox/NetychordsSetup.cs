using Netychords.Behaviors.Eyetracker;
using Netychords.Behaviors.HeadSensor;
using Netychords.Behaviors.KeyboardBehaviors;
using Netychords.Behaviors.NithHT;
using Netychords.DMIBox.KeyboardBehaviors;
using Netychords.Surface;
using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Eyetracking.Tobii;
using NITHdmis.Keyboard;
using NITHdmis.MIDI;
using NITHdmis.NithSensors;
using System;
using System.Windows;
using System.Windows.Interop;
using Tobii.Interaction.Framework;

namespace Netychords.DMIBox
{
    public class NetychordsSetup
    {
        public NetychordsSetup(MainWindow window)
        {
            R.NDB.MainWindow = window;
        }

        public void Setup()
        {
            // TEST SWITCH BEHAVIOR
            if(R.TEST_SWITCH == false)
            {
                R.NDB.MainWindow.brdSettings.Visibility = Visibility.Hidden;
                R.NDB.MainWindow.brdCustom.Visibility = Visibility.Hidden;
            }

            // KEYBOARD MODULE
            IntPtr windowHandle = new WindowInteropHelper(R.NDB.MainWindow).Handle;
            R.NDB.KeyboardModule = new KeyboardModule(windowHandle, RawInputProcessor.RawInputCaptureMode.Foreground);

            // MIDI
            R.NDB.MidiModule = new MidiModuleNAudio(R.UserSettings.MIDIPort, 1);
            //MidiDeviceFinder midiDeviceFinder = new MidiDeviceFinder(R.NDB.MidiModule);
            //midiDeviceFinder.SetToLastDevice();

            // TOBII
            R.NDB.TobiiModule = new TobiiModule(GazePointDataMode.Unfiltered);

            // MODULES
            //R.NDB.HeadTrackerModule = new NeeqHTModule(115200, "COM");
            R.NDB.NithModule = new NithModule();

            // BEHAVIORS
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBplayStop());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBemulateMouse());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBstopEmulateMouse());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBautoScroller());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBstopAutoScroller());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBautoStrum());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBcenterHeadTracker());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBmute());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBsmute());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBsimulateClick());

            R.NDB.NithModule.SensorBehaviors.Add(new NithBeh_HT_ReceiveData());
            R.NDB.NithModule.SensorBehaviors.Add(new NithBeh_HT_Yaw());
            R.NDB.NithModule.SensorBehaviors.Add(new NithBeh_HT_Pitch());
            R.NDB.NithModule.SensorBehaviors.Add(new NithBeh_Pressure(1f, 1.5f));

            R.NDB.TobiiModule.BlinkBehaviors.Add(new BBDoubleClosePlayChord());
            R.NDB.TobiiModule.BlinkBehaviors.Add(new BBLeftCloseStopNotes());
            R.NDB.TobiiModule.BlinkBehaviors.Add(new BBDoubleCloseClick());
            

            //SURFACE

            R.NDB.AutoScroller = new AutoScroller_ButtonFollower(R.NDB.MainWindow.scrlNetychords, 0, 140, new PointFilterMAExpDecaying(0.1f));

            R.NDB.NetychordsSurface = new NetychordsSurface(R.NDB.MainWindow.canvasNetychords);
            R.NDB.NetychordsSurface.HtFeedbackModule = new HTFeedbackModule(R.NDB.NetychordsSurface.Canvas, HTFeedbackModule.HTFeedbackModes.None); // Disabled feedback! Imho it's better.
            R.NDB.NetychordsSurface.DrawButtons();

            R.NDB.NetychordsSurface.HighLightMode = NetychordsSurfaceHighlightModes.CurrentNote;
        }
    }
}