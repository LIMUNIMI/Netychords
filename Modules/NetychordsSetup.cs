using System.Windows.Interop;
using Netychords.Behaviors.Eyetracker;
using Netychords.Behaviors.Headtracker;
using Netychords.Behaviors.Keyboard;
using Netychords.Surface;
using NITHdmis.Modules.Keyboard;
using NITHdmis.Modules.MIDI;
using NITHdmis.Modules.Mouse;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Tools.Filters.PointFilters;
using NITHlibrary.Tools.Ports;

namespace Netychords.Modules
{
    public class NetychordsSetup
    {
        private List<IDisposable> disposables = [];

        private bool disposed = false;

        public NetychordsSetup(MainWindow mainWindow)
        {
            Rack.MainWindow = mainWindow;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                foreach (IDisposable disposable in disposables)
                {
                    disposable.Dispose();
                }
            }
        }

        public void Setup()
        {
            // Keyboard module
            IntPtr windowHandle = new WindowInteropHelper(Rack.MainWindow).Handle;
            Rack.KeyboardModule = new KeyboardModuleWPF(windowHandle, RawInputProcessor.RawInputCaptureMode.Foreground);
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBplayStop());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBemulateMouse());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBstopEmulateMouse());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBautoScroller());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBstopAutoScroller());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBautoStrum());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBcenterHeadTracker());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBmute());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBsmute());
            Rack.KeyboardModule.KeyboardBehaviors.Add(new KBsimulateClick());

            // Mouse module
            Rack.MouseModule = new MouseModule();

            // Eye tracker module
            Rack.NithModuleEyeTracker = new NithModule();
            Rack.UDPreceiverEyeTracker = new UDPreceiver(20101);
            Rack.UDPreceiverEyeTracker.Listeners.Add(Rack.NithModuleEyeTracker);
            Rack.BehaviorGazeToMouse = new NithSensorBehavior_GazeToMouse(Rack.MouseModule);
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(Rack.BehaviorGazeToMouse);
            Rack.UDPreceiverEyeTracker.Connect();
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(new BBDoubleCloseClick());
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(new BBDoubleClosePlayChord());
            Rack.NithModuleEyeTracker.SensorBehaviors.Add(new BBLeftCloseStopNotes());
            

            // Head tracker module
            Rack.NithModuleSensor = new NithModule();
            Rack.USBreceiverHeadTracker = new USBreceiver();
            Rack.USBreceiverHeadTracker.Listeners.Add(Rack.NithModuleSensor);
            Rack.PreprocessorHeadTrackerCalibrator = new NithPreprocessor_HeadTrackerCalibrator();
            Rack.NithModuleSensor.Preprocessors.Add(Rack.PreprocessorHeadTrackerCalibrator);
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_HT_ReceiveData());
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_HT_Yaw());
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_HT_Pitch());
            //Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_Pressure(1f, 1.5f));

            // Midi module
            Rack.MidiModule = new MidiModuleNAudio(Rack.UserSettings.MIDIPort, 1);

            // Surface
            Rack.AutoScroller = new AutoScroller_ButtonFollower(Rack.MainWindow.scrlNetychords, 0, 140, new PointFilterMAExpDecaying(0.1f));
            Rack.Surface = new NetychordsSurface(Rack.MainWindow.canvasNetychords);
            Rack.Surface.HtFeedbackModule = new HTFeedbackModule(Rack.Surface.Canvas, HTFeedbackModule.HTFeedbackModes.None); // Disabled feedback! Imho it's better.
            Rack.Surface.DrawButtons();
            Rack.Surface.HighLightMode = NetychordsSurfaceHighlightModes.CurrentNote;

            // Disposables
            disposables.Add(Rack.UDPreceiverEyeTracker);
            disposables.Add(Rack.USBreceiverHeadTracker);
            disposables.Add(Rack.NithModuleEyeTracker);
            disposables.Add(Rack.NithModuleSensor);
        }
    }
}