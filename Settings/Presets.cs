using Netychords.Modules;
using Netychords.Modules.CustomRows;
using Netychords.Utils;

namespace Netychords.Settings
{
    public enum Presets
    {
        Empty,
        Pop,
        Jazz,
        OnlyMajor,
        OnlyMinor,
        Diatonic3,
        Diatonic4,
    }

    public static class PresetsExtensions
    {
        public static void Load(this Presets preset)
        {
            switch (preset)
            {
                case Presets.Empty:
                    ToCRs(new List<ChordType> { });
                    break;
                case Presets.Pop:
                    ToCRs(new List<ChordType> { ChordType.Sus4, ChordType.Sus2, ChordType.Maj, ChordType.Min });
                    break;
                case Presets.Jazz:
                    ToCRs(new List<ChordType> { ChordType.Dim7th, ChordType.SemiDim, ChordType.Min6th, ChordType.Maj6th, ChordType.Dom7th });
                    break;
                case Presets.OnlyMajor:
                    ToCRs(new List<ChordType> { ChordType.Maj });
                    break;
                case Presets.OnlyMinor:
                    ToCRs(new List<ChordType> { ChordType.Min});
                    break;
                case Presets.Diatonic3:
                    ToCRs(new List<ChordType> { ChordType.SemiDim, ChordType.Min, ChordType.Maj});
                    break;
                case Presets.Diatonic4:
                    ToCRs(new List<ChordType> { ChordType.SemiDim, ChordType.Min, ChordType.Maj, ChordType.Dom7th });
                    break;
            }
        }

        private static void ToCRs(List<ChordType> chordTypes)
        {
            // Reset
            Rack.CustomRowsManager.CR1 = new CustomRow(1);
            Rack.CustomRowsManager.CR2 = new CustomRow(2);
            Rack.CustomRowsManager.CR3 = new CustomRow(3);
            Rack.CustomRowsManager.CR4 = new CustomRow(4);
            Rack.CustomRowsManager.CR5 = new CustomRow(5);
            Rack.CustomRowsManager.CR6 = new CustomRow(6);
            Rack.CustomRowsManager.CR7 = new CustomRow(7);
            Rack.CustomRowsManager.CR8 = new CustomRow(8);
            Rack.CustomRowsManager.CR9 = new CustomRow(9);

            // Assign
            if (chordTypes.Count >= 1) Rack.CustomRowsManager.CR1 = new CustomRow(1) { Enabled = true, ChordType = chordTypes[0] };
            if (chordTypes.Count >= 2) Rack.CustomRowsManager.CR2 = new CustomRow(2) { Enabled = true, ChordType = chordTypes[1] };
            if (chordTypes.Count >= 3) Rack.CustomRowsManager.CR3 = new CustomRow(3) { Enabled = true, ChordType = chordTypes[2] };
            if (chordTypes.Count >= 4) Rack.CustomRowsManager.CR4 = new CustomRow(4) { Enabled = true, ChordType = chordTypes[3] };
            if (chordTypes.Count >= 5) Rack.CustomRowsManager.CR5 = new CustomRow(5) { Enabled = true, ChordType = chordTypes[4] };
            if (chordTypes.Count >= 6) Rack.CustomRowsManager.CR6 = new CustomRow(6) { Enabled = true, ChordType = chordTypes[5] };
            if (chordTypes.Count >= 7) Rack.CustomRowsManager.CR7 = new CustomRow(7) { Enabled = true, ChordType = chordTypes[6] };
            if (chordTypes.Count >= 8) Rack.CustomRowsManager.CR8 = new CustomRow(8) { Enabled = true, ChordType = chordTypes[7] };
            if (chordTypes.Count == 9) Rack.CustomRowsManager.CR9 = new CustomRow(9) { Enabled = true, ChordType = chordTypes[8] };
        }

        public static Presets Next(this Presets preset)
        {
            var count = Enum.GetNames(typeof(Presets)).Length;

            Presets num = preset + 1;

            if ((int)num < count)
            {
                return num;
            }
            else
            {
                return 0;
            }
        }

        public static Presets Previous(this Presets preset)
        {
            var count = Enum.GetNames(typeof(Presets)).Length;

            Presets num = preset - 1;

            if ((int)num >= 0)
            {
                return num;
            }
            else
            {
                return (Presets)(count - 1);
            }
        }
    }
}
