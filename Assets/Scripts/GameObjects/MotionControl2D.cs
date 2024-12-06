using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Applies a velocity to a rigidbody when the user presses the keys of
/// the x axis control in the InputManager
/// </summary>
public class MotionControl2D : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private Rigidbody target;
    [SerializeField]
    private float velocity;
    #endregion

    #region Private fields
    private float xAxis;
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Unity methods
    private void Start() {
        xAxis = 0f;
    }

    private void Update() {
        xAxis = Input.GetAxis(Constants.horizontal_axis);
                
        if (xAxis == 0 && target.velocity != Vector3.zero) {
            target.velocity = Vector2.zero;
        }
        else if (xAxis != 0) {
            target.velocity = Vector2.right * xAxis * velocity;
        }
    }
    #endregion

    #region Public methods
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}