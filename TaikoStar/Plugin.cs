using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using TaikoStar.Modules.RPC;
using TaikoStar.Patches;

namespace TaikoStar;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin {
    public new static ManualLogSource Log;

    public ConfigEntry<string> ConfigLanguage;
    
    public Dictionary<string, string> LocalizedData { get; private set; } = new ();
    private const string ResourceFormat = "TaikoStar.Localization.{0}.json";
    
    public override void Load() {
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        SetupConfig();
        LocalizedData = LoadLanguageFile(ConfigLanguage.Value);
        
        // Load Patch
        Harmony.CreateAndPatchAll(typeof(ReplaceVersionText));
        Harmony.CreateAndPatchAll(typeof(SkipSplashScreen));
        Harmony.CreateAndPatchAll(typeof(SkipCoinAndReward));
        Harmony.CreateAndPatchAll(typeof(SongInfoPlayerPatcher));
        
        // Discord Rich Presence
        DiscordRichPresence.Instance.Initialize(LocalizedData);
        AddComponent<RPCMonoBehavior>();
    }
    
    private Dictionary<string, string> LoadLanguageFile(string languageCode) {
        var result = new Dictionary<string, string>();
        var resourceName = string.Format(ResourceFormat, languageCode);
        var asm = Assembly.GetExecutingAssembly();

        try {
            var stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null) {
                Log.LogWarning($"Resource '{resourceName}' was not found. Fallback to 'ja'.");
                resourceName = string.Format(ResourceFormat, "ja");
                stream = asm.GetManifestResourceStream(resourceName);
                if (stream == null) {
                    Log.LogWarning($"Fallback resource '{resourceName}' was also not found. Returning empty dictionary.");
                    return result;
                }
            }

            using var reader = new StreamReader(stream);
            var jsonText = reader.ReadToEnd();

            var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText);
            if (parsed == null) {
                Log.LogWarning($"Deserialized dictionary is null for resource '{resourceName}'. Returning empty dictionary.");
                return result;
            }

            result = parsed;
            Log.LogInfo($"Loaded language resource '{resourceName}' with {result.Count} entries.");
        } catch (JsonException ex) {
            Log.LogError($"Failed to parse JSON (resource '{resourceName}'): {ex.Message}");
        } catch (Exception ex) {
            Log.LogError($"Unexpected error while loading resource '{resourceName}': {ex}");
        }
        
        return result;
    }

    private void SetupConfig() {
        ConfigLanguage = Config.Bind(
            "General",
            "Language",
            "ja",
            "Set the language to be used in the mod"
        );
    }
}