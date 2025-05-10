using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет поведением автомобиля: движение, торможение, поворот, эффекты.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public GameObject vent, Water;
    public bool isControllingCar = false;
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Wheel[] _wheels;

    [Header("Settings")]
    [SerializeField] private int _motorForce = 1500;
    [SerializeField] private int _brakeForce = 3000;
    [SerializeField] private AnimationCurve _steeringCurve;
    [SerializeField] private float _slipAllowance = 0.4f;
    #endregion

    #region Private Fields
    private Rigidbody _rigidbody;
    private float _verticalInput;
    private float _horizontalInput;
    private float _brakeInput;
    private float _speed;
    #endregion
    
    IEnumerator CaringCor() {
        yield return new WaitForSeconds(1.5f);
        isControllingCar = true;
    }
    public void Caring() {
        vent.transform.GetChild(1).gameObject.SetActive(false);
        vent.transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(CaringCor());
    }

    #region Unity Methods
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        if (transform.position.y < 1f) {
            isControllingCar = false;
            if (Water.transform.position.y > 4) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacterController>().StopCaring();
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacterController>().Die();
            }
        }
        if (isControllingCar) {
            CheckInput();
            Move();
            Brake();
            Steer();
        }
    }
    #endregion

    #region Car Logic
    /// <summary>
    /// Обрабатывает пользовательский ввод.
    /// </summary>
    private void CheckInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        float movingDirection = Vector3.Dot(transform.forward, _rigidbody.linearVelocity);
        if ((movingDirection > 0.1f && _verticalInput < 0) || (movingDirection < -0.1f && _verticalInput > 0))
        {
            _brakeInput = Mathf.Abs(_verticalInput);
        }
        else
        {
            _brakeInput = 0;
        }
    }

    /// <summary>
    /// Применяет силу к колесам для движения.
    /// </summary>
    private void Move()
    {
        _speed = _rigidbody.linearVelocity.magnitude;
        foreach (Wheel wheel in _wheels)
        {
            wheel.WheelCollider.motorTorque = _verticalInput * _motorForce;
            wheel.UpdateMeshPosition();
        }
    }

    /// <summary>
    /// Применяет тормозное усилие к колесам.
    /// </summary>
    private void Brake()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsFrontWheel)
            {
                wheel.WheelCollider.brakeTorque = _brakeInput * _brakeForce * 0.7f;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = _brakeInput * _brakeForce * 0.35f;
            }
        }
    }

    /// <summary>
    /// Управляет углом поворота передних колес.
    /// </summary>
    private void Steer()
    {
        float steeringAngle = _horizontalInput * _steeringCurve.Evaluate(_speed);
        float slipAngle = Vector3.Angle(transform.forward, _rigidbody.linearVelocity - transform.forward);

        if (slipAngle < 120)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, _rigidbody.linearVelocity, Vector3.up);
        }

        steeringAngle = Mathf.Clamp(steeringAngle, -48, 48);
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsFrontWheel)
            {
                wheel.WheelCollider.steerAngle = steeringAngle;
            }
        }
    }
    #endregion
}

/// <summary>
/// Класс, описывающий колесо автомобиля.
/// </summary>
[System.Serializable]
public class Wheel
{
    public Transform WheelMesh;
    public WheelCollider WheelCollider;
    public bool IsFrontWheel;

    /// <summary>
    /// Обновляет позицию и вращение меша колеса в соответствии с WheelCollider.
    /// </summary>
    public void UpdateMeshPosition()
    {
        Vector3 position;
        Quaternion rotation;
        WheelCollider.GetWorldPose(out position, out rotation);
        WheelMesh.position = position;
        WheelMesh.rotation = rotation;
    }
}