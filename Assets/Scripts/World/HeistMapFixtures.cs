using UnityEngine;

/// <summary>Builds the room finishes and inspectable fixtures of a heist map.</summary>
[RequireComponent(typeof(MapBuilder))]
public sealed class HeistMapFixtures : MonoBehaviour
{
    [System.Serializable]
    sealed class Fixture
    {
        public Vector2Int cell;
        public string label;
        public Color color = Color.white;
        public Vector2 size = new Vector2(0.82f, 0.52f);
        public bool pulse;
    }

    [SerializeField, TextArea(8, 16)] string roomLayout;
    [SerializeField] Fixture[] fixtures;
    [SerializeField, Min(0f)] float pulseSpeed = 5f;

    void Awake()
    {
        var map = GetComponent<MapBuilder>();
        map.BuildMap();
        string[] rooms = roomLayout.Replace("\r", "").Split('\n');
        for (int y = 0; y < map.Height; y++)
        for (int x = 0; x < map.Width; x++)
        {
            if (map.CharAt(x, y) == '#') continue;
            map.SetFloorColor(x, y, RoomColor(rooms[y][x]));
        }

        foreach (Fixture item in fixtures)
        {
            var fixture = new GameObject(item.label);
            fixture.transform.SetParent(transform);
            fixture.transform.position = map.DocToWorld(item.cell);
            var visual = new GameObject("Face");
            visual.transform.SetParent(fixture.transform, false);
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = map.WallLineSprite;
            renderer.color = item.color;
            renderer.sortingOrder = 3;
            Vector2 spriteSize = renderer.sprite.bounds.size;
            visual.transform.localScale = new Vector3(item.size.x / spriteSize.x, item.size.y / spriteSize.y, 1);
            var region = fixture.AddComponent<BoxCollider2D>();
            region.isTrigger = true;
            region.size = item.size;
            if (item.pulse)
            {
                pulses.Add(renderer);
                continue;
            }

            var label = new GameObject("Label");
            label.transform.SetParent(fixture.transform, false);
            var caption = label.AddComponent<TextMesh>();
            caption.text = item.label;
            caption.anchor = TextAnchor.MiddleCenter;
            caption.alignment = TextAlignment.Center;
            caption.fontSize = 48;
            caption.characterSize = 0.043f;
            label.GetComponent<MeshRenderer>().sortingOrder = 8;
        }
    }

    readonly System.Collections.Generic.List<SpriteRenderer> pulses = new();

    void Update()
    {
        float alpha = Mathf.Lerp(0.45f, 1f, 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed));
        foreach (SpriteRenderer beam in pulses)
        {
            Color color = beam.color;
            color.a = alpha;
            beam.color = color;
        }
    }

    static Color RoomColor(char room)
    {
        switch (room)
        {
            case 'A': return new Color(0.59f, 0.74f, 0.92f);
            case 'B': return new Color(0.72f, 0.75f, 0.81f);
            case 'C': return new Color(0.95f, 0.83f, 0.51f);
            case 'D': return new Color(0.76f, 0.64f, 0.91f);
            case 'E': return new Color(0.96f, 0.74f, 0.47f);
            case 'F': return new Color(0.54f, 0.77f, 0.70f);
            case 'G': return new Color(0.88f, 0.66f, 0.75f);
            default: return new Color(0.70f, 0.82f, 0.66f);
        }
    }
}
