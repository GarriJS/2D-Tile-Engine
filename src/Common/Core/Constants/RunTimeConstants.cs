namespace Common.Core.Constants
{
    /// <summary>
    /// Constants for run time.
    /// </summary>
    static public class RunTimeConstants
    {
        /// <summary>
        /// Gets the base user interface draw layer.
        /// </summary>
        static public int BaseUiDrawLayer { get; } = 15;

        /// <summary>
        /// Gets the base user interface update order.
        /// </summary>
        static public int BaseUiUpdateOrder { get; } = 15;

        /// <summary>
        /// Gets the base above user interface cursor draw layer.
        /// </summary>
        static public int BaseAboveUiCursorDrawLayer { get; } = 25;

        /// <summary>
        /// Gets the base below user interface cursor draw layer.
        /// </summary>
        static public int BaseBelowUiCursorDrawLayer { get; } = 5;

        /// <summary>
        /// GEts the base cursor update order.
        /// </summary>
        static public int BaseCursorUpdateOrder { get; } = 25;
    }
}
