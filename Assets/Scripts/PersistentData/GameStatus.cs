using System.Collections.Generic;
using System;

[Serializable]
public class GameStatus {
    #region public fields
    public string equipedObject;
    public List<string> playerInventory;
	public ChallengeData challengeData;
    public bool
		airView,
		isLoggedIn,
		musicMuted,
		effectMuted;
	public int
		currentLevel,
		playerMoney;
		//lastYearChallenge,
		//lastMounthChallenge,
		//lastDayChallenge;


	


	#endregion

	#region Constructors
	/// <summary>
	/// It creates a new Gamestatus.
	/// </summary>
	public GameStatus() {
		//DateTime now = DateTime.Now;

		airView = false;
		isLoggedIn = false;
		musicMuted = false;
		effectMuted = false;
		currentLevel = 1;
		playerMoney = 1500;
		challengeData = new ChallengeData();

		/*lastYearChallenge = now.Year - 1;
		lastMounthChallenge = now.Month;
		lastDayChallenge = now.Day;*/

		equipedObject = Constants.initial_player_id;
		playerInventory = new List<string> { Constants.initial_player_id };
	}
	#endregion

	#region Public methods
	/*public void SetChallengeDate(DateTime date) {
		lastYearChallenge = date.Year;
		lastMounthChallenge = date.Month;
		lastDayChallenge = date.Day;
	}*/
	#endregion

	#region Internal classes
	/// <summary>
	/// It stores the data of the last challenge.
	/// </summary>
	[Serializable]
	public class ChallengeData {
        #region Public fields
        //Was the last challeng overcome?
        public bool overcome;
		public int
			//The day of the last challenge.
			day,
			//The month of the last challenge.
			month,
			//The year of the last challenge.
			year,
			//Number of tiles by side of the last challenge.
			tilesBySide,
			//Number of bombs of the last challenge.
			bombs,
			//Time of the last challenge.
			time,
			//Dificulty index of the last challenge.
			dificultyIndex;
        #endregion

        #region Constructors
        /// <summary>
		/// 
        /// </summary>
        public ChallengeData() {
            DateTime now = DateTime.Now;

			overcome = false;
			day = now.Day;
			month = now.Month;
			year = now.Year - 1;
			tilesBySide = -1;
			bombs = -1;
			dificultyIndex = -1;
        }
        #endregion
    }
    #endregion
}
