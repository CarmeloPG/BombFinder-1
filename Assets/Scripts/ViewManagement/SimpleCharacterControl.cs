using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Mobile compatible character control.
/// </summary>
public class SimpleCharacterControl : MonoBehaviour {
    #region Readonly fields
    private readonly float m_interpolation = 10f;
    private readonly float m_runScale = 2f; //0.33f;
    //private readonly string out_of_tile_tag = "Respawn";
    //private readonly float m_backwardsWalkScale = 0.33f;
    //private readonly float m_backwardRunScale = 0.66f;
    #endregion

    #region Serialize fields
    [SerializeField] 
    private float m_moveSpeed = 2f;
    /*[SerializeField]
    private bool m_enableJump = false;*/
    [SerializeField] 
    private float m_jumpForce = 4f;

    [SerializeField] 
    private Animator m_animator;
    [SerializeField] 
    private Rigidbody m_rigidBody;
    #endregion

    #region Private fields
    private float m_currentV = 0f;
    private float m_currentH = 0f;
    private Vector2 
        movement,
        deltaCameraRotation;

    private bool run;
    private Vector3
        direction,
        m_currentDirection;

    private float m_jumpTimeStamp = 0f;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;

    private bool m_isGrounded;

    private VirtualControls virtualControls = null;
    private Transform cameraTransform;
    private TrackingObject controlsCamera;
    private InputAction 
        lookAction,
        moveAction,
        runAction;
    #endregion

    #region Properties
    public float MoveSpeed {
        set => m_moveSpeed = value;
    }

    public VirtualControls VirtualControls {
        set {
            //if (virtualControls == null) {
                virtualControls = value;
            //}
        }
    }

    public TrackingObject ControlsCamera {
        set {
            controlsCamera = value;
            cameraTransform = controlsCamera.transform;
        }
    }
    #endregion

    #region Events
    /*[Header("Events")]
    [Tooltip("When it is out of the tiles fiels, it sends an event each frame.")]
    public UnityEvent OnExitTile;*/
    #endregion

    #region Unity methods
    private void Start() {
        m_currentDirection = Vector3.zero;
        movement = Vector2.zero;
        lookAction = InputSystem.actions.FindAction("Look");
        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Run");
    }

    private void OnCollisionEnter(Collision collision) {
        m_isGrounded = true;
        
        m_animator.SetBool("Grounded", m_isGrounded);

        /*if (collision.gameObject.tag.Equals(out_of_tile_tag)) {
            OnExitTile.Invoke();
        }
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) {
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }*/
    }

    /*private void OnCollisionStay(Collision collision) {
        if (!m_isGrounded) {
            m_isGrounded = true;
        }
        /*ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) {
                validSurfaceNormal = true;
                break;
            }
        }

        if (validSurfaceNormal) {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider)) {
                m_collisions.Add(collision.collider);
            }
        }
        else {
            if (m_collisions.Contains(collision.collider)) {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) {
                m_isGrounded = false;
            }
        }
    }*/

    private void OnCollisionExit(Collision collision) {
        m_isGrounded = false;
        /*if (m_collisions.Contains(collision.collider)) {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) {
            m_isGrounded = false;
        }*/
    }

    /*private void Update() {
        if (m_enableJump && !m_jumpInput && Input.GetKey(KeyCode.Space)) {
            m_jumpInput = true;
        }
    }*/

    private void FixedUpdate() {
        if (cameraTransform != null) {
            if (virtualControls == null) {
                deltaCameraRotation = lookAction.ReadValue<Vector2>();
                movement = moveAction.ReadValue<Vector2>();
                run = runAction.ReadValue<float>() > 0f;
            }
            else {
                deltaCameraRotation = virtualControls.Look;
                movement = virtualControls.Move;
                run = virtualControls.Run;
            }

            if (deltaCameraRotation != Vector2.zero) {
                controlsCamera.Rotate(deltaCameraRotation);
            }

            if (movement != Vector2.zero) {
                ControlCharacter();
            }
            else if (direction != Vector3.zero) {
                direction = Vector3.zero;
                m_animator.SetFloat("MoveSpeed", 0f);
            }
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    public void ControlCharacter() {
        if (run) {
            movement *= m_runScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, movement.y, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, movement.x, Time.deltaTime * m_interpolation);

        direction = cameraTransform.forward * m_currentV + cameraTransform.right * m_currentH;

        //float directionLength = direction.magnitude;
        direction = direction.normalized * direction.magnitude;
        direction.y = 0;
        //direction = direction.normalized * directionLength;

        //if (direction != Vector3.zero) {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            if (m_animator != null) {
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        //}
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /*private void TankUpdate() {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        bool run = Input.GetKey(KeyCode.LeftShift);

        if (verticalAxis < 0) {
            if (run) {
                verticalAxis *= m_backwardRunScale;
            }
            else {
                verticalAxis *= m_backwardsWalkScale;
            }
        }
        else if (run) {
            verticalAxis *= m_runScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, verticalAxis, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, horizontalAxis, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        if (verticalAxis == 0f) {
            m_animator.SetFloat("MoveSpeed", 0f);
        }
        else if (verticalAxis > 0f) {
            m_animator.SetFloat("MoveSpeed", m_currentV);
        }
        else {
            m_animator.SetFloat("MoveSpeed", -m_currentV);
        }

        JumpingAndLanding();
    }

    private void MobileUpdate() {
        float verticalAxis = joystick.Vertical;
        float horizontalAxis = joystick.Horizontal;
        bool run = joystick.Run;

        if (run) {
            verticalAxis *= m_runScale;
            horizontalAxis *= m_runScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, verticalAxis, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, horizontalAxis, Time.deltaTime * m_interpolation);

        Vector3 direction = Vector3.forward * m_currentV + Vector3.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero) {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            if (m_animator != null) {
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        }

        JumpingAndLanding();
    }*/

    private void JumpingAndLanding() {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && m_jumpInput) {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}