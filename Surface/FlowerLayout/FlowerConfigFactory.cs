using Netychords.Utils;

namespace Netychords.Surface.FlowerLayout
{
    public static class FlowerConfigFactory
    {
        public static FlowerConfig DefaultMajor()
        {
            return new FlowerConfig
            {
                Family = FlowerFamilies.Major,
                ChordType_C = ChordType.Major,
                ChordType_L = ChordType.MajorSeventh,
                ChordType_U = ChordType.MajorSixth,
                ChordType_R = ChordType.DominantSeventh,
                ChordType_D = ChordType.Sus2
            };
        }

        public static FlowerConfig DefaultMinor()
        {
            return new FlowerConfig
            {
                Family = FlowerFamilies.Minor,
                ChordType_C = ChordType.Minor,
                ChordType_L = ChordType.MinorSeventh,
                ChordType_U = ChordType.MinorSixth,
                ChordType_R = ChordType.DiminishedSeventh,
                ChordType_D = ChordType.SemiDiminished
            };
        }
    }
}