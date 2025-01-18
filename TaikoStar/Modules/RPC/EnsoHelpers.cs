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
            EnsoData.SongGenre.Pops => "ポップス",
            EnsoData.SongGenre.Anime => "アニメ",
            EnsoData.SongGenre.Vocalo => "ボーカロイド™️曲",
            EnsoData.SongGenre.Variety => "バラエティ",
            EnsoData.SongGenre.Classic => "クラシック",
            EnsoData.SongGenre.Game => "ゲームミュージック",
            EnsoData.SongGenre.Namco => "ナムコオリジナル",
            _ => "不明"
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
            EnsoData.EnsoLevelType.Easy => "かんたん",
            EnsoData.EnsoLevelType.Normal => "ふつう",
            EnsoData.EnsoLevelType.Hard => "むずかしい",
            EnsoData.EnsoLevelType.Mania => "おに",
            EnsoData.EnsoLevelType.Ura => "おに（裏）",
            EnsoData.EnsoLevelType.Num => "不明",
            _ => "不明"
        };
    }
    
    private static void EnsoScreenRPC(EnsoType ensoType, string songName, EnsoData.SongGenre songGenre, EnsoData.EnsoLevelType ensoLevel) {
        var ensoTypeText = ensoType switch {
            EnsoType.Normal => "ノーマル",
            EnsoType.Scenario => "シナリオ",
            EnsoType.Training => "トレーニング",
            EnsoType.DonChanBand => "どんちゃんバンド",
            _ => "不明"
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