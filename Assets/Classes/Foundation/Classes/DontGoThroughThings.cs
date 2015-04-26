using Assets.Classes.Foundation.Extensions;
using UnityEngine;
using System.Collections;

public class DontGoThroughThings : MonoBehaviour
{
    public LayerMask layerMask; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody2D myRigidbody;
    private Collider2D c;
 


    //initialize values 
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        c = GetComponent<Collider2D>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(c.bounds.extents.x, c.bounds.extents.y), c.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    void FixedUpdate()
    {

        previousPosition = myRigidbody.position;
    }
}