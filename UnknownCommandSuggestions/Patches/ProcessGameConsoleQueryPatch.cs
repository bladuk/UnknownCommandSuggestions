using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using NorthwoodLib.Pools;
using RemoteAdmin;
using UnknownCommandSuggestions.API.Features;
using static HarmonyLib.AccessTools;

namespace UnknownCommandSuggestions.Patches
{
    [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
    public static class ProcessGameConsoleQueryPatch
    {
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instruction, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instruction);
            const int index = 0;
            
            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(QueryProcessor), nameof(QueryProcessor._sender))),
                new CodeInstruction(OpCodes.Call, Method(typeof(InternalEventsHandler), nameof(InternalEventsHandler.HandleConsoleCommand))),
            });
            
            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}