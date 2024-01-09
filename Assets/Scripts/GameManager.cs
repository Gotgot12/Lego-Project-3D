using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class GameManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Material myMaterial = Resources.Load("Materials/Red/RedBright.mat", typeof(Material)) as Material;
        StaticFunctions.createCube(8, 4, new Vector3(1f, 1f, 1f), true, 50);
        StaticFunctions.createCube(2, 6, new Vector3(3f, 1f, 3f), true, 50);
        StaticFunctions.createCube(6, 6, new Vector3(5f, 1f, 0f), true, 50);
        GameObject plane = GameObject.Find("Plane");
        // plane.GetComponent<Material>() = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject cube = GameObject.Find("Cube");
    }
}
