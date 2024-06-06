using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Netychords.Modules;
using NITHlibrary.Tools.Mappers;

namespace Netychords.Surface
{
    public class HTFeedbackModule
    {
        private const int Bar_horLineThickness = 4;
        private const int Bar_verLineThickness = 4;
        private const double Dead_midLineActiveOpacity = 1f;
        private const int Dead_midLineThickness = 5;
        private const double Half_midLineActiveOpacity = 1f;
        private const int Half_midLineThickness = 5;
        private readonly SolidColorBrush Bar_horLineStroke = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Bar_verLineStroke = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Dead_midLineColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Half_leftRectColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Half_midLineColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Half_rightRectColor = new SolidColorBrush(Colors.White);
        private Line Bar_horLine;
        private Line Bar_verLine;
        private double Bar_verLineHeight = 50;
        private Line Dead_midLine;
        private Rectangle Half_leftRect;
        private Line Half_midLine;
        private ValueMapperDouble Half_rectRangeMapper = new ValueMapperDouble(80, 1);
        private Rectangle Half_rightRect;
        private double Half_VLHeight;
        private HTFeedbackModes mode;

        public HTFeedbackModule(Canvas canvas, HTFeedbackModes mode)
        {
            Canvas = canvas;
            Mode = mode;

            DisposeElements();
            CreateElements();
        }

        public enum HTFeedbackModes
        {
            Bars,
            HalfButton,
            DeadZone,
            None
        }

        public Canvas Canvas { get; set; }

        public HTFeedbackModes Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                DisposeElements();
                CreateElements();
            }
        }

        public void UpdateGraphics(double TrackedValue, NetychordsButton checkedButton)
        {
            switch (Mode)
            {
                case HTFeedbackModes.Bars:
                    Update_Bars(TrackedValue, checkedButton);
                    break;

                case HTFeedbackModes.HalfButton:
                    Update_HalfButton(TrackedValue, checkedButton);
                    break;

                case HTFeedbackModes.DeadZone:
                    Update_DeadZone(TrackedValue, checkedButton);
                    break;

                case HTFeedbackModes.None:
                    break;
            }
        }

        private void CreateElements()
        {
            switch (Mode)
            {
                case HTFeedbackModes.Bars:
                    Bar_horLine = new Line();
                    Bar_horLine.StrokeThickness = Bar_horLineThickness;
                    Bar_horLine.Stroke = Bar_horLineStroke;
                    Panel.SetZIndex(Bar_horLine, 2000);
                    Canvas.Children.Add(Bar_horLine);

                    Bar_verLine = new Line();
                    Bar_verLine.StrokeThickness = Bar_verLineThickness;
                    Bar_verLine.Stroke = Bar_verLineStroke;
                    Panel.SetZIndex(Bar_verLine, 2000);
                    Canvas.Children.Add(Bar_verLine);

                    Half_VLHeight = Bar_verLineHeight / 2;

                    break;

                case HTFeedbackModes.HalfButton:
                    Half_leftRect = new Rectangle();
                    Half_leftRect.Fill = Half_leftRectColor;
                    Half_leftRect.Opacity = 0;
                    Panel.SetZIndex(Half_leftRect, 2000);
                    Canvas.Children.Add(Half_leftRect);

                    Half_rightRect = new Rectangle();
                    Half_rightRect.Fill = Half_rightRectColor;
                    Half_rightRect.Opacity = 0;
                    Panel.SetZIndex(Half_rightRect, 2000);
                    Canvas.Children.Add(Half_rightRect);

                    Half_midLine = new Line();
                    Half_midLine.Stroke = Half_midLineColor;
                    Half_midLine.StrokeThickness = Half_midLineThickness;
                    Half_midLine.Opacity = 0;
                    Panel.SetZIndex(Half_midLine, 2001);
                    Canvas.Children.Add(Half_midLine);

                    break;

                case HTFeedbackModes.DeadZone:
                    Dead_midLine = new Line();
                    Dead_midLine.Stroke = Dead_midLineColor;
                    Dead_midLine.StrokeThickness = Dead_midLineThickness;
                    Dead_midLine.Opacity = 0;
                    Panel.SetZIndex(Dead_midLine, 2001);
                    Canvas.Children.Add(Dead_midLine);
                    break;

                case HTFeedbackModes.None:
                    break;
            }
        }

        private void DisposeElements()
        {
            if (Bar_horLine != null) Canvas.Children.Remove(Bar_horLine);
            if (Bar_verLine != null) Canvas.Children.Remove(Bar_verLine);
            if (Half_leftRect != null) Canvas.Children.Remove(Half_leftRect);
            if (Half_rightRect != null) Canvas.Children.Remove(Half_rightRect);
            if (Half_midLine != null) Canvas.Children.Remove(Half_midLine);
            if (Dead_midLine != null) Canvas.Children.Remove(Dead_midLine);
        }

        #region Update resolvers

        private void Update_Bars(double TrackedValue, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                Bar_horLine.X1 = Canvas.GetLeft(checkedButton.Occluder) + checkedButton.Occluder.ActualWidth / 2;
                Bar_horLine.Y1 = Canvas.GetTop(checkedButton.Occluder) + checkedButton.Occluder.ActualHeight / 2;

                Bar_horLine.X2 = Bar_horLine.X1 + TrackedValue;
                Bar_horLine.Y2 = Bar_horLine.Y1;

                Bar_verLine.X1 = Bar_horLine.X2;
                Bar_verLine.Y1 = Bar_horLine.Y2 - Half_VLHeight;

                Bar_verLine.X2 = Bar_horLine.X2;
                Bar_verLine.Y2 = Bar_horLine.Y2 + Half_VLHeight;

                Canvas.UpdateLayout();
            }
        }

        private void Update_DeadZone(double TrackedValue, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                var Occ = checkedButton.Occluder;

                Dead_midLine.X1 = Canvas.GetLeft(Occ) + Occ.ActualWidth / 2;
                Dead_midLine.Y1 = Canvas.GetTop(Occ);
                Dead_midLine.X2 = Canvas.GetLeft(Occ) + Occ.ActualWidth / 2;
                Dead_midLine.Y2 = Canvas.GetTop(Occ) + Occ.ActualHeight / 2;

                if (Rack.MappingModule.InDeadZone)
                {
                    Dead_midLine.Opacity = Dead_midLineActiveOpacity;
                }
                else
                {
                    Dead_midLine.Opacity = 0;
                }
            }
        }

        private void Update_HalfButton(double TrackedValue, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                var Occ = checkedButton.Occluder;

                Half_leftRect.Height = Occ.ActualHeight;
                Half_leftRect.Width = Occ.ActualWidth / 2;

                Half_rightRect.Height = Occ.ActualHeight;
                Half_rightRect.Width = Occ.ActualWidth / 2;

                Half_midLine.X1 = Canvas.GetLeft(Occ) + Occ.ActualWidth / 2;
                Half_midLine.Y1 = Canvas.GetTop(Occ);
                Half_midLine.X2 = Canvas.GetLeft(Occ) + Occ.ActualWidth / 2;
                Half_midLine.Y2 = Canvas.GetTop(Occ) + Occ.ActualHeight / 2;

                Canvas.SetLeft(Half_leftRect, Canvas.GetLeft(Occ));
                Canvas.SetLeft(Half_rightRect, Canvas.GetLeft(Occ) + Occ.ActualWidth / 2);
                Canvas.SetTop(Half_leftRect, Canvas.GetTop(Occ));
                Canvas.SetTop(Half_rightRect, Canvas.GetTop(Occ));

                double posPart = 0;
                double negPart = 0;

                if (TrackedValue >= 0)
                {
                    posPart = TrackedValue;
                    negPart = 0;
                }
                else
                {
                    negPart = -TrackedValue;
                    posPart = 0;
                }

                if (Rack.MappingModule.InDeadZone)
                {
                    Half_midLine.Opacity = Half_midLineActiveOpacity;
                }
                else
                {
                    Half_midLine.Opacity = 0;
                }

                Half_leftRect.Opacity = Half_rectRangeMapper.Map(negPart);
                Half_rightRect.Opacity = Half_rectRangeMapper.Map(posPart);
            }
        }

        #endregion Update resolvers
    }
}