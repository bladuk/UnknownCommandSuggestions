using System;
using System.Linq;
using Exiled.API.Features;
using RemoteAdmin;

namespace UnknownCommandSuggestions.API.Features
{
    internal sealed class InternalEventsHandler
    {
        internal static void HandleCommand(string query, CommandSender sender)
        {
            var args = query.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            
            if (args[0].StartsWith("$")) return;
            
            if (!CommandProcessor.RemoteAdminCommandHandler.TryGetCommand(args[0], out _))
            {
                var suggestions = UnknownCommandSuggestions.Singleton.AllCommands
                    .OrderBy(c => LevenshteinDistance.Calculate(c.Command, args[0])).ToList();
                suggestions.RemoveRange(5, suggestions.Count - 5);
                
                sender.RaReply(UnknownCommandSuggestions.Singleton.Config.UnknownCommandReply + string.Join("\n", suggestions.Select(c => c.Command)), false, true, "");
            }
        }
        
        internal static void HandleConsoleCommand(string query, CommandSender sender)
        {
            var args = query.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            
            if (args[0].StartsWith("$")) return;

            var playerSender = Player.Get(sender) ?? Server.Host;
            
            if (!QueryProcessor.DotCommandHandler.TryGetCommand(args[0], out _))
            {
                var suggestions = UnknownCommandSuggestions.Singleton.AllClientCommands
                    .OrderBy(c => LevenshteinDistance.Calculate(c.Command, args[0])).ToList();
                suggestions.RemoveRange(5, suggestions.Count - 5);
                
                playerSender.SendConsoleMessage(UnknownCommandSuggestions.Singleton.Config.UnknownCommandReply + string.Join("\n", suggestions.Select(c => c.Command)), "red");
            }
        }
    }
}