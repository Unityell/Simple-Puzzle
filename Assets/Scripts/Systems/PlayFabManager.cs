using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Zenject;
using System.Linq;

public class PlayFabManager
{
    [Inject] EventBus EventBus;
    [Inject] GameData GameData;

    private string PlayFabId;

    public void Login()
    {
        var Request = new LoginWithCustomIDRequest
        {
            #if !UNITY_EDITOR
            CustomId = SystemInfo.deviceUniqueIdentifier,
            #else
            CustomId = "Test",
            #endif
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(Request, OnSuccess, LoginError);
    }

    public bool IsItMyId(string PlayFabId)
    {
        return this.PlayFabId == PlayFabId;
    }

    public void SetData(Dictionary<string, string> Data)
    {
        var Request = new UpdateUserDataRequest
        {
            Data = Data
        };
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.UpdateUserData(Request, UpdateDataSuccess, OnError);
        }
        else
        {
            EventBus.Invoke(EnumSignals.LoginError);
            Debug.Log("Not logged in!");
        }
    }

    public void GetData(List<string> Keys)
    {
        var Request = new GetUserDataRequest
        {
            Keys = Keys
        };
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetUserData(Request, GetDataSuccess, OnError);
        }
        else
        {
            EventBus.Invoke(EnumSignals.LoginError);
            Debug.Log("Not logged in!");
        }
    }

    public void UpdatePlayerStatistics(Dictionary<string, int> Statistics)
    {
        var StatisticsRequest = new UpdatePlayerStatisticsRequest
        {
            Statistics = Statistics.Select(stat => new StatisticUpdate
            {
                StatisticName = stat.Key,
                Value = stat.Value
            }).ToList()
        };

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.UpdatePlayerStatistics(StatisticsRequest, UpdateStatisticsSuccess, OnError);
        }
        else
        {
            EventBus.Invoke(EnumSignals.LoginError);
            Debug.Log("Not logged in!");
        }
    }

    public void GetPlayerStatistics(List<string> Keys)
    {
        var Request = new GetPlayerStatisticsRequest
        {
            StatisticNames = Keys
        };

        PlayFabClientAPI.GetPlayerStatistics(Request, OnGetPlayerStatisticsSuccess, OnError);
    }

    public void GetLeaderboardAroundPlayer(string StatisticName, int MaxResultsCountAboveBelow)
    {
        int MaxResultsCount = (MaxResultsCountAboveBelow * 2) + 1;
        var RequestData = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = StatisticName,
            MaxResultsCount = MaxResultsCount
        };

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetLeaderboardAroundPlayer(RequestData, GetLeaderboardAroundPlayerSuccess, OnError);
        }
        else
        {
            EventBus.Invoke(EnumSignals.LoginError);
            Debug.Log("Not logged in!");
        }
    }

    private void UpdateStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        var updatedStatistics = new Dictionary<string, int>();

        var updateStatisticSignal = new UpdateStatisticSignal(updatedStatistics);

        EventBus.Invoke(updateStatisticSignal);

        Debug.Log("Statistics updated!");
    }

    private void OnSuccess(LoginResult Result)
    {
        PlayFabId = Result.PlayFabId;
        EventBus.Invoke(EnumSignals.LoginSucces);
        Debug.Log("Login/Account successful!");
    }

    private void OnGetPlayerStatisticsSuccess(GetPlayerStatisticsResult result)
    {
        if (result.Statistics != null)
        {
            var statisticsDict = new Dictionary<string, int>();

            foreach (var stat in result.Statistics)
            {
                statisticsDict[stat.StatisticName] = stat.Value;
            }

            var statisticSignal = new GetStatisticSignal(statisticsDict);

            EventBus.Invoke(statisticSignal);
            Debug.Log("Statistics data received!");
        }
        else
        {
            Debug.LogWarning("No statistics found.");
        }
    }

    private void GetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult Result)
    {
        EventBus.Invoke(Result);
        Debug.Log("Leaderboard around player received!");
    }

    private void UpdateDataSuccess(UpdateUserDataResult Result)
    {
        EventBus.Invoke(EnumSignals.UpdateData);
        Debug.Log("Data saved!");
    }

    private void GetDataSuccess(GetUserDataResult Result)
    {
        EventBus.Invoke(Result);
        Debug.Log("Data received!");
    }

    void LoginError(PlayFabError Error)
    {
        EventBus.Invoke(EnumSignals.LoginError);
    }

    private void OnError(PlayFabError Error)
    {
        Debug.Log($"Error {Error.GenerateErrorReport()}");
        EventBus.Invoke(Error);
    }
}