using System;
using System.Globalization;

/// <summary>
/// Live Ops settings that are downloaded when the game starts (see LiveConfigLoader).
/// The field names match the keys in liveops/config.json. The default values mean
/// "no event": the game plays exactly as built.
/// </summary>
[Serializable]
public class LiveConfig
{
    const float NoChangeMultiplier = 1f;
    const float NoBonusSeconds = 0f;

    public int version;
    public string eventId = "";
    public string eventTitle = "";
    public string eventMessage = "";

    /// <summary>ISO 8601 UTC, for example "2026-11-27T00:00:00Z". Empty means no limit.</summary>
    public string startUtc = "";
    public string endUtc = "";

    /// <summary>Turns the event on regardless of the dates (used for grading and testing).</summary>
    public bool forceActive;

    /// <summary>Faulty cameras: multiplies every security camera's viewing radius.</summary>
    public float cameraRadiusMultiplier = NoChangeMultiplier;

    /// <summary>Faulty cameras: extra seconds a camera stays off after the hacker disables it.</summary>
    public float cameraOffBonusSeconds = NoBonusSeconds;

    /// <summary>The settings in use right now. Replaced once the download succeeds.</summary>
    public static LiveConfig Current { get; set; } = new LiveConfig();

    public bool IsEventActive => IsActiveAt(DateTime.UtcNow);

    public bool IsActiveAt(DateTime utcNow)
    {
        if (string.IsNullOrEmpty(eventId)) return false;
        if (forceActive) return true;
        if (TryParseUtc(startUtc, out DateTime start) && utcNow < start) return false;
        if (TryParseUtc(endUtc, out DateTime end) && utcNow >= end) return false;
        return !string.IsNullOrEmpty(startUtc) || !string.IsNullOrEmpty(endUtc);
    }

    /// <summary>The camera radius multiplier to apply now (1 when no event is running).</summary>
    public float ActiveCameraRadiusMultiplier => IsEventActive ? cameraRadiusMultiplier : NoChangeMultiplier;

    /// <summary>The extra camera-off seconds to apply now (0 when no event is running).</summary>
    public float ActiveCameraOffBonusSeconds => IsEventActive ? cameraOffBonusSeconds : NoBonusSeconds;

    static bool TryParseUtc(string text, out DateTime value)
    {
        return DateTime.TryParse(text, CultureInfo.InvariantCulture,
                                 DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out value);
    }
}
