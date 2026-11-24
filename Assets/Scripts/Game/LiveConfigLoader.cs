using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Downloads the Live Ops config (liveops/config.json, served by GitHub Pages)
/// once, when the game starts, and puts it in LiveConfig.Current. Changing that
/// file and pushing changes the game for every player without a new build.
/// If the download fails, the game keeps the default config (no event).
/// </summary>
public class LiveConfigLoader : MonoBehaviour
{
    const string ConfigUrl = "https://kfir-and-evyatar-game-dev.github.io/Perfect-Heist-Code/liveops/config.json";
    const string LocalConfigPath = "liveops/config.json";
    const int TimeoutSeconds = 10;

    /// <summary>In the Unity editor, read the config file from the repo instead of the web (no push needed).</summary>
    public static bool UseLocalFileInEditor = true;

    /// <summary>True once loading has finished, whether it succeeded or not.</summary>
    public static bool IsLoaded { get; private set; }

    /// <summary>Raised when loading finishes (also when it fails and the defaults stay).</summary>
    public static event Action Loaded;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
        IsLoaded = false;
        var go = new GameObject("Live Config Loader");
        DontDestroyOnLoad(go);
        go.AddComponent<LiveConfigLoader>();
    }

    async void Start()
    {
        string json = await DownloadAsync();
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                LiveConfig config = JsonUtility.FromJson<LiveConfig>(json);
                if (config != null) LiveConfig.Current = config;
                Debug.Log($"Live config v{LiveConfig.Current.version} loaded. " +
                          $"Event '{LiveConfig.Current.eventId}' active: {LiveConfig.Current.IsEventActive}.");
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning("Live config is not valid JSON, keeping the defaults. " + e.Message);
            }
        }

        IsLoaded = true;
        Loaded?.Invoke();
    }

    async Awaitable<string> DownloadAsync()
    {
        string url = ConfigUrl + "?t=" + DateTime.UtcNow.Ticks; // skip cached copies

        if (Application.isEditor && UseLocalFileInEditor)
        {
            string path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", LocalConfigPath));
            if (File.Exists(path)) return File.ReadAllText(path);
        }

        using UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = TimeoutSeconds;
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Could not download the live config, keeping the defaults. " + request.error);
            return null;
        }
        return request.downloadHandler.text;
    }
}
