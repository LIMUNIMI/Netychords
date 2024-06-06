using Netychords.Settings;
using Netychords.Utils;
using NITHdmis.Music;
using NITHlibrary.Tools.Timers;
using NITHlibrary.Tools.Types;

namespace Netychords.Modules
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class MappingModule
    {
        public bool Mute { get; set; } = false;

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
                    if (!Rack.UserSettings.KeyboardSustain)
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
                Rack.MidiModule.SetPressure(127);
                Rack.MidiModule.SetModulation(0);

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

                        if (Rack.UserSettings.Reeds().Contains(j))
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

                if (!(Rack.UserSettings.Reeds().Count == 0))
                {
                    int min = Rack.UserSettings.Reeds().Min();
                    notes.Add((int)chord.rootNote + (min - 1) * 12);
                }

                for (int i = 0; i < notes.Count; i++)
                {
                    Rack.MidiModule.PlayNote(notes[i], velocity);
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
                Rack.MidiModule.StopNote(i);
            }
        }

        public void KeyNodeChange(MidiChord newChord)
        {
            if (!(newChord.chordType == chord.chordType && newChord.rootNote == chord.rootNote))
            {
                chord = newChord;
                switch (Rack.UserSettings.KeyChangeMode)
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
            Rack.MidiModule.SetModulation(Modulation);
        }

        private void SetPressure()
        {
            Rack.MidiModule.SetPressure(pressure);
        }

        #endregion Instrument logic
        

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


        //public NeeqHTModule HeadTrackerModule { get; set; }


        //public NeeqHTData HTData { get; set; } = new NeeqHTData();
        public bool InDeadZone { get; private set; } = false;
        public string Str_HeadTrackerCalib { get; set; } = "Test";
        public string Str_HeadTrackerRaw { get; set; } = "Test";
        private double deadzoneBottom { get; set; } = 1f;
        private double deadzoneTop { get; set; } = 1f;
        public bool CursorHidden { get; set; } = false;
        public Polar3DData HeadPositionData { get; set; }
        public Polar3DData HeadAccelerationData { get; set; }
        public HeadTrackerModes HeadTrackerMode { get; set; } = HeadTrackerModes.Acceleration;

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
                autoStrumTimer.Stop();
                AutoStrumStarted = false;
            }
        }

        internal void SwitchReed(int reed)
        {
            switch (reed)
            {
                case 0:
                    Rack.UserSettings.Reed0 = !Rack.UserSettings.Reed0;
                    break;
                case 1:
                    Rack.UserSettings.Reed1 = !Rack.UserSettings.Reed1;
                    break;
                case 2:
                    Rack.UserSettings.Reed2 = !Rack.UserSettings.Reed2;
                    break;
                case 3:
                    Rack.UserSettings.Reed3 = !Rack.UserSettings.Reed3;
                    break;
                case 4:
                    Rack.UserSettings.Reed4 = !Rack.UserSettings.Reed4;
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