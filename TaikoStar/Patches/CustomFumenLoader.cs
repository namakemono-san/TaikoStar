using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx.Logging;
using HarmonyLib;

namespace TaikoStar.Patches;

[HarmonyPatch]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CustomFumenLoader {
    private static ManualLogSource Log => Plugin.Log;
    
    [HarmonyPatch(typeof(DataManager), nameof(DataManager.Awake))]
    [HarmonyPostfix]
    [HarmonyWrapSafe]
    private static void DataManager_Postfix(DataManager __instance) {
        if (__instance.MusicData == null) return;
        
        MusicDataInterface_Postfix(__instance.MusicData);
        NeiroDataInterface_Postfix(__instance.NeiroData);
    }

    private static void MusicDataInterface_Postfix(MusicDataInterface __instance) {
        try {
        } catch (Exception e) {
            Log.LogError(e);
        }
    }

    private static void NeiroDataInterface_Postfix(NeiroDataInterface __instance) {
        try {

        } catch (Exception e) {
            Log.LogError(e);
        }
    }
}