using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Home screen: the game title and a card for every heist. Created automatically in the
/// MainMenu scene by ScreenBootstrap.
/// </summary>
public class MainMenuScreen : MonoBehaviour
{
    Text eventBanner;

    void Start() => Build(GameManager.Ensure());

    void Build(GameManager gm)
    {
        Transform canvas = UIKit.CreateCanvas("Main Menu").transform;

        Image bg = UIKit.NewImage("Background", canvas, UIKit.Background);
        UIKit.Stretch(bg.rectTransform);

        Text title = UIKit.NewText(canvas, "PERFECT HEIST", 120, TextAnchor.MiddleCenter, UIKit.Gold, FontStyle.Bold);
        UIKit.Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -60), new Vector2(1600, 150));

        Text sub = UIKit.NewText(canvas, "Plan it.  Run it.  Get away clean.", 42, TextAnchor.MiddleCenter, UIKit.Muted);
        UIKit.Place(sub.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -210), new Vector2(1600, 60));

        Text choose = UIKit.NewText(canvas, "Choose a heist", 40, TextAnchor.MiddleCenter, Color.white, FontStyle.Bold);
        UIKit.Place(choose.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -310), new Vector2(1600, 60));

        RectTransform row = UIKit.NewRect("Heists", canvas);
        UIKit.Place(row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -110), new Vector2(1700, 460));
        var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 40;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = layout.childControlHeight = true;
        layout.childForceExpandWidth = layout.childForceExpandHeight = true;

        Sprite diamond = Resources.Load<Sprite>("Portraits/Diamond");
        foreach (LevelInfo level in LevelInfo.All)
        {
            BuildCard(row, level, diamond, gm);
        }

        Text credit = UIKit.NewText(canvas, "Kfir Nizri & Evyatar Aharon", 28, TextAnchor.MiddleCenter, UIKit.Muted);
        UIKit.Place(credit.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 30), new Vector2(1200, 40));

        // Live Ops banner: filled in when the live config finishes downloading.
        eventBanner = UIKit.NewText(canvas, "", 34, TextAnchor.MiddleCenter, UIKit.Gold, FontStyle.Bold);
        UIKit.Place(eventBanner.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 90), new Vector2(1700, 50));
        ShowLiveEvent();
        if (!LiveConfigLoader.IsLoaded) LiveConfigLoader.Loaded += ShowLiveEvent;
    }

    void ShowLiveEvent()
    {
        if (eventBanner == null) return;
        LiveConfig config = LiveConfig.Current;
        eventBanner.text = config.IsEventActive ? $"LIVE EVENT: {config.eventTitle}. {config.eventMessage}" : "";
    }

    void OnDestroy() => LiveConfigLoader.Loaded -= ShowLiveEvent;

    void BuildCard(Transform parent, LevelInfo level, Sprite icon, GameManager gm)
    {
        Image card = UIKit.NewImage(level.title, parent, UIKit.Card);
        RectTransform rt = card.rectTransform;

        Image art = UIKit.NewSprite("Icon", rt, icon, Color.white);
        UIKit.Place(art.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -40), new Vector2(200, 180));

        Text name = UIKit.NewText(rt, level.title, 44, TextAnchor.MiddleCenter, Color.white, FontStyle.Bold);
        UIKit.Place(name.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -240), new Vector2(380, 60));

        Text sub = UIKit.NewText(rt, level.subtitle, 28, TextAnchor.UpperCenter, UIKit.Muted);
        UIKit.Place(sub.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -305), new Vector2(340, 80));

        if (level.isTutorial)
        {
            string badge = gm.TutorialCompleted ? "COMPLETED" : "START HERE";
            Image badgeBg = UIKit.NewImage("Tutorial badge", rt, gm.TutorialCompleted ? UIKit.Green : UIKit.ButtonColor);
            badgeBg.raycastTarget = false;
            UIKit.Place(badgeBg.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, 26), new Vector2(260, 56));
            UIKit.NewText(badgeBg.rectTransform, badge, 28, TextAnchor.MiddleCenter, Color.white, FontStyle.Bold);
        }

        var button = card.gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => gm.ChooseLevel(level));
    }
}
