using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace TaikoStar.Modules.RPC;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class EnsoHelpers {
    private static readonly DiscordRichPresence Instance = DiscordRichPresence.Instance;

    public static EnsoType CurrentEnsoType;
    
    public enum EnsoType {
        Normal,
        Scenario,
        Training,
        DonChanBand
    }

    private static void GenreRPCState(EnsoData.SongGenre songGenre) {
        Instance.RichPresence.State = songGenre switch {
            EnsoData.SongGenre.Pops => Instance.T("song_genre_pops"),
            EnsoData.SongGenre.Anime => Instance.T("song_genre_anime"),
            EnsoData.SongGenre.Vocalo => Instance.T("song_genre_vocalo"),
            EnsoData.SongGenre.Variety => Instance.T("song_genre_variety"),
            EnsoData.SongGenre.Classic => Instance.T("song_genre_classic"),
            EnsoData.SongGenre.Game => Instance.T("song_genre_game"),
            EnsoData.SongGenre.Namco => Instance.T("song_genre_namco"),
            _ => Instance.T("unknown")
        };
    }
    
    private static void LevelRPCImage(EnsoData.EnsoLevelType ensoLevel) {
        Instance.RichPresence.Assets.SmallImageKey = ensoLevel switch {
            EnsoData.EnsoLevelType.Easy => "https://nmkmn.moe/taiko/icon_course_easy.png",
            EnsoData.EnsoLevelType.Normal => "https://nmkmn.moe/taiko/icon_course_normal.png",
            EnsoData.EnsoLevelType.Hard => "https://nmkmn.moe/taiko/icon_course_hard.png",
            EnsoData.EnsoLevelType.Mania => "https://nmkmn.moe/taiko/icon_course_mania.png",
            EnsoData.EnsoLevelType.Ura => "https://nmkmn.moe/taiko/icon_course_ura.png",
            EnsoData.EnsoLevelType.Num => "",
            _ => ""
        };

        Instance.RichPresence.Assets.SmallImageText = ensoLevel switch {
            EnsoData.EnsoLevelType.Easy => Instance.T("song_level_easy"),
            EnsoData.EnsoLevelType.Normal => Instance.T("song_level_normal"),
            EnsoData.EnsoLevelType.Hard => Instance.T("song_level_hard"),
            EnsoData.EnsoLevelType.Mania => Instance.T("song_level_mania"),
            EnsoData.EnsoLevelType.Ura => Instance.T("song_level_ura"),
            EnsoData.EnsoLevelType.Num => Instance.T("unknown"),
            _ => Instance.T("unknown")
        };
    }
    
    private static void EnsoScreenRPC(EnsoType ensoType, string songName, EnsoData.SongGenre songGenre, EnsoData.EnsoLevelType ensoLevel) {
        var ensoTypeText = ensoType switch {
            EnsoType.Normal => Instance.T("enso_type_normal"),
            EnsoType.Scenario => Instance.T("enso_type_scenario"),
            EnsoType.Training => Instance.T("enso_type_training"),
            EnsoType.DonChanBand => Instance.T("enso_type_DonChanBand"),
            _ => Instance.T("unknown")
        };

        Instance.RichPresence.Details = songName;
        Instance.RichPresence.Assets.LargeImageText = ensoTypeText;
        GenreRPCState(songGenre);
        LevelRPCImage(ensoLevel);
        
        Instance.UpdatePresence();
    }
    
    public static void SetEnso(object sender, EventArgs args) {
        if (sender is not SongInfoPlayer songInfoPlayer) return;
        
        var ensoDataManager = GameObject.Find("CommonObjects/Datas/EnsoDataManager").GetComponent<EnsoDataManager>();
        var ensoLevelDifficulty = ensoDataManager.GetDiffCourse(0);
        
        EnsoScreenRPC(CurrentEnsoType, songInfoPlayer.m_SongName, songInfoPlayer.m_Genre, ensoLevelDifficulty);
    }
}