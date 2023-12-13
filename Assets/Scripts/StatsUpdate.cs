using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUpdate : MonoBehaviour {

	public TMP_Text statsText;

	public TMP_Text tournamentScore;

	public ChatController stats;

	void Update () 
	{
		statsText.text = "\n<#FFFFFF>Cash: $" + stats.cash + "\nFish Caught: " + stats.totalFish + "\nLegendary Fish Caught: " +
			stats.totalLegFish + "\nTrash Caught: " + stats.totalTrash + "\nFish Tank Capacity: " + stats.fishTankCapacity + 
			"\nFish In Tank: " + stats.fishInTank + "\nLegendary Fish Tank Capacity: " + stats.legendaryFishTankCapacity + 
			"\nLegendary Fish In Tank: " + stats.legendaryFishInTank;

		if (stats.inTournamentStatsBool == true)
		{
			tournamentScore.text = "\n<#FFFFFF>Tournament Standings\n" + "Round: " +
				stats.tournamentRound + "\n" + stats.playerName + ": " + stats.tournamentPoints[0] + "\n" + stats.opponentName[0] +
				": " + stats.tournamentPoints[1] + "\n" + stats.opponentName[1] + ": " + stats.tournamentPoints[2] +
				"\n" + stats.opponentName[2] + ": " + stats.tournamentPoints[3] + "\n" + stats.opponentName[3] + ": " +
				stats.tournamentPoints[4];
		}
	}
}
