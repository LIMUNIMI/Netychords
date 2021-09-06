using NeeqDMIs.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netychords.Utils
{
    public class MidiChord
    {
        public MidiNotes rootNote;
        public ChordType chordType;
        public List<int> interval;
        public MidiChord nextFifth;

        public MidiChord(MidiNotes root, ChordType type)
        {
            rootNote = root;
            chordType = type;
            interval = GenerateInterval(type);
        //    nextFifth = generateNextFifth(this);
        }

        public List<int> GenerateInterval(ChordType type)
        {
            List<int> temp = new List<int>();

            temp.Add(0);

            if (type == ChordType.Major) { temp.Add(4); temp.Add(7); };
            if (type == ChordType.Minor) { temp.Add(3); temp.Add(7); };
            if (type == ChordType.MajorSixth) { temp.Add(4); temp.Add(7); temp.Add(9); };
            if (type == ChordType.MinorSixth) { temp.Add(3); temp.Add(7); temp.Add(9); };
            if (type == ChordType.SemiDiminished) { temp.Add(3); temp.Add(6); temp.Add(10); };
            if (type == ChordType.MajorSeventh) { temp.Add(4); temp.Add(7); temp.Add(11); };
            if (type == ChordType.MinorSeventh) { temp.Add(3); temp.Add(7); temp.Add(10); };
            if (type == ChordType.DominantSeventh) { temp.Add(4); temp.Add(7); temp.Add(10); };
            if (type == ChordType.DiminishedSeventh) { temp.Add(3); temp.Add(6); temp.Add(9); };
            if (type == ChordType.Sus2) { temp.Add(2); temp.Add(7); };
            if (type == ChordType.Sus4) { temp.Add(5); temp.Add(7); };
            if (type == ChordType.DominantNinth) { temp.Add(4); temp.Add(7); temp.Add(14); };
            if (type == ChordType.DominantEleventh) { temp.Add(4); temp.Add(7); temp.Add(14); temp.Add(17); };

            return temp;
        }

        public string ChordName()
        {
            string name = rootNote.ToStandardString().Remove(rootNote.ToStandardString().Length - 1) + GetChordTypeAbbreviation();
            return name;
        }

        public string GetChordTypeAbbreviation()
        {
            ChordType type = chordType;
            string name;
            switch (type)
            {
                case ChordType.Major:
                    name = "";
                    break;
                case ChordType.Minor:
                    name = "m";
                    break;
                case ChordType.DiminishedSeventh:
                    name = "dim7";
                    break;
                case ChordType.DominantEleventh:
                    name = "11";
                    break;
                case ChordType.DominantNinth:
                    name = "9";
                    break;
                case ChordType.DominantSeventh:
                    name = "7";
                    break;
                case ChordType.MajorSeventh:
                    name = "maj7";
                    break;
                case ChordType.MinorSeventh:
                    name = "min7";
                    break;
                case ChordType.Sus2:
                    name = "sus2";
                    break;
                case ChordType.Sus4:
                    name = "sus4";
                    break;
                case ChordType.MajorSixth:
                    name = "6";
                    break;
                case ChordType.MinorSixth:
                    name = "m6";
                    break;
                case ChordType.SemiDiminished:
                    name = "m7b5";
                    break;
                default:
                    name = "";
                    break;
            }

            return name;
        }

        public static MidiChord StandardAbsStringToChordFactory(string note, string octaveNumber, ChordType chordType)
        {
            string midiNote;


            if (note.Contains("#"))
            {
                midiNote = "s" + note[0];
            }
            else if (note.Contains("b"))
            {
                midiNote = "s";
                char oldnote = note[0];
                switch (oldnote)
                {
                    case 'A':
                        midiNote += "G";
                        break;
                    case 'B':
                        midiNote += "A";
                        break;
                    case 'D':
                        midiNote += "C";
                        break;
                    case 'E':
                        midiNote += "D";
                        break;
                    case 'G':
                        midiNote += "F";
                        break;
                }
            }
            else
            {
                midiNote = "" + note[0];
            };

            midiNote += octaveNumber;

            MidiNotes rootNote = (MidiNotes)System.Enum.Parse(typeof(MidiNotes), midiNote);

            return new MidiChord(rootNote, chordType);
        }

        public MidiChord generateNextFifth()
        {
            MidiNotes nextNote = (rootNote + 7);
            string tmp = nextNote.ToStandardString().Remove(nextNote.ToStandardString().Length - 1);
            MidiChord nextFifth = StandardAbsStringToChordFactory(tmp, "2", chordType);
            return nextFifth;
        }
    };

    public enum ChordType
    {
        Major,
        Minor,
        MajorSeventh,
        MinorSeventh,
        DominantSeventh,
        DiminishedSeventh,
        Sus2,
        Sus4,
        DominantNinth,
        DominantEleventh,
        SemiDiminished,
        MajorSixth,
        MinorSixth
    };
}
