using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;

    // Cheats Implementation
    public Select selectScreen;
    public Button magnifyingGlass;
    public Button sunGlass;
    public Button dagger;
    public Button sword;
    public Button rustySword;
    public Button hook;
    public Button cigar;
    public Button glove;

    // Initialize Cheat
    int magnifyingGlassN = 0;
    int sunGlassN = 0;
    int daggerN = 0;
    int swordN = 0;
    int rustySwordN = 0;
    int hookN = 0;
    int cigarN = 0;
    int gloveN = 0;

    public Text magnifyingGlassT;
    public Text sunGlassT;
    public Text daggerT;
    public Text swordT;
    public Text rustySwordT;
    public Text hookT;
    public Text cigarT;
    public Text gloveT;

    public bool addCheat = true;

    private int standClicks = 0;

    public DeckScript deckScript;

    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // public Text to access and update - hud
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text standBtnText;

    // Card hiding dealer's 2nd card
    public GameObject hideCard;

    // Top Deck
    public GameObject topDeck;

    // How much is bet
    int pot = 0;

    void Start()
    {
        // Add on click listeners to the buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        // betBtn.onClick.AddListener(() => BetClicked());

        // Add on click listeners to cheats
        magnifyingGlass.onClick.AddListener(() => MagnifyingGlass());
        sunGlass.onClick.AddListener(() => SunGlass());
        dagger.onClick.AddListener(() => Dagger());
        sword.onClick.AddListener(() => Sword());
        rustySword.onClick.AddListener(() => RustySword());
        hook.onClick.AddListener(() => Hook());
        cigar.onClick.AddListener(() => Cigar());
        glove.onClick.AddListener(() => Glove());

        DisableCheat();
        EnableCheat();
        InitializeCheatText();
    }

    private void DealClicked()
    {
        addCheat = false;
        // Reset round, hide text, prep for new hand
        playerScript.ResetAllHandAngle();
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Hide deal hand score at start of deal
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update the scores displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // Place card back on dealer card, hide card
        hideCard.GetComponent<Renderer>().enabled = true;
        // Adjust buttons visibility
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "";

        // Enable Cheats
        topDeck.SetActive(false);
        EnableCheat();

        // Set standard pot size
        pot = 2;
        betsText.text = "Bets: $" + (pot/2).ToString();
        playerScript.AdjustMoney(-1);
        cashText.text = "$" + playerScript.GetMoney().ToString();

    }

    private void HitClicked()
    {
        // Check that there is still room on the table
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            CheckBust();
        }

        topDeck.SetActive(false);
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) HitDealer();
        standBtnText.text = "Call";

        hitBtn.gameObject.SetActive(false);
        // Disable Cheat
        topDeck.SetActive(false);
        DisableCheat();
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 17 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
        RoundOver();
    }

    // Check for winnner and loser, hand is over
    void RoundOver()
    {
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // All bust, bets returned
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust.";
            playerScript.AdjustMoney(pot / 2);
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        //Check for tie, return bets
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Tied.";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Set ui up for next move / hand / turn
        if (roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }

        topDeck.SetActive(false);
    }

    // Add money to pot if bet clicked
    void BetClicked()
    {
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));    
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = "Bets: $" + (pot/2).ToString();
    }

    // Added Cheats
    void MagnifyingGlass()
    {
        if (addCheat) magnifyingGlassN++;
        else
        {
            int cardValue = deckScript.TopDeck(topDeck.GetComponent<CardScript>());
            topDeck.GetComponent<Renderer>().enabled = true;
            topDeck.SetActive(true);
            magnifyingGlassN--;
        }
        InitializeCheatText();
    }

    void SunGlass()
    {
        if (addCheat) sunGlassN++;
        else
        {
            hideCard.GetComponent<Renderer>().enabled = false;
            sunGlassN--;
        }
        InitializeCheatText();
    }

    void Dagger()
    {
        if (addCheat) daggerN++;
        else
        {
            // Check that there is still room on the table
            if (playerScript.cardIndex <= 10)
            {
                playerScript.GetHalfCard();
                scoreText.text = "Hand: " + playerScript.handValue.ToString();
                if (playerScript.handValue > 20)
                {
                    standBtnText.text = "Black Jack!";
                    if (playerScript.handValue > 21) standBtnText.text = "Busted!";
                    DisableCheat();
                    RoundOver();
                }
            }

            topDeck.SetActive(false);
            daggerN--;
        }
        InitializeCheatText();
    }

    void Sword()
    {
        if (addCheat) swordN++;
        else
        {
            DisableCheat();
            DisableButton();
            selectScreen.gameObject.SetActive(true);
            selectScreen.ChooseDestoryCard(playerScript.hand[0], playerScript.hand[1]);
            swordN--;
        }
        InitializeCheatText();
    }

    void RustySword()
    {
        if (addCheat) rustySwordN++;
        else
        {
            DisableCheat();
            DisableButton();
            selectScreen.gameObject.SetActive(true);
            selectScreen.ChooseDestoryOppoCard(dealerScript.hand[0], dealerScript.hand[1]);
            rustySwordN--;
        }
        InitializeCheatText();
    }

    void Hook()
    {
        if (addCheat) hookN++;
        else
        {
            deckScript.currentIndex++;
            topDeck.SetActive(false);
            hookN--;
        }
        InitializeCheatText();
    }

    void Cigar()
    {
        if (addCheat) cigarN++;
        else
        {
            playerScript.ResetHand();
            playerScript.StartHand();
            CheckBust();
            cigarN--;
        }
        InitializeCheatText();
    }

    void Glove()
    {
        if (addCheat) gloveN++;
        else
        {
            DisableCheat();
            DisableButton();
            selectScreen.gameObject.SetActive(true);
            selectScreen.ChooseGlove(playerScript.hand[0], playerScript.hand[1]);
            gloveN--;
        }
        InitializeCheatText();
    }

    void InitializeCheatText()
    {
        magnifyingGlassT.text = "x" + magnifyingGlassN.ToString();
        sunGlassT.text = "x" + sunGlassN.ToString();
        daggerT.text = "x" + daggerN.ToString();
        swordT.text = "x" + swordN.ToString();
        rustySwordT.text = "x" + rustySwordN.ToString();
        hookT.text = "x" + hookN.ToString();
        cigarT.text = "x" + cigarN.ToString();
        gloveT.text = "x" + gloveN.ToString();

        if (!addCheat) CheatCondition();
    }


    void DisableCheat()
    {
        selectScreen.gameObject.SetActive(false);

        magnifyingGlass.gameObject.SetActive(false);
        sunGlass.gameObject.SetActive(false);
        dagger.gameObject.SetActive(false);
        sword.gameObject.SetActive(false);
        rustySword.gameObject.SetActive(false);
        hook.gameObject.SetActive(false);
        cigar.gameObject.SetActive(false);
        glove.gameObject.SetActive(false);
    }

    void EnableCheat()
    {
        magnifyingGlass.gameObject.SetActive(true);
        sunGlass.gameObject.SetActive(true);
        dagger.gameObject.SetActive(true);
        sword.gameObject.SetActive(true);
        rustySword.gameObject.SetActive(true);
        hook.gameObject.SetActive(true);
        cigar.gameObject.SetActive(true);   
        glove.gameObject.SetActive(true);

        if (!addCheat) CheatCondition();
    }

    private void DisableButton()
    {
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
    }

    private void EnableButton()
    {
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
    }
    public void DisableSelect()
    {
        selectScreen.gameObject.SetActive(false);
        EnableCheat();
        EnableButton();

        CheatCondition();
    }

    void CheatCondition()
    {
        if (magnifyingGlassN == 0) magnifyingGlass.gameObject.SetActive(false);
        if (sunGlassN == 0) sunGlass.gameObject.SetActive(false);
        if (daggerN == 0) dagger.gameObject.SetActive(false);
        if (swordN == 0) sword.gameObject.SetActive(false);
        if (rustySwordN == 0) rustySword.gameObject.SetActive(false);
        if (hookN == 0) hook.gameObject.SetActive(false);
        if (cigarN == 0) cigar.gameObject.SetActive(false);
        if (gloveN == 0) glove.gameObject.SetActive(false);

        if (playerScript.hand[0].GetComponent<CardScript>().value == 0
            && playerScript.hand[1].GetComponent<CardScript>().value == 0)
        {
            sword.gameObject.SetActive(false);
            glove.gameObject.SetActive(false);
        }

        if (dealerScript.hand[0].GetComponent<CardScript>().value == 0
            && dealerScript.hand[1].GetComponent<CardScript>().value == 0)
        {
            rustySword.gameObject.SetActive(false);
            glove.gameObject.SetActive(false);
        }
    }

    public void CheckBust()
    {
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        if (playerScript.handValue > 20)
        {
            standBtnText.text = "Black Jack!";
            if (playerScript.handValue > 21) standBtnText.text = "Busted!";
            DisableCheat();
            RoundOver();
        }
    }

    public void CheckDealerBust()
    {
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        if (dealerScript.handValue > 20)
        {
            standBtnText.text = "Black Jack!";
            if (dealerScript.handValue > 21) standBtnText.text = "Busted!";
            DisableCheat();
            RoundOver();
        }
    }
}
