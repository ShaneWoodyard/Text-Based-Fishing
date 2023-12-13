using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ChatController : MonoBehaviour {

	public TMP_InputField TMP_ChatInput;

	public TMP_Text TMP_ChatOutput;

	public Scrollbar ChatScrollbar;

	public Buttons buttons;

	#region Input Bools
	public bool doingSomething = false;
	public bool shopping = false;
	public bool tournament = false;
	public bool hasName = false;
	public bool inTournament = false;
	public bool tournamentCheck = false;
	public bool quit = false;
	#endregion

	#region Stat Variables
	public string playerName;
	public long totalFish = 0;
	public long totalFishBites = 0;
	public long totalLegFish = 0;
	public long totalLegBites = 0;
	public long totalTrash = 0;
	public decimal cash = 0.00m;
	public decimal totalCashEarned = 0.00m;

	//index inside listoftextbasedfish.txt
	public long[] trashCatches = new long[] { 0, 0, 0 };
	public long[] fishCatches = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	public long[] legendaryCatches = new long[]	{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    #endregion

    #region Aquarium Variables
    public long fishTankCapacity = 0;
	public long fishInTank = 0;
	public long legendaryFishTankCapacity = 0;
	public long legendaryFishInTank = 0;
	public decimal aquariumEarnings = 0.00m;
	public decimal aquariumCounter = 0.00m;
	public decimal totalAquariumEarnings = 0.00m;

	public long[] aqFish = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	public long[] aqLegFish = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	#endregion

	#region Tournament Variables
	public string[] opponentName = new string[4];
	public int[] tournamentPoints = new int[] { 0, 0, 0, 0, 0 }; //player, opp01, opp02, opp03, opp04
	public int tournamentRound = 1;
	public int points = 0;
	public bool inTournamentStatsBool = false;
	public string catchText = "Shouldn't see this sentence.";

	//lifetime points, 1st place wins, 2nd, 3rd, 4th, 5th
	public long[] tournamentStats = new long[] { 0, 0, 0, 0, 0, 0 };

    #endregion

    #region Misc Variables
    //Fish Price Guide Stuff
    public bool fishPriceGuidePurchased = false;

	//RestartGame
	public bool restartCheck = false;

	//Misc Variables
	public float textPause = 0.5f;
	public string aOrAn = "a";

	//textBoxReset
	public int textBoxTimer = 0;
    #endregion

    #region Enable And Disable Methods
    void OnEnable()
    {
        TMP_ChatInput.onSubmit.AddListener(AddToChatOutput);

    }

    void OnDisable()
    {
        TMP_ChatInput.onSubmit.RemoveListener(AddToChatOutput);

    }
	#endregion

	void Awake()
	{
		doingSomething = true;
		if (PlayerPrefs.GetInt("save") == 10)
		{
			LoadGame();
			hasName = true;
			TMP_ChatOutput.text += "\nWelcome Back!\n";
		}
		else
		{
			TMP_ChatOutput.text += "\nWelcome To Text Based Fishing!\n\nWhat Is Your Name?\n";
		}
		doingSomething = false;
	}
	//^^^Loads Game

	#region Input/Output
	public void AddToChatOutput(string newText)
    {
        // Clear Input Field
        TMP_ChatInput.text = string.Empty;

		if (textBoxTimer >= 30)
		{
			TMP_ChatOutput.text = string.Empty;
			textBoxTimer = 0;
		}

		//TMP_ChatOutput.text += "<#FFFFFF>" + newText + "\n";

		if (hasName == false)
		{
			playerName = newText;
			//TMP_ChatOutput.text += "<#FFFFFF>" + newText + "\n";
			hasName = true;
			TMP_ChatOutput.text += "\n<#FFFFFF>Welcome to Text Based Fishing " + playerName + ".\n";
			TMP_ChatInput.ActivateInputField();
        	ChatScrollbar.value = 0;
			return;
		}
        #region Decide What To Do In Game
        if (doingSomething == false)
		{
			inTournamentStatsBool = false;
			switch (newText)
			{
				case "help": case "Help": case "h": case "H":
					Help();
					break;
				case "fish": case "Fish": case "f": case "F":
					StartCoroutine(Fish());
					break;
				case "shop": case "Shop": case "s": case "S":
					Shop();
					break;
				case "aquarium": case "Aquarium": case "a": case "A":
					Aquarium();
					break;
				case "log": case "Log": case "l": case "L":
					Log();
					break;
				case "FishPriceGuide": case "fishpriceguide": case "Fish Price Guide": case "fish price guide": case "fpg": case "FPG":
					FishPriceGuide();
					break;
				case "tournament": case "Tournament": case "tournement": case "Tournement": case "T": case "t":
					StartCoroutine(TournamentStart());
					break;
				case "Save": case "save":
					doingSomething = true;
					SaveGame();
					doingSomething = false;
					break;
				case "Restart": case "restart":
					StartCoroutine(RestartGame());
					break;
				case "CashCheatX3":
					cash += 10000;
					break;
				default:
					TMP_ChatOutput.text += "\n<#FFFFFF>Oops, you must have typed something wrong.\n";
					break;
			}
			textBoxTimer++;
			TMP_ChatInput.ActivateInputField();
        	ChatScrollbar.value = 0;
			return;
		}
        #endregion

        #region Shopping
        if (shopping == true)
		{
			newText.ToLower();
			switch (newText)
			{
				case "1ft": case "1FT": case "1 fish tanks": case "1 Fish Tanks": case "1 fish tank": case "1 Fish Tank":
					PurchaseFishTank(1, 25.00m);
					break;
				case "3ft": case "3FT": case "3 fish tanks": case "3 Fish Tanks": case "3 fish tank": case "3 Fish Tank":
					PurchaseFishTank(3, 75.00m);
					break;
				case "5ft": case "5FT": case "5 fish tanks": case "5 Fish Tanks": case "5 fish tank": case "5 Fish Tank":
					PurchaseFishTank(5, 125.00m);
					break;
				case "10ft": case "10FT": case "10 fish tanks": case "10 Fish Tanks": case "10 fish tank": case "10 Fish Tank":
					PurchaseFishTank(10, 250.00m);
					break;
				case "1lft": case "1LFT": case "1 legendary fish tanks": case "1 Legendary Fish Tanks": case "1 legendary fish tank":
				case "1 Legendary Fish Tank":
					PurchaseLegendaryFishTank(1, 100.00m);
					break;
				case "3lft": case "3LFT": case "3 legendary fish tanks": case "3 Legendary Fish Tanks": case "3 legendary fish tank":
				case "3 Legendary Fish Tank":
					PurchaseLegendaryFishTank(3, 300.00m);
					break;
				case "5lft": case "5LFT": case "5 legendary fish tanks": case "5 Legendary Fish Tanks": case "5 legendary fish tank":
				case "5 Legendary Fish Tank":
					PurchaseLegendaryFishTank(5, 500.00m);
					break;
				case "10lft": case "10LFT": case "10 legendary fish tanks": case "10 Legendary Fish Tanks": case "10 legendary fish tank":
				case "10 Legendary Fish Tank":
					PurchaseLegendaryFishTank(10, 1000.00m);
					break;
				case "Fish Guide": case "fish guide": case "Fish Price Guide": case "fish price guide": case "FPG": case "fpg":
					if (fishPriceGuidePurchased == true)
					{
						TMP_ChatOutput.text += "\n<#FFFFFF>You already own this.\n";
					}
					if (fishPriceGuidePurchased == false)
					{
						if (cash < 100.00m)
						{
							TMP_ChatOutput.text += "\n<#FFFFFF>You cannot afford that.\n";
						}
						if (cash >= 100.00m)
						{
							cash -= 100.00m;
							fishPriceGuidePurchased = true;
							TMP_ChatOutput.text += "\n<#FFFFFF>You purchased the Fish Price Guide.\n";
						}
					}
					break;
				default:
					TMP_ChatOutput.text += "\n<#FFFFFF>Oops, you typed something wrong.\n";
					break;
			}
			shopping = false;
			doingSomething = false;
			//RoundCash();
			textBoxTimer++;
			TMP_ChatInput.ActivateInputField();
        	ChatScrollbar.value = 0;
			return;
		}
        #endregion

        #region Tournament
        if (tournamentCheck == true)
		{
			switch (newText)
			{
				case "Yes": case "yes": case "Y": case "y":
					if (cash >= 150.00m)
					{
						cash -= 150.00m;
						//RoundCash();
						StartCoroutine(TournamentSetup());
						tournamentCheck = false;
						buttons.YesButton.SetActive(false);
						buttons.NoButton.SetActive(false);
						buttons.FishButton.SetActive(true);
						buttons.RulesButton.SetActive(true);
					}
					else
					{
						TMP_ChatOutput.text += "\n<#FFFFFF>You cannot afford to enter.\n";
						NoToTournament();
					}
				break;
				default:
				case "No": case "no": case "N": case "n":
					TMP_ChatOutput.text += "\n<#FFFFFF>You decide not to enter.\n";
					NoToTournament();
				break;
			}
		}
		if (inTournament == true)
		{
			switch (newText)
			{
				case "Fish": case "fish": case "F": case "f":
					StartCoroutine(TournamentRound());
				break;
				case "Help": case "help": case "H": case "h":
					Help();
				break;
				case "Rules": case "rules": case "R": case "r":
					TournamentRules();
					break;
				default:
					TMP_ChatOutput.text += "\n<#FFFFFF>Oops, you must have typed something wrong.\n";
				break;
			}
		}
		#endregion

		#region Restart
		if (restartCheck == true)
		{
			switch (newText)
			{
				case "Yes": case "yes": case "Y": case "y":
					PlayerPrefs.DeleteAll();
					TMP_ChatOutput.text += "\nYour game has been reset!\n";
					hasName = false;
					TMP_ChatOutput.text += "\nWelcome To Text Based Fishing!\n\nWhat Is Your Name?\n";
					break;
				case "No": case "no": case "N": case "n":
					TMP_ChatOutput.text += "\nYou did not restart the game!\n";
					restartCheck = false;
					doingSomething = false;
					break;
			}
			YesNoOff();
			buttons.MenuOn();
		}
        #endregion 
        textBoxTimer++;
        TMP_ChatInput.ActivateInputField();
        ChatScrollbar.value = 0;
    }

	private void NoToTournament()
	{
		tournamentCheck = false;
		doingSomething = false;
		buttons.YesButton.SetActive(false);
		buttons.NoButton.SetActive(false);
		buttons.FishButton.SetActive(true);
		buttons.ShopButton.SetActive(true);
		buttons.AquariumButton.SetActive(true);
		buttons.TournamentButton.SetActive(true);
	}
	private void YesNoOff()
	{
		buttons.YesButton.SetActive(false);
		buttons.NoButton.SetActive(false);
	}
    #endregion

    #region Main Methods
    public void Help() 
	{
		if (inTournament == false)
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>Main Commands:\n\nHelp: 'Help' 'help' 'H' 'h'\n\n" +
				"Fish: 'Fish' 'fish' 'F' 'f'\n\nShop: 'Shop' 'shop' 'S' 's'\n\nAquarium: 'A' 'a' " +
				"'Aquarium' 'aquarium'\n\nLog: 'Log' 'log' 'L' l'\n\nFish Price Guide: 'Fish Price Guide'" +
				" 'fish price guide' 'FishPriceGuide' 'fishpriceguide' 'FPG' 'fpg'\n\n" +
				"Tournament: 'Tournament' 'tournament' 'T' 't'\n\nQuit: 'Quit' 'quit' 'Q' 'q' 'Close' 'close' 'Exit' 'exit'\n";
		}
		else
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>Tournament Commands:\n\nHelp: 'Help' 'help' 'H' 'h'\n\n" +
				"Fish: 'Fish' 'fish' 'F' 'f'\n\nRules: 'Rules' 'rules' 'R' 'r'\n";
		}
	}
	public IEnumerator Fish()
	{
		doingSomething = true;
		aquariumCounter++;
		TMP_ChatOutput.text += "\n<#FFFFFF>" + "You cast your pole.\n";
		yield return new WaitForSeconds(textPause);
		totalFishBites++;

		int legendaryChance = Random.Range(1, 16);

		if (legendaryChance == 1)
		{
			StartCoroutine(LegendaryFish());
			yield break;
		}
		if (legendaryChance > 1)
		{
			int catchChance = Random.Range(1, 101);
			if (catchChance <= 90)
			{
				TMP_ChatOutput.text += "\n<#FFFFFF>Something is on the line!\n";
				yield return new WaitForSeconds(textPause);
				int fishIndex = Random.Range(1, 47);

				switch (fishIndex)
				{
					case 1:
					case 2:
					case 3:
						StartCoroutine(CatchTrash("Aluminum Can", 0, 0.10m, false));
						break;
					case 4:
					case 5:
						StartCoroutine(CatchFish("Smallmouth Bass", 0, 2.00m, true));
						break;
					case 6:
						StartCoroutine(CatchFish("Largemouth Bass", 1, 3.00m, true));
						break;
					case 7:
					case 8:
					case 9:
						StartCoroutine(CatchFish("Bluegill", 2, 1.50m, true));
						break;
					case 10:
						StartCoroutine(CatchFish("Spotted Bass", 3, 2.20m, true));
						break;
					case 11:
					case 12:
						StartCoroutine(CatchFish("Black Crappie", 4, 0.75m, true));
						break;
					case 13:
					case 14:
					case 15:
						StartCoroutine(CatchFish("Redbreast Sunfish", 5, 0.50m, true));
						break;
					case 16:
					case 17:
						StartCoroutine(CatchFish("Rock Bass", 6, 1.50m, true));
						break;
					case 18:
						StartCoroutine(CatchFish("Striped Bass", 7, 5.00m, true));
						break;
					case 19:
					case 20:
					case 21:
						StartCoroutine(CatchTrash("Stick", 1, 0.05m, true));
						break;
					case 22:
						StartCoroutine(CatchFish("White Bass", 8, 2.00m, true));
						break;
					case 23:
						StartCoroutine(CatchFish("White Perch", 9, 2.35m, true));
						break;
					case 24:
					case 25:
						StartCoroutine(CatchFish("Yellow Perch", 10, 1.05m, true));
						break;
					case 26:
					case 27:
						StartCoroutine(CatchFish("Walleye", 11, 1.65m, true));
						break;
					case 28:
						StartCoroutine(CatchFish("Chain Pickerel", 12, 5.85m, true));
						break;
					case 29:
						StartCoroutine(CatchFish("Northern Pike", 13, 6.25m, true));
						break;
					case 30:
					case 31:
					case 32:
						StartCoroutine(CatchTrash("Nasty Tire", 2, 0.00m, true));
						break;
					case 33:
						StartCoroutine(CatchFish("Brook Trout", 14, 3.15m, true));
						break;
					case 34:
						StartCoroutine(CatchFish("Brown Trout", 15, 2.80m, true));
						break;
					case 35:
						StartCoroutine(CatchFish("Rainbow Trout", 16, 3.85m, true));
						break;
					case 36:
						StartCoroutine(CatchFish("Blue Catfish", 17, 6.45m, true));
						break;
					case 37:
						StartCoroutine(CatchFish("Channel Catfish", 18, 5.90m, true));
						break;
					case 38:
						StartCoroutine(CatchFish("Carp", 19, 4.45m, true));
						break;
					case 39:
					case 40:
						StartCoroutine(CatchFish("White Crappie", 20, 0.85m, true));
						break;
					case 41:
					case 42:
					case 43:
						StartCoroutine(CatchFish("Green Sunfish", 21, 0.65m, true));
						break;
					case 44:
						StartCoroutine(CatchFish("Flathead Catfish", 22, 3.45m, true));
						break;
					case 45:
						StartCoroutine(CatchFish("Hybrid Striped Bass", 23, 3.25m, true));
						break;
					case 46:
						StartCoroutine(CatchFish("Longnose Gar", 24, 7.00m, true));
						break;
				}
			}
			if (catchChance > 90)
			{
				TMP_ChatOutput.text += "\n<#FFFFFF>Something is on the line!\n";
				yield return new WaitForSeconds(textPause);
				TMP_ChatOutput.text += "\n<#FFFFFF>The fish got away.\n";
				doingSomething = false;
			}
		}
	}
	public IEnumerator LegendaryFish()
	{
		TMP_ChatOutput.text += "\n<#FFFFFF>A massive fish is on the line!\n";
		yield return new WaitForSeconds(textPause);
		totalLegBites += 1;

		int catchLeg = Random.Range(1, 4);

		int legPutsUpFight = Random.Range(1, 4);

		switch (legPutsUpFight)
		{
			case 1:
				TMP_ChatOutput.text += "\n<#FFFFFF>It's putting up a fight!\n";
				break;
			case 2:
				TMP_ChatOutput.text += "\n<#FFFFFF>The fish is trying to get away!\n";
				break;
			case 3:
				TMP_ChatOutput.text += "\n<#FFFFFF>It's getting closer!\n";
				break;
		}

		yield return new WaitForSeconds(textPause);

		if (catchLeg == 1)
		{
			int legFishIndex = Random.Range(1, 14);
			switch (legFishIndex)
			{
				case 1:
					StartCoroutine(CatchLegendaryFish("Blue Catfish", 0, 50.00m));
					break;
				case 2:
					StartCoroutine(CatchLegendaryFish("Carp", 1, 55.00m));
					break;
				case 3:
					StartCoroutine(CatchLegendaryFish("Smallmouth Bass", 2, 40.00m));
					break;
				case 4:
					StartCoroutine(CatchLegendaryFish("Largemouth Bass", 3, 50.00m));
					break;
				case 5:
					StartCoroutine(CatchLegendaryFish("Striped Bass", 4, 63.50m));
					break;
				case 6:
					StartCoroutine(CatchLegendaryFish("White Bass", 5, 46.00m));
					break;
				case 7:
					StartCoroutine(CatchLegendaryFish("Chain Pickerel", 6, 62.00m));
					break;
				case 8:
					StartCoroutine(CatchLegendaryFish("Northern Pike", 7, 58.95m));
					break;
				case 9:
					StartCoroutine(CatchLegendaryFish("Rainbow Trout", 8, 60.00m));
					break;
				case 10:
					StartCoroutine(CatchLegendaryFish("Channel Catfish", 9, 52.00m));
					break;
				case 11:
					StartCoroutine(CatchLegendaryFish("Flathead Catfish", 10, 54.35m));
					break;
				case 12:
					StartCoroutine(CatchLegendaryFish("Hybrid Striped Bass", 11, 43.45m));
					break;
				case 13:
					StartCoroutine(CatchLegendaryFish("Longnose Gar", 12, 65.70m));
					break;

			}
		}

		if (catchLeg > 1)
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>The fish got away.\n";
			doingSomething = false;
		}
	}
	public void Shop()
	{
		doingSomething = true;
		TMP_ChatOutput.text += "\n<#FFFFFF>\nWhat will you buy?\n\n1 Fish Tank - $25.00\n3 Fish Tanks - $75.00\n5 Fish Tanks - $125.00\n10 Fish Tanks - $250.00" +
			"\nFish Price Guide - $150.00\n1 Legendary Fish Tank - $100.00\n3 Legendary Fish Tanks - $300.00\n5 Legendary Fish Tanks - $500.00" + 
			"\n10 Legendary Fish Tanks - $1,000.00\n";
		shopping = true;
	}
	public void Aquarium()
	{
		doingSomething = true;
		TMP_ChatOutput.text += "\n<#FFFFFF>Fish Tank Capacity: " + fishTankCapacity + "\nFish In Tank: " + fishInTank 
			+ "\nLegendary Fish Tank Capacity: " + legendaryFishTankCapacity + "\nLegendary Fish In Tank: " + legendaryFishInTank
			+ "\n\nSmallmouth Bass: " + aqFish[0] + "\nLargemouth Bass: " + aqFish[1] + "\nBluegill: "
			+ aqFish[2] + "\nSpotted Bass: " + aqFish[3] + "\nBlack Crappie: " + aqFish[4] + "\nRedbreast Sunfish: "
			+ aqFish[5] + "\nRock Bass: " + aqFish[6] + "\nStriped Bass: " + aqFish[7] + "\nWhite Bass: " + aqFish[8]
			+ "\nWhite Perch: " + aqFish[9] + "\nYellow Perch: " + aqFish[10] + "\nWalleye: " + aqFish[11] + "\nChain Pickerel: "
			+ aqFish[12] + "\nNorthern Pike: " + aqFish[13] + "\nBrook Trout: " + aqFish[14] + "\nBrown Trout: " 
			+ aqFish[15] + "\nRainbow Trout: " + aqFish[16] + "\nBlue Catfish: " + aqFish[17] + "\nChannel Catfish: "
			+ aqFish[18] + "\nCarp: " + aqFish[19] + "\nWhite Crappie: " + aqFish[20] + "\nGreen Sunfish: "
			+ aqFish[21] + "\nFlathead Catfish: " + aqFish[22] + "\nHybrid Striped Bass: " + aqFish[23]
			+ "\nLongnose Gar: " + aqFish[24] + "\n\nLegendary Blue Catfish: " + aqLegFish[0] + "\nLegendary Carp: "
			+ aqLegFish[1] + "\nLegendary Smallmouth Bass: " + aqLegFish[2] + "\nLegendary Largemouth Bass: " + aqLegFish[3]
			+ "\nLegendary Striped Bass: " + aqLegFish[4] + "\nLegendary White Bass: " + aqLegFish[5]
			+ "\nLegendary Chain Pickerel: " + aqLegFish[6] + "\nLegendary Northern Pike: " + aqLegFish[7]
			+ "\nLegendary Rainbow Trout: " + aqLegFish[8] + "\nLegendary Channel Catfish: " + aqLegFish[9]
			+ "\nLegendary Flathead Catfish: " + aqLegFish[10] + "\nLegendary Hybrid Striped Bass: " + aqLegFish[11]
			+ "\nLegendary Longnose Gar: " + aqLegFish[12] + "\n";

		StartCoroutine(AquariumMoney());
	}
	public IEnumerator AquariumMoney()
	{
		if (aquariumCounter >= 5m)
		{
			if (fishInTank > 0 || legendaryFishInTank > 0)
			{
				yield return new WaitForSeconds(textPause);
				aquariumEarnings = 0;
				aquariumEarnings += ((aquariumCounter/5.00m)*((fishInTank * 0.03m) + (legendaryFishInTank * 0.10m)));
				aquariumEarnings = (decimal)(Mathf.Round((float)(aquariumEarnings * 100.00m)) / (float)100.00m);
				aquariumEarnings += 0.00m;
				TMP_ChatOutput.text += "\n<#FFFFFF>Your Aquarium Has Earned $" + aquariumEarnings + "\n";
				EarnCash(aquariumEarnings);
				totalAquariumEarnings += aquariumEarnings;
				aquariumCounter %= 5;
			}
		}
		doingSomething = false;
	}
	public void Log()
	{
		TMP_ChatOutput.text += "\n<#FFFFFF>Name: " + playerName + "\n\nLifetime Cash Earnings: $" + totalCashEarned
			+ "\nLifetime Aquarium Earnings: $" + totalAquariumEarnings + "\n\nTotal Bites: " + totalFishBites 
			+ "\nTotal Fish Caught: " + totalFish + "\nTotal Legendary Bites: " + totalLegBites + 
			"\nTotal Legendary Fish Caught: " + totalLegFish + "\n\nTournament Wins: " + tournamentStats[1] + "\nSecond Place: " + 
			tournamentStats[2] + "\nThird Place: " + tournamentStats[3] + "\nFourth Place: " + tournamentStats[4] +
			"\nLast Place: " + tournamentStats[5] + "\nLifetime Points: " + tournamentStats[0] + "\n\nTotal Smallmouth Bass: "
			+ fishCatches[0] + "\nTotal Largemouth Bass: " + fishCatches[1] + "\nTotal Bluegill: " + fishCatches[2] +
			"\nTotal Spotted Bass: " + fishCatches[3] + "\nTotal Black Crappie: " + fishCatches[4] +
			"\nTotal Redbreast Sunfish: " + fishCatches[5] + "\nTotal Rock Bass: " + fishCatches[6] +
			"\nTotal Striped Bass: " + fishCatches[7] +
			"\nTotal White Bass: " + fishCatches[8] + "\nTotal White Perch: " + fishCatches[9] + "\nTotal Yellow Perch: " +
			fishCatches[10] + "\nTotal Walleye: " + fishCatches[11] + "\nTotal Chain Pickerel: " + fishCatches[12] +
			"\nTotal Northern Pike: " + fishCatches[13] + "\nTotal Brook Trout: " + fishCatches[14] +
			"\nTotal Brown Trout: " + fishCatches[15] + "\nTotal Rainbow Trout: " + fishCatches[16] +
			"\nTotal Blue Catfish: " + fishCatches[17] + "\nTotal Channel Catfish: " + fishCatches[18] +
			"\nTotal Carp: " + fishCatches[19] + "\nTotal White Crappie: " + fishCatches[20] + "\nTotal Green Sunfish: "
			+ fishCatches[21] + "\nTotal Flathead Catfish: " + fishCatches[22] + "\nTotal Hybrid Striped Bass: " +
			fishCatches[23] + "\nTotal Longnose Gar: " + fishCatches[24] + "\n\nTotal Legendary Blue Catfish: " +
			legendaryCatches[0] + "\nTotal Legendary Carp: " + legendaryCatches[1] + "\nTotal Legendary Smallmouth Bass: " +
			legendaryCatches[2] + "\nTotal Legendary Largemouth Bass: " + legendaryCatches[3] + "\nTotal Legendary Striped Bass: "
			+ legendaryCatches[4] + "\nTotal Legendary White Bass: " + legendaryCatches[5] + "\nTotal Legendary Chain Pickerel: "
			+ legendaryCatches[6] + "\nTotal Legendary Northern Pike: " + legendaryCatches[7] +
			"\nTotal Legendary Rainbow Trout: " + legendaryCatches[8] + "\nTotal Legendary Channel Catfish: " + legendaryCatches[9] +
			"\nTotal Legendary Flathead Catfish: " + legendaryCatches[10] + "\nTotal Legendary Hybrid Striped Bass: " + legendaryCatches[11] +
			"\nTotal Legendary Longnose Gar: " + legendaryCatches[12] + "\n\nTotal Trash: " + totalTrash + 
			"\nTotal Aluminum Cans: " + trashCatches[0] + "\nTotal Sticks: " + trashCatches[1] + "\nTotal Nasty Tires: " + 
			trashCatches[2] + "\n";
	}
	public void FishPriceGuide()
	{
		if (fishPriceGuidePurchased == false)
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>You do not own a Fish Price Guide.\n";
		}
		if (fishPriceGuidePurchased == true)
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>Fish Price Guide: \n\nSmallmouth Bass: $2.00\nLargemouth Bass: $3.00\nBluegill: $1.50" +
				                  "\nSpotted Bass: $2.20\nBlack Crappie: $0.75\nRedbreast Sunfish: $0.50\nRock Bass: $1.50" +
				                  "\nStriped Bass: $5.00\nWhite Bass: $2.00\nWhite Perch: $2.35\nYellow Perch: $1.05" +
				                  "\nWalleye: $1.65\nChain Pickerel $5.85\nNorthern Pike: $6.25\nBrook Trout: $3.15\nBrown Trout: $2.80" +
				                  "\nRainbow Trout: $3.85\nBlue Catfish: $6.45\nChannel Catfish: $5.90\nCarp: $4.45" +
				                  "\nWhite Crappie: $0.85\nGreen Sunfish: $0.65\nFlathead Catfish: $3.45\nHybrid Striped Bass: $3.25" +
				                  "\nLongnose Gar: $7.00" +
				                  "\n\nLegendary Blue Catfish: $50.00\nLegendary Carp: $55.00" +
				                  "\nLegendary Smallmouth Bass: $40.00\nLegendary Largemouth Bass: $50.00" +
				                  "\nLegendary Striped Bass: $63.50\nLegendary White Bass: $46.00\nLegendary Chain Pickerel: $62.00" +
				                  "\nLegendary Northern Pike: $58.95\nLegendary Rainbow Trout: $60.00\nLegendary Channel Catfish: $52.00" +
				                  "\nLegendary Flathead Catfish: $54.35\nLegendary Hybrid Striped Bass: $43.45\nLegendary Longnose Gar: $65.70\n";
		}
	}
	public void SaveGame()
	{
		//stat variables
		PlayerPrefs.SetInt("save", 10);
		PlayerPrefs.SetString("name", playerName);
		PlayerPrefs.SetInt("totalfish", System.Convert.ToInt32(totalFish));
		PlayerPrefs.SetInt("totalfishbites", System.Convert.ToInt32(totalFishBites));
		PlayerPrefs.SetInt("totallegfish", System.Convert.ToInt32(totalLegFish));
		PlayerPrefs.SetInt("totallegbites", System.Convert.ToInt32(totalLegBites));
		PlayerPrefs.SetString("cash", cash.ToString());
		PlayerPrefs.SetString("totalcashearned", totalCashEarned.ToString());

		int[] trash = trashCatches.Select(i => (int)i).ToArray();
		int[] fish = fishCatches.Select(i => (int)i).ToArray();
		int[] legFish = legendaryCatches.Select(i => (int)i).ToArray();

		SaveHelper("trash", trash);
		SaveHelper("fish", fish);
		SaveHelper("legFish", legFish);

		//aquarium variables
		PlayerPrefs.SetInt("fishtankcapacity", System.Convert.ToInt32(fishTankCapacity));
		PlayerPrefs.SetInt("fishintank", System.Convert.ToInt32(fishInTank));
		PlayerPrefs.SetInt("legfishtankcapacity", System.Convert.ToInt32(legendaryFishTankCapacity));
		PlayerPrefs.SetInt("legfishintank", System.Convert.ToInt32(legendaryFishInTank));
		PlayerPrefs.SetString("aqearnings", aquariumEarnings.ToString());
		PlayerPrefs.SetString("aqcounter", aquariumCounter.ToString());
		PlayerPrefs.SetString("totalaqearnings", totalAquariumEarnings.ToString());

		int[] aqFishTemp = aqFish.Select(i => (int)i).ToArray();
		int[] aqLegFishTemp = aqLegFish.Select(i => (int)i).ToArray();

		SaveHelper("aqFish", aqFishTemp);
		SaveHelper("aqLegFish", aqLegFishTemp);

		//Tournament varibles
		int[] tourneyStats = tournamentStats.Select(i => (int)i).ToArray();

		SaveHelper("tournament", tourneyStats);

		//Fish price guide
		if (fishPriceGuidePurchased == true)
		{
			PlayerPrefs.SetInt("fpg", 1);
		}
		else
		{
			PlayerPrefs.SetInt("fpg", 0);
		}

		PlayerPrefs.Save();

		TMP_ChatOutput.text += "\nGame has been saved.\n";
	}
	public void LoadGame()
	{
		//if decimals have wrong amount of significant digits, add 0.00M to them
		//stat variables
		playerName = PlayerPrefs.GetString("name");
		totalFish = System.Convert.ToInt64(PlayerPrefs.GetInt("totalfish"));
		totalFishBites = System.Convert.ToInt64(PlayerPrefs.GetInt("totalfishbites"));
		totalLegFish = System.Convert.ToInt64(PlayerPrefs.GetInt("totallegfish"));
		totalLegBites = System.Convert.ToInt64(PlayerPrefs.GetInt("totallegbites"));
		cash = System.Convert.ToDecimal(PlayerPrefs.GetString("cash"));
		totalCashEarned = System.Convert.ToDecimal(PlayerPrefs.GetString("totalcashearned"));

		trashCatches = LoadHelper("trash");
		fishCatches = LoadHelper("fish");
		legendaryCatches = LoadHelper("legFish");

		//aquarium variables
		fishTankCapacity = System.Convert.ToInt64(PlayerPrefs.GetInt("fishtankcapacity"));
		fishInTank = System.Convert.ToInt64(PlayerPrefs.GetInt("fishintank"));
		legendaryFishTankCapacity = System.Convert.ToInt64(PlayerPrefs.GetInt("legfishtankcapacity"));
		legendaryFishInTank = System.Convert.ToInt64(PlayerPrefs.GetInt("legfishintank"));
		aquariumEarnings = System.Convert.ToDecimal(PlayerPrefs.GetString("aqearnings"));
		aquariumCounter = System.Convert.ToDecimal(PlayerPrefs.GetString("aqcounter"));
		totalAquariumEarnings = System.Convert.ToDecimal(PlayerPrefs.GetString("totalaqearnings"));

		aqFish = LoadHelper("aqFish");
		aqLegFish = LoadHelper("aqLegFish");

		//Tournament
		tournamentStats = LoadHelper("tournament");

		//fish price guide
		if (PlayerPrefs.GetInt("fpg") == 1)
		{
			fishPriceGuidePurchased = true;
		}
		else
		{
			fishPriceGuidePurchased = false;
		}
	}
    #endregion

    #region Tournament Methods
    public IEnumerator TournamentStart()
	{
		doingSomething = true;
		TMP_ChatOutput.text += "\n<#FFFFFF>Would you like to enter the Tournament for $150?\n";
		buttons.FishButton.SetActive(false);
		buttons.ShopButton.SetActive(false);
		buttons.AquariumButton.SetActive(false);
		buttons.TournamentButton.SetActive(false);
		yield return new WaitForSeconds(textPause);
		tournamentCheck = true;
		buttons.YesButton.SetActive(true);
		buttons.NoButton.SetActive(true);
	}
	public IEnumerator TournamentSetup()
	{
		TMP_ChatOutput.text += "\n<#FFFFFF>You have entered the Tournament.\n";
		aquariumCounter += 10.00m;
		tournamentRound = 1;
		tournamentPoints[0] = 0;
		tournamentPoints[1] = 0;
		tournamentPoints[2] = 0;
		tournamentPoints[3] = 0;
		tournamentPoints[4] = 0;
		NameAssign();
		yield return new WaitForSeconds(textPause * 4f);
		inTournamentStatsBool = true;
		TMP_ChatOutput.text += "\n<#FFFFFF>Your opponents are " + opponentName[0] + ", " + opponentName[1] + ", " +
			opponentName[2] + ", and " + opponentName[3] + ".\n";
		yield return new WaitForSeconds(textPause * 4f);
		TMP_ChatOutput.text += "\n<#FFFFFF>You may begin.\n";
		inTournament = true;
	}
	public IEnumerator TournamentRound()
	{
		inTournament = false;
		//make player and all 4 opponents fish then increment timer
		TournamentFish(playerName);
		tournamentPoints[0] += points;
		tournamentStats[0] += points;
		//add bools for tournament stats here
		yield return new WaitForSeconds(textPause);
		TournamentFish(opponentName[0]);
		tournamentPoints[1] += points;
		yield return new WaitForSeconds(textPause);
		TournamentFish(opponentName[1]);
		tournamentPoints[2] += points;
		yield return new WaitForSeconds(textPause);
		TournamentFish(opponentName[2]);
		tournamentPoints[3] += points;
		yield return new WaitForSeconds(textPause);
		TournamentFish(opponentName[3]);
		tournamentPoints[4] += points;
		yield return new WaitForSeconds(textPause);
		tournamentRound++;
		if (tournamentRound > 10)
		{
			tournamentRound = 10;
			yield return new WaitForSeconds(textPause);
			TMP_ChatOutput.text += "\n<#FFFFFF>The Tournament is over!\n";
			yield return new WaitForSeconds(textPause * 4f);
			TournamentResults();
			ResetButtonsAfterTournament();
			doingSomething = false;
		}
		else { inTournament = true; }
		
	}
	public void TournamentResults()
	{
		List<int> scores = new List<int>();
		scores.Add(tournamentPoints[0]);
		scores.Add(tournamentPoints[1]);
		scores.Add(tournamentPoints[2]);
		scores.Add(tournamentPoints[3]);
		scores.Add(tournamentPoints[4]);
		scores.Sort();

		if (tournamentPoints[0] == scores[4])
		{
			tournamentStats[1]++;
			if (scores[4] == scores[3])
			{
				TMP_ChatOutput.text += "\n<#FFFFFF>You tied First Place! You earned $300.00!\n";
				EarnCash(300.00m);
			}
			else 
			{
				TMP_ChatOutput.text += "\n<#FFFFFF>You got First Place! You earned $500.00!\n";
				EarnCash(500.00m);
			}
			return;
		}
		if (tournamentPoints[0] == scores[3])
		{
			tournamentStats[2]++;
			TMP_ChatOutput.text += "\n<#FFFFFF>You got Second Place! You earned $250.00!\n";
			EarnCash(250.00m);
			return;
		}
		if (tournamentPoints[0] == scores[2])
		{
			tournamentStats[3]++;
			TMP_ChatOutput.text += "\n<#FFFFFF>You got Third Place! You earned $100.00!\n";
			EarnCash(100.00m);
			return;
		}
		if (tournamentPoints[0] == scores[1])
		{
			tournamentStats[4]++;
			TMP_ChatOutput.text += "\n<#FFFFFF>You got Fourth Place, better luck next time!\n";
			return;
		}
		if (tournamentPoints[0] == scores[0])
		{
			tournamentStats[5]++;
			TMP_ChatOutput.text += "\n<#FFFFFF>You got Last Place, better luck next time!\n";
			return;
		}

	}

	private void ResetButtonsAfterTournament()
	{
		buttons.RulesButton.SetActive(false);
		buttons.ShopButton.SetActive(true);
		buttons.AquariumButton.SetActive(true);
		buttons.TournamentButton.SetActive(true);
	}
	public void TournamentRules()
	{
		TMP_ChatOutput.text += "\n<#FFFFFF>Tournament Rules:\n\nYou get 10 attempts to catch fish.\n\nEach fish is worth " +
			"a set amount of points depending on rarity.\n\nLegendary fish are worth the most.\n\nThe participant with the " +
			"most points at the end wins.\n\nYou do not get to keep any caught fish.\n\nFirst place rewards you with $500.00, " +
			"second place gets $250.00, third place gets $100.00\n\nIf first place is a tie, everyone who ties gets $300.00\n";
	}
	public void TournamentFish(string name)
	{
		points = 0;
		int legCatchChance = Random.Range(1, 16);
		if (legCatchChance > 1)
		{
			int catchChance = Random.Range(0, 100);
			if (catchChance <= 90)
			{
				int fishIndex = Random.Range(1, 15);
				switch (fishIndex)
				{
					case 1:
					case 2:
					case 3:
						TournamentTrash(name);
						break;
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
						OnePointFish(name);
						points = 1;
						break;
					case 11:
					case 12:
					case 13:
						TwoPointFish(name);
						points = 2;
						break;
					case 14:
						ThreePointFish(name);
						points = 3;
						break;
				}
			}
			else 
			{
				TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " did not catch anything.\n";
			}
		}
		else 
		{
			int catchLeg = Random.Range(1, 4);
			if (catchLeg > 1)
			{
				TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " did not catch anything.\n";
			}
			else
			{
				int legIndex = Random.Range(1, 4);
				switch (legIndex)
				{
					case 1: case 2:
						FourPointFish(name);
						points += 4;
						break;
					case 3:
						FivePointFish(name);
						points += 5;
						break;
				}
			}
		}
	}
	public void TournamentTrash(string name)
	{
		int trashIndex = Random.Range(1, 4);
		switch (trashIndex)
		{
			case 1:
				CatchTextSet("Aluminum Can", "an");
				break;
			case 2:
				CatchTextSet("Stick", "a");
				break;
			case 3:
				CatchTextSet("Nasty Tire", "a");
				break;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " caught " + aOrAn + " " + catchText + ".\n";
	}
	public void OnePointFish(string name)
	{
		int fishIndex = Random.Range(1, 4);
		switch (fishIndex)
		{
			case 1:
				CatchTextSet("Bluegill", "a");
				break;
			case 2:
				CatchTextSet("Redbreast Sunfish", "a");
				break;
			case 3:
				CatchTextSet("Green Sunfish", "a");
				break;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " caught " + aOrAn + " " + catchText + ", it is worth 1 point.\n";
	}
	public void TwoPointFish(string name)
	{
		int fishIndex = Random.Range(1, 7);
		switch (fishIndex)
		{
			case 1:
				CatchTextSet("Smallmouth Bass", "a");
				break;
			case 2:
				CatchTextSet("Black Crappie", "a");
				break;
			case 3:
				CatchTextSet("Rock Bass", "a");
				break;
			case 4:
				CatchTextSet("Yellow Perch", "a");
				break;
			case 5:
				CatchTextSet("Walleye", "a");
				break;
			case 6:
				CatchTextSet("White Crappie", "a");
				break;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " caught " + aOrAn + " " + catchText + ", it is worth 2 points.\n";
	}
	public void ThreePointFish(string name)
	{
		int fishIndex = Random.Range(1, 17);
		switch (fishIndex)
		{
			case 1:
				CatchTextSet("Largemouth Bass", "a");
				break;
			case 2:
				CatchTextSet("Spotted Bass", "a");
				break;
			case 3:
				CatchTextSet("Striped Bass", "a");
				break;
			case 4:
				CatchTextSet("White Bass", "a");
				break;
			case 5:
				CatchTextSet("White Perch", "a");
				break;
			case 6:
				CatchTextSet("Chain Pickerel", "a");
				break;
			case 7:
				CatchTextSet("Northern Pike", "a");
				break;
			case 8:
				CatchTextSet("Brook Trout", "a");
				break;
			case 9:
				CatchTextSet("Brown Trout", "a");
				break;
			case 10:
				CatchTextSet("Rainbow Trout", "a");
				break;
			case 11:
				CatchTextSet("Blue Catfish", "a");
				break;
			case 12:
				CatchTextSet("Channel Catfish", "a");
				break;
			case 13:
				CatchTextSet("Carp", "a");
				break;
			case 14:
				CatchTextSet("Flathead Catfish", "a");
				break;
			case 15:
				CatchTextSet("Hybrid Striped Bass", "a");
				break;
			case 16:
				CatchTextSet("Longnose Gar", "a");
				break;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " caught " + aOrAn + " " + catchText + ", it is worth 3 points.\n";
	}
	public void FourPointFish(string name)
	{
		int fishIndex = Random.Range(1, 9);
		switch (fishIndex)
		{
			case 1:
				CatchTextSet("Legendary Blue Catfish", "a");
				break;
			case 2:
				CatchTextSet("Legendary Smallmouth Bass", "a");
				break;
			case 3:
				CatchTextSet("Legendary Largemouth Bass", "a");
				break;
			case 4:
				CatchTextSet("Legendary Carp", "a");
				break;
			case 5:
				CatchTextSet("Legendary White Bass", "a");
				break;
			case 6:
				CatchTextSet("Legendary Channel Catfish", "a");
				break;
			case 7:
				CatchTextSet("Legendary Flathead Catfish", "a");
				break;
			case 8:
				CatchTextSet("Legendary Hybrid Striped Bass", "a");
				break;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " caught " + aOrAn + " " + catchText + ", it is worth 4 points.\n";
	}
	public void FivePointFish(string name)
	{
		int fishIndex = Random.Range(1, 6);
		switch (fishIndex)
		{
			case 1:
				CatchTextSet("Legendary Striped Bass", "a");
				break;
			case 2:
				CatchTextSet("Legendary Chain Pickerel", "a");
				break;
			case 3:
				CatchTextSet("Legendary Northern Pike", "a");
				break;
			case 4:
				CatchTextSet("Legendary Rainbow Trout", "a");
				break;
			case 5:
				CatchTextSet("Legendary Longnose Gar", "a");
				break;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>" + name + " caught " + aOrAn + " " + catchText + ", it is worth 5 points.\n";
	}
	public void NameAssign()
	{
		var names = new List<string>() { "Dan", "Billy Bob", "Rick", "Marie", "Lucy", "Joe", "Harold", "Garvey", "Big Al", "Alyssa" };
		int whichName = Random.Range(0, 10);
		opponentName[0] = names[whichName];
		names.Remove(names[whichName]);
		whichName = Random.Range(0, 9);
		opponentName[1] = names[whichName];
		names.Remove(names[whichName]);
		whichName = Random.Range(0, 8);
		opponentName[2] = names[whichName];
		names.Remove(names[whichName]);
		whichName = Random.Range(0, 7);
		opponentName[3] = names[whichName];
	}
	public void CatchTextSet(string CT, string AN)
	{
		catchText = CT;
		aOrAn = AN;
	}
	#endregion

	#region Misc Methods

	public IEnumerator RestartGame()
	{
		doingSomething = true;
		TMP_ChatOutput.text += "\n<#FFFFFF>Would you like to restart the game? (Progress will be deleted forever!)\n";
		buttons.SaveButton.SetActive(false);
		buttons.RestartButton.SetActive(false);
		buttons.FPGButton.SetActive(false);
		buttons.StatsButton.SetActive(false);
		yield return new WaitForSeconds(textPause);
		restartCheck = true;
		buttons.YesButton.SetActive(true);
		buttons.NoButton.SetActive(true);
	}
	public void SaveHelper(string name, int[] toSave)
	{
		for (int i = 0; i < toSave.Length; i++)
		{
			PlayerPrefs.SetInt(name + i, toSave[i]);
		}
		PlayerPrefs.SetInt(name + "TotalLength", toSave.Length);
	}

	public long[] LoadHelper(string name)
	{
		long[] toLoad = new long[PlayerPrefs.GetInt(name + "TotalLength")];

		for (int i = 0; i < toLoad.Length; i++)
		{
			toLoad[i] = System.Convert.ToInt64(PlayerPrefs.GetInt(name + i));
		}

		return toLoad;
	}
    public void EarnCash(decimal money)
	{
		cash += money;
		totalCashEarned += money;
	}
	public void ItSellsFor(decimal money)
	{
		TMP_ChatOutput.text += "\n<#FFFFFF>It sells for $" + money.ToString() + "\n";
		EarnCash(money);
	}
	public void FishCounter(bool legend)
	{
		if (legend == true)
		{
			totalLegFish++;
		}
		totalFish++;
		doingSomething = false;
	}
	public void PurchaseFishTank(int tankNumber, decimal cost)
	{
		string s = "";
		if (cash < cost)
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>You cannot afford that.\n";
		}
		if (cash >= cost)
		{
			if (tankNumber > 1)
			{
				s = "s";
			}
			cash -= cost;
			fishTankCapacity += tankNumber;
			TMP_ChatOutput.text += "\n<#FFFFFF>You purchased " + tankNumber.ToString() + " Fish Tank" + s + ".\n";
		}
	}
	public void PurchaseLegendaryFishTank(int tankNumber, decimal cost)
	{
		string s = "";
		if (cash < cost)
		{
			TMP_ChatOutput.text += "\n<#FFFFFF>You cannot afford that.\n";
		}
		if (cash >= cost)
		{
			if (tankNumber > 1)
			{
				s = "s";
			}
			cash -= cost;
			legendaryFishTankCapacity += tankNumber;
			TMP_ChatOutput.text += "\n<#FFFFFF>You purchased 1 Legendary Fish Tank" + s.ToString() + ".\n";
		}
	}
	public void AddToFishTank(bool legend)
	{
		if (legend == true)
		{
			legendaryFishInTank++;
		}
		else
		{
			fishInTank++;
		}
		TMP_ChatOutput.text += "\n<#FFFFFF>You add the fish to your aquarium.\n";
	}
	public void TrashCounter()
	{
		totalFishBites--;
		totalTrash++;
		doingSomething = false;
	}
    #endregion

    #region Catch Methods
    public IEnumerator CatchFish(string name, int fishIndex, decimal price, bool aAn)
	{
		if (aAn == true) aOrAn = "a";
		else aOrAn = "an";
		TMP_ChatOutput.text += "\n<#FFFFFF>You caught " + aOrAn + " " + name + ".\n";
		yield return new WaitForSeconds(textPause);
		fishCatches[fishIndex]++;
		FishCounter(false);
		if (fishTankCapacity > fishInTank)
		{
			AddToFishTank(false);
			aqFish[fishIndex]++;
			yield break;
		}
		if (fishTankCapacity <= fishInTank)
		{
			ItSellsFor(price);
		}
	}
	public IEnumerator CatchTrash(string name, int trashIndex, decimal price, bool aAn)
	{
		if (aAn == true) aOrAn = "a";
		else aOrAn = "an";
		TMP_ChatOutput.text += "\n<#FFFFFF>You reeled in " + aOrAn + " " + name + ".\n";
		yield return new WaitForSeconds(textPause);
		TrashCounter();
		trashCatches[trashIndex]++;
		if (price > 0.00m) ItSellsFor(price);
		else TMP_ChatOutput.text += "\n<#FFFFFF>It doesn't sell for anything.\n";
	}
	public IEnumerator CatchLegendaryFish(string name, int fishIndex, decimal price)
	{
		TMP_ChatOutput.text += "\n<#FFFFFF>You caught a Legendary " + name + "!\n";
		yield return new WaitForSeconds(textPause);
		legendaryCatches[fishIndex]++;
		FishCounter(true);
		if (legendaryFishTankCapacity > legendaryFishInTank)
		{
			AddToFishTank(true);
			aqLegFish[fishIndex]++;
			yield break;
		}
		if (legendaryFishTankCapacity <= legendaryFishInTank)
		{
			ItSellsFor(price);
		}
	}
    #endregion
}
