using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class World : MonoBehaviour {

    public event Action? SafeAreaCompleted;

    [SerializeField] private GameObject safeAreaBlockGameObject = null;
    [SerializeField] private Transform areaContainer = null;

    private readonly List<Vector2> areaPoints = new();
    private readonly Random rand = new();

    private void Awake() {

        foreach (Transform child in areaContainer) {
            areaPoints.Add(new Vector2(child.position.x, child.position.z));
        }

        if (areaPoints.Count < 3) {
            throw new ArgumentException("World Area must have at least 3 Points!");
        }

        FindAnyObjectByType<CellHandler>().SafeAreaCompleted += OnSafeAreaCompleted;

    }

    public void OnSafeAreaCompleted() {
        Destroy(safeAreaBlockGameObject);
        SafeAreaCompleted?.Invoke();
    }

    public Vector3 GetRandomWorldPoint() {
        Vector2 randomPoint = GetRandomPointInArea(areaPoints);

        return new Vector3(randomPoint.x, 1, randomPoint.y);
    }

    private Vector2 GetRandomPointInArea(List<Vector2> polygon) {

        float minX = polygon[0].x;
        float maxX = polygon[0].x;
        float minY = polygon[0].y;
        float maxY = polygon[0].y;

        for (int i = 1; i < polygon.Count; i++) {
            if (polygon[i].x < minX) minX = polygon[i].x;
            if (polygon[i].x > maxX) maxX = polygon[i].x;
            if (polygon[i].y < minY) minY = polygon[i].y;
            if (polygon[i].y > maxY) maxY = polygon[i].y;
        }

        while (true) {
            float x = (float)(rand.NextDouble() * (maxX - minX) + minX);
            float y = (float)(rand.NextDouble() * (maxY - minY) + minY);
            Vector2 randomPoint = new(x, y);

            if (IsPointInArea(polygon, randomPoint)) {
                return randomPoint;
            }
        }
    }

    private static bool IsPointInArea(List<Vector2> polygon, Vector2 point) {
        bool isInside = false;
        int n = polygon.Count;

        for (int i = 0, j = n - 1; i < n; j = i++) {
            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) +
                    polygon[i].x)) {
                isInside = !isInside;
            }
        }

        return isInside;
    }

}