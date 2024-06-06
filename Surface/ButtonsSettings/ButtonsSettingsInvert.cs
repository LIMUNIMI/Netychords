namespace Netychords.Surface.ButtonsSettings
{
    class ButtonsSettingsInvert : IButtonsSettings
    {
        private const int nCols = 14; // OLD 8
        private const int nRows = 14; // OLD 8
        private const int spacing = 100; // OLD 160
        private const int generativeNote = 40;
        private const int startPositionX = 600;
        private const int startPositionY = 1700;
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
