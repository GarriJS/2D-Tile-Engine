namespace Common.Core.Constants
{
    /// <summary>
    /// Constants for run time.
    /// </summary>
    public static class RunTimeConstants
    {
        /// <summary>
        /// Gets the base user interface draw layer.
        /// </summary>
        public static int BaseUiDrawLayer { get; } = 15;

        /// <summary>
        /// Gets the base user interface update order.
        /// </summary>
        public static int BaseUiUpdateOrder { get; } = 15;

        /// <summary>
        /// Gets the base above user interface cursor draw layer.
        /// </summary>
        public static int BaseAboveUiCursorDrawLayer { get; } = 25;

        /// <summary>
        /// Gets the base below user interface cursor draw layer.
        /// </summary>
        public static int BaseBelowUiCursorDrawLayer { get; } = 5;

        /// <summary>
        /// GEts the base cursor update order.
        /// </summary>
        public static int BaseCursorUpdateOrder { get; } = 25;
    }
}
