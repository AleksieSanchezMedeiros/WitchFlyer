using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public Transform spawnPostion;

    [SerializeField] private float distanceFromPlayer = 1f;

    // Update is called once per frame
    void Update()
    {
        RotateSpawnPoint();
    }

    private void RotateSpawnPoint()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = 0f;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spawnPostion.rotation = Quaternion.Euler(0f, 0f, angle);
        spawnPostion.position = transform.position + direction * distanceFromPlayer;
    }
}
