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
                ChordType_C = ChordType.Maj,
                ChordType_L = ChordType.Maj7th,
                ChordType_U = ChordType.Maj6th,
                ChordType_R = ChordType.Dom7th,
                ChordType_D = ChordType.Sus2
            };
        }

        public static FlowerConfig DefaultMinor()
        {
            return new FlowerConfig
            {
                Family = FlowerFamilies.Minor,
                ChordType_C = ChordType.Min,
                ChordType_L = ChordType.Min7th,
                ChordType_U = ChordType.Min6th,
                ChordType_R = ChordType.Dim7th,
                ChordType_D = ChordType.SemiDim
            };
        }
    }
}