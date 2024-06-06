using NITHdmis.Music;
using System.Collections.Generic;
using System.Drawing;

namespace Netychords.Surface.FlowerLayout
{
    public class Plant
    {
        public readonly List<int> MajorRule = new List<int> { 0, 2, 4, 5, 7, 9, 11 };
        public readonly List<int> MinorRule = new List<int> { 0, 2, 3, 5, 7, 8, 10 };

        #region Flowers
        public Flower Flower1 { get; set; }
        public Flower Flower2 { get; set; }
        public Flower Flower3 { get; set; }
        public Flower Flower4 { get; set; }
        public Flower Flower5 { get; set; }
        public Flower Flower6 { get; set; }
        public Flower Flower7 { get; set; }
        #endregion

        public List<Flower> Flowers { get; private set; }

        public Plant(MidiNotes rootNote, PlantFamilies rootFamily, Point center)
        {
            List<MidiNotes> allnotes = MusicConversions.GetAllMidiNotesList();
            List<int> rule = new List<int>();

            Point p1 = new Point(center.X, center.Y);
            Point p2 = new Point(center.X - 3, center.Y - 1);
            Point p3 = new Point(center.X - 2, center.Y + 1);
            Point p4 = new Point(center.X + 1, center.Y + 2);
            Point p5 = new Point(center.X + 3, center.Y + 1);
            Point p6 = new Point(center.X + 2, center.Y - 1);
            Point p7 = new Point(center.X - 1, center.Y - 2);

            MidiNotes n1 = MidiNotes.NaN;
            MidiNotes n2 = MidiNotes.NaN;
            MidiNotes n3 = MidiNotes.NaN;
            MidiNotes n4 = MidiNotes.NaN;
            MidiNotes n5 = MidiNotes.NaN;
            MidiNotes n6 = MidiNotes.NaN;
            MidiNotes n7 = MidiNotes.NaN;

            FlowerConfig c1 = null;
            FlowerConfig c2 = null;
            FlowerConfig c3 = null;
            FlowerConfig c4 = null;
            FlowerConfig c5 = null;
            FlowerConfig c6 = null;
            FlowerConfig c7 = null;

            switch (rootFamily)
            {
                case PlantFamilies.Major:

                    rule = MajorRule;

                    n1 = rootNote;
                    n2 = rootNote + rule[1];
                    n3 = rootNote + rule[2];
                     n4 = rootNote + rule[3];
                     n5 = rootNote + rule[4];
                     n6 = rootNote + rule[5];
                     n7 = rootNote + rule[6];

                    c1 = FlowerConfigFactory.DefaultMajor();
                    c2 = FlowerConfigFactory.DefaultMinor();
                    c3 = FlowerConfigFactory.DefaultMinor();
                    c4 = FlowerConfigFactory.DefaultMajor();
                    c5 = FlowerConfigFactory.DefaultMajor();
                    c6 = FlowerConfigFactory.DefaultMinor();
                    c7 = FlowerConfigFactory.DefaultMinor();

                    break;

                case PlantFamilies.Minor:

                    rule = MinorRule;

                    n1 = rootNote;
                    n2 = rootNote + rule[1];
                    n3 = rootNote + rule[2];
                    n4 = rootNote + rule[3];
                    n5 = rootNote + rule[4];
                    n6 = rootNote + rule[5];
                    n7 = rootNote + rule[6];

                    c1 = FlowerConfigFactory.DefaultMinor();
                    c2 = FlowerConfigFactory.DefaultMinor();
                    c3 = FlowerConfigFactory.DefaultMajor();
                    c4 = FlowerConfigFactory.DefaultMinor();
                    c5 = FlowerConfigFactory.DefaultMajor();
                    c6 = FlowerConfigFactory.DefaultMajor();
                    c7 = FlowerConfigFactory.DefaultMinor();

                    break;
            }

            Flower1 = new Flower(n1, c1, p1);
            Flower2 = new Flower(n2, c2, p2);
            Flower3 = new Flower(n3, c3, p3);
            Flower4 = new Flower(n4, c4, p4);
            Flower5 = new Flower(n5, c5, p5);
            Flower6 = new Flower(n6, c6, p6);
            Flower7 = new Flower(n7, c7, p7);

            Flowers = new List<Flower>();
            Flowers.Add(Flower1);
            Flowers.Add(Flower2);
            Flowers.Add(Flower3);
            Flowers.Add(Flower4);
            Flowers.Add(Flower5);
            Flowers.Add(Flower6);
            Flowers.Add(Flower7);
        }
    }
}