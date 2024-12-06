using UnityEngine;
using System.Collections;
 
public struct Constants {
    public const int
        //Tiles by side in classic mode
        begginer_tiles_by_side = 8,
        normal_tiles_by_side = 12,
        expert_tiles_by_side = 16;

	public const string
		initial_player_id = "Sticklow", //"Dirhost",
        horizontal_axis = "Horizontal",

		//I2 terms
		i2_term_easy = "Easy",
		i2_term_normal = "Normal",
		i2_term_hard = "Hard",
		
		//Tags
		tag_player = "Player";
}