using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerBootstrap : MonoBehaviour
{
    public static bool SessionStarted { get; private set; }

    string playerIdentifier = "";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartGuest();
    }

    void StartGuest()
    {
        LootLockerSDKManager.StartGuestSession(playerIdentifier, response =>
        {
            if (!response.success)
            {
                Debug.LogError("Fallo");
                return;
            }
            SessionStarted = true;
            Debug.Log("Conectado");
        });
    }

}
