using Netychords.Modules;
using Netychords.Modules.CustomRows;
using Netychords.Settings;
using Netychords.Surface;
using Netychords.Utils;
using NITHdmis.Music;
using NITHlibrary.Tools.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Netychords.Behaviors.Headtracker;

namespace Netychords
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush GazeButtonColor = new SolidColorBrush(Colors.DarkGoldenrod);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        private List<CustomRow> CustomRows;
        private Brush LastGazedBrush = null;
        private NetychordsSetup netychordsSetup;
        private bool netychordsStarted = false;
        private DispatcherTimer updater;

        public MainWindow()
        {
            InitializeComponent();
            TraceAdder.AddTrace();

            DataContext = this;

            // Initializing dispatcher timer, i.e. the timer that updates every graphical value in
            // the interface.
            updater = new DispatcherTimer();
            updater.Tick += UpdateTimedVisuals!;
            updater.Start();
        }

        public Button LastSettingsGazedButton { get; set; } = null;
        public bool NetychordsStarted { get => netychordsStarted; set => netychordsStarted = value; }

        public int SensorPort
        {
            get { return Rack.UserSettings.SensorPort; }
            set
            {
                if (value > 0)
                {
                    Rack.UserSettings.SensorPort = value;
                }
            }
        }

        private bool IsSettingsShown { get; set; } = false;

        public void Global_NetychordsButtonEntered()
        {
            if (netychordsStarted)
            {
                if (LastSettingsGazedButton != null)
                {
                    // Reset Previous Button
                    LastSettingsGazedButton.Background = LastGazedBrush;
                    LastSettingsGazedButton = null;
                }
            }
        }

        public void StartNetychords()
        {
            if (!netychordsStarted)
            {
                // Hide settings
                brdSettings.Visibility = Visibility.Hidden;

                // EventHandler for all buttons
                EventManager.RegisterClassHandler(typeof(Button), MouseEnterEvent, new RoutedEventHandler(Global_SettingsButton_MouseEnter));

                // Load settings
                Rack.UserSettings = Rack.SavingSystem.LoadSettings();

                // Load custom rows
                Rack.CustomRowsManager.SetRows(Rack.SavingSystem.LoadCustomRows());
                CreateCustomLayout();

                // Launches the Setup class
                netychordsSetup = new NetychordsSetup(this);
                netychordsSetup.Setup();

                // Initialize sensor behavior
                ChangeInteractionMethod();

                // Checks the selected MIDI port is available
                CheckMidiPort();
                InitializeSensorPortText();

                // Connect
                UpdateSensorConnection();

                // LEAVE AT THE END! This keeps track of the started state
                netychordsStarted = true;
                UpdateButtonVisuals();
            }
        }

        private void btnAutoStrum_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.AutoStrum = !Rack.UserSettings.AutoStrum;
            if (Rack.UserSettings.AutoStrum)
            {
                Rack.MappingModule.StartAutostrum(Rack.UserSettings.AutoStrumBPM);
            }
            else
            {
                Rack.MappingModule.StopAutostrum();
            }
            UpdateButtonVisuals();
        }

        private void btnAutoStrumM_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.AutoStrumBPM -= 1;
                if (Rack.UserSettings.AutoStrumBPM < Rack.MIN_BPM) Rack.UserSettings.AutoStrumBPM = Rack.MIN_BPM;

                if (Rack.UserSettings.AutoStrum)
                {
                    Rack.MappingModule.StopAutostrum();
                    Rack.MappingModule.StartAutostrum(Rack.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void btnAutoStrumMM_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.AutoStrumBPM -= 10;
                if (Rack.UserSettings.AutoStrumBPM < Rack.MIN_BPM) Rack.UserSettings.AutoStrumBPM = Rack.MIN_BPM;

                if (Rack.UserSettings.AutoStrum)
                {
                    Rack.MappingModule.StopAutostrum();
                    Rack.MappingModule.StartAutostrum(Rack.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void btnAutoStrumP_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.AutoStrumBPM += 1;
                if (Rack.UserSettings.AutoStrumBPM > Rack.MAX_BPM) Rack.UserSettings.AutoStrumBPM = Rack.MAX_BPM;

                if (Rack.UserSettings.AutoStrum)
                {
                    Rack.MappingModule.StopAutostrum();
                    Rack.MappingModule.StartAutostrum(Rack.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void btnAutoStrumPP_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.AutoStrumBPM += 10;
                if (Rack.UserSettings.AutoStrumBPM > Rack.MAX_BPM) Rack.UserSettings.AutoStrumBPM = Rack.MAX_BPM;

                if (Rack.UserSettings.AutoStrum)
                {
                    Rack.MappingModule.StopAutostrum();
                    Rack.MappingModule.StartAutostrum(Rack.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void btnBlinkPlay_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.BlinkLeftStop = !Rack.UserSettings.BlinkLeftStop;
            UpdateButtonVisuals();
        }

        private void BtnCenter_Click(object sender, RoutedEventArgs e)
        {
            Rack.PreprocessorHeadTrackerCalibrator.SetCenterToCurrentPosition();
            UpdateButtonVisuals();
        }

        private void btnControl_Blink_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.InteractionMethod = NetychordsInteractionMethod.Blink;
                ChangeInteractionMethod();
                UpdateButtonVisuals();
            }
        }

        private void btnControl_Head_Pitch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.InteractionMethod = NetychordsInteractionMethod.HeadPitch;
                ChangeInteractionMethod();
                UpdateButtonVisuals();
            }
        }

        private void btnControl_Head_Yaw_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.InteractionMethod = NetychordsInteractionMethod.HeadYaw;

                ChangeInteractionMethod();
                UpdateButtonVisuals();
            }
        }

        private void btnControl_Pressure_Blink_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.InteractionMethod = NetychordsInteractionMethod.PressureBlink;
                ChangeInteractionMethod();
                UpdateButtonVisuals();
            }
        }

        private void btnCR1_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR1.ChordType = Rack.CustomRowsManager.CR1.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR1_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR1.ChordType = Rack.CustomRowsManager.CR1.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR1_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR1.Enabled = !Rack.CustomRowsManager.CR1.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR2_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR2.ChordType = Rack.CustomRowsManager.CR2.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR2_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR2.ChordType = Rack.CustomRowsManager.CR2.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR2_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR2.Enabled = !Rack.CustomRowsManager.CR2.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR3_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR3.ChordType = Rack.CustomRowsManager.CR3.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR3_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR3.ChordType = Rack.CustomRowsManager.CR3.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR3_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR3.Enabled = !Rack.CustomRowsManager.CR3.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR4_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR4.ChordType = Rack.CustomRowsManager.CR4.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR4_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR4.ChordType = Rack.CustomRowsManager.CR4.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR4_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR4.Enabled = !Rack.CustomRowsManager.CR4.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR5_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR5.ChordType = Rack.CustomRowsManager.CR5.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR5_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR5.ChordType = Rack.CustomRowsManager.CR5.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR5_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR5.Enabled = !Rack.CustomRowsManager.CR5.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR6_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR6.ChordType = Rack.CustomRowsManager.CR6.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR6_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR6.ChordType = Rack.CustomRowsManager.CR6.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR6_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR6.Enabled = !Rack.CustomRowsManager.CR6.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR7_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR7.ChordType = Rack.CustomRowsManager.CR7.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR7_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR7.ChordType = Rack.CustomRowsManager.CR7.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR7_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR7.Enabled = !Rack.CustomRowsManager.CR7.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR8_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR8.ChordType = Rack.CustomRowsManager.CR8.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR8_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR8.ChordType = Rack.CustomRowsManager.CR8.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR8_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR8.Enabled = !Rack.CustomRowsManager.CR8.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR9_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR9.ChordType = Rack.CustomRowsManager.CR9.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR9_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR9.ChordType = Rack.CustomRowsManager.CR9.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR9_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.CustomRowsManager.CR9.Enabled = !Rack.CustomRowsManager.CR9.Enabled;
                RemakeCustom();
            }
        }

        private void btnDistanceM10_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                if (Rack.UserSettings.HorizontalSpacer > Rack.MINDISTANCE)
                {
                    Rack.UserSettings.HorizontalSpacer -= 10;
                    Rack.UserSettings.VerticalSpacer = Rack.UserSettings.HorizontalSpacer / 2;
                    Update_Distance();
                    Rack.Surface.DrawButtons();
                }
            }
        }

        private void btnDistanceP10_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                if (Rack.UserSettings.HorizontalSpacer < Rack.MAXDISTANCE)
                {
                    Rack.UserSettings.HorizontalSpacer += 10;
                    Rack.UserSettings.VerticalSpacer = Rack.UserSettings.HorizontalSpacer / 2;
                    Update_Distance();
                    Rack.Surface.DrawButtons();
                }
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Rack.MappingModule.StopNotes();
            Rack.MappingModule.StopAutostrum();

            Rack.SavingSystem.SaveSettings(Rack.UserSettings);
            Rack.SavingSystem.SaveCustomRows(Rack.CustomRowsManager.GetRows());

            Close();
        }

        private void btnLayout_Custom_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.Layout = Layouts.Custom;
                CreateCustomLayout();
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayout_FifthCirc_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.Layout = Layouts.FifthCircle;
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayout_Flowerpot_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.Layout = Layouts.Flower;
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayout_Stradella_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.Layout = Layouts.Stradella;
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayoutGrid_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.Margins = MarginModes.Grid;
                Update_Margins();
                Rack.Surface.DrawButtons();
            }
        }

        private void btnLayoutSlant_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.Margins = MarginModes.Slant;
                Update_Margins();
                Rack.Surface.DrawButtons();
            }
        }

        private void BtnMIDIchMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.MIDIPort--;
                Rack.MidiModule.OutDevice = Rack.UserSettings.MIDIPort;
                UpdateButtonVisuals();
            }
        }

        private void BtnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.UserSettings.MIDIPort++;
                Rack.MidiModule.OutDevice = Rack.UserSettings.MIDIPort;
                UpdateButtonVisuals();
            }
        }

        private void btnNoCursor_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.MappingModule.CursorHidden = !Rack.MappingModule.CursorHidden;
                Cursor = Rack.MappingModule.CursorHidden ? Cursors.None : Cursors.Arrow;
            }

            UpdateButtonVisuals();
        }

        private void btnOnlyDiatonic_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.OnlyDiatonic = !Rack.UserSettings.OnlyDiatonic;
            switch (Rack.UserSettings.OnlyDiatonic)
            {
                case true:
                    Rack.UserSettings.NCols = 3;
                    break;

                case false:
                    Rack.UserSettings.NCols = 12;
                    break;
            }
            Rack.Surface.DrawButtons();
            UpdateButtonVisuals();
        }

        private void btnPresetMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.UserSettings.Preset = Rack.UserSettings.Preset.Previous();
                Rack.UserSettings.Preset.Load();
                RemakeCustom();
            }
        }

        private void btnPresetPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && Rack.UserSettings.Layout == Layouts.Custom)
            {
                Rack.UserSettings.Preset = Rack.UserSettings.Preset.Next();
                Rack.UserSettings.Preset.Load();
                RemakeCustom();
            }
        }

        private void btnReed1_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.MappingModule.SwitchReed(0);
                Update_Reeds();
            }
        }

        private void btnReed2_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.MappingModule.SwitchReed(1);
                Update_Reeds();
            }
        }

        private void btnReed3_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.MappingModule.SwitchReed(2);
                Update_Reeds();
            }
        }

        private void btnReed4_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.MappingModule.SwitchReed(3);
                Update_Reeds();
            }
        }

        private void btnReed5_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.MappingModule.SwitchReed(4);
                Update_Reeds();
            }
        }

        private void btnRootNoteMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                MidiNotes currentRootMidi = Rack.UserSettings.FirstRoot.ToMidiNote(4);
                currentRootMidi--;
                Rack.UserSettings.FirstRoot = currentRootMidi.ToAbsNote();

                canvasNetychords.Children.Clear();
                Rack.Surface.FirstChord = MidiChord.StandardAbsStringToChordFactory(Rack.UserSettings.FirstRoot.ToStandardString(), Rack.MappingModule.octaveNumber, ChordType.Maj);
                Rack.Surface.DrawButtons();

                Update_RootNoteIndicator();
            }
        }

        private void btnRootNotePlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                MidiNotes currentRootMidi = Rack.UserSettings.FirstRoot.ToMidiNote(4);
                currentRootMidi++;
                Rack.UserSettings.FirstRoot = currentRootMidi.ToAbsNote();

                canvasNetychords.Children.Clear();
                Rack.Surface.FirstChord = MidiChord.StandardAbsStringToChordFactory(Rack.UserSettings.FirstRoot.ToStandardString(), Rack.MappingModule.octaveNumber, ChordType.Maj);
                Rack.Surface.DrawButtons();

                Update_RootNoteIndicator();
            }
        }

        private void BtnSensorPortMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                SensorPort--;
                UpdateSensorConnection();
                UpdateButtonVisuals();
            }
        }

        private void BtnSensorPortPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                SensorPort++;
                UpdateSensorConnection();
                UpdateButtonVisuals();
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            switch (IsSettingsShown)
            {
                case false:
                    IsSettingsShown = true;
                    brdSettings.Visibility = Visibility.Visible;
                    if (Rack.UserSettings.Layout == Layouts.Custom)
                    {
                        brdCustom.Visibility = Visibility.Visible;
                    }
                    break;

                case true:
                    IsSettingsShown = false;
                    brdSettings.Visibility = Visibility.Hidden;
                    if (Rack.UserSettings.Layout == Layouts.Custom)
                    {
                        brdCustom.Visibility = Visibility.Hidden;
                    }
                    break;
            }

            UpdateButtonVisuals();
        }

        /// <summary>
        /// This gets called when the Start button is pressed
        /// </summary>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartNetychords();
        }

        private void btnSustain_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.KeyboardSustain = !Rack.UserSettings.KeyboardSustain;
            UpdateButtonVisuals();
        }

        private void btnToggleAutoScroll_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.AutoScroller.Enabled = !Rack.AutoScroller.Enabled;
            }

            UpdateButtonVisuals();
        }

        private void btnToggleEyeTracker_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.BehaviorGazeToMouse.Enabled = !Rack.BehaviorGazeToMouse.Enabled;
            }

            UpdateButtonVisuals();
        }

        /// <summary>
        /// Checks if the selected MIDI port is available
        /// </summary>
        private void CheckMidiPort()
        {
            if (Rack.MidiModule.IsMidiOk())
            {
                lblMIDIch.Foreground = ActiveBrush;
            }
            else
            {
                lblMIDIch.Foreground = WarningBrush;
            }
        }

        private void CreateCustomLayout()
        {
            if (CustomRows == null)
            {
                CustomRows = new List<CustomRow>();
            }
            else
            {
                CustomRows.Clear();
            }

            CustomRows.Add(Rack.CustomRowsManager.CR1);
            CustomRows.Add(Rack.CustomRowsManager.CR2);
            CustomRows.Add(Rack.CustomRowsManager.CR3);
            CustomRows.Add(Rack.CustomRowsManager.CR4);
            CustomRows.Add(Rack.CustomRowsManager.CR5);
            CustomRows.Add(Rack.CustomRowsManager.CR6);
            CustomRows.Add(Rack.CustomRowsManager.CR7);
            CustomRows.Add(Rack.CustomRowsManager.CR8);
            CustomRows.Add(Rack.CustomRowsManager.CR9);

            if (Rack.MappingModule.CustomLines == null)
            {
                Rack.MappingModule.CustomLines = new List<ChordType>();
            }
            else
            {
                Rack.MappingModule.CustomLines.Clear();
            }

            foreach (CustomRow cr in CustomRows)
            {
                if (cr.Enabled)
                {
                    Rack.MappingModule.CustomLines.Add(cr.ChordType);
                }
            }
        }

        private void Global_SettingsButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                if (LastSettingsGazedButton != null)
                {
                    // Reset Previous Button
                    LastSettingsGazedButton.Background = LastGazedBrush;
                }

                LastSettingsGazedButton = (Button)sender;
                LastGazedBrush = LastSettingsGazedButton.Background;
                LastSettingsGazedButton.Background = GazeButtonColor;
            }
        }

        private void InitializeSensorPortText()
        {
            txtSensorPort.Foreground = WarningBrush;
            txtSensorPort.Text = "COM " + SensorPort;
            UpdateSensorConnection();
        }

        private void Margins_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (netychordsStarted)
            {
                Rack.Surface.DrawButtons();
            }
        }

        private void ChangeInteractionMethod()
        {
            Rack.NithModuleSensor.SensorBehaviors.Clear();

            switch (Rack.UserSettings.InteractionMethod)
            {
                case NetychordsInteractionMethod.HeadYaw:
                    Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_HT_Yaw(Rack.UserSettings.SensorIntensityYaw));
                    break;

                case NetychordsInteractionMethod.HeadPitch:
                    Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_HT_Pitch(Rack.UserSettings.SensorIntensityPitch));
                    break;

                case NetychordsInteractionMethod.Blink:
                    
                    break;

                case NetychordsInteractionMethod.PressureBlink:
                    Rack.NithModuleSensor.SensorBehaviors.Add(new NithBeh_Pressure(1f,Rack.UserSettings.SensorIntensityPressure));
                    break;
            }
        }

        private void ProcessLayoutChange()
        {
            canvasNetychords.Children.Clear();
            Rack.Surface.FirstChord = MidiChord.StandardAbsStringToChordFactory(Rack.UserSettings.FirstRoot.ToStandardString(), Rack.MappingModule.octaveNumber, ChordType.Maj);
            Rack.Surface.DrawButtons();
        }

        private void RemakeCustom()
        {
            CreateCustomLayout();
            ProcessLayoutChange();
            UpdateButtonVisuals();
        }

        private void TabSolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Update_AllLayoutsBlank()
        {
            indLayout_Custom.Background = BlankBrush;
            indLayout_Stradella.Background = BlankBrush;
            indLayout_FifthCirc.Background = BlankBrush;
            indLayout_Flowerpot.Background = BlankBrush;
        }

        private void Update_AutoStrum()
        {
            lblAutoStrum.Text = Rack.UserSettings.AutoStrumBPM.ToString();
        }

        private void Update_CR(CustomRow cr, Border indCR_switch, TextBlock txtCR)
        {
            if (cr.Enabled)
            {
                indCR_switch.Background = ActiveBrush;
            }
            else
            {
                indCR_switch.Background = BlankBrush;
            }

            txtCR.Text = cr.ChordType.ToString();
        }

        private void Update_CustomRows()
        {
            Update_CR(Rack.CustomRowsManager.CR1, indCR1_Switch, txtCR1);
            Update_CR(Rack.CustomRowsManager.CR2, indCR2_Switch, txtCR2);
            Update_CR(Rack.CustomRowsManager.CR3, indCR3_Switch, txtCR3);
            Update_CR(Rack.CustomRowsManager.CR4, indCR4_Switch, txtCR4);
            Update_CR(Rack.CustomRowsManager.CR5, indCR5_Switch, txtCR5);
            Update_CR(Rack.CustomRowsManager.CR6, indCR6_Switch, txtCR6);
            Update_CR(Rack.CustomRowsManager.CR7, indCR7_Switch, txtCR7);
            Update_CR(Rack.CustomRowsManager.CR8, indCR8_Switch, txtCR8);
            Update_CR(Rack.CustomRowsManager.CR9, indCR9_Switch, txtCR9);
            txtPreset.Text = Rack.UserSettings.Preset.ToString();
        }

        private void Update_Distance()
        {
            txtDistance.Text = Rack.UserSettings.HorizontalSpacer.ToString() + " px";
        }

        private void Update_Layout()
        {
            Update_AllLayoutsBlank();
            switch (Rack.UserSettings.Layout)
            {
                case Layouts.FifthCircle:
                    indLayout_FifthCirc.Background = ActiveBrush;
                    break;

                case Layouts.Custom:
                    indLayout_Custom.Background = ActiveBrush;
                    break;

                case Layouts.Stradella:
                    indLayout_Stradella.Background = ActiveBrush;
                    break;

                case Layouts.Flower:
                    indLayout_Flowerpot.Background = ActiveBrush;
                    break;
            }
        }

        private void Update_Margins()
        {
            switch (Rack.UserSettings.Margins)
            {
                case MarginModes.Slant:
                    indLayoutGrid.Background = BlankBrush;
                    indLayoutSlant.Background = ActiveBrush;
                    break;

                case MarginModes.Grid:
                    indLayoutGrid.Background = ActiveBrush;
                    indLayoutSlant.Background = BlankBrush;
                    break;
            }
        }

        private void Update_Reeds()
        {
            if (Rack.UserSettings.Reeds().Contains(0))
                indReed1.Background = ActiveBrush;
            else
                indReed1.Background = BlankBrush;
            if (Rack.UserSettings.Reeds().Contains(1))
                indReed2.Background = ActiveBrush;
            else
                indReed2.Background = BlankBrush;
            if (Rack.UserSettings.Reeds().Contains(2))
                indReed3.Background = ActiveBrush;
            else
                indReed3.Background = BlankBrush;
            if (Rack.UserSettings.Reeds().Contains(3))
                indReed4.Background = ActiveBrush;
            else
                indReed4.Background = BlankBrush;
            if (Rack.UserSettings.Reeds().Contains(4))
                indReed5.Background = ActiveBrush;
            else
                indReed5.Background = BlankBrush;
        }

        private void Update_RootNoteIndicator()
        {
            indRootNoteColor.Background = Rack.GetNoteColor(Rack.UserSettings.FirstRoot);
            txtRootNote.Text = Rack.UserSettings.FirstRoot.ToStandardString();
        }

        private void UpdateButtonVisuals()
        {
            if (netychordsStarted)
            {
                indControl_Head_Yaw.Background = Rack.UserSettings.InteractionMethod == NetychordsInteractionMethod.HeadYaw ? ActiveBrush : BlankBrush;
                indControl_Head_Pitch.Background = Rack.UserSettings.InteractionMethod == NetychordsInteractionMethod.HeadPitch ? ActiveBrush : BlankBrush;
                indControl_Pressure_Blink.Background = Rack.UserSettings.InteractionMethod == NetychordsInteractionMethod.PressureBlink ? ActiveBrush : BlankBrush;
                indControl_Blink.Background = Rack.UserSettings.InteractionMethod == NetychordsInteractionMethod.Blink ? ActiveBrush : BlankBrush;

                indOnlyDiatonic.Background = Rack.UserSettings.OnlyDiatonic ? ActiveBrush : BlankBrush;
                indBlinkPlay.Background = Rack.UserSettings.BlinkLeftStop ? ActiveBrush : BlankBrush;
                indSustain.Background = Rack.UserSettings.KeyboardSustain ? ActiveBrush : BlankBrush;
                indSettings.Background = IsSettingsShown ? ActiveBrush : BlankBrush;
                indToggleAutoScroll.Background = Rack.AutoScroller.Enabled ? ActiveBrush : BlankBrush;
                indToggleEyeTracker.Background = Rack.BehaviorGazeToMouse.Enabled ? ActiveBrush : BlankBrush;
                indToggleCursor.Background = Rack.MappingModule.CursorHidden ? ActiveBrush : BlankBrush;
                indAutoStrum.Background = Rack.MappingModule.AutoStrumStarted ? ActiveBrush : BlankBrush;

                Update_RootNoteIndicator();
                Update_Reeds();
                Update_AutoStrum();
                Update_Margins();
                Update_Layout();
                Update_Distance();
                Update_CustomRows();
                Update_SensorIntensity();

                if (Rack.UserSettings.Layout == Layouts.Custom && IsSettingsShown)
                {
                    brdCustom.Visibility = Visibility.Visible;
                }
                else
                {
                    brdCustom.Visibility = Visibility.Hidden;
                }

                lblAutoStrum.Text = Rack.UserSettings.AutoStrumBPM.ToString();
            }

            lblMIDIch.Text = "MP " + Rack.UserSettings.MIDIPort.ToString();
            CheckMidiPort();
        }

        private void Update_SensorIntensity()
        {
            switch(Rack.UserSettings.InteractionMethod)
            { 
                case NetychordsInteractionMethod.PressureBlink:
                    txtSensingIntensity.Text = Rack.UserSettings.SensorIntensityPressure.ToString("F1");
                    break;
                case NetychordsInteractionMethod.HeadYaw:
                    txtSensingIntensity.Text = Rack.UserSettings.SensorIntensityYaw.ToString("F1");
                    break;
                case NetychordsInteractionMethod.HeadPitch:
                    txtSensingIntensity.Text = Rack.UserSettings.SensorIntensityPitch.ToString("F1");
                    break;
                case NetychordsInteractionMethod.Blink:
                    break;
            }
        }

        // [Corrente]
        private void UpdateSensorConnection()
        {
            txtSensorPort.Text = "COM " + SensorPort.ToString();

            if (Rack.USBreceiverHeadTracker.Connect(SensorPort))
            {
                txtSensorPort.Foreground = ActiveBrush;
                Rack.Surface.HtFeedbackModule.Mode = Rack.UserSettings.HTFeedbackMode;
            }
            else
            {
                txtSensorPort.Foreground = WarningBrush;
                Rack.Surface.HtFeedbackModule.Mode = HTFeedbackModule.HTFeedbackModes.None;
            }
        }

        /// <summary>
        /// This method gets called every millisecond (or something like?) in order to update the
        /// elements of the GUI that need a DispatcherTimer
        /// </summary>
        private void UpdateTimedVisuals(object sender, EventArgs e)
        {
            if (netychordsStarted)
            {
                switch (Rack.MappingModule.Mute)
                {
                    case true:
                        lblMIDIch.Background = brdMIDIch.Background = new SolidColorBrush(Colors.Black);
                        break;

                    case false:
                        lblMIDIch.Background = brdMIDIch.Background = new SolidColorBrush(Colors.DarkSlateGray);
                        break;
                }

                Rack.Surface.UpdateHeadTrackerFeedback(Rack.MappingModule.HeadPositionData, Rack.MappingModule.HeadAccelerationData, Rack.MappingModule.HeadTrackerMode);

                if (Rack.RaiseClickEvent)
                {
                    LastSettingsGazedButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    Rack.RaiseClickEvent = false;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartNetychords();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            netychordsSetup.Dispose();
        }

        private void BtnSensingIntensityMinus_OnClick(object sender, RoutedEventArgs e)
        {
            switch(Rack.UserSettings.InteractionMethod)
            {
                case NetychordsInteractionMethod.PressureBlink:
                    Rack.UserSettings.SensorIntensityPressure -= 0.1f;
                    break;
                case NetychordsInteractionMethod.HeadYaw:
                    Rack.UserSettings.SensorIntensityYaw -= 0.1f;
                    break;
                case NetychordsInteractionMethod.HeadPitch:
                    Rack.UserSettings.SensorIntensityPitch -= 0.1f;
                    break;
                case NetychordsInteractionMethod.Blink:
                    break;
            }

            ChangeInteractionMethod();
            UpdateButtonVisuals();
        }

        private void BtnSensingIntensityPlus_OnClick(object sender, RoutedEventArgs e)
        {
            switch (Rack.UserSettings.InteractionMethod)
            {
                case NetychordsInteractionMethod.PressureBlink:
                    Rack.UserSettings.SensorIntensityPressure += 0.1f;
                    break;
                case NetychordsInteractionMethod.HeadYaw:
                    Rack.UserSettings.SensorIntensityYaw += 0.1f;
                    break;
                case NetychordsInteractionMethod.HeadPitch:
                    Rack.UserSettings.SensorIntensityPitch += 0.1f;
                    break;
                case NetychordsInteractionMethod.Blink:
                    break;
            }

            ChangeInteractionMethod();
            UpdateButtonVisuals();
        }
    }
}