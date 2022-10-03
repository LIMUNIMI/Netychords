using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Filters.ValueFilters;
using NeeqDMIs.Headtracking.NeeqHT;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MicroLibrary;
using NeeqDMIs.Music;
using NeeqDMIs.Utils;
using Netychords.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netychords
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetychordsDMIBox : NeeqDMIs.DMIBox
    {
        public NetychordsDMIBox() : base()
        {

        }
        public KeyboardModule KeyboardModule;
        public bool Mute { get; set; } = false;
        public EyetrackerModels Eyetracker { get; set; } = EyetrackerModels.Tobii;
        public MainWindow MainWindow { get; set; }

        #region Instrument logic

        public List<string> arbitraryLines = new List<string>();

        public string isPlaying = "";

        public bool keyboardEmulator = true;

        public MidiChord lastChord;

        public string octaveNumber = "2";

        public List<int> reeds = new List<int>();

        private MidiChord chord = new MidiChord(MidiNotes.C4, ChordType.Major);

        private bool keyDown = false;

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

        public bool KeyDown
        {
            get { return keyDown; }
            set
            {
                if (keyDown && !value)
                {
                    if (!R.UserSettings.KeyboardSustain)
                    {
                        StopNotes();
                    }

                    keyDown = value;
                    isPlaying = "";
                }
                else
                if (!keyDown && value)
                {
                    PlayChord(chord);
                    keyDown = value;
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

                        if (reeds.Contains(j))
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

                if (!(reeds.Count == 0))
                {
                    int min = reeds.Min();
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
            if(!(newChord.chordType == chord.chordType && newChord.rootNote == chord.rootNote))
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

        private bool autoStrumStarted = false;

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
        public NeeqHTModule HeadTrackerModule { get; set; }

        public int HeadTrackerPortNumber
        {
            get
            {
                return headTrackerPortNumber;
            }
            set
            {
                if (value < 0)
                {
                    headTrackerPortNumber = 0;
                }
                else
                {
                    headTrackerPortNumber = value;
                }
            }
        }

        public HeadTrackerData HTData { get; set; } = new HeadTrackerData();
        public bool InDeadZone { get; private set; } = false;
        public string Str_HeadTrackerCalib { get; set; } = "Test";
        public string Str_HeadTrackerRaw { get; set; } = "Test";
        private double deadzoneBottom { get; set; } = 1f;
        private double deadzoneTop { get; set; } = 1f;

        public void CalibrationHeadSensor()
        {
            HTData.CalibrateCenter();
            isCentered = true;
        }

        public void ElaborateStrumming()
        {
            if (isCentered && MainWindow.NetychordsStarted)
            {
                if(HTData.HeadTrackerMode == HeadTrackerMode.Absolute)
                {
                    if (HTData.TranspYaw <= deadzoneTop && HTData.TranspYaw >= deadzoneBottom)
                    {
                        //startStrum = HeadTrackerData.TranspYaw;
                        isEndedStrum = false;
                        InDeadZone = true;
                    }
                    else if (!isStartedStrum && !isEndedStrum)
                    {
                        InDeadZone = false;
                        if (HTData.TranspYaw < deadzoneBottom)
                        {
                            dirStrum = DirectionStrum.Left;
                            isStartedStrum = true;
                            isEndedStrum = false;
                            lastYaw = HTData.TranspYaw;
                        }
                        if (HTData.TranspYaw > deadzoneTop)
                        {
                            dirStrum = DirectionStrum.Right;
                            isStartedStrum = true;
                            isEndedStrum = false;
                            lastYaw = HTData.TranspYaw;
                        }
                    }
                    else if (!isEndedStrum)
                    {
                        InDeadZone = false;
                        switch (dirStrum)
                        {
                            case DirectionStrum.Left:
                                if (HTData.TranspYaw > lastYaw)
                                {
                                    endStrum = lastYaw;
                                    Distance = endStrum - deadzoneBottom;
                                    int midiVelocity = (int)(40 + 1.4 * Math.Abs(Distance));
                                    isEndedStrum = true;
                                    isStartedStrum = false;
                                    Velocity = midiVelocity;

                                    if (lastChord != null)
                                    {
                                        StopNotes();
                                    }
                                    PlayChord(Chord);
                                    lastChord = Chord;
                                }
                                else
                                {
                                    lastYaw = HTData.TranspYaw;
                                }
                                break;

                            case DirectionStrum.Right:
                                if (HTData.TranspYaw < lastYaw)
                                {
                                    endStrum = lastYaw;
                                    Distance = endStrum - deadzoneTop;
                                    int midiVelocity = (int)(40 + 1.4 * Math.Abs(Distance));
                                    isEndedStrum = true;
                                    isStartedStrum = false;
                                    Velocity = midiVelocity;
                                    if (lastChord != null)
                                    {
                                        StopNotes();
                                    }
                                    PlayChord(Chord);
                                    lastChord = Chord;
                                }
                                else
                                {
                                    lastYaw = HTData.TranspYaw;
                                }
                                break;
                        }
                    }
                }
                else if(HTData.HeadTrackerMode == HeadTrackerMode.Acceleration)
                {
                    VelocityFilter.Push(Math.Abs(HTData.AccYaw));

                    FilteredVelocity = (VelocityFilter.Pull());
                    Velocity = (int)Mapper_AccToVelocity.Map(FilteredVelocity);

                    if (Math.Sign(lastVelocity) != Math.Sign(HTData.AccYaw) && Math.Abs(lastVelocity - HTData.AccYaw) > STRUMTHRESHOLD)
                    {
                        if (lastChord != null)
                        {
                            StopNotes();
                        }
                        PlayChord(Chord);
                    }

                    lastVelocity = HTData.AccYaw;
                }
            }
        }

        private IDoubleFilter VelocityFilter = new DoubleFilterMAExpDecaying(0.04f);
        private IDoubleFilter ThresholdFilter = new DoubleFilterMAExpDecaying(0.1f);
        private double lastVelocity = 0f;
        const double STRUMTHRESHOLD = 0.016f;
        private ValueMapperDouble Mapper_AccToVelocity = new ValueMapperDouble(0.3f, 127);
        public double FilteredVelocity { get; set; } = 0;

        public void StartAutostrum(int bpm)
        {
            if (!autoStrumStarted)
            {
                autoStrumTimer = new MicroTimer();
                autoStrumTimer.Interval = (60_000_000 / bpm);
                autoStrumTimer.MicroTimerElapsed += AutoStrumTimer_MicroTimerElapsed;
                autoStrumTimer.Start();

                autoStrumStarted = true;
            }
        }

        public void StopAutostrum()
        {
            if (autoStrumStarted)
            {
                autoStrumTimer.Abort();
                autoStrumStarted = false;
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