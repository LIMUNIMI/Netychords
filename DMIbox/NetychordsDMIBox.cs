using NITHdmis.Eyetracking.Tobii;
using NITHdmis.Eyetracking.Utils;
using NITHdmis.Filters.ValueFilters;
using NITHdmis.Headtracking.NeeqHT;
using NITHdmis.Keyboard;
using NITHdmis.MicroLibrary;
using NITHdmis.MIDI;
using NITHdmis.Music;
using NITHdmis.Utils;
using Netychords.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using NITHdmis.NithSensors;

namespace Netychords
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetychordsDMIBox
    {
        public KeyboardModule KeyboardModule;

        public NetychordsDMIBox() : base()
        {
        }

        public IMidiModule MidiModule { get; set; }
        public TobiiModule TobiiModule { get; set; }
        public bool Mute { get; set; } = false;
        public EyetrackerModels Eyetracker { get; set; } = EyetrackerModels.Tobii;
        public MainWindow MainWindow { get; set; }

        #region Instrument logic

        public List<ChordType> CustomLines = new List<ChordType>();

        public string isPlaying = "";

        public bool keyboardEmulator = true;

        public MidiChord LastChord;

        public string octaveNumber = "2";

        private MidiChord chord = new MidiChord(MidiNotes.C4, ChordType.Maj);

        private bool playKeyDown = false;

        private int modulation = 0;

        private int pressure = 127;

        private int velocity = 127;

        public MidiChord Chord
        {
            get { return chord; }
            set
            {
                chord = value;
            }
        }

        public bool PlayKeyDown
        {
            get { return playKeyDown; }
            set
            {
                if (playKeyDown && !value)
                {
                    if (!R.UserSettings.KeyboardSustain)
                    {
                        StopNotes();
                    }

                    playKeyDown = value;
                    isPlaying = "";
                }
                else if (!playKeyDown && value)
                {
                    PlayChord(chord);
                    playKeyDown = value;
                    isPlaying = "Playing";
                }
            }
        }

        public int Modulation
        {
            get { return modulation; }
            set
            {
                modulation = 0;
                SetModulation();
            }
        }

        public int Pressure
        {
            get { return pressure; }
            set
            {
                pressure = 127;
                SetPressure();
            }
        }

        public int Velocity
        {
            get { return velocity; }
            set
            {
                if (value < 0)
                {
                    velocity = 0;
                }
                else if (value > 127)
                {
                    velocity = 127;
                }
                else
                {
                    velocity = value;
                }
            }
        }

        public void PlayChord(MidiChord chord)
        {
            if (!Mute)
            {
                MidiModule.SetPressure(127);
                MidiModule.SetModulation(0);

                List<int> notes = new List<int>();
                int minInterval;
                int maxInterval;

                for (int i = 0; i < chord.interval.Count; i++)
                {
                    int thisNote = (int)chord.rootNote + chord.interval[i];

                    for (int j = 0; j < 5; j++)
                    {
                        minInterval = 36 + j * 12;
                        maxInterval = 47 + j * 12;

                        if (R.UserSettings.Reeds().Contains(j))
                        {
                            if ((thisNote + (j + 1) * 12 <= maxInterval && thisNote + (j + 1) * 12 >= minInterval))
                            {
                                if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j + 1) * 12)))
                                {
                                    notes.Add((int)chord.rootNote + chord.interval[i] + (j + 1) * 12);
                                }
                            }
                            if (thisNote + j * 12 <= maxInterval && thisNote + j * 12 >= minInterval)
                            {
                                if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + j * 12)))
                                {
                                    notes.Add((int)chord.rootNote + chord.interval[i] + j * 12);
                                }
                            }
                            if (thisNote + (j - 1) * 12 <= maxInterval && thisNote + (j - 1) * 12 >= minInterval)
                            {
                                if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j - 1) * 12)))
                                {
                                    notes.Add((int)chord.rootNote + chord.interval[i] + (j - 1) * 12);
                                }
                            }
                        }
                    }
                }

                if (!(R.UserSettings.Reeds().Count == 0))
                {
                    int min = R.UserSettings.Reeds().Min();
                    notes.Add((int)chord.rootNote + (min - 1) * 12);
                }

                for (int i = 0; i < notes.Count; i++)
                {
                    MidiModule.PlayNote(notes[i], velocity);
                }
            }
        }

        public void ResetModulationAndPressure()
        {
            Modulation = 0;
            Pressure = 127;
            Velocity = 127;
        }

        public void StopNotes()
        {
            for (int i = 12; i < 128; i++)
            {
                MidiModule.StopNote(i);
            }
        }

        public void KeyNodeChange(MidiChord newChord)
        {
            if (!(newChord.chordType == chord.chordType && newChord.rootNote == chord.rootNote))
            {
                chord = newChord;
                switch (R.UserSettings.KeyChangeMode)
                {
                    case KeyChangeModes.StopOnChanges:
                        StopNotes();
                        break;

                    case KeyChangeModes.Sustain:
                        //
                        break;
                }
            }
        }

        private void SetModulation()
        {
            MidiModule.SetModulation(Modulation);
        }

        private void SetPressure()
        {
            MidiModule.SetPressure(pressure);
        }

        #endregion Instrument logic

        #region Graphic components

        private AutoScroller autoScroller;
        private NetychordsSurface netychordsSurface;
        public AutoScroller AutoScroller { get => autoScroller; set => autoScroller = value; }
        public NetychordsSurface NetychordsSurface { get => netychordsSurface; set => netychordsSurface = value; }

        #endregion Graphic components

        #region HeadSensor

        public bool isCentered = true;
        private DirectionStrum dirStrum;

        private double endStrum;

        private int headTrackerPortNumber = 0;
        private bool isEndedStrum = false;
        private bool isStartedStrum = false;
        private double lastYaw = 0;
        //private DateTime startingTime;

        private MicroTimer autoStrumTimer;

        public bool AutoStrumStarted = false;



        public enum DirectionStrum
        {
            Right,
            Left
        }

        //private double startStrum;
        public double CenterZone
        {
            get { return deadzoneTop; }
            set { deadzoneTop = value; deadzoneBottom = -value; }
        }

        public double Distance { get; private set; }

        public NithModule NithModule { get; set; }
        public HeadtrackerCenteringHelper HeadData { get; set; } = new HeadtrackerCenteringHelper();

        public HeadTrackerModes HeadTrackerMode { get; set; } = HeadTrackerModes.Acceleration;


        //public NeeqHTModule HeadTrackerModule { get; set; }


        //public NeeqHTData HTData { get; set; } = new NeeqHTData();
        public bool InDeadZone { get; private set; } = false;
        public string Str_HeadTrackerCalib { get; set; } = "Test";
        public string Str_HeadTrackerRaw { get; set; } = "Test";
        public double FilteredVelocity { get; set; } = 0;
        private double deadzoneBottom { get; set; } = 1f;
        private double deadzoneTop { get; set; } = 1f;
        public bool CursorHidden { get; set; } = false;

        public void CalibrationHeadSensor()
        {
            HeadData.SetCenterToCurrentPosition();
            isCentered = true;
        }

        [System.Obsolete("Moved into specific behaviors")]
        public void ElaborateStrumming(float accelMultiplier)
        {
            if (isCentered && MainWindow.NetychordsStarted)
            {
                if (HeadTrackerMode == HeadTrackerModes.Absolute)
                {
                    if (HeadData.CenteredPosition.Yaw <= deadzoneTop && HeadData.CenteredPosition.Yaw >= deadzoneBottom)
                    {
                        //startStrum = HeadTrackerData.CenteredPosition.Yaw;
                        isEndedStrum = false;
                        InDeadZone = true;
                    }
                    else if (!isStartedStrum && !isEndedStrum)
                    {
                        InDeadZone = false;
                        if (HeadData.CenteredPosition.Yaw < deadzoneBottom)
                        {
                            dirStrum = DirectionStrum.Left;
                            isStartedStrum = true;
                            isEndedStrum = false;
                            lastYaw = HeadData.CenteredPosition.Yaw;
                        }
                        if (HeadData.CenteredPosition.Yaw > deadzoneTop)
                        {
                            dirStrum = DirectionStrum.Right;
                            isStartedStrum = true;
                            isEndedStrum = false;
                            lastYaw = HeadData.CenteredPosition.Yaw;
                        }
                    }
                    else if (!isEndedStrum)
                    {
                        InDeadZone = false;
                        switch (dirStrum)
                        {
                            case DirectionStrum.Left:
                                if (HeadData.CenteredPosition.Yaw > lastYaw)
                                {
                                    endStrum = lastYaw;
                                    Distance = endStrum - deadzoneBottom;
                                    int midiVelocity = (int)(40 + 1.4 * Math.Abs(Distance));
                                    isEndedStrum = true;
                                    isStartedStrum = false;
                                    Velocity = midiVelocity;

                                    if (LastChord != null)
                                    {
                                        StopNotes();
                                    }
                                    PlayChord(Chord);
                                    LastChord = Chord;
                                }
                                else
                                {
                                    lastYaw = HeadData.CenteredPosition.Yaw;
                                }
                                break;

                            case DirectionStrum.Right:
                                if (HeadData.CenteredPosition.Yaw < lastYaw)
                                {
                                    endStrum = lastYaw;
                                    Distance = endStrum - deadzoneTop;
                                    int midiVelocity = (int)(40 + 1.4 * Math.Abs(Distance));
                                    isEndedStrum = true;
                                    isStartedStrum = false;
                                    Velocity = midiVelocity;
                                    if (LastChord != null)
                                    {
                                        StopNotes();
                                    }
                                    PlayChord(Chord);
                                    LastChord = Chord;
                                }
                                else
                                {
                                    lastYaw = HeadData.CenteredPosition.Yaw;
                                }
                                break;
                        }
                    }
                }
                else if (HeadTrackerMode == HeadTrackerModes.Acceleration)
                {

                }
            }
        }

        public void StartAutostrum(int bpm)
        {
            if (!AutoStrumStarted)
            {
                autoStrumTimer = new MicroTimer();
                autoStrumTimer.Interval = (60_000_000 / bpm);
                autoStrumTimer.MicroTimerElapsed += AutoStrumTimer_MicroTimerElapsed;
                autoStrumTimer.Start();

                AutoStrumStarted = true;
            }
        }

        public void StopAutostrum()
        {
            if (AutoStrumStarted)
            {
                autoStrumTimer.Abort();
                AutoStrumStarted = false;
            }
        }

        internal void SwitchReed(int reed)
        {
            switch (reed)
            {
                case 0:
                    R.UserSettings.Reed0 = !R.UserSettings.Reed0;
                    break;
                case 1:
                    R.UserSettings.Reed1 = !R.UserSettings.Reed1;
                    break;
                case 2:
                    R.UserSettings.Reed2 = !R.UserSettings.Reed2;
                    break;
                case 3:
                    R.UserSettings.Reed3 = !R.UserSettings.Reed3;
                    break;
                case 4:
                    R.UserSettings.Reed4 = !R.UserSettings.Reed4;
                    break;
                default:
                    break;
            }
        }

        private void AutoStrumTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs e)
        {
            StopNotes();
            PlayChord(chord);
        }

        #endregion HeadSensor
    }
}