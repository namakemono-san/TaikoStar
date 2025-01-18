using System.Collections.Generic;
using DiscordRPC;
using DiscordRPC.Logging;
using TaikoStar.Patches;

namespace TaikoStar.Modules.RPC
{
    public class DiscordRichPresence
    {
        public static DiscordRichPresence Instance { get; } = new();

        private DiscordRpcClient _rpc;

        private const string ClientId = "1309050701800017970";

        private Dictionary<string, string> _localizedData = new();

        public readonly RichPresence RichPresence = new() {
            Details = "Initializing...",
            State = "",
            Timestamps = Timestamps.Now,
            Assets = new DiscordRPC.Assets()
        };

        public void Initialize(Dictionary<string, string> translations) {
            _localizedData = translations ?? new Dictionary<string, string>();

            SongInfoPlayerPatcher.Instance.OnSongInfoPlayerFinished += EnsoHelpers.SetEnso;

            _rpc = new DiscordRpcClient(ClientId) {
                Logger = new ConsoleLogger
                {
                    Level = LogLevel.Warning,
                    Coloured = true
                }
            };

            _rpc.OnReady += (sender, e) => {
                Plugin.Log.LogInfo($"Ready received from user {e.User.Username}");
                UpdatePresence();
            };

            _rpc.OnPresenceUpdate += (sender, e) => {
                Plugin.Log.LogInfo($"Presence updated: {e.Presence.Details}, {e.Presence.State}");
            };

            _rpc.Initialize();
        }

        public string T(string key) {
            if (_localizedData.TryGetValue(key, out var value))
            {
                return value;
            }

            Plugin.Log.LogWarning($"Key not found: {key}");
            return "LOCALE_KEY_NOT_FOUND";
        }

        public void UpdatePresence() {
            _rpc.SetPresence(RichPresence);
        }
    }
}
