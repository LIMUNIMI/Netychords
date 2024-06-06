using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Netychords.Modules;
using Netychords.Utils;
using NITHdmis.Music;
using NITHlibrary.Tools.Types;

namespace Netychords.Surface
{
    public enum NetychordsSurfaceDrawModes
    {
        AllLines,
        OnlyScaleLines,
        NoLines
    }

    public enum NetychordsSurfaceHighlightModes
    {
        CurrentNote,
        None
    }

    public class NetychordsSurface
    {
        public Ellipse highlighter = new Ellipse();
        private NetychordsButton checkedButton;
        private NetychordsSurfaceDrawModes drawMode;
        private NetychordsButton lastCheckedButton;

        public NetychordsSurface(Canvas canvas)
        {
            NetychordsButtons = new NetychordsButton[Rack.UserSettings.NRows, Rack.UserSettings.NCols];

            this.Canvas = canvas;

            DrawHighlighter();
        }

        public NetychordsButton CheckedButton { get => checkedButton; }

        public NetychordsSurfaceDrawModes DrawMode { get => drawMode; set => drawMode = value; }

        public NetychordsSurfaceHighlightModes HighLightMode { get; set; } = NetychordsSurfaceHighlightModes.CurrentNote;

        public HTFeedbackModule HtFeedbackModule { get; set; }

        public void DrawButtons()
        {
            // CHIAMATA AL LAYOUTDRAWER
            FirstChord = MidiChord.StandardAbsStringToChordFactory(Rack.UserSettings.FirstRoot.ToStandardString(), "2", ChordType.Maj);
            NetychordsButtons = new NetychordsButton[Rack.UserSettings.NRows, Rack.UserSettings.NCols];
            Rack.UserSettings.Layout.Draw(FirstChord, Canvas, NetychordsButtons);
            DrawHighlighter();
            HtFeedbackModule.Mode = HTFeedbackModule.HTFeedbackModes.Bars;
        }

        public void FlashMovementLine()
        {
            if (lastCheckedButton != null)
            {
                Point point1 = new Point(Canvas.GetLeft(CheckedButton) + 6, Canvas.GetTop(CheckedButton) + 6);
                Point point2 = new Point(Canvas.GetLeft(lastCheckedButton) + 6, Canvas.GetTop(lastCheckedButton) + 6);
                IndependentLineFlashTimer timer = new IndependentLineFlashTimer(point1, point2, Canvas, Colors.NavajoWhite);
            }
        }

        public void NetychordsButton_OccluderMouseEnter(NetychordsButton sender)
        {
            if (sender != CheckedButton)
            {
                Rack.MappingModule.LastChord = Rack.MappingModule.Chord;
                Rack.MappingModule.KeyNodeChange(sender.Chord);

                lastCheckedButton = checkedButton;
                checkedButton = sender;

                FlashMovementLine();

                if (HighLightMode == NetychordsSurfaceHighlightModes.CurrentNote)
                {
                    MoveHighlighter(CheckedButton);
                }
            }
        }

        public void UpdateHeadTrackerFeedback(Polar3DData position, Polar3DData acceleration, HeadTrackerModes headTrackerMode)
        {
            switch (headTrackerMode)
            {
                case HeadTrackerModes.Absolute:
                    HtFeedbackModule.UpdateGraphics(position.Yaw, checkedButton);
                    break;

                case HeadTrackerModes.Acceleration:
                    HtFeedbackModule.UpdateGraphics(Rack.MappingModule.Velocity * Math.Sign(acceleration.Yaw) * UPDATERACCMULTIPLIER, checkedButton);
                    break;
            }
        }

        private void DrawHighlighter()
        {
            highlighter = new Ellipse();
            highlighter.Width = Rack.UserSettings.ButtonWidth + 10;
            highlighter.Height = Rack.UserSettings.ButtonHeight + 10;
            highlighter.StrokeThickness = Rack.UserSettings.HighlightStrokeThickness;
            highlighter.Stroke = new SolidColorBrush(Colors.White);
            highlighter.Fill = new SolidColorBrush(Colors.Transparent);
            highlighter.HorizontalAlignment = HorizontalAlignment.Center;
            highlighter.VerticalAlignment = VerticalAlignment.Center;
            Panel.SetZIndex(highlighter, 10);
            Canvas.Children.Add(highlighter);
        }

        #region Settings

        public MidiChord FirstChord;

        private MidiChord actualChord;

        private List<Color> keysColorCode = new List<Color>()
        {
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.LightGreen,
            Colors.Blue,
            Colors.Purple,
            Colors.Coral
        };

        private SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);

        #endregion Settings

        #region Surface components

        public NetychordsButton[,] NetychordsButtons { get; set; }
        private readonly double UPDATERACCMULTIPLIER = 2f; // OLD 80
        private List<Ellipse> drawnEllipses = new List<Ellipse>();
        private List<Line> drawnLines = new List<Line>();
        public Canvas Canvas { get; set; }

        #endregion Surface components

        private void DisposeImage(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Remove(((Image)sender));
        }

        private void MoveHighlighter(NetychordsButton checkedButton)
        {
            Canvas.SetLeft(highlighter, Canvas.GetLeft(checkedButton.Occluder) - 5);
            Canvas.SetTop(highlighter, Canvas.GetTop(checkedButton.Occluder) - 5);
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="button"></param>
        private void NoteToColor(NetychordsButton button)
        {
            string n = actualChord.rootNote.ToStandardString();
            switch (n.Remove(n.Length - 1))
            {
                case "C":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x00, 0x00));
                    break;

                case "C#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xa9, 0x8a, 0x4d));
                    break;

                case "D":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xA5, 0x00));
                    break;

                case "D#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xD7, 0x00));//
                    break;

                case "E":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0x00));
                    break;

                case "F":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x90, 0xEE, 0x90));//
                    break;

                case "F#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0x00));
                    break;

                case "G":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0xFF));
                    break;

                case "G#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x00, 0xFF));
                    break;

                case "A":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0x00, 0xFF));
                    break;

                case "A#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xC0, 0xCB));//
                    break;

                case "B":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x6F, 0x00, 0xFF));
                    break;

                default:
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0xFF));
                    break;
            }
        }
    }
}