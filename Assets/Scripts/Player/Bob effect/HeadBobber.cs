using DefaultNamespace.player;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [SerializeField] private HeadBob _headBob;
    private Vector3 _originalLocalPos;
    private Vector2 _lookAt;
    private float _sensitivity;
    private float _nextStepTime = 0.5f;
    private float _headBobCycle;
    private float headBobFade;

    // Fields for simple spring calculation:
    private float springPos = 0;
    private float springVelocity = 0;
    private float springElastic = 1.1f;
    private float springDampen = 0.8f;
    private float springVelocityThreshold = 0.05f;
    private float springPositionThreshold = 0.05f;
    
    private Vector3 prevPosition; // the position from last frame
    private Vector3 prevVelocity = Vector3.zero; // the velocity from last frame
    private Vector3 velocity, velocityChange;
    private bool prevGrounded = true; // whether the character was grounded last frame

    private float flatVelocity, strideLengthen, bobFactor, bobSwayFactor, speedHeightFactor, xPos, yPos, xTilt, zTilt, stepVolume;
    private float InputX, InputY;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        _originalLocalPos = _headBob.MainCamera.localPosition;
        prevPosition = transform.position;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = (transform.position - prevPosition) / Time.deltaTime;
        velocityChange = velocity - prevVelocity;
        prevPosition = transform.position;
        prevVelocity = velocity;

        // vertical head position "spring simulation" for jumping/landing impacts
        springVelocity -= velocityChange.y; // input to spring from change in character Y velocity
        springVelocity -= springPos * springElastic; // elastic spring force towards zero position
        springVelocity *= springDampen; // damping towards zero velocity
        springPos += springVelocity * Time.deltaTime; // output to head Y position
        springPos = Mathf.Clamp(springPos, -.3f, .3f); // clamp spring distance

        // snap spring values to zero if almost stopped:
        if ((Mathf.Abs(springVelocity) < springVelocityThreshold && Mathf.Abs(springPos) < springPositionThreshold))
        {
            springVelocity = 0;
            springPos = 0;
        }

        // head bob cycle is based on "flat" velocity (i.e. excluding Y)
        flatVelocity = new Vector3(velocity.x, 0, velocity.z).magnitude;

        // lengthen stride based on speed (so run bobbing isn't lots of little steps)
        strideLengthen = 1 + (flatVelocity * _headBob.strideSpeedLengthen);

        // increment cycle
        _headBobCycle += (flatVelocity / strideLengthen) * (Time.deltaTime / _headBob.BobFrequency);

        // actual bobbing and swaying values calculated using Sine wave
        bobFactor = Mathf.Sin(_headBobCycle * Mathf.PI * 2);
        bobSwayFactor =
            Mathf.Sin(_headBobCycle * Mathf.PI * 2 + Mathf.PI * .5f); // sway is offset along the sin curve by a quarter-turn in radians
        bobFactor = 1 - (bobFactor * .5f + 1); // bob value is brought into 0-1 range and inverted
        bobFactor *= bobFactor; // bob value is biased towards 0

        // fade head bob effect to zero if not moving
        if (new Vector3(velocity.x, 0, velocity.z).magnitude < 0.1f)
        {
            headBobFade = Mathf.Lerp(headBobFade, 0, Time.deltaTime);
        }
        else
        {
            headBobFade = Mathf.Lerp(headBobFade, 1, Time.deltaTime);
        }

        // height of bob is exaggerated based on speed
        speedHeightFactor = 1 + (flatVelocity * _headBob.heightSpeedMultiplier);

        // finally, set the position and rotation values
        xPos = -_headBob.BobSideMovement * bobSwayFactor;
        yPos = springPos * _headBob.jumpLandMove + bobFactor * _headBob.BobHeight * headBobFade * speedHeightFactor;
        xTilt = -springPos * _headBob.jumpLandTilt;
        zTilt = bobSwayFactor * _headBob.BobSwayAngle * headBobFade;

        _headBob.MainCamera.localPosition = _originalLocalPos + new Vector3(xPos, yPos, 0);
        _headBob.MainCamera.localRotation = Quaternion.Euler(xTilt, 0, zTilt);
    }
}