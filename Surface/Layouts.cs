using NITHdmis.Music;
using Netychords.Surface.FlowerLayout;
using Netychords.Utils;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Netychords.Modules;

namespace Netychords.Surface
{
    public enum Layouts
    {
        FifthCircle,
        Custom,
        Stradella,
        Flower
    }

    public static class LayoutsMethods
    {
        private const double BACKGROUNDLINE_OPACITY = 0.25f;
        private static int buttonHeight;
        private static int buttonWidth;
        private static int horizontalSpacer;
        private static int nCols;
        private static int nRows;
        private static int occluderAlpha;
        private static int occluderOffset;
        private static int startPositionX;
        private static int startPositionY;
        private static int verticalSpacer;

        public static void Draw(this Layouts layout, MidiChord firstChord, Canvas canvas, NetychordsButton[,] NetychordsButtons)
        {
            canvas.Children.Clear();
            LoadSettings();

            switch (layout)
            {
                case Layouts.FifthCircle:
                    DrawFifthCircle(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Custom:
                    DrawCustom(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Stradella:
                    DrawStradella(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Flower:
                    DrawFlower(firstChord, canvas, NetychordsButtons);
                    break;
            }
            ResetCanvasDimensions(canvas);
            Rack.AutoScroller.ScrollTo(Rack.UserSettings.StartPositionX, Rack.UserSettings.StartPositionY);
        }

        private static void ResetCanvasDimensions(Canvas canvas)
        {
            canvas.Width = startPositionX * 2 + (horizontalSpacer + 13) * (nCols - 1);
            canvas.Height = startPositionY * 2 + (Math.Abs(verticalSpacer) + 13) * (nRows - 1);
        }

        /// <summary>
        /// Deprecated!
        /// </summary>
        /// <param name="firstChord">        </param>
        /// <param name="canvas">            </param>
        /// <param name="netychordsButtons"> </param>

        private static void DrawCustom(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            firstChord = firstChord.GeneratePreviousFifth(); // First note fix

            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // INIZIALIZZAZIONE NUMERO RIGHE =====================
            nRows = Rack.MappingModule.CustomLines.Count;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if (Rack.UserSettings.Margins == MarginModes.Slant)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

                        if (row % 2 != 0)
                        {
                            isPairRow = false;
                        }
                        else
                        {
                            isPairRow = true;
                        }
                    }
                    else
                    {
                        spacer = verticalSpacer;
                        firstSpacer = 0;
                        isPairRow = true;
                    }

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.Surface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    if (Rack.MappingModule.CustomLines.Count != 0)
                    {
                        ChordType type = Rack.MappingModule.CustomLines[0 + row];

                        switch (type)
                        {
                            case ChordType.Sus2:
                                thisChordType = ChordType.Sus2;
                                firstSpacer = 0;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Sus4:
                                thisChordType = ChordType.Sus4;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Dim7th:
                                thisChordType = ChordType.Dim7th;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Maj:
                                thisChordType = ChordType.Maj;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Maj6th:
                                thisChordType = ChordType.Maj6th;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Dom7th:
                                thisChordType = ChordType.Dom7th;
                                if (firstChord.chordType != ChordType.Dom7th)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Dom7th);
                                    firstChord.chordType = ChordType.Dom7th;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Min:
                                thisChordType = ChordType.Min;
                                if (firstChord.chordType != ChordType.Min)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Min);
                                    firstChord.chordType = ChordType.Min;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Min6th:
                                thisChordType = ChordType.Min6th;
                                if (firstChord.chordType != ChordType.Min6th)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Min6th);
                                    firstChord.chordType = ChordType.Min6th;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Dom9th:
                                thisChordType = ChordType.Dom9th;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Maj7th:
                                thisChordType = ChordType.Maj7th;
                                if (firstChord.chordType != ChordType.Maj7th)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.Maj7th);
                                    firstChord.chordType = ChordType.Maj7th;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Dom11th:
                                thisChordType = ChordType.Dom11th;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.Min7th:
                                thisChordType = ChordType.Min7th;
                                if (firstChord.chordType != ChordType.Min7th)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.Min7th);
                                    firstChord.chordType = ChordType.Min7th;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case ChordType.SemiDim:
                                thisChordType = ChordType.SemiDim;
                                if (firstChord.chordType != ChordType.SemiDim)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDim);
                                    firstChord.chordType = ChordType.SemiDim;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            default:
                                thisChordType = ChordType.Maj;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                netychordsButtons[row, col].Chord = actualChord;
                                break;
                        }
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    int X = startPositionX + firstSpacer + col * spacer;
                    int Y = startPositionY + verticalSpacer * row;

                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight;
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    SetButtonColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }

                backgroundLine.X1 = Canvas.GetLeft(netychordsButtons[row, 0]) + 7;
                backgroundLine.X2 = Canvas.GetLeft(netychordsButtons[row, nCols - 1]) + 7;
                backgroundLine.Y1 = Canvas.GetTop(netychordsButtons[row, 0]) + 7;
                backgroundLine.Y2 = Canvas.GetTop(netychordsButtons[row, nCols - 1]) + 7;
                SetChordLineColor(backgroundLine, actualChord);
                backgroundLine.Opacity = BACKGROUNDLINE_OPACITY;
                backgroundLine.StrokeThickness = 50;
                canvas.Children.Add(backgroundLine);
            }
        }

        private static void DrawFifthCircle(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            firstChord = firstChord.GeneratePreviousFifth(); // First note fix

            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if (Rack.UserSettings.Margins == MarginModes.Slant)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

                        if (row % 2 != 0)
                        {
                            isPairRow = false;
                        }
                        else
                        {
                            isPairRow = true;
                        }
                    }
                    else
                    {
                        spacer = verticalSpacer;
                        firstSpacer = 0;
                        isPairRow = true;
                    }

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.Surface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.Dim7th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.Dom11th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
                            thisChordType = ChordType.Dom9th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 3:
                            thisChordType = ChordType.Sus2;
                            firstSpacer = 0;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 4:
                            thisChordType = ChordType.Sus4;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 5:
                            thisChordType = ChordType.Dom7th;
                            if (firstChord.chordType != ChordType.Dom7th)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 5, ChordType.Dom7th);
                                firstChord.chordType = ChordType.Dom7th;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 6:
                            thisChordType = ChordType.Maj;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 7:
                            thisChordType = ChordType.Min;
                            if (firstChord.chordType != ChordType.Min)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Min);
                                firstChord.chordType = ChordType.Min;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 8:
                            thisChordType = ChordType.SemiDim;
                            if (firstChord.chordType != ChordType.SemiDim)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDim);
                                firstChord.chordType = ChordType.SemiDim;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 9:
                            thisChordType = ChordType.Maj7th;
                            if (firstChord.chordType != ChordType.Maj7th)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.Maj7th);
                                firstChord.chordType = ChordType.Maj7th;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 10:
                            thisChordType = ChordType.Min7th;
                            if (firstChord.chordType != ChordType.Min7th)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Min7th);
                                firstChord.chordType = ChordType.Min7th;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.GenerateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (!isPairRow)
                    {
                        firstSpacer = 0;
                    }
                    else
                    {
                        firstSpacer = spacer / 2;
                    }

                    int X = startPositionX + firstSpacer + col * spacer;
                    int Y = startPositionY + verticalSpacer * row;

                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight;
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    SetButtonColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }

                backgroundLine.X1 = Canvas.GetLeft(netychordsButtons[row, 0]) + 7;
                backgroundLine.X2 = Canvas.GetLeft(netychordsButtons[row, nCols - 1]) + 7;
                backgroundLine.Y1 = Canvas.GetTop(netychordsButtons[row, 0]) + 7;
                backgroundLine.Y2 = Canvas.GetTop(netychordsButtons[row, nCols - 1]) + 7;
                SetChordLineColor(backgroundLine, actualChord);
                backgroundLine.Opacity = BACKGROUNDLINE_OPACITY;
                backgroundLine.StrokeThickness = 50;
                canvas.Children.Add(backgroundLine);
            }
        }

        private static void DrawFlower(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            FlowerButton.DimButton = buttonWidth;
            FlowerButton.DimOccluder = buttonWidth;

            System.Drawing.Point center = new System.Drawing.Point(6, 4);
            FlowerGridDimensions gridDim = new FlowerGridDimensions(82, 82);

            Plant plant = new Plant(firstChord.rootNote, PlantFamilies.Major, center);

            foreach (Flower flower in plant.Flowers)
            {
                foreach (FlowerButton flowerButton in flower.FlowerButtons)
                {
                    Canvas.SetLeft(flowerButton, flowerButton.Coordinates.X * gridDim.X + center.X);
                    Canvas.SetTop(flowerButton, flowerButton.Coordinates.Y * gridDim.Y + center.Y);

                    Canvas.SetLeft(flowerButton.Occluder, Canvas.GetLeft(flowerButton) - occluderOffset);
                    Canvas.SetTop(flowerButton.Occluder, Canvas.GetTop(flowerButton) - occluderOffset);

                    canvas.Children.Add(flowerButton);
                    canvas.Children.Add(flowerButton.Occluder);
                }
                flower.DrawLines(canvas);
            }
        }

        private static void DrawStradella(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            firstChord = firstChord.GeneratePreviousFifth(); // First note fix

            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // CICLO PRINCIPALE =====================
            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if (Rack.UserSettings.Margins == MarginModes.Slant)
                    {
                        spacer = 100;
                        firstSpacer = row * spacer / 4;

                        if (row % 2 != 0)
                        {
                            isPairRow = false;
                        }
                        else
                        {
                            isPairRow = true;
                        }
                    }
                    else
                    {
                        spacer = verticalSpacer;
                        firstSpacer = 0;
                        isPairRow = true;
                        //canvas.Height = startPositionY * 2 + (verticalSpacer + 13) * (nRows - 1);
                    }

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.Surface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.SemiDim;

                            firstSpacer = 0;
                            if (firstChord.chordType != ChordType.SemiDim)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.SemiDim);
                                firstChord.chordType = ChordType.SemiDim;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote + 7;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote - 5;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.Sus2;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
                            thisChordType = ChordType.Sus4;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 3:
                            thisChordType = ChordType.Dim7th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote + 7;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote - 5;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 4:
                            thisChordType = ChordType.Maj;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 5:
                            thisChordType = ChordType.Dom7th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 6:
                            thisChordType = ChordType.Min;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 7:
                            thisChordType = ChordType.Dom9th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 8:
                            thisChordType = ChordType.Maj7th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 9:
                            thisChordType = ChordType.Dom11th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 10:
                            thisChordType = ChordType.Min7th;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;
                        /*case 10:
                            thisChordType = ChordType.Augmented;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                            }
                            else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }
                            else
                            {
                                thisNote = actualChord.rootNote + 7;
                            };
                            actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;*/
                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    int X = startPositionX + firstSpacer + col * spacer;
                    int Y = startPositionY + verticalSpacer * row;

                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    SetButtonColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }

                backgroundLine.X1 = Canvas.GetLeft(netychordsButtons[row, 0]) + 7;
                backgroundLine.X2 = Canvas.GetLeft(netychordsButtons[row, nCols - 1]) + 7;
                backgroundLine.Y1 = Canvas.GetTop(netychordsButtons[row, 0]) + 7;
                backgroundLine.Y2 = Canvas.GetTop(netychordsButtons[row, nCols - 1]) + 7;
                SetChordLineColor(backgroundLine, actualChord);
                backgroundLine.Opacity = BACKGROUNDLINE_OPACITY;
                backgroundLine.StrokeThickness = 50;
                canvas.Children.Add(backgroundLine);
            }
        }

        private static void LoadSettings()
        {
            horizontalSpacer = Rack.UserSettings.HorizontalSpacer;
            verticalSpacer = Rack.UserSettings.VerticalSpacer;
            startPositionY = Rack.UserSettings.StartPositionY;
            startPositionX = Rack.UserSettings.StartPositionX;
            nRows = Rack.UserSettings.NRows;
            nCols = Rack.UserSettings.NCols;
            occluderOffset = Rack.UserSettings.OccluderOffset;
            buttonWidth = Rack.UserSettings.ButtonWidth;
            buttonHeight = Rack.UserSettings.ButtonHeight;
            occluderAlpha = Rack.UserSettings.OccluderAlpha;

            //startPositionY = startPositionY + Math.Abs(verticalSpacer) * nRows; // FIX TEMPORANEO
        }

        private static void SetButtonColor(NetychordsButton button, MidiChord actualChord)
        {
            string n = actualChord.rootNote.ToStandardString();
            button.Occluder.Fill = Rack.GetNoteColor(actualChord.rootNote.ToAbsNote());
        }

        private static void SetChordLineColor(Line background, MidiChord actualChord)
        {
            ChordType n = actualChord.chordType;
            switch (n)
            {
                case ChordType.Maj:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Maj7th:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Maj6th:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Sus2:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Sus4:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Dom9th:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Dom11th:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Min:
                    background.Stroke = new SolidColorBrush(Colors.Blue);
                    break;

                case ChordType.Min7th:
                    background.Stroke = new SolidColorBrush(Colors.Blue);
                    break;

                case ChordType.Dom7th:
                    background.Stroke = new SolidColorBrush(Colors.Green);
                    break;

                default:
                    background.Stroke = new SolidColorBrush(Colors.LightGray);
                    break;
            }
        }
    }
}