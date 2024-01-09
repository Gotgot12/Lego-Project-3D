using UnityEngine;

public class StickLegos : MonoBehaviour
{
    public float snapDistance = 1.0f;
    public Material glowMaterial;
    private Material originalMaterial1;
    private Material originalMaterial2;
    private bool isStuck = false;
    private bool lego1Glowing = false;
    private bool lego2Glowing = false;

    public GameObject lego1;  // Assign in the inspector
    public GameObject lego2;  // Assign in the inspector
    public GameObject combinedObject;

    void Awake()
    {
        combinedObject = new GameObject("CombinedObject");
    }

    void Start()
    {
        // Set up original materials for legos
        originalMaterial1 = lego1.GetComponent<Renderer>().material;
        originalMaterial2 = lego2.GetComponent<Renderer>().material;

        // Set up materials for glowing
        // ...
    }

    void Update()
    {
        // Handle input or other conditions to trigger attachment
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStuck && lego1Glowing && lego2Glowing)
            {
                AttachLegos();
            }
            else
            {
                DetachLegos();
            }
        }

        // Check if legos are close enough to glow
        UpdateGlowStatus();
    }

    void AttachLegos()
    {
        // Check if lego1 and lego2 are not null
        if (lego1 != null && lego2 != null)
        {
            // Check if legos are within the snap range
            if (AreLegosWithinSnapRange())
            {
                // Find close snapPoints on lego1 and lego2
                Vector3 snapPoint1 = FindClosestSnapPoint(lego1, lego2);
                Vector3 snapPoint2 = FindClosestSnapPoint(lego2, lego1);

                // Move lego2 to snapPoint2 on lego1
                lego2.transform.position = snapPoint2;

                // Parent lego2 to lego1
                lego2.transform.parent = lego1.transform;

                // Add Rigidbody to the new combined object
                Rigidbody combinedObjectRigidbody = combinedObject.AddComponent<Rigidbody>();
                combinedObject.AddComponent<GrabObject>(); // Add the GrabObject script
                combinedObjectRigidbody.mass = lego1.GetComponent<Rigidbody>().mass + lego2.GetComponent<Rigidbody>().mass;

                // Destroy individual Rigidbody components and GrabObject script
                Destroy(lego1.GetComponent<Rigidbody>());
                Destroy(lego2.GetComponent<Rigidbody>());
                Destroy(lego2.GetComponent<GrabObject>());

                // Set the position of the new combined object to the average position of lego1 and lego2
                combinedObject.transform.position = (lego1.transform.position + lego2.transform.position) / 2;

                isStuck = true;

                // Reset the materials after sticking them
                ResetMaterial(lego1);
                ResetMaterial(lego2);
            }
        }
    }

    void DetachLegos()
    {
        // Implement detaching logic if needed
        // For example, you may want to unparent lego2 and remove the Rigidbody from combinedObject
    }

    void UpdateGlowStatus()
    {
        // Implement logic to check if legos are close enough to glow
        float distance = Vector3.Distance(lego1.transform.position, lego2.transform.position);
        lego1Glowing = distance < snapDistance;
        lego2Glowing = distance < snapDistance;

        // Update materials based on glow status
        if (lego1Glowing)
        {
            SetGlowMaterial(lego1);
        }
        else
        {
            ResetMaterial(lego1);
        }

        if (lego2Glowing)
        {
            SetGlowMaterial(lego2);
        }
        else
        {
            ResetMaterial(lego2);
        }
    }

    Vector3 FindClosestSnapPoint(GameObject lego, GameObject otherLego)
    {
        // Implement logic to find the closest snapPoint on lego to otherLego
        // For now, return the center of the lego as a placeholder
        return lego.transform.position;
    }

    bool AreLegosWithinSnapRange()
    {
        // Implement logic to check if lego1 and lego2 are within snapDistance
        float distance = Vector3.Distance(lego1.transform.position, lego2.transform.position);
        return distance < snapDistance;
    }

    void SetGlowMaterial(GameObject lego)
    {
        // Implement logic to set glow material on lego
        Renderer legoRenderer = lego.GetComponent<Renderer>();
        if (legoRenderer != null && glowMaterial != null)
        {
            legoRenderer.material = glowMaterial;
        }
    }

    void ResetMaterial(GameObject lego)
    {
        // Implement logic to reset the material on lego
        Renderer legoRenderer = lego.GetComponent<Renderer>();
        if (legoRenderer != null)
        {
            if (lego == lego1)
            {
                legoRenderer.material = originalMaterial1;
            }
            else if (lego == lego2)
            {
                legoRenderer.material = originalMaterial2;
            }
        }
    }
}