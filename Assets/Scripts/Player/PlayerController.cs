using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;
    [Space]
    [SerializeField] private float _speed = 20;
    [SerializeField] private float _speedShift = 40;
    [SerializeField] private float _factor = 40;
    [Space]
    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private float _lookXLimit = 80;
    [Space]

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;
    private float _speedVar;

    public bool CanMove = true;

    private void Start()
    {
        if (!_rigidbody) TryGetComponent(out _rigidbody);
        _rotation.y = transform.eulerAngles.y;
        _speedVar = _speed;

        StartCoroutine(FootSound());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) _speedVar = _speedShift;
        else _speedVar = _speed;

        var mouseX = CanMove ? Input.GetAxis("Mouse X") : 0;
        var mouseY = CanMove ? -Input.GetAxis("Mouse Y") : 0;

        _rotation.y += mouseX * _lookSpeed;
        _rotation.x += mouseY * _lookSpeed;
        _rotation.x = Mathf.Clamp(_rotation.x, -_lookXLimit, _lookXLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, _rotation.y);
    }

    private void FixedUpdate()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
        var curSpeedX = CanMove ? _speedVar * Input.GetAxis("Vertical") : 0;
        var curSpeedY = CanMove ? _speedVar * Input.GetAxis("Horizontal") : 0;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        _rigidbody.AddForce(_moveDirection, ForceMode.Impulse);
    }

    private IEnumerator FootSound()
    {
        while (true)
        {
            if (_moveDirection != Vector3.zero)
            {
                _audioSource.clip = _clips[0];
                _audioSource.Play();
                _clips = Shuffle(_clips);
                yield return new WaitForSeconds(_factor * (1 / _speedVar));
            }
            yield return null;
        }
    }

    private T[] Shuffle<T>(T[] array)
    {
        var rnd = new System.Random();
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            var r = rnd.Next(0, n);
            var t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
        return array;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}