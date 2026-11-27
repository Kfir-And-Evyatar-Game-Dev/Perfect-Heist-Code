using UnityEngine;

/// <summary>What the crew screen shows about one character.</summary>
public class CharacterInfo
{
    public CharacterRole role;
    public string name;
    public string tagline;
    public string[] abilities;
    /// <summary>Image in Resources/Portraits.</summary>
    public string portrait;
}

/// <summary>The five characters and their abilities (from the formal elements document).</summary>
public static class CharacterCatalog
{
    public static readonly CharacterInfo[] All =
    {
        new CharacterInfo
        {
            role = CharacterRole.Hacker, name = "Hacker", portrait = "Hacker",
            tagline = "Owns every screen in the building.",
            abilities = new[] { "Switches off a security camera for 8 seconds.", "Knows the guards' patrol routes." },
        },
        new CharacterInfo
        {
            role = CharacterRole.Burglar, name = "Burglar", portrait = "Sneak",
            tagline = "No lock stays shut for long.",
            abilities = new[] { "Opens locks and safes quickly.", "Carries heavy loot without slowing down." },
        },
        new CharacterInfo
        {
            role = CharacterRole.Sneak, name = "Sneak", portrait = "Sneak",
            tagline = "Moves like a shadow.",
            abilities = new[] { "Moves silently.", "Squeezes through narrow service passages." },
        },
        new CharacterInfo
        {
            role = CharacterRole.Impostor, name = "Impostor", portrait = "Guard",
            tagline = "Always looks like they belong.",
            abilities = new[] { "Distracts a guard for 6 seconds.", "Walks freely in guest areas." },
        },
        new CharacterInfo
        {
            role = CharacterRole.Technician, name = "Technician", portrait = "Hacker",
            tagline = "Fixes anything. Breaks anything.",
            abilities = new[] { "Disables laser barriers for 8 seconds.", "Opens service doors." },
        },
    };

    public static CharacterInfo Get(CharacterRole role) => System.Array.Find(All, c => c.role == role);

    public static Sprite Portrait(CharacterInfo info) => Resources.Load<Sprite>("Portraits/" + info.portrait);
}
