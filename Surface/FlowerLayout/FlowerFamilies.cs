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
                            break;
                        default:
                            return Colors.Red;
                            break;
                    }
                    break;
                case FlowerFamilies.Minor:
                    switch (position)
                    {
                        case FlowerButtonPositions.C:
                            return Colors.DarkBlue;
                            break;
                        default:
                            return Colors.Blue;
                            break;
                    }
                    break;
            }
            return Colors.White;
        }
    }
}