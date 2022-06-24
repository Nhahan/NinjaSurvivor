using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, -15 * Time.deltaTime * Random.Range(0, 15));
    }
}
