using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Headtracking.NeeqHT;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MicroLibrary;
using NeeqDMIs.Music;
using Netychords.Surface;
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
        public KeyboardModule KeyboardModule;
        public EyetrackerModels Eyetracker { get; set; } = EyetrackerModels.Tobii;
        public MainWindow MainWindow { get; set; }

        #region Instrument logic

        public List<string> arbitraryLines = new List<string>();

        public string isPlaying = "";

        public bool keyboardEmulator = true;

        public MidiChord lastChord;

        public string octaveNumber = "2";

        public List<int> reeds = new List<int>();

        public bool strummed = false;

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
                try // LOL TODO TOFIX (generava problemi)
                {
                    if (keyboardEmulator)
                    {
                        if (!(value.chordType == chord.chordType && value.rootNote == chord.rootNote))
                        {
                            if (keyDown)
                            {
                                //StopChord(chord);
                                //PlayChord(value);
                                //isPlaying = "Playing";
                                //isEndedStrum = false;
                            }
                            else
                            {
                                StopChord(chord);
                                isPlaying = "";
                            }
                            chord = value;
                        }
                    }
                    else
                    {
                        if (!(value.chordType == chord.chordType && value.rootNote == chord.rootNote))
                        {
                            chord = value;
                        }
                    }
                }
                catch
                {
                }
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
                        StopChord(chord);
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

        public void ResetModulationAndPressure()
        {
            Modulation = 0;
            Pressure = 127;
            Velocity = 127;
        }

        public void StopChord(MidiChord chord)
        {
            for (int i = 12; i < 128; i++)
            {
                MidiModule.StopNote(i);
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

        public bool isCentered = false;
        private DirectionStrum dirStrum;

        private double endStrum;

        private int headTrackerPortNumber = 0;
        private bool isEndedStrum = false;
        private bool isStartedStrum = false;
        private double lastYaw = 0;
        //private DateTime startingTime;

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
        public SensorModule HeadTrackerModule { get; set; }

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
                        dirStrum = NetychordsDMIBox.DirectionStrum.Left;
                        isStartedStrum = true;
                        isEndedStrum = false;
                        lastYaw = HTData.TranspYaw;
                    }
                    if (HTData.TranspYaw > deadzoneTop)
                    {
                        dirStrum = NetychordsDMIBox.DirectionStrum.Right;
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
                        case NetychordsDMIBox.DirectionStrum.Left:
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
                                    StopChord(lastChord);
                                }
                                PlayChord(Chord);
                                lastChord = Chord;
                            }
                            else
                            {
                                lastYaw = HTData.TranspYaw;
                            }
                            break;

                        case NetychordsDMIBox.DirectionStrum.Right:
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
                                    StopChord(lastChord);
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
        }

        private MicroTimer autoStrumTimer;
        private bool autoStrumStarted = false;
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

        private void AutoStrumTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs e)
        {
            StopChord(chord);
            PlayChord(chord);
        }

        public void StopAutostrum()
        {
            if (autoStrumStarted)
            {
                autoStrumTimer.Abort();
                autoStrumStarted = false;
            }
        }

        #endregion HeadSensor
    }
}