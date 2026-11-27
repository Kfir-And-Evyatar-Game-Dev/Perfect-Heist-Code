using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class AssignmentSixTests
{
    [UnityTest]
    public IEnumerator OpeningPresentsStoryAndHighlightsTheFirstCrewAction()
    {
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return null;
        Click("Tutorial");
        yield return null;
        yield return null;

        Assert.AreEqual("TeamSelect", SceneManager.GetActiveScene().name);
        Assert.IsTrue(VisibleText("Sophie Alvarez"));
        Click("Rehearse");
        yield return null;
        Assert.IsTrue(VisibleText("Tap the Hacker"));
        Assert.IsTrue(VisibleText("▼") || VisibleText("▲"));

        Click("Hacker");
        yield return null;
        Click("Got it");
        yield return null;
        Click("Add to crew");
        yield return null;
        Click("Sneak");
        yield return null;
        Click("Got it");
        yield return null;
        Click("Add to crew");
        yield return null;
        Click("Got it");
        yield return null;
        Click("Start planning  >");
        yield return null;
        yield return null;
        yield return null;

        Assert.AreEqual("Tutorial", SceneManager.GetActiveScene().name);
        Assert.IsTrue(VisibleText("This is the heist map"));
        Click("Show me");
        yield return new WaitForSecondsRealtime(30f);
        Assert.IsTrue(VisibleText("This is the diamond"));
        LogAssert.NoUnexpectedReceived();
    }

    [UnityTest]
    public IEnumerator MenusAndAllThreeMapsLoadWithTheirDistinctFixtures()
    {
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return null;
        foreach (string title in new[] { "Jewelry Store", "Museum", "Bank" })
            Assert.IsTrue(VisibleText(title));

        string[] scenes = { "JewelryStore", "Museum", "Bank" };
        string[] titles = { "Jewelry Store", "Museum", "Bank" };
        string[] crew = { "Hacker", "Sneak", "Technician", "Burglar" };
        string[] fixtures = { "Vault door", "_ _ 1 5", "Dual gate" };
        for (int i = 0; i < scenes.Length; i++)
        {
            yield return SceneManager.LoadSceneAsync("MainMenu");
            yield return null;
            Click(titles[i]);
            yield return null;
            yield return null;
            for (int member = 0; member < i + 2; member++)
            {
                Click(crew[member]);
                Click("Add to crew");
            }
            LogAssert.Expect(LogType.Log, "MapBuilder: built a 19x13 map.");
            Click("Start planning  >");
            yield return null;
            yield return null;
            Assert.AreEqual(scenes[i], SceneManager.GetActiveScene().name);
            Assert.IsNotNull(GameObject.Find(fixtures[i]));
            Assert.IsNotNull(GameObject.Find("Service").GetComponent<BoxCollider2D>());
            Assert.IsNotNull(GameObject.Find("Guard G1"));
            Assert.IsNotNull(GameObject.Find("Laser").GetComponent<BoxCollider2D>());
            Assert.AreEqual(i > 0, GameObject.Find("Guard G3") != null);
        }

        Assert.IsNotNull(GameObject.Find("Panel A"));
        Assert.IsNotNull(GameObject.Find("Panel B"));
        Assert.IsNotNull(GameObject.Find("Grate"));

        // The runtime scripts live in Assembly-CSharp, so exercise the patrol through its scene component.
        Component clock = GameObject.Find("Level").GetComponent("RunClock");
        Transform guard = GameObject.Find("Guard G3").transform;
        var previewTime = clock.GetType().GetProperty("PreviewTime");
        Vector3 start = guard.position;
        previewTime.SetValue(clock, 10f);
        yield return new WaitForFixedUpdate();
        yield return null;
        Vector3 storage = guard.position;
        Assert.That(storage.x, Is.EqualTo(start.x - 2f).Within(0.01f));
        Assert.That(storage.y, Is.EqualTo(start.y - 8f).Within(0.01f));
        previewTime.SetValue(clock, 16f);
        yield return new WaitForFixedUpdate();
        yield return null;
        Assert.AreEqual(storage, guard.position);
        LogAssert.NoUnexpectedReceived();
    }

    static bool VisibleText(string text) =>
        Object.FindObjectsByType<Text>(FindObjectsSortMode.None).Any(label => label.enabled && label.text.Contains(text));

    static void Click(string label)
    {
        Button button = Object.FindObjectsByType<Button>(FindObjectsSortMode.None)
            .Single(item => item.GetComponentsInChildren<Text>()
                .Any(text => text.text == label && text.GetComponentInParent<Button>() == item));
        Assert.IsTrue(button.interactable);
        button.onClick.Invoke();
    }
}
