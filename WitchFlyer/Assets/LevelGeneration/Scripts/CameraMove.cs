using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //Aleksie Sanchez
    //Camera Movement as well as invisible wall movement

    [Header("Scrolling")]
    public float scrollSpeed = 0.1f; // horizontal auto-scroll speed, adjust as needed

    [Header("Push Collider")]
    public BoxCollider2D pushCollider; // the collider that pushes the, has to be a child object, see LEVELGENEXAMPLE SCENE
    public float colliderThickness = 0.5f; // how thick the left wall is, 0.5 usually stops things from escaping
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (pushCollider == null)
        {
            Debug.LogError("Assign a BoxCollider2D as the pushCollider. Do so as a child, see example");
            return;
        }
    }

    void Update()
    {
        //move camera to the right
        transform.position -= Vector3.right * -scrollSpeed * Time.deltaTime;

        // position collider at left edge of screen
        PositionPushCollider();
    }

    void PositionPushCollider()
    {
        // lamera height in world units
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // Left edge of visible screen in world space
        float leftEdgeX = transform.position.x - (camWidth / 2f);

        // position collider at left edge
        Vector3 colPos = pushCollider.transform.position;
        colPos.x = leftEdgeX + (colliderThickness / 2f);
        colPos.y = transform.position.y; // match camera center vertically
        pushCollider.transform.position = colPos;

        // resize collider to match camera height
        pushCollider.size = new Vector2(colliderThickness, camHeight);
    }
}
