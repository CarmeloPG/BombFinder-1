using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mobile compatible character control.
/// </summary>
public class SimpleCharacterControl : MonoBehaviour {
    #region Readonly fields
    private readonly float m_interpolation = 10f;
    private readonly float m_runScale = 2f; //0.33f;
    private readonly float m_backwardsWalkScale = 0.33f;
    private readonly float m_backwardRunScale = 0.66f;
    #endregion

    #region Serialize fields
    [SerializeField] 
    private float m_moveSpeed = 2f;
    [SerializeField] 
    private float m_turnSpeed = 200f;
    [SerializeField]
    private bool m_enableJump = false;
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

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0f;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;

    private bool m_isGrounded;

    private List<Collider> m_collisions = new List<Collider>();

    private VirtualControls virtualControls = null;
    #endregion

    #region Properties
    public float MoveSpeed {
        set => m_moveSpeed = value;
    }

    public VirtualControls VirtualControls {
        set {
            if (virtualControls == null) {
                Debug.Log("Joystick is null.");
            }
            else {
                Debug.Log("Joystick already exists.");
            }

            virtualControls = value;
        }
    }
    #endregion

    #region Events
    /*[Header("Events")]
    [Tooltip("When it is walking, it sends an event each frame.")]
    public UnityEvent OnWalk;*/
    #endregion

    #region Unity methods
    /*private void Awake() {
        if (!m_animator) {
            gameObject.GetComponent<Animator>();
        }
        if (!m_rigidBody) {
            gameObject.GetComponent<Animator>();
        }
    }^*/

    private void OnCollisionEnter(Collision collision) {
        m_isGrounded = true;
        /*ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++) {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f) {
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }*/
    }

    private void OnCollisionStay(Collision collision) {
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
        }*/
    }

    private void OnCollisionExit(Collision collision) {
        m_isGrounded = false;
        /*if (m_collisions.Contains(collision.collider)) {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) {
            m_isGrounded = false;
        }*/
    }

    private void Update() {
        if (m_enableJump && !m_jumpInput && Input.GetKey(KeyCode.Space)) {
            m_jumpInput = true;
        }
    }

    private void FixedUpdate() {
        if (virtualControls == null) {
            ControlCharacter(
                Input.GetAxis("Vertical"),
                Input.GetAxis("Horizontal"),
                Input.GetKey(KeyCode.LeftShift));
        }
        else {
            ControlCharacter(
                virtualControls.VerticalAxis,
                virtualControls.HorizontalAxis,
                virtualControls.Run);
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticalAxis"></param>
    /// <param name="horizontalAxis"></param>
    /// <param name="run"></param>
    public void ControlCharacter(float verticalAxis, float horizontalAxis, bool run) {
        m_animator.SetBool("Grounded", m_isGrounded);

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

        //JumpingAndLanding();

        m_wasGrounded = m_isGrounded;
        m_jumpInput = false;
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /*private void TankUpdate() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        bool run = Input.GetKey(KeyCode.LeftShift);

        if (v < 0) {
            if (run) {
                v *= m_backwardRunScale;
            }
            else {
                v *= m_backwardsWalkScale;
            }
        }
        else if (run) {
            v *= m_runScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        if (v == 0f) {
            m_animator.SetFloat("MoveSpeed", 0f);
        }
        else if (v > 0f) {
            m_animator.SetFloat("MoveSpeed", m_currentV);
        }
        else {
            m_animator.SetFloat("MoveSpeed", -m_currentV);
        }

        JumpingAndLanding();
    }

    private void MobileUpdate() {
        float v = joystick.Vertical;
        float h = joystick.Horizontal;
        bool run = joystick.Run;

        if (run) {
            v *= m_runScale;
            h *= m_runScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

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