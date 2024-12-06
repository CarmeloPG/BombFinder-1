using UnityEngine;
using System.Collections;
 
/// <summary>
/// When it enter on a collision whith an object whitch has
/// a rigidbody, it applies a force to the rigidbody
/// of the collision object
/// </summary>
public class ForceObject : MonoBehaviour {
    #region Serialize fields
	[SerializeField]
	private Vector3 forceVector = Vector3.zero;
    //[SerializeField]
    //private float AngleVariation = 0f;
    [SerializeField]
    private ForceMode forceMode = ForceMode.Impulse;
    #endregion

    #region Private fields
    //Vector3 force = Vector3.zero;
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Unity methods
    private void OnCollisionEnter(Collision collision) {
        if (collision != null) {
            var rigidBody  = collision.rigidbody;
            
            if (rigidBody != null) {
                //force = collision.impulse * forceVector.y;
                rigidBody.AddForce(forceVector, forceMode);
            }
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