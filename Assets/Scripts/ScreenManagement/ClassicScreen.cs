using UnityEngine;
using System.Collections;
 
public class ClassicScreen : PlayScreen {
	#region Readonly fields
	//The bombs density in the classic game
	private readonly float classic_Bombs_Density = .157f;
	#endregion

	#region Serialize fields
	#endregion

	#region Private fields
	#endregion

	#region Properties
	/*public int TilesBySide {
		set => tilesBySide = value;
	}*/
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// It starts the play with the selected tiles by side.
    /// </summary>
    /// <param name="thePlayer">The player avatar</param>
    public override void Play(PlayObject thePlayer) {
        base.Play(thePlayer);

        time = 0;
        pause = false;
        country = new Tile[tilesBySide, tilesBySide];

        levelReport.levelPlayed = tilesBySide;

        BombsCount = Mathf.CeilToInt(tilesCount * classic_Bombs_Density);

        startlevelInfoBox.ShowInfo(this);
        timeDisplay.ShowTime(time);

        StartCoroutine(SetPlayScreen());
    }

    /// <summary>
    /// It updates the levelReport with the game result.
    /// </summary>
    /// <param name="levelSuccess">True if the level is success.</param>
    public override void EndPlay(bool levelSuccess) {
        base.EndPlay(levelSuccess);

        if (levelSuccess) {
            levelReport.timeReward = time;
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    /// <summary>
    /// The contdown of the challenge.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play() {
        while (true) {
            yield return new WaitForSeconds(1f);

            time++;

            CheckPlayerPosition();

            timeDisplay.ShowTime(time);
        }

        //EndPlay(false);
    }
    #endregion
}