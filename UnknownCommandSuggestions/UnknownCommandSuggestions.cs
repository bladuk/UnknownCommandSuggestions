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
    internal class UnknownCommandSuggestions : Plugin<Config>
    {
        public override string Prefix => "unknowncommandsuggestions";
        public override string Name => "UnknownCommandSuggestions";
        public override string Author => "bladuk.";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(8, 2, 1);
        public static UnknownCommandSuggestions Singleton = new UnknownCommandSuggestions();

        internal List<ICommand> AllCommands { get; private set; } = new List<ICommand>();
        internal List<ICommand> AllClientCommands { get; private set; } = new List<ICommand>();
        
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
            AllCommands = CommandProcessor.GetAllCommands();
            AllClientCommands = QueryProcessor.DotCommandHandler.AllCommands.ToList();
        }
    }
}