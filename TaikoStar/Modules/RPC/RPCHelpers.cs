using System.Diagnostics.CodeAnalysis;
using UnityEngine.SceneManagement;

namespace TaikoStar.Modules.RPC;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class RPCHelpers {
    private static readonly DiscordRichPresence Instance = DiscordRichPresence.Instance;

    private static void ChangeRichPresence(string key) {
        Instance.RichPresence.Details = Instance.T($"scene_{key}_details");
        Instance.RichPresence.State = Instance.T($"scene_{key}_state");
        Instance.RichPresence.Assets.SmallImageKey = Instance.T($"scene_{key}_smallImageKey");
        Instance.RichPresence.Assets.SmallImageText = Instance.T($"scene_{key}_smallImageText");
    }

    public static void SceneChange(Scene scene, LoadSceneMode mode) {
        switch (scene.name) {
            case "Boot":
                ChangeRichPresence("boot");
                break;
            case "Title":
                ChangeRichPresence("title");
                break;
            case "MainMenu":
                ChangeRichPresence("mainMenu");
                break;
            case "ThunderShrine":
                ChangeRichPresence("thunderShrine");
                break;
            case "SongSelect":
                ChangeRichPresence("songSelect");
                break;
            case "SongSelectTraining":
                ChangeRichPresence("songSelectTraining");
                break;
            case "Enso":
                EnsoHelpers.CurrentEnsoType = EnsoHelpers.EnsoType.Normal;
                break;
            case "EnsoScenario":
                EnsoHelpers.CurrentEnsoType = EnsoHelpers.EnsoType.Scenario;
                break;
            case "EnsoTrainingFull":
                EnsoHelpers.CurrentEnsoType = EnsoHelpers.EnsoType.Training;
                break;
            case "EnsoDonChanBand":
                EnsoHelpers.CurrentEnsoType = EnsoHelpers.EnsoType.DonChanBand;
                break;
        }
        
        Instance.UpdatePresence();
    }
}