using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

/// <summary>
/// This object remains at a fixed distance from the object it follows.
/// </summary>
public class TrackingObject : MonoBehaviour {

	#region Readonly fileds
	#endregion

	#region Serialize fields
    [SerializeField]
    private Vector3 trackingDistance;
    [SerializeField]
    private Vector3 initialRotation;
    #endregion

    #region Private fields
    private float yRadians;
    private Transform transformToTrack;
    private Vector3
        rotation,
        trackingPosition;
    #endregion

    #region Properties
    public Transform TransformToTrack {
        set {
            transformToTrack = value;
            rotation = initialRotation;

            Rotate(new Vector2(transformToTrack.eulerAngles.y - transform.eulerAngles.y, 0f));
        }
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// 
    /// </summary>
    private void Update() {
        transform.position = transformToTrack.position + trackingPosition;
    }
    #endregion

    #region Public methods
    public void Rotate(Vector2 deltaRotation) {
        rotation.y += deltaRotation.x;
        //rotation.x = Mathf.Clamp(rotation.x - deltaRotation.y, -45f, 60f);
        yRadians = Mathf.Deg2Rad * rotation.y;

        trackingPosition.x = trackingDistance.x * Mathf.Sin(yRadians);
        trackingPosition.y = trackingDistance.y;
        trackingPosition.z = trackingDistance.z * Mathf.Cos(yRadians);

        transform.eulerAngles = rotation;
        transform.position = transformToTrack.position + trackingPosition;
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}