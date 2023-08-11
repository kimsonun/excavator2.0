using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class ExcavatorController : MonoBehaviour
{
    public static ExcavatorController Instance { get; private set; }

    [SerializeField] WheelCollider[] wheels;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject centerOfMass;

    [SerializeField] private GameObject[] tracks;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject boom;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject bucket;
    [SerializeField] private float rotateTrackSpeed = 50f;
    [SerializeField] private float rotateBodySpeed = 50f;
    [SerializeField] private float rotateBoomSpeed = 50f;
    [SerializeField] private float rotateArmSpeed = 50f;
    [SerializeField] private float rotateBucketSpeed = 50f;
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] private float score;

    private float highScore;

    private Rigidbody rig;
    private BoxCollider boxCollider;
    private bool isMoving = false;
    private float scoreReduceAmount = 10f;

    public event EventHandler onScoreChanged;

    private void Awake()
    {
        Instance = this;
        score = 1000f;
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rig = GetComponent<Rigidbody>();
        rig.centerOfMass = centerOfMass.transform.localPosition;
    }


    private void Update()
    {
        updateMotor();
        updateRotateAll();
        updateRotateBody();
        updateRotateBoom();
        updateRotateArm();
        updateRotateBucket();
    }

    private void FixedUpdate()
    {
        //movementWheel();
    }

    private void movementWheel()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = UnityEngine.Input.GetAxis("Vertical") * ((moveSpeed * 5) / 4);
        }
    }

    // movement
    private void updateMotor()
    {
        
        float t_vmove = UnityEngine.Input.GetAxisRaw("Vertical");
        if (t_vmove != 0)
        {
            transform.position += moveSpeed * t_vmove * Time.deltaTime * transform.forward;
            isMoving = true;
        } else
        {
            isMoving = false;
        }
        if (!isMoving)
        {
            rig.velocity = Vector3.zero;
        }
    }

    private void updateRotateAll()
    {
        if (UnityEngine.Input.GetKey(KeyCode.A))
        {   // rotate y axis
            transform.Rotate(Vector3.down * rotateTrackSpeed * Time.deltaTime);          
        }
        if (UnityEngine.Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.down * rotateTrackSpeed * Time.deltaTime);
        }
    }

    private void updateRotateBody()
    {
        if (UnityEngine.Input.GetKey(KeyCode.Q))
        { // rotate z axis
            body.transform.Rotate(Vector3.back * rotateBodySpeed * Time.deltaTime);
        }
        if (UnityEngine.Input.GetKey(KeyCode.E))
        {
            body.transform.Rotate(-Vector3.back * rotateBodySpeed * Time.deltaTime);
        }
    }

    private void updateRotateBoom()
    {
        Vector3 rotateBoomAmount = boom.transform.localRotation.eulerAngles;
        if (UnityEngine.Input.GetKey(KeyCode.U))
        { // rotate x axis
            rotateBoomAmount.x -= rotateBoomSpeed * Time.deltaTime;
        }
        if (UnityEngine.Input.GetKey(KeyCode.J))
        {
            rotateBoomAmount.x += rotateBoomSpeed * Time.deltaTime;
        }
        // clamp
        boom.transform.localEulerAngles = clampRotationWithMinMax(rotateBoomAmount, -50, 10);
    }

    private void updateRotateArm()
    {
        Vector3 rotateArmAmount = arm.transform.localEulerAngles;
        if (UnityEngine.Input.GetKey(KeyCode.I))
        { // rotate x axis
            rotateArmAmount.x -= rotateArmSpeed * Time.deltaTime;
        }
        if (UnityEngine.Input.GetKey(KeyCode.K))
        {
            rotateArmAmount.x += rotateArmSpeed * Time.deltaTime;
        }
        arm.transform.localEulerAngles = clampRotationWithMinMax(rotateArmAmount, -90, -30);
    }

    private void updateRotateBucket()
    {
        Vector3 rotateBucketAmount = bucket.transform.localEulerAngles;
        if (UnityEngine.Input.GetKey(KeyCode.O))
        { // rotate x axis
            rotateBucketAmount.x -= rotateBucketSpeed * Time.deltaTime;
        }
        if (UnityEngine.Input.GetKey(KeyCode.L))
        {
            rotateBucketAmount.x += rotateBucketSpeed * Time.deltaTime;
        }
        bucket.transform.localEulerAngles = clampRotationWithMinMax(rotateBucketAmount, -50, 10);
    }

    private Vector3 clampRotationWithMinMax(Vector3 rotateAmount, float minRotation, float maxRotation)
    {
        if (rotateAmount.x > 180) rotateAmount.x -= 360;
        rotateAmount.x = Mathf.Clamp(rotateAmount.x, minRotation, maxRotation);
        return rotateAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            reduceScore();
        }
    }

    private void reduceScore()
    {
        score -= scoreReduceAmount;
        onScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public float getScore()
    {
        return score;
    }

    public float getTrackRotateSpeed()
    {
        return rotateTrackSpeed;
    }
    public float getBodyRotateSpeed()
    {
        return rotateBodySpeed;
    }
    public float getBoomRotateSpeed()
    {
        return rotateBoomSpeed;
    }
    public float getArmRotateSpeed()
    {
        return rotateArmSpeed;
    }
    public float getBucketRotateSpeed()
    {
        return rotateBucketSpeed;
    }
    
    public void setTrackRotateSpeed(float s)
    {
        rotateTrackSpeed = s;
    }
    public void setBodyRotateSpeed(float s)
    {
        rotateBodySpeed = s;
    }
    public void setBoomRotateSpeed(float s)
    {
        rotateBoomSpeed = s;
    }
    public void setArmRotateSpeed(float s)
    {
        rotateArmSpeed = s;
    }
    public void setBucketRotateSpeed(float s)
    {
        rotateBucketSpeed = s;
    }
}