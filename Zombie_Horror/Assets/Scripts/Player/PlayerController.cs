using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [Space]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _speedShift = 6f;
    [Space]
    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private float _lookXLimit = 45.0f;
    [Space]

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;
    private float _speedVar;

    public bool _canMove = true;

    private void Start()
    {
        TryGetComponent(out _rigidbody);
        _rotation.y = transform.eulerAngles.y;
        _speedVar = _speed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) _speedVar = _speedShift;
        else _speedVar = _speed;
            
        _rotation.y += Input.GetAxis("Mouse X") * _lookSpeed;
        _rotation.x += -Input.GetAxis("Mouse Y") * _lookSpeed;
        _rotation.x = Mathf.Clamp(_rotation.x, -_lookXLimit, _lookXLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, _rotation.y);
    }

    private void FixedUpdate()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
        var curSpeedX = _canMove ? _speedVar * Input.GetAxis("Vertical") : 0;
        var curSpeedY = _canMove ? _speedVar * Input.GetAxis("Horizontal") : 0;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        
        _rigidbody.AddForce(_moveDirection, ForceMode.Impulse);
    }
}