using System;

namespace CLI_Engine
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }
    }
}