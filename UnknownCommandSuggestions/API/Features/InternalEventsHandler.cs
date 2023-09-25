using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

namespace UnknownCommandSuggestions.API.Features
{
    internal sealed class InternalEventsHandler
    {
        internal static void HandleCommand(string query, CommandSender sender)
        {
            var args = query.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            
            if (args[0].StartsWith("$") || CommandProcessor.RemoteAdminCommandHandler.TryGetCommand(args[0], out _)) return;

            sender.RaReply(UnknownCommandSuggestions.Singleton.Config.UnknownCommandReply + GetSuggestions(args[0], UnknownCommandSuggestions.Singleton.AllCommands), false, true, "");
        }
        
        internal static void HandleConsoleCommand(string query, CommandSender sender)
        {
            var args = query.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            
            if (args[0].StartsWith("$") || QueryProcessor.DotCommandHandler.TryGetCommand(args[0], out _)) return;

            var playerSender = Player.Get(sender) ?? Server.Host;
            
            playerSender.SendConsoleMessage(UnknownCommandSuggestions.Singleton.Config.UnknownCommandReply + GetSuggestions(args[0], UnknownCommandSuggestions.Singleton.AllClientCommands), "red");
        }

        private static string GetSuggestions(string input, List<(string Name, ICommand Command)> dictionary)
        {
            var suggestions = dictionary
                .OrderBy(c => LevenshteinDistance.Calculate(c.Name, input))
                .Select(c => c.Command)
                .Distinct()
                .Take(5);

            return string.Join("\n", suggestions.Select(c => $"{c.Command} {(c.Aliases != null && c.Aliases.Any() ? $"({string.Join("; ", c.Aliases)})" : "")}"));
        }
    }
}