using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeIn {
    INSIDE,
    OUTSIDE,
    ONBOUND
}

public class Misc {
    public static GameObject GeneratePlatform(Vector3 startPoint, Vector3 endPoint, float width) {
        Vector3 centralPoint = (startPoint + endPoint) / 2f;
        float height = (endPoint - startPoint).magnitude;

        GameObject result = GameObject.Instantiate(GameSceneManager.GAME.platformPrefab);
        result.transform.position = centralPoint;
        result.transform.localScale = new Vector3(width, 0.1f, height);

        return result;
    }

    public static List<Vector3> RandomGenerationPlatformPoints(Vector3 startPoint, int numPoints) {
        List<Vector3> points = new List<Vector3>();
        points.Add(startPoint);

        float prevAngle = 0;

        for (int i = 1; i < numPoints; ++i) {
            Vector3 newPoint = points[i - 1];

            float randomHeight = 
                Constants.availableHeights[
                    (int)Random.Range(0, Constants.availableHeights.Length) %
                    Constants.availableHeights.Length
                ];

            float randomAngle; 

            if (prevAngle == 0f) {
                randomAngle =
                    Constants.availableAngles[
                        (int)Random.Range(0, Constants.availableAngles.Length) %
                        Constants.availableAngles.Length
                    ];
                if (i == 1) {
                    points[0] = startPoint + Vector3.right * (randomAngle == 90f ? -1 : 1);
                }
            } else {
                randomAngle = 0f;
            }
            
            prevAngle = randomAngle;

            newPoint.x += Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomHeight;
            newPoint.z += Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomHeight;

            points.Add(newPoint);
        }

        return points;
    }

    public static CubeIn CubeInPlatform(GameObject cube, GameObject platform) {
        Vector3 cubePos = cube.transform.position;
        Vector3 platformPos = platform.transform.position;

        Vector3 cubeHalfScale = cube.transform.localScale;
        Vector3 platformHalfScale = platform.transform.localScale;

        if (platform.transform.rotation.eulerAngles != Vector3.zero) { // rotated
            float temp = platformHalfScale.x;
            platformHalfScale.x = platformHalfScale.z;
            platformHalfScale.z = temp;
        }

        cubeHalfScale /= 2f;
        platformHalfScale /= 2f;

        if (cubePos.x - cubeHalfScale.x > platformPos.x - platformHalfScale.x && // check x
            cubePos.x + cubeHalfScale.x < platformPos.x + platformHalfScale.x &&
            cubePos.z - cubeHalfScale.z > platformPos.z - platformHalfScale.z && // check z
            cubePos.z + cubeHalfScale.z < platformPos.z + platformHalfScale.z) {
            return CubeIn.INSIDE;
        }

        if (cubePos.x + cubeHalfScale.x < platformPos.x - platformHalfScale.x || // check x
            cubePos.x - cubeHalfScale.x > platformPos.x + platformHalfScale.x ||
            cubePos.z + cubeHalfScale.z < platformPos.z - platformHalfScale.z || // check z
            cubePos.z - cubeHalfScale.z > platformPos.z + platformHalfScale.z) {
            return CubeIn.OUTSIDE;
        }

        return CubeIn.ONBOUND;
    }

    public static Vector3[] edgesOfCube(GameObject cube) {
        Vector3[] edges = new Vector3[8];

        Vector3 cubePos = cube.transform.position;
        Vector3 cubeDiams = cube.transform.localScale / 2;

        if (cube.transform.rotation.eulerAngles != Vector3.zero) {
            float temp = cubeDiams.x;
            cubeDiams.x = cubeDiams.z;
            cubeDiams.z = temp;
        }

        edges[0] = cubePos + Vector3.Scale(cubeDiams, new Vector3(-1, -1, -1)); // left-bottom-back
        edges[1] = cubePos + Vector3.Scale(cubeDiams, new Vector3(-1, -1, 1)); // left-bottom-forward
        edges[2] = cubePos + Vector3.Scale(cubeDiams, new Vector3(1, -1, 1)); // right-bottom-forward
        edges[3] = cubePos + Vector3.Scale(cubeDiams, new Vector3(1, -1, -1)); // right-bottom-back
        edges[4] = cubePos + Vector3.Scale(cubeDiams, new Vector3(-1, 1, -1)); // left-up-back
        edges[5] = cubePos + Vector3.Scale(cubeDiams, new Vector3(-1, 1, 1)); // left-up-forward
        edges[6] = cubePos + Vector3.Scale(cubeDiams, new Vector3(1, 1, 1)); // right-up-forward
        edges[7] = cubePos + Vector3.Scale(cubeDiams, new Vector3(1, 1, -1)); // right-up-back

        return edges;
    }

    public static Vector3[] GetRectIntersection(GameObject cube, GameObject platform) { // platformEdges.Length == 4
        Vector3[] cubeEdges = Misc.edgesOfCube(cube);
        Vector3[] platformEdges = Misc.edgesOfCube(platform);

        Vector3[] result = new Vector3[4];

        result[0].x = Mathf.Max(cubeEdges[0].x, platformEdges[0].x);
        result[0].z = Mathf.Max(cubeEdges[0].z, platformEdges[0].z);

        result[1].x = result[0].x;
        result[1].z = Mathf.Min(cubeEdges[1].z, platformEdges[1].z);

        result[2].x = Mathf.Min(cubeEdges[2].x, platformEdges[2].x);
        result[2].z = result[1].z;

        result[3].x = result[2].x;
        result[3].z = result[0].z;

        return result;
    }

    public static bool TransformCubeFromEdges(GameObject cube, Vector3[] points) {
        Vector3 center = new Vector3();
        center.x = (points[3].x + points[0].x) / 2f;
        center.z = (points[1].z + points[0].z) / 2f;
        center.y = cube.transform.position.y;

        Vector3 scale = new Vector3();
        scale.y = cube.transform.localScale.y;
        scale.x = points[3].x - points[0].x;
        scale.z = points[1].z - points[0].z;

        cube.transform.position = center;
        cube.transform.localScale = scale;

        return !(scale.x <= 0.01f || scale.y <= 0.01f || scale.z <= 0.01f);
    }

    public static GameObject GeneratePlatformObject(Vector3 p1, Vector3 p2,
                                              float margin, // platforms[i - 1].transform.localeScale.x
                                              float width,
                                              GameObject platformPrefab) {
       // margin = 0;
        GameObject platform = GameObject.Instantiate(platformPrefab);

        platform.transform.position = (p1 + p2) / 2f;

        bool isNormalOrientation = p1.x == p2.x;

        platform.name = platformIndex.ToString();
        ++platformIndex;

        Vector3 p = platform.transform.position;

        if (!isNormalOrientation) {
            platform.transform.Rotate(0, 90f, 0);
            p.x = p.x + (p1.x > p2.x ? 1 : -1) * margin / 4f;   
        } else {
            p.z -= margin / 4f;
        }

        platform.transform.position = p;

        platform.transform.localScale = new Vector3(width, 0.1f, (p2 - p1).magnitude + margin / 2f);

        return platform;
    }

    public static float CubeArea(GameObject cube) {
        Vector3[] vertices = Misc.edgesOfCube(cube);

        float area = (vertices[1].z - vertices[0].z) * (vertices[3].x - vertices[0].x);
        return area;
    }

    /* This method works only for right/left moving */
    public static void GetRectDifference(GameObject cube, GameObject platform, ref Vector3[] resultVertices) {
        // if not on bound return zero

        Vector3[] cubeEdges = Misc.edgesOfCube(cube);
        Vector3[] platformEdges = Misc.edgesOfCube(platform);

        if (platformEdges[2].x < cubeEdges[2].x) {
            resultVertices[0].x = platformEdges[2].x;
            resultVertices[0].z = cubeEdges[0].z;
            resultVertices[1].x = resultVertices[0].x;
            resultVertices[1].z = cubeEdges[2].z;
            resultVertices[2] = cubeEdges[2];
            resultVertices[3] = cubeEdges[3];
        } else {
            resultVertices[0] = cubeEdges[0];
            resultVertices[1] = cubeEdges[1];
            resultVertices[2].x = platformEdges[1].x;
            resultVertices[2].z = cubeEdges[2].z;
            resultVertices[3].x = resultVertices[2].x;
            resultVertices[3].z = cubeEdges[3].z;
        }
    }

    public static int platformIndex = 0;
}
