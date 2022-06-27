using UnityEngine;

public class Indicator : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
    }
}
