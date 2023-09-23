using System.Collections.Generic;
using System.Reflection.Emit;
using UnknownCommandSuggestions.API.Features;
using HarmonyLib;
using NorthwoodLib.Pools;
using RemoteAdmin;
using static HarmonyLib.AccessTools;

namespace UnknownCommandSuggestions.Patches
{
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    public static class ProcessQueryPatch
    {
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiller(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            const int index = 0;
            
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(InternalEventsHandler), nameof(InternalEventsHandler.HandleCommand)))
            });
            
            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
            
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}