using UnityEngine;
using System.Collections;
using UnityEditor;

public class GameScreen : MonoBehaviour {
    #region Readonly fields
    //private readonly string Rotation = "_Rotation";
    #endregion

    #region Serialize fields
    //[Header("Screen data")]
    [SerializeField]
    private string screenName;
    /*[SerializeField]
    private Sprite screenIcon;

    [Header("SkyBox")]
    [SerializeField]
    private Material skyBox;
    [SerializeField]
    [Tooltip("In degrees per second. SkyVelocity only works with cubemaps. In skyboxes with Shader 'Skybox/Procedural' skyVelocity must be equals to 0.")]
    private float skyVelocity;*/

    /*[Header("Lights")]
    [SerializeField]
    private LightingSettings lightingSettings;
    [SerializeField]
    private LightingDataAsset lightingDataAsset;*/

    #endregion

    #region Private fields
    /*private float
        skyRotation,
        deltaSkyRotation;*/
    #endregion

    #region Properties
    public string ScreenName {
        get => screenName;
    }

    /*public Sprite ScreenIcon {
        get => screenIcon;
    }*/
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /*private void Awake() {
        deltaSkyRotation = skyVelocity * Time.deltaTime;

        if (skyVelocity > 0) {
            skyRotation = skyBox.GetFloat(Rotation);
        }

        //Lightmapping.lightingSettings = lightingSettings;
        //Lightmapping.lightingDataAsset = lightingDataAsset;

        RenderSettings.skybox = skyBox;
    }

    private void Update() {
        if (skyVelocity > 0) {
            skyRotation += deltaSkyRotation;

            if (skyRotation >= 360f) {
                skyRotation = 0f;
            }

            skyBox.SetFloat(Rotation, skyRotation);
        }
    }*/
    #endregion

    #region Public methods
    public void Show(bool show) {
        gameObject.SetActive(show);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}