namespace CopperDevs.Logger
{
    /// <summary>
    /// Ways to have duplicate messages logged
    /// </summary>
    public enum DuplicatesLogType
    {
        /// <summary>
        /// Do nothing different when two messages of the same contents are logged one after another 
        /// </summary>
        Nothing,

        /// <summary>
        /// Adds the number of times the message is logged after the message when it's the exact same
        /// </summary>
        Numbered,

        /// <summary>
        /// Similar to <see cref="DuplicatesLogType.Numbered"/>, except internally added timestamps are ignored when comparing messages
        /// </summary>
        /// <remarks>
        /// Acts the same as <see cref="DuplicatesLogType.Numbered"/> when <see cref="CopperLogger.IncludeTimestamps"/> is set to false
        /// </remarks>
        IgnoreTime
    }
}