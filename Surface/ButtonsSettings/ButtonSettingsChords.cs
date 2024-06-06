using Netychords.Utils;

namespace Netychords.Surface.ButtonsSettings
{
    class ButtonsSettingsChords : IButtonsSettings
    {
        private const int nCols = 12;
        private int nRows = System.Enum.GetNames(typeof(ChordType)).Length;
        private const int spacing = 100;
        private const int generativeNote = 40;
        private const int startPositionX = 300;
        private const int startPositionY = 1000;
        private const int occluderAlpha = 10;

        #region Interface
        public int NCols { get => nCols; }
        public int NRows { get => nRows; }
        public int Spacing { get => spacing; }
        public int GenerativeNote { get => generativeNote; }
        public int StartPositionX { get => startPositionX; }
        public int StartPositionY { get => startPositionY; }
        public int OccluderAlpha { get => occluderAlpha; }
        #endregion
    }
}
