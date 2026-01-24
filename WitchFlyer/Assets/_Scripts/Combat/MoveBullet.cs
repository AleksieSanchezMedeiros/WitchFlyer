using System.Collections;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 10f;

    void OnEnable()
    {
        StartCoroutine(ReturnBullet());
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private IEnumerator ReturnBullet()
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.GameObjects);
    }
}
