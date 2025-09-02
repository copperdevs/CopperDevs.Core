namespace CopperDevs.Logger
{
    /// <summary>
    /// Ways to have lists be printed
    /// </summary>
    public enum ListLogType
    {
        /// <summary>
        /// Directly convert the list object to a string
        /// </summary>
        /// <example>
        /// <code>
        /// Log.Debug(new List&lt;int&gt; { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        ///  
        /// // Resulting Log
        /// Debug: System.Collections.Generic.List`1[System.String]
        /// </code>
        /// </example>
        Direct,

        /// <summary>
        /// Print each value of the list to its own line
        /// </summary>
        /// <example>
        /// <code>
        /// Log.Debug(new List&lt;int&gt; { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        ///  
        /// // Resulting Log
        /// Debug: 0
        ///        1
        ///        2
        ///        3
        ///        4
        ///        5
        ///        6
        ///        7
        ///        8
        ///        9
        /// </code>
        /// </example>
        Multiple,

        /// <summary>
        /// Formats the values of the list into a single line
        /// </summary>
        /// <example>
        /// <code>
        /// Log.Debug(new List&lt;int&gt; { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        ///  
        /// // Resulting Log
        /// Debug: [0,1,2,3,4,5,6,7,8,9]
        /// </code>
        /// </example>
        Single
    }
}