using Netychords.DMIbox.CustomRows;
using Netychords.DMIbox.Settings;
using Netychords.DMIBox;
using Netychords.Surface;
using Netychords.Utils;
using NITHdmis.ErrorLogging;
using NITHdmis.Headtracking.NeeqHT;
using NITHdmis.Music;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Netychords.Settings;

namespace Netychords
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush GazeButtonColor = new SolidColorBrush(Colors.DarkGoldenrod);

        private bool netychordsStarted = false;
        private DispatcherTimer updater;

        private List<CustomRow> CustomRows;

        public MainWindow()
        {
            InitializeComponent();
            TraceAdder.AddTrace();

            DataContext = this;

            // Initializing dispatcher timer, i.e. the timer that updates every graphical value in
            // the interface.
            updater = new DispatcherTimer();
            updater.Interval = new TimeSpan(1000); //OLD 6000
            updater.Tick += UpdateTimedVisuals;
            updater.Start();
        }

        public bool NetychordsStarted { get => netychordsStarted; set => netychordsStarted = value; }

        public int SensorPort
        {
            get { return R.UserSettings.SensorPort; }
            set
            {
                if (value > 0)
                {
                    R.UserSettings.SensorPort = value;
                }
            }
        }

        private bool IsSettingsShown { get; set; } = false;

        public void StartNetychords()
        {
            if (!netychordsStarted)
            {
                // EventHandler for all buttons
                EventManager.RegisterClassHandler(typeof(Button), Button.MouseEnterEvent, new RoutedEventHandler(Global_SettingsButton_MouseEnter));

                // Load settings
                R.UserSettings = R.SavingSystem.LoadSettings();

                // Load custom rows
                R.CustomRowsManager.SetRows(R.SavingSystem.LoadCustomRows());
                CreateCustomLayout();

                // Launches the Setup class
                NetychordsSetup netychordsSetup = new NetychordsSetup(this);
                netychordsSetup.Setup();

                // Checks the selected MIDI port is available
                CheckMidiPort();
                InitializeSensorPortText();

                // LEAVE AT THE END! This keeps track of the started state
                netychordsStarted = true;
                UpdateButtonVisuals();
            }
        }

        private void Global_SettingsButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                if(LastSettingsGazedButton != null)
                {
                    // Reset Previous Button
                    LastSettingsGazedButton.Background = LastGazedBrush;
                }
                
                LastSettingsGazedButton = (Button)sender;
                LastGazedBrush = LastSettingsGazedButton.Background;
                LastSettingsGazedButton.Background = GazeButtonColor;
            }
        }

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

        private Brush LastGazedBrush = null;

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

            CustomRows.Add(R.CustomRowsManager.CR1);
            CustomRows.Add(R.CustomRowsManager.CR2);
            CustomRows.Add(R.CustomRowsManager.CR3);
            CustomRows.Add(R.CustomRowsManager.CR4);
            CustomRows.Add(R.CustomRowsManager.CR5);
            CustomRows.Add(R.CustomRowsManager.CR6);
            CustomRows.Add(R.CustomRowsManager.CR7);
            CustomRows.Add(R.CustomRowsManager.CR8);
            CustomRows.Add(R.CustomRowsManager.CR9);

            if (R.NDB.CustomLines == null)
            {
                R.NDB.CustomLines = new List<ChordType>();
            }
            else
            {
                R.NDB.CustomLines.Clear();
            }

            foreach (CustomRow cr in CustomRows)
            {
                if (cr.Enabled)
                {
                    R.NDB.CustomLines.Add(cr.ChordType);
                }
            }
        }

        private void BtnCenter_Click(object sender, RoutedEventArgs e)
        {
            R.NDB.HeadData.SetCenterToCurrentPosition();
            R.NDB.CalibrationHeadSensor();
            UpdateButtonVisuals();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            R.NDB.StopNotes();
            R.NDB.StopAutostrum();
            R.NDB.TobiiModule.Dispose();

            R.SavingSystem.SaveSettings(R.UserSettings);
            R.SavingSystem.SaveCustomRows(R.CustomRowsManager.GetRows());

            Close();
        }

        private void BtnMIDIchMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.MIDIPort--;
                R.NDB.MidiModule.OutDevice = R.UserSettings.MIDIPort;
                UpdateButtonVisuals();
            }
        }

        private void BtnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.MIDIPort++;
                R.NDB.MidiModule.OutDevice = R.UserSettings.MIDIPort;
                UpdateButtonVisuals();
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

        /// <summary>
        /// Checks if the selected MIDI port is available
        /// </summary>
        private void CheckMidiPort()
        {
            if (R.NDB.MidiModule.IsMidiOk())
            {
                lblMIDIch.Foreground = ActiveBrush;
            }
            else
            {
                lblMIDIch.Foreground = WarningBrush;
            }
        }

        private void InitializeSensorPortText()
        {
            txtSensorPort.Foreground = WarningBrush;
            txtSensorPort.Text = "COM " + SensorPort;
            UpdateSensorConnection();
        }

        private void ProcessLayoutChange()
        {
            canvasNetychords.Children.Clear();
            R.NDB.NetychordsSurface.FirstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstRoot.ToStandardString(), R.NDB.octaveNumber, ChordType.Maj);
            R.NDB.NetychordsSurface.DrawButtons();
        }

        private void Margins_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (netychordsStarted)
            {
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        /// <summary>
        /// This gets called when the Start button is pressed
        /// </summary>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartNetychords();
        }

        private void TabSolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        // [Corrente]
        private void UpdateSensorConnection()
        {
            txtSensorPort.Text = "COM " + SensorPort.ToString();

            if (R.NDB.NithModule.Connect(SensorPort))
            {
                txtSensorPort.Foreground = ActiveBrush;
                R.NDB.NetychordsSurface.HtFeedbackModule.Mode = R.UserSettings.HTFeedbackMode;
            }
            else
            {
                txtSensorPort.Foreground = WarningBrush;
                R.NDB.NetychordsSurface.HtFeedbackModule.Mode = HTFeedbackModule.HTFeedbackModes.None;
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
                lblYaw.Text = R.NDB.Velocity.ToString();

                //switch (R.NDB.HeadTrackerMode)
                //{
                //    case HeadTrackerModes.Absolute:
                //        lblYaw.Text = R.NDB.HeadData.CenteredPosition.Yaw.ToString();
                //        break;

                //    case HeadTrackerModes.Acceleration:
                //        lblYaw.Text = R.NDB.Velocity.ToString();
                //        break;
                //}

                //txtCenterValue.Text = Math.Round(R.NDB.CenterZone, 0).ToString();
                //txtCenterPitchValue.Text = Math.Round(centerPitchZone.Value, 0).ToString();

                switch (R.NDB.Mute)
                {
                    case true:
                        lblMIDIch.Background = brdMIDIch.Background = new SolidColorBrush(Colors.Black);
                        break;

                    case false:
                        lblMIDIch.Background = brdMIDIch.Background = new SolidColorBrush(Colors.DarkSlateGray);
                        break;
                }

                R.NDB.NetychordsSurface.UpdateHeadTrackerFeedback(R.NDB.HeadData.Position, R.NDB.HeadData.Acceleration, R.NDB.HeadTrackerMode);

                if (R.RaiseClickEvent)
                {
                    LastSettingsGazedButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    R.RaiseClickEvent = false;
                }
            }
        }

        /* Visuals that should be updated only when a button is pressed */

        private void UpdateButtonVisuals()
        {
            if (netychordsStarted)
            {
                indControl_Head_Yaw.Background = R.UserSettings.InteractionMethod == NetychordsInteractionMethod.HeadYaw ? ActiveBrush : BlankBrush;
                indControl_Head_Pitch.Background = R.UserSettings.InteractionMethod == NetychordsInteractionMethod.HeadPitch ? ActiveBrush : BlankBrush;
                indControl_Pressure_Blink.Background = R.UserSettings.InteractionMethod == NetychordsInteractionMethod.PressureBlink ? ActiveBrush : BlankBrush;
                indControl_Blink.Background = R.UserSettings.InteractionMethod == NetychordsInteractionMethod.Blink ? ActiveBrush : BlankBrush;

                indOnlyDiatonic.Background = R.UserSettings.OnlyDiatonic ? ActiveBrush : BlankBrush;
                indBlinkPlay.Background = R.UserSettings.BlinkLeftStop ? ActiveBrush : BlankBrush;
                indSustain.Background = R.UserSettings.KeyboardSustain ? ActiveBrush : BlankBrush;
                indSettings.Background = IsSettingsShown ? ActiveBrush : BlankBrush;
                indToggleAutoScroll.Background = R.NDB.AutoScroller.Enabled ? ActiveBrush : BlankBrush;
                indToggleEyeTracker.Background = R.NDB.TobiiModule.MouseEmulator.Enabled ? ActiveBrush : BlankBrush;
                indToggleCursor.Background = R.NDB.CursorHidden ? ActiveBrush : BlankBrush;
                indAutoStrum.Background = R.NDB.AutoStrumStarted ? ActiveBrush : BlankBrush;

                Update_RootNoteIndicator();
                Update_Reeds();
                Update_AutoStrum();
                Update_Margins();
                Update_Layout();
                Update_Distance();
                Update_CustomRows();

                if (R.UserSettings.Layout == Layouts.Custom && IsSettingsShown)
                {
                    brdCustom.Visibility = Visibility.Visible;
                }
                else
                {
                    brdCustom.Visibility = Visibility.Hidden;
                }

                lblAutoStrum.Text = R.UserSettings.AutoStrumBPM.ToString();
            }

            lblMIDIch.Text = "MP " + R.UserSettings.MIDIPort.ToString();
            CheckMidiPort();
        }

        private void Update_CustomRows()
        {
            Update_CR(R.CustomRowsManager.CR1, indCR1_Switch, txtCR1);
            Update_CR(R.CustomRowsManager.CR2, indCR2_Switch, txtCR2);
            Update_CR(R.CustomRowsManager.CR3, indCR3_Switch, txtCR3);
            Update_CR(R.CustomRowsManager.CR4, indCR4_Switch, txtCR4);
            Update_CR(R.CustomRowsManager.CR5, indCR5_Switch, txtCR5);
            Update_CR(R.CustomRowsManager.CR6, indCR6_Switch, txtCR6);
            Update_CR(R.CustomRowsManager.CR7, indCR7_Switch, txtCR7);
            Update_CR(R.CustomRowsManager.CR8, indCR8_Switch, txtCR8);
            Update_CR(R.CustomRowsManager.CR9, indCR9_Switch, txtCR9);
            txtPreset.Text = R.UserSettings.Preset.ToString();
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

        private void Update_Layout()
        {
            Update_AllLayoutsBlank();
            switch (R.UserSettings.Layout)
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

        private void Update_AllLayoutsBlank()
        {
            indLayout_Custom.Background = BlankBrush;
            indLayout_Stradella.Background = BlankBrush;
            indLayout_FifthCirc.Background = BlankBrush;
            indLayout_Flowerpot.Background = BlankBrush;
        }

        private void btnOnlyDiatonic_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.OnlyDiatonic = !R.UserSettings.OnlyDiatonic;
            switch (R.UserSettings.OnlyDiatonic)
            {
                case true:
                    R.UserSettings.NCols = 3;
                    break;

                case false:
                    R.UserSettings.NCols = 12;
                    break;
            }
            R.NDB.NetychordsSurface.DrawButtons();
            UpdateButtonVisuals();
        }

        private void btnBlinkPlay_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.BlinkLeftStop = !R.UserSettings.BlinkLeftStop;
            UpdateButtonVisuals();
        }

        private void btnSustain_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.KeyboardSustain = !R.UserSettings.KeyboardSustain;
            UpdateButtonVisuals();
        }

        private void btnAutoStrum_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.AutoStrum = !R.UserSettings.AutoStrum;
            if (R.UserSettings.AutoStrum)
            {
                R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);
            }
            else
            {
                R.NDB.StopAutostrum();
            }
            UpdateButtonVisuals();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            switch (IsSettingsShown)
            {
                case false:
                    IsSettingsShown = true;
                    brdSettings.Visibility = Visibility.Visible;
                    if (R.UserSettings.Layout == Layouts.Custom)
                    {
                        brdCustom.Visibility = Visibility.Visible;
                    }
                    break;

                case true:
                    IsSettingsShown = false;
                    brdSettings.Visibility = Visibility.Hidden;
                    if (R.UserSettings.Layout == Layouts.Custom)
                    {
                        brdCustom.Visibility = Visibility.Hidden;
                    }
                    break;
            }

            UpdateButtonVisuals();
        }

        private void btnRootNotePlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                MidiNotes currentRootMidi = R.UserSettings.FirstRoot.ToMidiNote(4);
                currentRootMidi++;
                R.UserSettings.FirstRoot = currentRootMidi.ToAbsNote();

                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.FirstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstRoot.ToStandardString(), R.NDB.octaveNumber, ChordType.Maj);
                R.NDB.NetychordsSurface.DrawButtons();

                Update_RootNoteIndicator();
            }
        }

        private void Update_RootNoteIndicator()
        {
            indRootNoteColor.Background = R.GetNoteColor(R.UserSettings.FirstRoot);
            txtRootNote.Text = R.UserSettings.FirstRoot.ToStandardString();
        }

        private void btnRootNoteMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                MidiNotes currentRootMidi = R.UserSettings.FirstRoot.ToMidiNote(4);
                currentRootMidi--;
                R.UserSettings.FirstRoot = currentRootMidi.ToAbsNote();

                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.FirstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstRoot.ToStandardString(), R.NDB.octaveNumber, ChordType.Maj);
                R.NDB.NetychordsSurface.DrawButtons();

                Update_RootNoteIndicator();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartNetychords();
        }

        private void btnReed5_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.SwitchReed(4);
                Update_Reeds();
            }
        }

        private void Update_Reeds()
        {
            if (R.UserSettings.Reeds().Contains(0))
                indReed1.Background = ActiveBrush;
            else
                indReed1.Background = BlankBrush;
            if (R.UserSettings.Reeds().Contains(1))
                indReed2.Background = ActiveBrush;
            else
                indReed2.Background = BlankBrush;
            if (R.UserSettings.Reeds().Contains(2))
                indReed3.Background = ActiveBrush;
            else
                indReed3.Background = BlankBrush;
            if (R.UserSettings.Reeds().Contains(3))
                indReed4.Background = ActiveBrush;
            else
                indReed4.Background = BlankBrush;
            if (R.UserSettings.Reeds().Contains(4))
                indReed5.Background = ActiveBrush;
            else
                indReed5.Background = BlankBrush;
        }

        private void btnReed4_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.SwitchReed(3);
                Update_Reeds();
            }
        }

        private void btnReed3_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.SwitchReed(2);
                Update_Reeds();
            }
        }

        private void btnReed2_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.SwitchReed(1);
                Update_Reeds();
            }
        }

        private void btnReed1_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.SwitchReed(0);
                Update_Reeds();
            }
        }

        private void btnAutoStrumMM_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.AutoStrumBPM -= 10;
                if (R.UserSettings.AutoStrumBPM < R.MIN_BPM) R.UserSettings.AutoStrumBPM = R.MIN_BPM;

                if (R.UserSettings.AutoStrum)
                {
                    R.NDB.StopAutostrum();
                    R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void Update_AutoStrum()
        {
            lblAutoStrum.Text = R.UserSettings.AutoStrumBPM.ToString();
        }

        private void btnAutoStrumM_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.AutoStrumBPM -= 1;
                if (R.UserSettings.AutoStrumBPM < R.MIN_BPM) R.UserSettings.AutoStrumBPM = R.MIN_BPM;

                if (R.UserSettings.AutoStrum)
                {
                    R.NDB.StopAutostrum();
                    R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        
        private void btnAutoStrumP_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.AutoStrumBPM += 1;
                if(R.UserSettings.AutoStrumBPM > R.MAX_BPM) R.UserSettings.AutoStrumBPM = R.MAX_BPM;
                
                if (R.UserSettings.AutoStrum)
                {
                    R.NDB.StopAutostrum();
                    R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void btnAutoStrumPP_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.AutoStrumBPM += 10;
                if (R.UserSettings.AutoStrumBPM > R.MAX_BPM) R.UserSettings.AutoStrumBPM = R.MAX_BPM;

                if (R.UserSettings.AutoStrum)
                {
                    R.NDB.StopAutostrum();
                    R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);
                }
                Update_AutoStrum();
            }
        }

        private void btnLayoutGrid_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.Margins = MarginModes.Grid;
                Update_Margins();
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void btnLayoutSlant_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.Margins = MarginModes.Slant;
                Update_Margins();
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void Update_Margins()
        {
            switch (R.UserSettings.Margins)
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

        private void btnLayout_FifthCirc_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.Layout = Layouts.FifthCircle;
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayout_Stradella_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.Layout = Layouts.Stradella;
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayout_Flowerpot_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.Layout = Layouts.Flower;
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnLayout_Custom_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.Layout = Layouts.Custom;
                CreateCustomLayout();
                ProcessLayoutChange();
                UpdateButtonVisuals();
            }
        }

        private void btnDistanceM10_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                if (R.UserSettings.HorizontalSpacer > R.MINDISTANCE)
                {
                    R.UserSettings.HorizontalSpacer -= 10;
                    R.UserSettings.VerticalSpacer = R.UserSettings.HorizontalSpacer / 2;
                    Update_Distance();
                    R.NDB.NetychordsSurface.DrawButtons();
                }
            }
        }

        private void btnDistanceP10_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                if (R.UserSettings.HorizontalSpacer < R.MAXDISTANCE)
                {
                    R.UserSettings.HorizontalSpacer += 10;
                    R.UserSettings.VerticalSpacer = R.UserSettings.HorizontalSpacer / 2;
                    Update_Distance();
                    R.NDB.NetychordsSurface.DrawButtons();
                }
            }
        }

        private void Update_Distance()
        {
            txtDistance.Text = R.UserSettings.HorizontalSpacer.ToString() + " px";
        }

        private void btnCR1_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR1.Enabled = !R.CustomRowsManager.CR1.Enabled;
                RemakeCustom();
            }
        }

        private void RemakeCustom()
        {
            CreateCustomLayout();
            ProcessLayoutChange();
            UpdateButtonVisuals();
        }

        private void btnCR1_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR1.ChordType = R.CustomRowsManager.CR1.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR1_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR1.ChordType = R.CustomRowsManager.CR1.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR2_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR2.Enabled = !R.CustomRowsManager.CR2.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR2_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR2.ChordType = R.CustomRowsManager.CR2.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR2_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR2.ChordType = R.CustomRowsManager.CR2.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR3_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR3.Enabled = !R.CustomRowsManager.CR3.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR3_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR3.ChordType = R.CustomRowsManager.CR3.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR3_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR3.ChordType = R.CustomRowsManager.CR3.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR4_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR4.Enabled = !R.CustomRowsManager.CR4.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR4_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR4.ChordType = R.CustomRowsManager.CR4.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR4_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR4.ChordType = R.CustomRowsManager.CR4.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR5_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR5.Enabled = !R.CustomRowsManager.CR5.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR5_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR5.ChordType = R.CustomRowsManager.CR5.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR5_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR5.ChordType = R.CustomRowsManager.CR5.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR6_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR6.Enabled = !R.CustomRowsManager.CR6.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR6_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR6.ChordType = R.CustomRowsManager.CR6.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR6_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR6.ChordType = R.CustomRowsManager.CR6.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR7_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR7.Enabled = !R.CustomRowsManager.CR7.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR7_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR7.ChordType = R.CustomRowsManager.CR7.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR7_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR7.ChordType = R.CustomRowsManager.CR7.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR8_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR8.Enabled = !R.CustomRowsManager.CR8.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR8_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR8.ChordType = R.CustomRowsManager.CR8.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR8_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR8.ChordType = R.CustomRowsManager.CR8.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnCR9_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR9.Enabled = !R.CustomRowsManager.CR9.Enabled;
                RemakeCustom();
            }
        }

        private void btnCR9_M_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR9.ChordType = R.CustomRowsManager.CR9.ChordType.Previous();
                RemakeCustom();
            }
        }

        private void btnCR9_P_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.CustomRowsManager.CR9.ChordType = R.CustomRowsManager.CR9.ChordType.Next();
                RemakeCustom();
            }
        }

        private void btnPresetMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.UserSettings.Preset = R.UserSettings.Preset.Previous();
                R.UserSettings.Preset.Load();
                RemakeCustom();
            }
        }

        private void btnPresetPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted && R.UserSettings.Layout == Layouts.Custom)
            {
                R.UserSettings.Preset = R.UserSettings.Preset.Next();
                R.UserSettings.Preset.Load();
                RemakeCustom();
            }
        }

        private void btnToggleAutoScroll_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.AutoScroller.Enabled = !R.NDB.AutoScroller.Enabled;
            }

            UpdateButtonVisuals();
        }

        private void btnToggleEyeTracker_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.TobiiModule.MouseEmulator.Enabled = !R.NDB.TobiiModule.MouseEmulator.Enabled;
            }

            UpdateButtonVisuals();
        }

        private void btnNoCursor_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.CursorHidden = !R.NDB.CursorHidden;
                Cursor = R.NDB.CursorHidden ? Cursors.None : Cursors.Arrow;
            }

            UpdateButtonVisuals();
        }

        public Button LastSettingsGazedButton { get; set; } = null;

        private void btnControl_Head_Yaw_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.InteractionMethod = NetychordsInteractionMethod.HeadYaw;
                ProcessInteractionMethodChange();
                UpdateButtonVisuals();
            }
        }

        private void ProcessInteractionMethodChange()
        {
            //TODO (o anche no)
        }

        private void btnControl_Head_Pitch_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.InteractionMethod = NetychordsInteractionMethod.HeadPitch;
                ProcessInteractionMethodChange();
                UpdateButtonVisuals();
            }
        }

        private void btnControl_Pressure_Blink_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.InteractionMethod = NetychordsInteractionMethod.PressureBlink;
                ProcessInteractionMethodChange();
                UpdateButtonVisuals();
            }
        }

        private void btnControl_Blink_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.InteractionMethod = NetychordsInteractionMethod.Blink;
                ProcessInteractionMethodChange();
                UpdateButtonVisuals();
            }
        }
    }
}