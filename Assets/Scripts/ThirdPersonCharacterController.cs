using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCharacterController : MonoBehaviour
{
    public CharacterController controller;
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float TurnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f; // Используем стандартное значение гравитации
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    [Space]
    public Transform cameraTransform;
    
    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // Проверка земли с визуализацией в редакторе
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, 0.1f, groundLayer);
        Debug.Log("Grounded: " + isGrounded);

        // Прыжок
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && velocity.y < 0.1f)
        {
            velocity.y = jumpForce; // Физически корректный прыжок
        }

        // Применяем гравитацию
        velocity.y -= gravity * Time.deltaTime;
        
        // Движение
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(hor, 0, ver).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // Применяем вертикальное движение (прыжок/гравитация)
        controller.Move(velocity * Time.deltaTime);
    }

    // Визуализация проверки земли в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, 0.2f);
    }
}