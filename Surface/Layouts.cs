using NeeqDMIs.Music;
using Netychords.Surface.FlowerLayout;
using Netychords.Utils;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Netychords.Surface
{
    public enum Layouts
    {
        FifthCircle,
        Arbitrary,
        Stradella,
        Jazz,
        Pop,
        Rock,
        OnlyMajor,
        Diatonic_3,
        Diatonic_4,
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

                case Layouts.Arbitrary:
                    DrawArbitrary(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Stradella:
                    DrawStradella(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Jazz:
                    DrawJazz(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Pop:
                    DrawPop(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Rock:
                    DrawRock(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Flower:
                    DrawFlower(firstChord, canvas, NetychordsButtons);
                    break;
                case Layouts.OnlyMajor:
                    DrawOnlyMajor(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Diatonic_3:
                    DrawDiatonic_3(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Diatonic_4:
                    DrawDiatonic_4(firstChord, canvas, NetychordsButtons);
                    break;
            }
            ResetCanvasDimensions(canvas);
            R.NDB.AutoScroller.ScrollTo(R.UserSettings.StartPositionX, R.UserSettings.StartPositionY);
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
        private static void Draw_Old(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            if (R.NDB.arbitraryLines.Count != 0)
            {
                nRows = R.NDB.arbitraryLines.Count;
            }
            else
            {
                if (R.UserSettings.Layout == Layouts.Stradella || R.UserSettings.Layout == Layouts.FifthCircle)
                {
                    nRows = 11;
                }
                else if (R.UserSettings.Layout == Layouts.Jazz)
                {
                    nRows = 7;
                }
                else if (R.UserSettings.Layout == Layouts.Pop)
                {
                    nRows = 4;
                }
                else if (R.UserSettings.Layout == Layouts.Rock)
                {
                    nRows = 5;
                }
                else
                {
                    nRows = 11;
                }
            }

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
                    {
                        if (R.UserSettings.Layout == Layouts.Stradella)
                        {
                            spacer = 100;
                            firstSpacer = row * spacer / 4;
                        }
                        else
                        {
                            spacer = horizontalSpacer;
                            firstSpacer = row * spacer / 2;
                        }

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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    if (R.UserSettings.Layout == Layouts.Arbitrary && R.NDB.arbitraryLines.Count != 0)
                    {
                        string type = R.NDB.arbitraryLines[R.NDB.arbitraryLines.Count - row - 1];

                        switch (type)
                        {
                            case "Sus2":
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

                            case "Sus4":
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

                            case "DiminishedSeventh":
                                thisChordType = ChordType.DiminishedSeventh;
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

                            case "Major":
                                thisChordType = ChordType.Major;
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

                            case "MajorSixth":
                                thisChordType = ChordType.MajorSixth;
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

                            case "DominantSeventh":
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;
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

                            case "Minor":
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
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

                            case "MinorSixth":
                                thisChordType = ChordType.MinorSixth;
                                if (firstChord.chordType != ChordType.MinorSixth)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                    firstChord.chordType = ChordType.MinorSixth;
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

                            case "DominantNinth":
                                thisChordType = ChordType.DominantNinth;
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

                            case "MajorSeventh":
                                thisChordType = ChordType.MajorSeventh;
                                if (firstChord.chordType != ChordType.MajorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                    firstChord.chordType = ChordType.MajorSeventh;
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

                            case "DominantEleventh":
                                thisChordType = ChordType.DominantEleventh;
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

                            case "MinorSeventh":
                                thisChordType = ChordType.MinorSeventh;
                                if (firstChord.chordType != ChordType.MinorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                    firstChord.chordType = ChordType.MinorSeventh;
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

                            case "SemiDiminished":
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                thisChordType = ChordType.Major;
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
                    else if (R.UserSettings.Layout == Layouts.Pop)
                    {
                        switch (row)
                        {
                            case 0:
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

                            case 1:
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

                            case 2:
                                thisChordType = ChordType.Major;
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
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
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
                                break;
                        }
                    }
                    else if (R.UserSettings.Layout == Layouts.Rock)
                    {
                        switch (row)
                        {
                            case 0:
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

                            case 1:
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

                            case 2:
                                thisChordType = ChordType.Major;
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
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
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

                            case 4:
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;
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
                                break;
                        }
                    }
                    else if (R.UserSettings.Layout == Layouts.Jazz)
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                thisChordType = ChordType.DiminishedSeventh;
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

                            case 2:
                                thisChordType = ChordType.MajorSixth;
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
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;
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

                            case 4:
                                thisChordType = ChordType.MinorSixth;
                                if (firstChord.chordType != ChordType.MinorSixth)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                    firstChord.chordType = ChordType.MinorSixth;
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

                            case 5:
                                thisChordType = ChordType.MajorSeventh;
                                if (firstChord.chordType != ChordType.MajorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                    firstChord.chordType = ChordType.MajorSeventh;
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

                            case 6:
                                thisChordType = ChordType.MinorSeventh;
                                if (firstChord.chordType != ChordType.MinorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                    firstChord.chordType = ChordType.MinorSeventh;
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

                            default:
                                break;
                        }
                    }
                    else if (R.UserSettings.Layout != Layouts.Stradella)
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                thisChordType = ChordType.DiminishedSeventh;
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
                                thisChordType = ChordType.Major;
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
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;
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

                            case 6:
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
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

                            case 7:
                                thisChordType = ChordType.DominantNinth;
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
                                thisChordType = ChordType.MajorSeventh;
                                if (firstChord.chordType != ChordType.MajorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                    firstChord.chordType = ChordType.MajorSeventh;
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

                            case 9:
                                thisChordType = ChordType.DominantEleventh;
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
                                thisChordType = ChordType.MinorSeventh;
                                if (firstChord.chordType != ChordType.MinorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                    firstChord.chordType = ChordType.MinorSeventh;
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

                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.SemiDiminished;

                                firstSpacer = 0;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                thisChordType = ChordType.DiminishedSeventh;
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
                                thisChordType = ChordType.Major;
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
                                thisChordType = ChordType.DominantSeventh;
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
                                thisChordType = ChordType.Minor;
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
                                thisChordType = ChordType.DominantNinth;
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
                                thisChordType = ChordType.MajorSeventh;
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
                                thisChordType = ChordType.DominantEleventh;
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
                                thisChordType = ChordType.MinorSeventh;
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
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;*/
                            default:
                                break;
                        }
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (R.UserSettings.Layout != Layouts.Stradella)
                    {
                        if (!isPairRow)
                        {
                            firstSpacer = 0;
                        }
                        else
                        {
                            firstSpacer = spacer / 2;
                        }
                    }

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
            }
        }

        private static void DrawArbitrary(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // INIZIALIZZAZIONE NUMERO RIGHE =====================
            if (R.NDB.arbitraryLines.Count != 0)
            {
                nRows = R.NDB.arbitraryLines.Count;
            }

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    if (R.NDB.arbitraryLines.Count != 0)
                    {
                        string type = R.NDB.arbitraryLines[R.NDB.arbitraryLines.Count - row - 1];

                        switch (type)
                        {
                            case "Sus2":
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

                            case "Sus4":
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

                            case "DiminishedSeventh":
                                thisChordType = ChordType.DiminishedSeventh;
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

                            case "Major":
                                thisChordType = ChordType.Major;
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

                            case "MajorSixth":
                                thisChordType = ChordType.MajorSixth;
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

                            case "DominantSeventh":
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;
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

                            case "Minor":
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
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

                            case "MinorSixth":
                                thisChordType = ChordType.MinorSixth;
                                if (firstChord.chordType != ChordType.MinorSixth)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                    firstChord.chordType = ChordType.MinorSixth;
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

                            case "DominantNinth":
                                thisChordType = ChordType.DominantNinth;
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

                            case "MajorSeventh":
                                thisChordType = ChordType.MajorSeventh;
                                if (firstChord.chordType != ChordType.MajorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                    firstChord.chordType = ChordType.MajorSeventh;
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

                            case "DominantEleventh":
                                thisChordType = ChordType.DominantEleventh;
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

                            case "MinorSeventh":
                                thisChordType = ChordType.MinorSeventh;
                                if (firstChord.chordType != ChordType.MinorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                    firstChord.chordType = ChordType.MinorSeventh;
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

                            case "SemiDiminished":
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                thisChordType = ChordType.Major;
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
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 11;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.DiminishedSeventh;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.DominantEleventh;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
                            thisChordType = ChordType.DominantNinth;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
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
                                actualChord = actualChord.generateNextFifth();
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
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 5:
                            thisChordType = ChordType.DominantSeventh;
                            if (firstChord.chordType != ChordType.DominantSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 5, ChordType.DominantSeventh);
                                firstChord.chordType = ChordType.DominantSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 6:
                            thisChordType = ChordType.Major;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 7:
                            thisChordType = ChordType.Minor;
                            if (firstChord.chordType != ChordType.Minor)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                firstChord.chordType = ChordType.Minor;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 8:
                            thisChordType = ChordType.SemiDiminished;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 9:
                            thisChordType = ChordType.MajorSeventh;
                            if (firstChord.chordType != ChordType.MajorSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                firstChord.chordType = ChordType.MajorSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 10:
                            thisChordType = ChordType.MinorSeventh;
                            if (firstChord.chordType != ChordType.MinorSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSeventh);
                                firstChord.chordType = ChordType.MinorSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
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

        private static void DrawJazz(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================

            nRows = 7;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.SemiDiminished;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
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
                            thisChordType = ChordType.DiminishedSeventh;
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

                        case 2:
                            thisChordType = ChordType.MajorSixth;
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
                            thisChordType = ChordType.DominantSeventh;
                            if (firstChord.chordType != ChordType.DominantSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                firstChord.chordType = ChordType.DominantSeventh;
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

                        case 4:
                            thisChordType = ChordType.MinorSixth;
                            if (firstChord.chordType != ChordType.MinorSixth)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                firstChord.chordType = ChordType.MinorSixth;
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

                        case 5:
                            thisChordType = ChordType.MajorSeventh;
                            if (firstChord.chordType != ChordType.MajorSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                firstChord.chordType = ChordType.MajorSeventh;
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

                        case 6:
                            thisChordType = ChordType.MinorSeventh;
                            if (firstChord.chordType != ChordType.MinorSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                firstChord.chordType = ChordType.MinorSeventh;
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

        private static void DrawPop(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 4;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();

                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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
                        spacer = verticalSpacer; //90;
                        firstSpacer = 0;
                        isPairRow = true;
                    }

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
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

                        case 1:
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

                        case 2:
                            thisChordType = ChordType.Major;
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
                            thisChordType = ChordType.Minor;
                            if (firstChord.chordType != ChordType.Minor)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                firstChord.chordType = ChordType.Minor;
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

        private static void DrawRock(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================

            nRows = 5;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
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

                        case 1:
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

                        case 2:
                            thisChordType = ChordType.Major;
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
                            thisChordType = ChordType.Minor;
                            if (firstChord.chordType != ChordType.Minor)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                firstChord.chordType = ChordType.Minor;
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

        private static void DrawStradella(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 11;

            // CICLO PRINCIPALE =====================
            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.SemiDiminished;

                            firstSpacer = 0;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
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
                            thisChordType = ChordType.DiminishedSeventh;
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
                            thisChordType = ChordType.Major;
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
                            thisChordType = ChordType.DominantSeventh;
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
                            thisChordType = ChordType.Minor;
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
                            thisChordType = ChordType.DominantNinth;
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
                            thisChordType = ChordType.MajorSeventh;
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
                            thisChordType = ChordType.DominantEleventh;
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
                            thisChordType = ChordType.MinorSeventh;
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
            horizontalSpacer = R.UserSettings.HorizontalSpacer;
            verticalSpacer = R.UserSettings.VerticalSpacer;
            startPositionY = R.UserSettings.StartPositionY;
            startPositionX = R.UserSettings.StartPositionX;
            nRows = R.UserSettings.NRows;
            nCols = R.UserSettings.NCols;
            occluderOffset = R.UserSettings.OccluderOffset;
            buttonWidth = R.UserSettings.ButtonWidth;
            buttonHeight = R.UserSettings.ButtonHeight;
            occluderAlpha = R.UserSettings.OccluderAlpha;

            //startPositionY = startPositionY + Math.Abs(verticalSpacer) * nRows; // FIX TEMPORANEO
        }

        private static void SetButtonColor(NetychordsButton button, MidiChord actualChord)
        {
            string n = actualChord.rootNote.ToStandardString();
            switch (n.Remove(n.Length - 1))
            {
                case "C":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x00, 0x00));
                    break;

                case "C#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(128, 0xFF, 0x00, 0x00));
                    break;

                case "D":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x99, 0x00));
                    break;

                case "D#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(128, 0xFF, 0x99, 0x00));//
                    break;

                case "E":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0x00));
                    break;

                case "F":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x99, 0xFF, 0x66));//
                    break;

                case "F#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(128, 0x99, 0xFF, 0x66));
                    break;

                case "G":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0x00, 0xFF));
                    break;

                case "G#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(128, 0x99, 0xFF, 0x66));
                    break;

                case "A":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x66, 0x00, 0xFF));
                    break;

                case "A#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(128, 0x66, 0x00, 0xFF));//
                    break;

                case "B":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xCC, 0x99));
                    break;

                default:
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0xFF));
                    break;
            }
        }

        private static void SetChordLineColor(Line background, MidiChord actualChord)
        {
            ChordType n = actualChord.chordType;
            switch (n)
            {
                case ChordType.Major:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.MajorSeventh:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.MajorSixth:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Sus2:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Sus4:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.DominantNinth:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.DominantEleventh:
                    background.Stroke = new SolidColorBrush(Colors.Red);
                    break;

                case ChordType.Minor:
                    background.Stroke = new SolidColorBrush(Colors.Blue);
                    break;

                case ChordType.MinorSeventh:
                    background.Stroke = new SolidColorBrush(Colors.Blue);
                    break;

                case ChordType.DominantSeventh:
                    background.Stroke = new SolidColorBrush(Colors.Green);
                    break;

                default:
                    background.Stroke = new SolidColorBrush(Colors.LightGray);
                    break;
            }
        }

        private static void DrawDiatonic_4(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 4;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.DominantSeventh;
                            if (firstChord.chordType != ChordType.DominantSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 5, ChordType.DominantSeventh);
                                firstChord.chordType = ChordType.DominantSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.Major;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
                            thisChordType = ChordType.Minor;
                            if (firstChord.chordType != ChordType.Minor)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                firstChord.chordType = ChordType.Minor;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 3:
                            thisChordType = ChordType.SemiDiminished;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
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

        private static void DrawDiatonic_3(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 3;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        

                        case 0:
                            thisChordType = ChordType.Major;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.Minor;
                            if (firstChord.chordType != ChordType.Minor)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                firstChord.chordType = ChordType.Minor;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
                            };
                            netychordsButtons[row, col].Chord = actualChord;
                            break;
                        case 2:
                            thisChordType = ChordType.SemiDiminished;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
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

                private static void DrawOnlyMajor(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 1;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                Line backgroundLine = new Line();
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)R.NDB.MainWindow.Margins.Value == 1)
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

                    netychordsButtons[row, col] = new NetychordsButton(R.NDB.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        

                        case 0:
                            thisChordType = ChordType.Major;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            else
                            {
                                actualChord = actualChord.generateNextFifth();
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

        /*
         * Deprecated
         */
    }
}