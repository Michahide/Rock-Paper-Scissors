using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using System;

public class BotDifficultyConfig : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;

    [Header("Remote Config Parameters: ")]
    [SerializeField] BotStats[] botDifficulties;
    [SerializeField] bool enableRemoteConfig = false;
    public string difficulty = "Hard";


    struct userAttributes { };
    struct appAttributes { };
    IEnumerator Start()
    {
        //tunggu bot selesai set up
        yield return new WaitUntil(() => bot.IsReady);

        // set stats default dari diffucluty manager
        // sesuai dengan selected difficulty dari inspector
        var newStats = botDifficulties[selectedDifficulty];
        bot.SetStats(newStats, true);

        // ambil difficulty dari remote config bila enabled
        if (enableRemoteConfig == false)
            yield break;
        //menuggu hingga unity service siap
        yield return new WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized &&
        AuthenticationService.Instance.IsSignedIn
        );

        //daftar dulu untuk event fetch completed
        RemoteConfigService.Instance.FetchCompleted += OnRemoteConfigFetched;

        //lalu fetch disini,cukup sekali diawal permainan
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void OnDestroy()
    {
        //jangan lupa untuk unregister event untuk menghindari memeroy leak
        RemoteConfigService.Instance.FetchCompleted -= OnRemoteConfigFetched;
    }
    //setiap kali data baru didapatkan melalui fetch fungsi ini akan dipanggi;
    private void OnRemoteConfigFetched(ConfigResponse response)
    {
        // if (RemoteConfigService.Instance.appConfig.HasKey(difficulty) == false)
        // {
        //     Debug.LogWarning($"Difficulty : {difficulty} not found on remote config server");
        //     Debug.LogWarning($"Difficulty : {RemoteConfigService.Instance.appConfig.GetString("Difficulty")}");
        //     return;
        // }

        switch (response.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session. Using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session. Using cached values from previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session. Update values accordingly.");
                //fetch and set Remote Config value for Difficulty
                difficulty = RemoteConfigService.Instance.appConfig.GetString("Difficulty");
                if (difficulty == "Easy")
                {
                    difficulty = "2";
                }
                else if (difficulty == "Normal")
                {
                    difficulty = "4";
                }
                else if (difficulty == "Hard")
                {
                    difficulty = "6";
                }
                selectedDifficulty = Int32.Parse(difficulty);
                Debug.Log("Difficulty: "+selectedDifficulty);
                selectedDifficulty = Mathf.Clamp(selectedDifficulty, 0, botDifficulties.Length - 1);
                Debug.Log("Difficulty Clamp: "+selectedDifficulty);

                var newStats = botDifficulties[selectedDifficulty];
                Debug.Log("newStats: "+selectedDifficulty);
                bot.SetStats(newStats, true);
                break;
        }
    }
}
