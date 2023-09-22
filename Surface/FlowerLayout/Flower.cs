using NITHdmis.Music;
using Netychords.Utils;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Netychords.Surface.FlowerLayout
{
    public class Flower
    {
        #region Buttons

        public FlowerButton ButtonC { get; set; }
        public FlowerButton ButtonD { get; set; }
        public FlowerButton ButtonL { get; set; }
        public FlowerButton ButtonR { get; set; }
        public FlowerButton ButtonU { get; set; }
        public List<FlowerButton> FlowerButtons { get; private set; }

        private const int LineStrokeThickness = 20;

        #endregion Buttons

        private Line horLine;
        private Line verLine;
        public void DrawLines(Canvas canvas)
        {
            horLine = new Line();
            verLine = new Line();

            horLine.X1 = Canvas.GetLeft(ButtonL) + ButtonL.Occluder.ActualWidth / 2 + LineStrokeThickness / 2;
            horLine.Y1 = Canvas.GetTop(ButtonL) + ButtonL.Occluder.ActualHeight / 2 + LineStrokeThickness / 2;
            horLine.X2 = Canvas.GetLeft(ButtonR) + ButtonL.Occluder.ActualWidth / 2 + LineStrokeThickness / 2;
            horLine.Y2 = Canvas.GetTop(ButtonR) + ButtonL.Occluder.ActualHeight / 2 + LineStrokeThickness / 2;

            verLine.X1 = Canvas.GetLeft(ButtonU) + ButtonU.Occluder.ActualWidth / 2 + LineStrokeThickness / 2;
            verLine.Y1 = Canvas.GetTop(ButtonU) + ButtonU.Occluder.ActualHeight / 2 + LineStrokeThickness / 2;
            verLine.X2 = Canvas.GetLeft(ButtonD) + ButtonD.Occluder.ActualWidth / 2 + LineStrokeThickness / 2;
            verLine.Y2 = Canvas.GetTop(ButtonD) + ButtonD.Occluder.ActualHeight / 2 + LineStrokeThickness / 2;

            horLine.Stroke = new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.C));
            verLine.Stroke = new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.C));

            horLine.StrokeThickness = LineStrokeThickness;
            verLine.StrokeThickness = LineStrokeThickness;

            canvas.Children.Add(verLine);
            canvas.Children.Add(horLine);
        }

        public FlowerFamilies FlowerFamily { get; private set; }

        public MidiNotes RootNote { get; private set; }

        public Flower(MidiNotes rootNote, FlowerConfig config, Point center)
        {
            this.RootNote = rootNote;
            FlowerFamily = config.Family;

            MidiChord ChordC = new MidiChord(RootNote, config.ChordType_C);
            MidiChord ChordL = new MidiChord(RootNote, config.ChordType_L);
            MidiChord ChordU = new MidiChord(RootNote, config.ChordType_U);
            MidiChord ChordR = new MidiChord(RootNote, config.ChordType_R);
            MidiChord ChordD = new MidiChord(RootNote, config.ChordType_D);

            Point CoordC = new Point(center.X, center.Y);
            Point CoordL = new Point(center.X - 1, center.Y);
            Point CoordU = new Point(center.X, center.Y + 1);
            Point CoordR = new Point(center.X + 1, center.Y);
            Point CoordD = new Point(center.X, center.Y - 1);

            ButtonC = new FlowerButton(CoordC);
            ButtonC.SetColor(new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.C)));
            ButtonL = new FlowerButton(CoordL);
            ButtonL.SetColor(new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.L)));
            ButtonU = new FlowerButton(CoordU);
            ButtonU.SetColor(new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.U)));
            ButtonR = new FlowerButton(CoordR);
            ButtonR.SetColor(new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.R)));
            ButtonD = new FlowerButton(CoordD);
            ButtonD.SetColor(new SolidColorBrush(FlowerFamily.GetColor(FlowerButtonPositions.D)));

            ButtonC.Chord = ChordC;
            ButtonL.Chord = ChordL;
            ButtonU.Chord = ChordU;
            ButtonR.Chord = ChordR;
            ButtonD.Chord = ChordD;

            FlowerButtons = new List<FlowerButton>
            {
                ButtonC,
                ButtonD,
                ButtonL,
                ButtonU,
                ButtonR
            };
        }
    }
}