using System.Collections.Generic;
using UnityEngine;

/// <summary>Draws route previews and selection markers on the planning map.</summary>
internal sealed class PlanningMapPreview
{
    readonly Transform root;
    readonly MapBuilder map;
    readonly LevelSpawner spawner;
    readonly Dictionary<Character, LineRenderer> paths = new Dictionary<Character, LineRenderer>();
    readonly List<SpriteRenderer> cellMarkers = new List<SpriteRenderer>();
    readonly List<SpriteRenderer> cameraMarkers = new List<SpriteRenderer>();

    public PlanningMapPreview(Transform parent, MapBuilder map, LevelSpawner spawner)
    {
        root = parent;
        this.map = map;
        this.spawner = spawner;
        foreach (Character character in spawner.Characters) CreatePath(character);
        CreateGuardRoutes();
        CreateCellMarkers();
    }

    public void SetMode(PlanMode mode)
    {
        foreach (SpriteRenderer marker in cellMarkers) marker.enabled = mode == PlanMode.ChooseCell;
        foreach (SpriteRenderer marker in cameraMarkers) marker.enabled = mode == PlanMode.ChooseCamera;
    }

    public void UpdateHighlights(float time)
    {
        float pulse = 0.18f + 0.12f * Mathf.Sin(time * 5f);
        foreach (SpriteRenderer marker in cellMarkers) marker.color = new Color(1f, 1f, 1f, pulse);
        foreach (SpriteRenderer marker in cameraMarkers) marker.color = new Color(1f, 0.2f, 0.2f, pulse + 0.2f);
    }

    void CreatePath(Character c)
    {
        var lr = new GameObject("Path " + c.DisplayName).AddComponent<LineRenderer>();
        lr.transform.SetParent(root, false);
        lr.useWorldSpace = true;
        lr.sortingOrder = 4;
        lr.numCapVertices = 4;
        lr.numCornerVertices = 2;
        if (c.Body != null) lr.sharedMaterial = c.Body.sharedMaterial; // a sprite material that is always in the build
        paths[c] = lr;
    }

    public void RefreshPaths(Character selected)
    {
        foreach (var pair in paths)
        {
            Character c = pair.Key;
            LineRenderer lr = pair.Value;
            if (c == null || lr == null) continue;

            Color col = PlanningUI.RoleColor(c.Role);
            col.a = c == selected ? 0.95f : 0.45f;
            lr.startColor = lr.endColor = col;
            lr.widthMultiplier = c == selected ? 0.1f : 0.06f;

            var points = new List<Vector3>();
            float end = c.PlanEndTime;
            for (float t = 0f; t <= end + 0.001f; t += 0.1f) points.Add(map.DocToWorld(c.PositionAt(t)));
            if (points.Count < 2) points.Clear();
            lr.positionCount = points.Count;
            lr.SetPositions(points.ToArray());
        }
    }

    /// <summary>Dotted line along every guard's patrol, so the player sees where the guard walks.</summary>
    void CreateGuardRoutes()
    {
        foreach (Guard g in spawner.Guards)
        {
            Vector2 a = g.PatrolStart, b = g.PatrolEnd;
            int dots = Mathf.Max(1, Mathf.CeilToInt(Vector2.Distance(a, b) / 0.3f));
            for (int i = 0; i <= dots; i++)
            {
                SpriteRenderer dot = NewMarker("Patrol dot", map.DocToWorld(Vector2.Lerp(a, b, i / (float)dots)), 0.12f);
                dot.color = new Color(0.3f, 0.55f, 1f, 0.8f);
                dot.enabled = true;
            }
        }
    }

    /// <summary>Highlights shown while choosing where to walk or which camera to switch off.</summary>
    void CreateCellMarkers()
    {
        for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
                if (map.IsWalkable(new Vector2Int(x, y)))
                    cellMarkers.Add(NewMarker("Cell marker", map.DocToWorld(new Vector2(x, y)), 0.86f));

        foreach (SecurityCamera cam in spawner.Cameras)
            cameraMarkers.Add(NewMarker("Camera marker", cam.transform.position, 1f));
    }

    SpriteRenderer NewMarker(string name, Vector3 position, float size)
    {
        var sr = new GameObject(name).AddComponent<SpriteRenderer>();
        sr.transform.SetParent(root, false);
        sr.transform.position = position;
        sr.sprite = map.WallLineSprite; // any plain white square
        sr.sortingOrder = 1;
        Vector2 s = sr.sprite != null ? (Vector2)sr.sprite.bounds.size : Vector2.one;
        sr.transform.localScale = new Vector3(size / s.x, size / s.y, 1f);
        sr.enabled = false;
        return sr;
    }

}

