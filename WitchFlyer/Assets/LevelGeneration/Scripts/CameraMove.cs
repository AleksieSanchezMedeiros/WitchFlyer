using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //Aleksie Sanchez
    //Camera Movement as well as invisible wall movement

    [Header("Scrolling")]
    public float scrollSpeed = 0.1f; // horizontal auto-scroll speed, adjust as needed

    [Header("Wall Colliders")]
    public BoxCollider2D leftWall;
    public BoxCollider2D rightWall;
    public BoxCollider2D topWall;
    public BoxCollider2D bottomWall;
    public BoxCollider2D killWall;
    

    [Header("Wall Settings")]
    public float wallThickness = 0.5f;
    public float verticalPadding = 2f;
    public float horizontalPadding = 1f;
    
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (!leftWall || !rightWall || !topWall || !bottomWall)
        {
            Debug.LogError("Assign all four wall colliders.");
            enabled = false;
        }
    }

    void Update()
    {
        //move camera to the right
        transform.position -= Vector3.right * -scrollSpeed * Time.deltaTime;

        // position collider at left edge of screen
        PositionPushColliders();

        IgnoreEnemiesWithRightWall();
    }

    void PositionPushColliders()
    {
       float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float leftX   = transform.position.x - camWidth / 2f;
        float rightX  = transform.position.x + camWidth / 2f;
        float topY    = transform.position.y + camHeight / 2f;
        float bottomY = transform.position.y - camHeight / 2f;

        // LEFT WALL
        leftWall.transform.position = new Vector3(
            leftX - horizontalPadding + wallThickness / 2f,
            transform.position.y,
            0f
        );
        leftWall.size = new Vector2(
            wallThickness,
            camHeight + verticalPadding
        );

        // KILL WALL
        killWall.transform.position = new Vector3(
            leftX - horizontalPadding + wallThickness / 2f,
            transform.position.y,
            0f
        );
        killWall.size = new Vector2(
            wallThickness,
            camHeight + verticalPadding
        );

        // RIGHT WALL
        rightWall.transform.position = new Vector3(
            rightX + horizontalPadding - wallThickness / 2f,
            transform.position.y,
            0f
        );
        rightWall.size = new Vector2(
            wallThickness,
            camHeight + verticalPadding
        );

        // TOP WALL
        topWall.transform.position = new Vector3(
            transform.position.x,
            topY + verticalPadding / 2f,
            0f
        );
        topWall.size = new Vector2(
            camWidth + horizontalPadding,
            wallThickness
        );

        // BOTTOM WALL
        bottomWall.transform.position = new Vector3(
            transform.position.x,
            bottomY - verticalPadding / 2f,
            0f
        );
        bottomWall.size = new Vector2(
            camWidth + horizontalPadding,
            wallThickness
        );
    }

    void IgnoreEnemiesWithRightWall()
{
    GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

    foreach (var enemy in allEnemies)
    {
        Collider2D enemyCol = enemy.GetComponent<Collider2D>();
        if (enemyCol != null)
            Physics2D.IgnoreCollision(rightWall, enemyCol, true);
    }
}
}
