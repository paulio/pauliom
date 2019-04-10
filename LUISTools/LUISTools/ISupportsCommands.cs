namespace LUISTools
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface ISupportsCommands
    /// </summary>
    public interface ISupportsCommands
    {
        /// <summary>
        /// Gets the help message.
        /// </summary>
        /// <value>The help message for this command.</value>
        string Help { get; }

        /// <summary>
        /// Determines whether this instance can handle the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns><c>true</c> if this instance can handle the specified commands; otherwise, <c>false</c>.</returns>
        bool CanHandle(Dictionary<string,string> commands);

        /// <summary>
        /// Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        /// <returns>Message about the execution</returns>
        string Execute(Dictionary<string, string> commands);
    }
}
