using Status;
using TMPro;
using UnityEngine;

public class BouncingText : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;

    [SerializeField] private float floorHeight = 0.05f;
    [SerializeField] private float sleepThreshold = 0.05f;
    [SerializeField] private float bounceMultiplier = 0.75f;
    [SerializeField] private float gravity = -1f;

    private Player _player;
    private string _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>().text;
    }
    
    private void FixedUpdate()
    {
        if (_text != "New") return;
        if (_velocity.magnitude > sleepThreshold || transform.position.y > floorHeight) 
        {
            _velocity.y += gravity * Time.fixedDeltaTime;
        }
        transform.position += _velocity * Time.fixedDeltaTime;
                
        if (transform.position.y <= floorHeight) 
        {
            transform.position = new Vector3(transform.position.x, floorHeight, transform.position.z);
            _velocity.y = -_velocity.y;
            _velocity *= bounceMultiplier;
        }
    }
}
