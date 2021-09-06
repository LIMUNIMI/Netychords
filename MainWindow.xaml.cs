using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Music;
using Netychords.DMIBox;
using Netychords.Surface;
using Netychords.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Netychords
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        //private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);

        private DateTime centering = new DateTime(2020, 01, 01, 0, 0, 0);
        private DateTime clicked;
        private bool clickedButton = false;
        private bool netychordsStarted = false;
        private int sensorPort = 1;
        private DispatcherTimer updater;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initializing dispatcher timer, i.e. the timer that updates every graphical value in
            // the interface.
            updater = new DispatcherTimer();
            updater.Interval = new TimeSpan(6000);
            updater.Tick += UpdateWindow;
            updater.Start();
        }

        public bool NetychordsStarted { get => netychordsStarted; set => netychordsStarted = value; }

        public int SensorPort
        {
            get { return sensorPort; }
            set
            {
                if (value > 0)
                {
                    sensorPort = value;
                }
            }
        }

        private void ArbitraryStart_Click(object sender, RoutedEventArgs e)
        {
            List<ComboBox> boxes = new List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            R.NDB.arbitraryLines.Clear();

            for (int i = 0; i < 11; i++)
            {
                if (boxes[i].SelectedItem != null)
                {
                    R.NDB.arbitraryLines.Add(boxes[i].SelectedItem.ToString());
                }
                else
                {
                    break;
                }
            }
            if (netychordsStarted)
            {
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, R.NDB.octaveNumber, ChordType.Major);
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.DrawButtons();

                arbitraryStart.IsEnabled = false;
            }
        }

        private void BtnCenter_Click(object sender, RoutedEventArgs e)
        {
            R.NDB.HTData.CalibrateCenter();
            R.NDB.CalibrationHeadSensor();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMIDIchMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.MidiModule.OutDevice--;
                lblMIDIch.Text = "MP" + R.NDB.MidiModule.OutDevice.ToString();
                clicked = DateTime.Now;
                clickedButton = true;
                btnMIDIchMinus.IsEnabled = false;
                CheckMidiPort();
            }
        }

        private void BtnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.NDB.MidiModule.OutDevice++;
                lblMIDIch.Text = "MP" + R.NDB.MidiModule.OutDevice.ToString();
                clicked = DateTime.Now;
                clickedButton = true;
                btnMIDIchPlus.IsEnabled = false;

                CheckMidiPort();
            }
        }

        private void BtnSensorPortMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                SensorPort--;
                UpdateSensorConnection();
                clicked = DateTime.Now;
                clickedButton = true;
                btnSensorPortMinus.IsEnabled = false;
            }
        }

        private void BtnSensorPortPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                SensorPort++;
                UpdateSensorConnection();
                clicked = DateTime.Now;
                clickedButton = true;
                btnSensorPortPlus.IsEnabled = false;
            }
        }

        private void CanvasNetytchords_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void centerZone_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R.NDB.CenterZone = sldCenterZone.Value;
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

        private void five_Checked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Add(4);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void five_Unchecked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Remove(4);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void four_Checked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Add(3);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void four_Unchecked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Remove(3);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void InitializeSensorPortText()
        {
            txtSensorPort.Foreground = WarningBrush;
            txtSensorPort.Text = "COM" + SensorPort;
            UpdateSensorConnection();
        }

        private void lstFeedbackModeChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBoxItem)lstFeedbackModeChanger.SelectedItem).Content.ToString())
            {
                case "Bars":
                    R.NDB.NetychordsSurface.HtFeedbackModule.Mode = (Netychords.Surface.HTFeedbackModule.HTFeedbackModes)0;
                    break;

                case "HalfButton":
                    R.NDB.NetychordsSurface.HtFeedbackModule.Mode = (Netychords.Surface.HTFeedbackModule.HTFeedbackModes)1;
                    break;

                case "DeadZone":
                    R.NDB.NetychordsSurface.HtFeedbackModule.Mode = (Netychords.Surface.HTFeedbackModule.HTFeedbackModes)2;
                    break;
            }
        }

        private void LstLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBoxItem)lstLayout.SelectedItem).Content.ToString())
            {
                case "Fifth Circle":
                    R.UserSettings.Layout = Layouts.FifthCircle;
                    break;

                case "Arbitrary":
                    R.UserSettings.Layout = Layouts.Arbitrary;
                    break;

                case "Jazz":
                    R.UserSettings.Layout = Layouts.Jazz;
                    break;

                case "Pop":
                    R.UserSettings.Layout = Layouts.Pop;
                    break;

                case "Rock":
                    R.UserSettings.Layout = Layouts.Rock;
                    break;

                case "Stradella":
                    R.UserSettings.Layout = Layouts.Stradella;
                    break;

                case "Flower":
                    R.UserSettings.Layout = Layouts.Flower;
                    break;

                case "Only Major":
                    R.UserSettings.Layout = Layouts.OnlyMajor;
                    break;

                case "Diatonic_3":
                    R.UserSettings.Layout = Layouts.Diatonic_3;
                    break;

                case "Diatonic_4":
                    R.UserSettings.Layout = Layouts.Diatonic_4;
                    break;
            }

            if (R.UserSettings.Layout == Layouts.Arbitrary)
            {
                SetupComboBox();
                FirstRow.IsEnabled = true;
            }
            else
            {
                System.Collections.Generic.List<ComboBox> boxes = new System.Collections.Generic.List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
                for (int i = 0; i < 11; i++)
                {
                    boxes[i].IsEnabled = false;
                    boxes[i].SelectedItem = null;
                }
            }

            if (netychordsStarted && R.UserSettings.Layout != Layouts.Arbitrary)
            {
                R.NDB.arbitraryLines = new List<string>();

                arbitraryStart.IsEnabled = false;
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, R.NDB.octaveNumber, ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void LstNoteChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var b = ((ListBoxItem)lstNoteChanger.SelectedItem).Content.ToString();

            // ROITO ORRIBILE DA SISTEMARE, MA FUNZIONA
            MidiNotes temp = MidiNotesUtils.StandardStringToAbsNote(b).ToMidiNote(5);
            temp = (MidiNotes)(temp - 7);
            R.UserSettings.FirstNote = temp.ToAbsNote().ToStandardString();

            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, R.NDB.octaveNumber, ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void LstOctaveChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Margins_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (netychordsStarted)
            {
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void one_Checked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Add(0);
            if (netychordsStarted)
            {
                R.NDB.NetychordsSurface.DrawButtons();
            }
            lblYaw.Text = R.NDB.reeds[0].ToString();
        }

        private void one_Unchecked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Remove(0);
            if (netychordsStarted)
            {
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void SelectorRow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ComboBox> boxes = new List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            for (int i = 0; i < 11; i++)
            {
                if (sender == boxes[i] && i != 10)
                {
                    boxes[i + 1].IsEnabled = true;
                    //Rack.DMIBox.arbitraryLines.Add(boxes[i].SelectedItem.ToString());
                    break;
                }
            }
            if (arbitraryStart.IsEnabled == false) arbitraryStart.IsEnabled = true;
        }

        private void SetupComboBox()
        {
            System.Collections.Generic.List<ComboBox> boxes = new System.Collections.Generic.List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            for (int i = 0; i < 11; i++)
            {
                boxes[i].Items.Clear();
                for (int j = 0; j < 13; j++)
                {
                    boxes[i].Items.Add(((ChordType)j).ToString());
                }
            }
        }

        /// <summary>
        /// This gets called when the Start button is pressed
        /// </summary>
        private void StartNetytar(object sender, RoutedEventArgs e)
        {
            if (!netychordsStarted)
            {
                // Launches the Setup class
                NetychordsSetup netychordsSetup = new NetychordsSetup(this);
                netychordsSetup.Setup();

                // Changes the aspect of the Start button
                btnStart.IsEnabled = false;
                btnStart.Foreground = new SolidColorBrush(Colors.Black);
                // Checks the selected MIDI port is available
                CheckMidiPort();
                InitializeSensorPortText();
                sldDistance.Value = R.UserSettings.HorizontalSpacer;
                sldDistanceValue.Text = R.UserSettings.HorizontalSpacer.ToString();
                lstNoteChanger.SelectedIndex = 0;
                two.IsChecked = true;
                three.IsChecked = true;

                // LEAVE AT THE END! This keeps track of the started state
                netychordsStarted = true;

                //LoadSettingsToIndicators();
            }
            else
            {
                canvasNetychords.Children.Clear();
                //R.NDB.AutoScroller = new AutoScroller(R.NDB.MainWindow.scrlNetychords, 0, 100, new PointFilterMAExpDecaying(0.1f));
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, R.NDB.octaveNumber, ChordType.Major);

                R.NDB.NetychordsSurface = new NetychordsSurface(R.NDB.MainWindow.canvasNetychords);

                R.NDB.NetychordsSurface.DrawButtons();
                //canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
                btnStart.IsEnabled = false;
                btnStart.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        //private void LoadSettingsToIndicators()
        //{
        //    string strTC = R.UserSettings.TonalCenter.ToStandardString();
        //    var items = lstTonalCenter.Items;
        //    for (int i = 0; i< items.Count; i++)
        //    {
        //        if(items[i].ToString() == strTC)
        //        {
        //            lstTonalCenter.SelectedIndex = i;
        //        }
        //    }
        //}

        private void TabSolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void three_Checked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Add(2);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void three_Unchecked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Remove(2);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void two_Checked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Add(1);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void two_Unchecked(object sender, RoutedEventArgs e)
        {
            R.NDB.reeds.Remove(1);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                R.NDB.NetychordsSurface.firstChord = MidiChord.StandardAbsStringToChordFactory(R.UserSettings.FirstNote, "2", ChordType.Major);
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        // [Corrente]
        private void UpdateSensorConnection()
        {
            txtSensorPort.Text = "COM" + SensorPort.ToString();

            if (R.NDB.HeadTrackerModule.Connect(SensorPort))
            {
                txtSensorPort.Foreground = ActiveBrush;
            }
            else
            {
                txtSensorPort.Foreground = WarningBrush;
            }
        }

        /// <summary>
        /// This method gets called every millisecond (or something like?) in order to update the
        /// elements of the GUI
        /// </summary>
        private void UpdateWindow(object sender, EventArgs e)
        {
            if (netychordsStarted)
            {
                lblIsPlaying.Text = R.NDB.isPlaying;
                lblPlayedNote.Text = R.NDB.Chord.ChordName();
                lblYaw.Text = R.NDB.HTData.TranspYaw.ToString();
                centerValue.Text = Math.Round(R.NDB.CenterZone, 0).ToString();
                centerPitchValue.Text = Math.Round(centerPitchZone.Value, 0).ToString();

                switch (R.UserSettings.OnlyDiatonic)
                {
                    case true:
                        indOnlyDiatonic.Background = ActiveBrush;
                        break;

                    case false:
                        indOnlyDiatonic.Background = WarningBrush;
                        break;
                }

                switch (R.UserSettings.BlinkPlay)
                {
                    case true:
                        indBlinkPlay.Background = ActiveBrush;
                        break;

                    case false:
                        indBlinkPlay.Background = WarningBrush;
                        break;
                }

                switch (R.UserSettings.KeyboardSustain)
                {
                    case true:
                        indSustain.Background = ActiveBrush;
                        break;

                    case false:
                        indSustain.Background = WarningBrush;
                        break;
                }

                switch (R.UserSettings.AutoStrum)
                {
                    case true:
                        indAutoStrum.Background = ActiveBrush;
                        break;

                    case false:
                        indAutoStrum.Background = WarningBrush;
                        break;
                }

                indAutoStrumValue.Text = R.UserSettings.AutoStrumBPM.ToString();

                R.NDB.NetychordsSurface.UpdateHeadTrackerFeedback(R.NDB.HTData);
            }

            if (clickedButton)
            {
                TimeSpan limit = new TimeSpan(0, 0, 0, 0, 30);
                TimeSpan button = DateTime.Now.Subtract(clicked);
                if (button >= limit)
                {
                    btnMIDIchMinus.IsEnabled = true;
                    btnMIDIchPlus.IsEnabled = true;
                    btnSensorPortMinus.IsEnabled = true;
                    btnSensorPortPlus.IsEnabled = true;
                    clickedButton = false;
                }
            }
        }

        private void sldDistance_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (netychordsStarted)
            {
                R.UserSettings.HorizontalSpacer = (int)sldDistance.Value;
                R.UserSettings.VerticalSpacer = (int)(sldDistance.Value / 2);
                sldDistanceValue.Text = ((int)(sldDistance.Value)).ToString();
                R.NDB.NetychordsSurface.DrawButtons();
            }
        }

        private void lstTonalCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string temp = ((ListBoxItem)lstNoteChanger.SelectedItem).Content.ToString();
            R.UserSettings.TonalCenter = MidiNotesUtils.StandardStringToAbsNote(temp);
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
        }

        private void btnBlinkPlay_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.BlinkPlay = !R.UserSettings.BlinkPlay;
        }

        private void btnSustain_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.KeyboardSustain = !R.UserSettings.KeyboardSustain;
        }

        private void btnAutoStrum_Click(object sender, RoutedEventArgs e)
        {
            R.UserSettings.AutoStrum = !R.UserSettings.AutoStrum;
            switch (R.UserSettings.AutoStrum)
            {
                case true:
                    R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);
                    break;
                case false:
                    R.NDB.StopAutostrum();
                    break;
            }
        }


        private void sldAutoStrum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R.UserSettings.AutoStrumBPM = (int)sldAutoStrum.Value;
        }
    }
}