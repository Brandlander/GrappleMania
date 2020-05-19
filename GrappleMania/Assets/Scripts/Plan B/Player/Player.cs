using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(PlayerState))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
        public bool lockMouse;
    }

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] MouseInput MouseControl;
    [SerializeField] AudioController footSteps;
    [SerializeField] float minMoveThreshold;

    Vector3 previousPosition;

    bool isFalling;

    public PlayerAim playerAim;

    private Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (m_Rigidbody == null)
                m_Rigidbody = GetComponent<Rigidbody>();
            return m_Rigidbody;
        }
    }

    private MovementController m_MovementController;
    public MovementController MovementController
    {
        get
        {
            if (m_MovementController == null)
                m_MovementController = GetComponent<MovementController>();
            return m_MovementController;
        }
    }

    private PlayerState m_PlayerState;
    public PlayerState PlayerState
    {
        get
        {
            if (m_PlayerState == null)
                m_PlayerState = GetComponentInChildren<PlayerState>();
            return m_PlayerState;
        }
    }

    private PlayerShoot m_PlayerShoot;
    public PlayerShoot PlayerShoot
    {
        get
        {
            if (m_PlayerShoot == null)
                m_PlayerShoot = GetComponentInChildren<PlayerShoot>();
            return m_PlayerShoot;
        }
    }

    InputController playerInput;
    Vector2 mouseInput;

    void Awake()
    {
        playerInput = GameManager.Instance.InputController;
        GameManager.Instance.LocalPlayer = this;

        if(MouseControl.lockMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!Paused.GameIsPaused)
        {
            Move();
            Jump();
            LookAround();
        }
    }

    void Move()
    {
        float moveSpeed = walkSpeed;

        if (playerInput.IsRunning)
            moveSpeed = runSpeed;

        Vector2 direction = new Vector2(playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);
        MovementController.Move(direction);

        if (Vector3.Distance(transform.position, previousPosition) > minMoveThreshold && !isFalling)
            footSteps.Play();

        previousPosition = transform.position;
    }

    void Jump()
    {
        // isFalling is false if player is colliding with something
        if (playerInput.Jump && isFalling == false)
        {
            Rigidbody.velocity = new Vector3(0f, jumpHeight, 0f);
            isFalling = true;
        }
    }

    void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);

        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collided with " + collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        isFalling = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isFalling = true;
    }
}
