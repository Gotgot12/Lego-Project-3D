using UnityEngine;

public class StickCubes : MonoBehaviour
{
    public float glowDistance = 1.0f;
    public float gridSize = 1.0f;
    public Material glowMaterial;
    private Material originalMaterial1;
    private Material originalMaterial2;
    private bool isStuck = false;
    private bool cube1Glowing = false;
    private bool cube2Glowing = false;
 

    public GameObject cube1;  // Assign in the inspector
    public GameObject cube2;  // Assign in the inspector
    public GameObject combinedObject;
    
    void Awake()
    {
        combinedObject = new GameObject("CombinedObject");
    }

    void Start()
    {
        if (cube1 != null)
        {
            Renderer cube1Renderer = cube1.GetComponent<Renderer>();
            if (cube1Renderer != null)
            {
                originalMaterial1 = cube1Renderer.material;
            }
        }

        if (cube2 != null)
        {
            Renderer cube2Renderer = cube2.GetComponent<Renderer>();
            if (cube2Renderer != null)
            {
                originalMaterial2 = cube2Renderer.material;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStuck && cube1Glowing && cube2Glowing)
            {
                AttachCubes();
            }
            else
            {
                DetachCubes();
            }
        }

        if (cube1 != null && cube2 != null)
        {
            float distance = Vector3.Distance(
                GetNearestGridPoint(cube1.transform.position),
                GetNearestGridPoint(cube2.transform.position)
            );

            if (Mathf.Abs(distance) < glowDistance && !isStuck)
            {
                SetGlowMaterial(cube1);
                cube1Glowing = true;
                SetGlowMaterial(cube2);
                cube2Glowing = true;
            }
            else
            {
                ResetMaterial1(cube1);
                cube1Glowing = false;
                ResetMaterial2(cube2);
                cube2Glowing = false;
            }
        }
    }

    void AttachCubes()
    {
        if (cube1 != null && cube2 != null)
        {
            float distance = Vector3.Distance(
                GetNearestGridPoint(cube1.transform.position),
                GetNearestGridPoint(cube2.transform.position)
            );

            Rigidbody cube1Rigidbody = cube1.GetComponent<Rigidbody>();
            Rigidbody cube2Rigidbody = cube2.GetComponent<Rigidbody>();

            // Check if the cubes are within the stick range
            if (distance <= glowDistance)
            {
                if (cube1 != null && cube2 != null)
                {
                    Vector3 relativePosition = cube2.transform.position - cube1.transform.position;
                    if (Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.z) && Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.y))
                    {
                        if (relativePosition.x < 0)
                        {
                            // Cube 2 à gauche de Cube 1
                            Vector3 nearestGridPoint = GetNearestGridPoint(cube1.transform.position - cube1.transform.right * gridSize);
                            cube2.transform.position = nearestGridPoint;
                        }
                        else
                        {
                            // Cube 2 à droite de Cube 1
                            Vector3 nearestGridPoint = GetNearestGridPoint(cube1.transform.position + cube1.transform.right * gridSize);
                            cube2.transform.position = nearestGridPoint;
                        }
                    }

                    else if (Mathf.Abs(relativePosition.y) > Mathf.Abs(relativePosition.z) && Mathf.Abs(relativePosition.y) > Mathf.Abs(relativePosition.x))
                    {
                        if (relativePosition.y < 0)
                        {
                            // Cube 2 en dessous de Cube 1
                            Vector3 nearestGridPoint = GetNearestGridPoint(cube1.transform.position - cube1.transform.up * gridSize);
                            cube2.transform.position = nearestGridPoint;
                        }
                        else
                        {
                            // Cube 2 au dessus de Cube 1
                            Vector3 nearestGridPoint = GetNearestGridPoint(cube1.transform.position + cube1.transform.up * gridSize);
                            cube2.transform.position = nearestGridPoint;
                        }
                    }
                    else if (Mathf.Abs(relativePosition.z) > Mathf.Abs(relativePosition.y) && Mathf.Abs(relativePosition.z) > Mathf.Abs(relativePosition.x))
                    {
                        if (relativePosition.z < 0)
                        {
                            // Cube 2 devant Cube 1
                            Vector3 nearestGridPoint = GetNearestGridPoint(cube1.transform.position - cube1.transform.forward * gridSize);
                            cube2.transform.position = nearestGridPoint;
                        }
                        else
                        {
                            // Cube 2 derrière Cube 1
                            Vector3 nearestGridPoint = GetNearestGridPoint(cube1.transform.position + cube1.transform.forward * gridSize);
                            cube2.transform.position = nearestGridPoint;
                        }
                    }
                   
                }

                // Add Rigidbody to the new combined object
                Rigidbody combinedObjectRigidbody = combinedObject.AddComponent<Rigidbody>();
                combinedObject.AddComponent<GrabObject>(); // Add the GrabObject script
                combinedObjectRigidbody.mass = cube1Rigidbody.mass + cube2Rigidbody.mass;

                // Destroy individual Rigidbody components and GrabObject script
                Destroy(cube1Rigidbody);
                Destroy(cube2Rigidbody);
                Destroy(cube1.GetComponent<GrabObject>());

                // Set the position of the new combined object to the nearest grid point
                combinedObject.transform.position = GetNearestGridPoint((cube1.transform.position + cube2.transform.position) / 2);

                // Parent both cubes to the combined object
                cube1.transform.parent = combinedObject.transform;
                cube2.transform.parent = combinedObject.transform;

                isStuck = true;

                // Reset the material after sticking them
                ResetMaterial1(cube1);
                ResetMaterial2(cube2);

            }
        }
    }

    void DetachCubes()
    {
        if (cube1 != null && cube2 != null)
        {
            FixedJoint fixedJoint = cube1.GetComponent<FixedJoint>();
            if (fixedJoint != null)
            {
                // Destroy the FixedJoint component on cube1
                Destroy(fixedJoint);

                // Enable the Rigidbody and movement controls on Cube1
                Rigidbody cube1Rigidbody = cube1.GetComponent<Rigidbody>();
                cube1Rigidbody.constraints = RigidbodyConstraints.None;
            }

            Rigidbody cube2Rigidbody = cube2.GetComponent<Rigidbody>();
            cube2Rigidbody.useGravity = true;

            isStuck = false;
        }
    }

    void SetGlowMaterial(GameObject cube)
    {
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        if (cubeRenderer != null && glowMaterial != null)
        {
            cubeRenderer.material = glowMaterial;
        }
    }

    void ResetMaterial1(GameObject cube)
    {
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        if (cubeRenderer != null && originalMaterial1 != null)
        {
            cubeRenderer.material = originalMaterial1;
        }
    }

    void ResetMaterial2(GameObject cube)
    {
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        if (cubeRenderer != null && originalMaterial2 != null)
        {
            cubeRenderer.material = originalMaterial2;
        }
    }

    Vector3 GetNearestGridPoint(Vector3 position)
    {
        // Snap the position to the nearest grid point
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(x, y, z);
    }
}
