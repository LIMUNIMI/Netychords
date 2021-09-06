using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MIDI;
using Netychords.Behaviors.Eyetracker;
using Netychords.Behaviors.Sensor;
using Netychords.DMIBox.KeyboardBehaviors;
using Netychords.Surface;
using System;
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
            // KEYBOARD MODULE
            IntPtr windowHandle = new WindowInteropHelper(R.NDB.MainWindow).Handle;
            R.NDB.KeyboardModule = new KeyboardModule(windowHandle);

            // MIDI
            R.NDB.MidiModule = new MidiModuleNAudio(1, 1);
            MidiDeviceFinder midiDeviceFinder = new MidiDeviceFinder(R.NDB.MidiModule);
            midiDeviceFinder.SetToLastDevice();

            // EYETRACKER
            //if(R.NDB.Eyetracker == EyetrackerModels.Tobii)
            //{
            R.NDB.TobiiModule = new TobiiModule(GazePointDataMode.Unfiltered);
            //Rack.NetychordsDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBpitchPlay(10, 15, 1.5f, 30f));
            //Rack.NetychordsDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBvelocityPlay(8, 12, 2f, 120f, 0.2f));
            //}

            //if(R.NDB.Eyetracker == EyetrackerModels.EyeTribe)
            //{
            //    R.NDB.EyeTribeModule = new EyeTribeModule();
            //    R.NDB.EyeTribeModule.Start();
            //    R.NDB.EyeTribeModule.MouseEmulator = new MouseEmulator(new PointFilterBypass());
            //    R.NDB.EyeTribeModule.MouseEmulatorGazeMode = GazeMode.Raw;
            //}

           

            // MODULES
            R.NDB.HeadTrackerModule = new SensorModule(115200);

            // BEHAVIORS
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBplayStop());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBemulateMouse());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBstopEmulateMouse());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBautoScroller());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBstopAutoScroller());
            R.NDB.KeyboardModule.KeyboardBehaviors.Add(new KBautoStrum());

            R.NDB.HeadTrackerModule.Behaviors.Add(new HSreadSerial());

            //R.NDB.TobiiModule.BlinkBehaviors.Add(new BBDoubleCloseStopChords());
            R.NDB.TobiiModule.BlinkBehaviors.Add(new BBDoubleClosePlayChord());

            //SURFACE
            R.NDB.AutoScroller = new AutoScroller(R.NDB.MainWindow.scrlNetychords, 0, 200, new PointFilterMAExpDecaying(0.1f));

            R.NDB.NetychordsSurface = new NetychordsSurface(R.NDB.MainWindow.canvasNetychords);
            R.NDB.NetychordsSurface.DrawButtons();

            R.NDB.NetychordsSurface.HtFeedbackModule = new HTFeedbackModule(R.NDB.NetychordsSurface.Canvas, HTFeedbackModule.HTFeedbackModes.DeadZone);

            R.NDB.NetychordsSurface.HighLightMode = NetychordsSurfaceHighlightModes.CurrentNote;
        }
    }
}