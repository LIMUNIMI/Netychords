using System.Windows.Media;

namespace Netychords.Surface.FlowerLayout
{

    public enum FlowerFamilies
    {
        Major,
        Minor
    }

    public static class FlowerFamiliesMethods
    {
        public static Color GetColor(this FlowerFamilies family, FlowerButtonPositions position)
        {
            switch (family)
            {
                case FlowerFamilies.Major:
                    switch (position)
                    {
                        case FlowerButtonPositions.C:
                            return Colors.DarkRed;
                        default:
                            return Colors.Red;
                    }
                    break;
                case FlowerFamilies.Minor:
                    switch (position)
                    {
                        case FlowerButtonPositions.C:
                            return Colors.DarkBlue;
                        default:
                            return Colors.Blue;
                    }
            }
            return Colors.White;
        }
    }
}