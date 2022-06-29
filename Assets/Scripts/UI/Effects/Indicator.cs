using TMPro;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private TextMeshPro _text;
    
    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        Destroy(gameObject, 1f);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - Time.deltaTime);
    }
}
