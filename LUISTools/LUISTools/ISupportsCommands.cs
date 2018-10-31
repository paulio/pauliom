using System;
using System.Collections.Generic;
using System.Text;

namespace LUISTools
{
    public interface ISupportsCommands
    {
        string Help { get; }

        bool CanHandle(Dictionary<string,string> commands);

        string Execute(Dictionary<string, string> commands);
    }
}
