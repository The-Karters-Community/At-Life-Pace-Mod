using HarmonyLib;

namespace AtLifePace.Core;

/// <summary>
/// This class will patch the Ant_CurrentGameConfiguration::Start method.
/// </summary>
[HarmonyPatch(typeof(Ant_CurrentGameConfiguration), nameof(Ant_CurrentGameConfiguration.Start))]
public class Ant_CurrentGameConfiguration__Start {
    /*public static void Prefix(Ant_CurrentGameConfiguration __instance) {
        // Executed before the original method.

        // If the method returns void, then the original method will be executed next.
        // Or if the methods returns a bool...
        // - Skip original method:
        // return false;
        // - Execute the original method next:
        // return true;
    }*/

    public static void Postfix(Ant_CurrentGameConfiguration __instance) {
        // Executed after the original method.
        AtLifePace.Get().logger.Log("(From patched method) The game has been started.");
    }
}