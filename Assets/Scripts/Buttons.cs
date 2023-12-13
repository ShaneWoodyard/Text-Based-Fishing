using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

    public ChatController control;

    //textboxes
    public GameObject MainTextbox;
    public GameObject InputTextbox;

    //Main Buttons
    public GameObject FishButton;
    public GameObject ShopButton;
    public GameObject AquariumButton;
    public GameObject TournamentButton;
    public GameObject MenuButton; // enables menu

    //Menu Sub Buttons
    public GameObject Menu2Button; // disables menu
    public GameObject SaveButton;
    public GameObject RestartButton;
    public GameObject FPGButton; //Fish Price Guide
    public GameObject StatsButton; //log

    //Shop Buttons
    public GameObject FTButton1;   //FT = Fish Tank
    public GameObject FTButton5;
    public GameObject FTButton10;
    public GameObject LFTButton1;  //LFT = Legendary Fish Tank
    public GameObject LFTButton5;
    public GameObject LFTBUtton10;
    public GameObject FPGShopButton;//FPG = Fish Price Guide
    public GameObject CloseShopButton;

    //tournament Buttons
    public GameObject YesButton;
    public GameObject NoButton;
    public GameObject RulesButton;

    public void Fish()
    {
        control.AddToChatOutput("fish");
    }

    #region Shop
    public void Shop()
    {
        EnableDisableShopButtons(false);
    }

    public void FTButtonOne()
    {
        FTandLFTButton(1, 25, false);
    }

    public void FTButtonFive()
    {
        FTandLFTButton(5, 25, false);
    }

    public void FTButtonTen()
    {
        FTandLFTButton(10, 25, false);
    }

    public void LFTButtonOne()
    {
        FTandLFTButton(1, 100, true);
    }

    public void LFTButtonFive()
    {
        FTandLFTButton(5, 100, true);
    }

    public void LFTButtonTen()
    {
        FTandLFTButton(10, 100, true);
    }

    private void FTandLFTButton(int amount, int cost, bool legend)
    {
        if (control.cash >= (amount * cost) && legend == false)
        {
            control.cash -= (amount * cost);
            control.cash += 0.00m;
            control.fishTankCapacity += amount;
        }
        else if (control.cash >= (amount * cost) && legend == true)
        {
            control.cash -= (amount * cost);
            control.cash += 0.00m;
            control.legendaryFishTankCapacity += amount;
        }
    }

    public void PurchaseFishPriceGuide()
    {
        if (control.fishPriceGuidePurchased == false && control.cash >= 150.00m)
        {
            control.cash -= 150.00m;
            control.fishPriceGuidePurchased = true;
        }
    }

    public void CloseShop()
    {
        EnableDisableShopButtons(true);
    }

    private void EnableDisableShopButtons(bool enable)
    {
        MainTextbox.SetActive(enable);
        InputTextbox.SetActive(enable);
        FishButton.SetActive(enable);
        ShopButton.SetActive(enable);
        AquariumButton.SetActive(enable);
        TournamentButton.SetActive(enable);
        MenuButton.SetActive(enable);

        FTButton1.SetActive(!enable);
        FTButton5.SetActive(!enable);
        FTButton10.SetActive(!enable);
        LFTButton1.SetActive(!enable);
        LFTButton5.SetActive(!enable);
        LFTBUtton10.SetActive(!enable);
        FPGShopButton.SetActive(!enable);
        CloseShopButton.SetActive(!enable);
    }

    #endregion
    public void Aquarium()
    {
        control.AddToChatOutput("aquarium");
    }

    #region Tournament
    public void Tournament()
    {
        control.AddToChatOutput("tournament");
    }

    public void Yes()
    {
        control.AddToChatOutput("yes");
    }

    public void No()
    {
        control.AddToChatOutput("no");
    }

    public void Rules()
    {
        control.AddToChatOutput("rules");
    }

    #endregion

    #region Menu
    public void MenuOn()
    {
        EnableDisableMenuButtons(false);
    }

    public void MenuOff()
    {
        EnableDisableMenuButtons(true);
    }

    public void EnableDisableMenuButtons(bool enable)
    {
        //make everything switch to opposite, so button turns on or off depending on press
        FishButton.SetActive(enable);
        ShopButton.SetActive(enable);
        AquariumButton.SetActive(enable);
        TournamentButton.SetActive(enable);
        MenuButton.SetActive(enable);

        Menu2Button.SetActive(!enable);
        SaveButton.SetActive(!enable);
        RestartButton.SetActive(!enable);
        FPGButton.SetActive(!enable);
        StatsButton.SetActive(!enable);
    }
    #endregion
    public void Save()
    {
        control.AddToChatOutput("save");
    }
    public void Restart()
    {
        control.AddToChatOutput("restart");

    }
    public void FPG()
    {
        control.AddToChatOutput("fpg");
    }
    public void Stats()
    {
        control.AddToChatOutput("log");
    }
}
