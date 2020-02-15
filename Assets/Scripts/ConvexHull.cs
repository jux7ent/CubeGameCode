using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConvexHull {
    private class GFG : IComparer<Vector3> {
        public int Compare(Vector3 a, Vector3 b) {
            return Convert.ToInt32(a.x < b.x || a.x == b.x && a.z < b.z);
        }
    }
    private static bool cw(Vector3 a, Vector3 b, Vector3 c) {
        return a.x * (b.z - c.z) + b.x * (c.z - a.z) + c.x * (a.z - b.z) < 0;
    }

    private static bool ccw(Vector3 a, Vector3 b, Vector3 c) {
        return a.x * (b.z - c.z) + b.x * (c.z - a.z) + c.x * (a.z - b.z) > 0;
    }

    public static List<Vector3> convex_hull(List<Vector3> points) {
        if (points.Count == 1)
            return null;

        points.Sort(new GFG());

        Vector3 p1 = points[0];
        Vector3 p2 = points[points.Count - 1];

        List<Vector3> up = new List<Vector3>();
        List<Vector3> down = new List<Vector3>();

        up.Add(p1);
        down.Add(p1);

        for (int i = 1; i < points.Count; ++i) {
            if (i == points.Count - 1 || cw(p1, points[i], p2)) {
                while (up.Count >= 2 && !cw(up[up.Count - 2], up[up.Count - 1], points[i])) {
                    up.RemoveAt(up.Count - 1);
                }
                up.Add(points[i]);
            }
            if (i == points.Count - 1 || ccw(p1, points[i], p2)) {
                while (down.Count >= 2 && !ccw(down[down.Count - 2], down[down.Count - 1], points[i])) {
                    down.RemoveAt(down.Count - 1);
                }
                down.Add(points[i]);
            }
        }

        points.Clear();

        for (int i = 0; i < up.Count; ++i)
            points.Add(up[i]);
        for (int i = down.Count - 2; i > 0; --i)
            points.Add(down[i]);

        return points;
    }
}
