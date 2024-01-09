using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Timeline;

public class StaticFunctions : MonoBehaviour
{

    public static float scale = 500f;               // Level of zoom
    public const float ldu = 0.0004f;               // LDU = 0.4mm - Real value
    public static float unitLength = 24 * ldu;
    public static float unitWidth = 24 * ldu;
    public static float unitHeight = 8 * ldu;
    public static float unitEndSize = 4 * ldu;
    public static float unitShift = 4 * ldu;
    public static float unitEndRadius = 6 * ldu;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public static ProBuilderMesh createCube(int cubeWidth, int cubeLength, Vector3 position, bool thick = false, float thisScale = 0f, bool enableSnapping = true)
    {
        if (thisScale != 0f)
        {
            scale = thisScale;
        }

        float length = (float)(cubeLength * unitLength * scale);
        float height = (float)(thick ? unitHeight * 3 * scale : unitHeight * scale);
        float width = (float)(cubeWidth * unitWidth * scale);
        float endSize = (float)(unitEndSize * scale);
        float endRadius = (float)(unitEndRadius * scale);

        ProBuilderMesh cube = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(length, height, width));
        cube.gameObject.tag = "Lego";
        cube.GetComponent<Renderer>().material.color = Color.cyan;
        cube.AddComponent<BoxCollider>();
        cube.GetComponent<BoxCollider>().providesContacts = true;
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Rigidbody>().freezeRotation = true;

        if (enableSnapping)
        {
            CreateSnapPoints(cube.transform, cubeWidth, cubeLength);
        }

        for (int x = 0; x < cubeLength; x++)
        {
            for (int y = 0; y < cubeWidth; y++)
            {
                ProBuilderMesh cylinder = ShapeGenerator.GenerateCylinder(PivotLocation.Center,
                                                                            axisDivisions: 64,
                                                                            radius: endRadius,
                                                                            height: endSize,
                                                                            heightCuts: 0);
                cylinder.GetComponent<Renderer>().material.color = Color.blue;
                cylinder.transform.position = new Vector3((x + 0.5f) * length / cubeLength - 0.5f * length, height + 0.5f * endSize - 0.5f * height, (y + 0.5f) * width / cubeWidth - 0.5f * width);
                cylinder.transform.parent = cube.transform;
            }
        }

        cube.transform.position = position;
        return cube;
    }

    private static void CreateSnapPoints(Transform parent, int cubeWidth, int cubeLength)
    {
        CreateSnapPoint(parent, new Vector3(0, 0, 0));

        if (cubeLength > cubeWidth)
        {
            for (int i = 0; i < cubeLength / 2 -1; i++)
            {
                for (int j = 0; j < cubeWidth / 2; j++)
                {
                    if (i+0.5f<cubeLength/2 - 2)
                    {
                        CreateSnapPoint(parent, new Vector3(-i - 0.5f, 0, -j));
                        CreateSnapPoint(parent, new Vector3(i + 0.5f, 0, j));
                    }
                    CreateSnapPoint(parent, new Vector3(i, 0, j));
                    CreateSnapPoint(parent, new Vector3(-i, 0, -j));
                }
            }
        }
        else if (cubeWidth>cubeLength)
        {
            for (int i = 0; i < cubeWidth / 2 -1 ; i++)
            {
                for (int j = 0; j < cubeLength / 2; j++)
                {

                    if (i + 0.5f < cubeWidth / 2 - 2)
                    {
                        CreateSnapPoint(parent, new Vector3(j, 0, i + 0.5f));
                        CreateSnapPoint(parent, new Vector3(-j, 0, -i - 0.5f));
                    }
                    CreateSnapPoint(parent, new Vector3(j, 0, i));
                    CreateSnapPoint(parent, new Vector3(-j, 0, -i));
                    
                }
            }
        }
        else if (cubeWidth == cubeLength)
        {
            for (int i = 0; i < cubeWidth / 2 - 1; i++)
            {
                for (int j = 0; j < cubeLength / 2 - 1 ; j++)
                {
                    if (i + 0.5f < cubeWidth / 2 - 1)
                    {
                        CreateSnapPoint(parent, new Vector3(j, 0, i + 0.5f));
                        CreateSnapPoint(parent, new Vector3(-j, 0, -i - 0.5f));
                    }

                    if (i + 0.5f < cubeLength / 2 - 1)
                    {
                        CreateSnapPoint(parent, new Vector3(-i - 0.5f, 0, -j));
                        CreateSnapPoint(parent, new Vector3(i + 0.5f, 0, j));
                    }
                    CreateSnapPoint(parent, new Vector3(j, 0, i));
                    CreateSnapPoint(parent, new Vector3(-j, 0, -i));
                }
            }
        }
    }

    private static void CreateSnapPoint(Transform parent, Vector3 localPosition)
    {
        GameObject snapPoint = new GameObject("SnapPoint");
        snapPoint.transform.parent = parent;
        snapPoint.transform.localPosition = localPosition;
        snapPoint.tag = "SnapPoint";
        snapPoint.AddComponent<BoxCollider>();
    }

}