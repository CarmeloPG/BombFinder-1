

using UnityEngine;

public interface I_MouseClickListner {

    /// <summary>
    /// Actions when the left mouse button is released.
    /// </summary>
	public void OnLeftMouseButtonUp();

    /// <summary>
    /// Actions when the right mouse button is released.
    /// </summary>
    public void OnRightMouseButtonUp();
}