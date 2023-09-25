using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using HarmonyLib;
using Server = Exiled.Events.Handlers.Server;

namespace UnknownCommandSuggestions
{
    internal sealed class UnknownCommandSuggestions : Plugin<Config>
    {
        public override string Prefix => "unknowncommandsuggestions";
        public override string Name => "UnknownCommandSuggestions";
        public override string Author => "bladuk.";
        public override Version Version { get; } = new Version(1, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(8, 2, 1);
        public static UnknownCommandSuggestions Singleton = new UnknownCommandSuggestions();
        
        internal List<(string Name, ICommand Command)> AllCommands { get; } = new List<(string Name, ICommand Command)>();
        internal List<(string Name, ICommand Command)> AllClientCommands { get; } = new List<(string Name, ICommand Command)>();
        
        private Harmony _harmony;

        public override void OnEnabled()
        {
            Singleton = this;

            _harmony = new Harmony($"{Name}-{DateTime.Now.Ticks}");
            _harmony.PatchAll();

            Server.WaitingForPlayers += LoadAllCommands;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _harmony.UnpatchAll();
            _harmony = null;

            Server.WaitingForPlayers -= LoadAllCommands;
            
            base.OnDisabled();
        }

        private void LoadAllCommands()
        {
            foreach (var command in CommandProcessor.GetAllCommands())
            {
                AllCommands.Add((command.Command, command));
                
                if (command.Aliases != null)
                    AllCommands.AddRange(command.Aliases.Select(alias => (alias, command)));
            }
            
            foreach (var command in QueryProcessor.DotCommandHandler.AllCommands.ToList())
            {
                AllClientCommands.Add((command.Command, command));
                
                if (command.Aliases != null)
                    AllClientCommands.AddRange(command.Aliases.Select(alias => (alias, command)));
            }
        }
    }
}