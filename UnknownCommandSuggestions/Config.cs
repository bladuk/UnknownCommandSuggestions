using System.ComponentModel;
using Exiled.API.Interfaces;

namespace UnknownCommandSuggestions
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("Show debug messages in the server console?")]
        public bool Debug { get; set; } = false;
        
        [Description("String displayed before the suggestions")]
        public string UnknownCommandReply { get; set; } = "Maybe you meant:\n";
    }
}