using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int stage;
    public int playerHealth;
    public int dealerHealth;
    public int startingItems;

    // Game Buttons
    public UnityEngine.UI.Button dealBtn;
    public UnityEngine.UI.Button hitBtn;
    public UnityEngine.UI.Button standBtn;
    public UnityEngine.UI.Button betBtn;

    // Cheats Implementation
    public Select selectScreen;
    public UnityEngine.UI.Button magnifyingGlass;
    public UnityEngine.UI.Button sunGlass;
    public UnityEngine.UI.Button dagger;
    public UnityEngine.UI.Button sword;
    public UnityEngine.UI.Button rustySword;
    public UnityEngine.UI.Button hook;
    public UnityEngine.UI.Button cigar;
    public UnityEngine.UI.Button glove;

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
    public Text dealerCashText;
    public Text cashText;
    public Text mainText;
    public Text standBtnText;

    // Game Ending Text
    public Text gameEnd;
    public UnityEngine.UI.Button returnMain;

    // Card hiding dealer's 2nd card
    public GameObject hideCard;

    // Top Deck
    public GameObject topDeck;

    // How much is bet
    int pot = 0;

    // Health
    public GameObject playerHealth1;
    public GameObject playerHealth2;
    public GameObject playerHealth3;
    public GameObject playerHealth4;

    public GameObject dealerHealth1;
    public GameObject dealerHealth2;
    public GameObject dealerHealth3;
    public GameObject dealerHealth4;
    public GameObject dealerHealth5;
    public GameObject dealerHealth6;
    public GameObject dealerHealth7;
    public GameObject dealerHealth8;
    public GameObject dealerHealth9;
    public GameObject dealerHealth10;

    void Start()
    {
        RemoveItemDescription();

        returnMain.gameObject.SetActive(false);
        gameEnd.gameObject.SetActive(false);

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

        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);

        DisableCheat();
        EnableCheat();
        InitializeCheatText();

        playerScript.money = playerHealth;
        dealerScript.money = dealerHealth;

        dealerCashText.text = "Dealer Health: " + dealerScript.GetMoney().ToString();
        cashText.text = "Your Health: " + playerScript.GetMoney().ToString();

        mainText.text = "";
        scoreText.text = "";
        dealerScoreText.text = "";

        playerHealth1.gameObject.SetActive(true);
        playerHealth2.gameObject.SetActive(true);
        playerHealth3.gameObject.SetActive(true);
        playerHealth4.gameObject.SetActive(true);

        dealerHealth1.gameObject.SetActive(true);
        dealerHealth2.gameObject.SetActive(true);
        dealerHealth3.gameObject.SetActive(true);
        dealerHealth4.gameObject.SetActive(true);
        dealerHealth5.gameObject.SetActive(true);

        if (dealerHealth > 5)
        {
            dealerHealth6.gameObject.SetActive(true);
            dealerHealth7.gameObject.SetActive(true);
        }
        else
        {
            dealerHealth6.gameObject.SetActive(false);
            dealerHealth7.gameObject.SetActive(false);
        }

        if (dealerHealth > 8)
        {
            dealerHealth8.gameObject.SetActive(true);
            dealerHealth9.gameObject.SetActive(true);
            dealerHealth10.gameObject.SetActive(true);

        }
        else
        {
            dealerHealth8.gameObject.SetActive(false);
            dealerHealth9.gameObject.SetActive(false);
            dealerHealth10.gameObject.SetActive(false);
        }

        // Gain 3 items
        if (startingItems == 1)
        {
            magnifyingGlassN++;
            sunGlassN++;
            daggerN++;
            swordN++;
            rustySwordN++;
            hookN++;
            cigarN++;
            gloveN++;
        }

        if (startingItems == 3)
        {
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
        }

        if (startingItems == 5)
        {
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
        }

        if (startingItems == 8)
        {
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
            AddItem();
        }

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
        // dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        // dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update the scores displayed
        // scoreText.text = "Hand: " + playerScript.handValue.ToString();
        // dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
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
        dealerCashText.text = "Dealer Health: " + dealerScript.GetMoney().ToString();
        cashText.text = "Your Health: " + playerScript.GetMoney().ToString();

        CheckBust();
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
            // dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20)
            {
                RoundOver();
                return;
            }
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

        // dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21) return;
        bool roundOver = true;
        // All bust, bets returned
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust.";
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
            playerScript.AdjustMoney(-1);
        }
        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            dealerScript.AdjustMoney(-1);
        }
        //Check for tie, return bets
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Tied.";
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
            // dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            dealerCashText.text = "Dealer Health: " + dealerScript.GetMoney().ToString();
            cashText.text = "Your Health: " + playerScript.GetMoney().ToString();
            standClicks = 0;
        }

        topDeck.SetActive(false);

        AddItem();
        CheckHealth();
        CheckGameOver();
    }

    void CheckHealth()
    {
        if (playerScript.GetMoney() < 4)
        {
            playerHealth4.gameObject.SetActive(false);
        }
        if (playerScript.GetMoney() < 3)
        {
            playerHealth3.gameObject.SetActive(false);
        }
        if (playerScript.GetMoney() < 2)
        {
            playerHealth2.gameObject.SetActive(false);
        }
        if (playerScript.GetMoney() < 1)
        {
            playerHealth1.gameObject.SetActive(false);
        }

        if (dealerScript.GetMoney() < 10)
        {
            dealerHealth10.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 9)
        {
            dealerHealth9.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 8)
        {
            dealerHealth8.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 7)
        {
            dealerHealth7.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 6)
        {
            dealerHealth6.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 5)
        {
            dealerHealth5.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 4)
        {
            dealerHealth4.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 3)
        {
            dealerHealth3.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 2)
        {
            dealerHealth2.gameObject.SetActive(false);
        }
        if (dealerScript.GetMoney() < 1)
        {
            dealerHealth1.gameObject.SetActive(false);
        }
    }

    private int round = 0;
    void AddItem()
    {
        round++;

        if (round == 2)
        {
            System.Random random = new System.Random();
            int rint = random.Next(0, 20);

            if (rint >= 0 && rint <= 7) magnifyingGlassN++;
            else if (rint >= 8 && rint <= 8) sunGlassN++;
            else if (rint >= 9 && rint <= 10) daggerN++;
            else if (rint >= 11 && rint <= 12) swordN++;
            else if (rint >= 13 && rint <= 13) rustySwordN++;
            else if (rint >= 14 && rint <= 14) hookN++;
            else if (rint >= 15 && rint <= 16) cigarN++;
            else if (rint >= 17 && rint <= 19) gloveN++;

            InitializeCheatText();
            round = 0;
        }
    }

    void CheckGameOver()
    {
        if (playerScript.money == 0)
        {
            gameEnd.text = "Game Over!.";
            GameOver();
        }
        else if (dealerScript.money == 0)
        {
            gameEnd.text = "Won Stage!";
            GameOver();
        }
    }

    void Won()
    {
        DisableCheat();
        DisableButton();
        gameEnd.text = "Won Stage!";

        mainText.text = "";
    }

    void GameOver()
    {
        dealBtn.gameObject.SetActive(false);
        DisableCheat();
        DisableButton();
        gameEnd.gameObject.SetActive(true);
        returnMain.gameObject.SetActive(true);
        returnMain.onClick.AddListener(() => ReturnClicked());
        mainText.text = "";
    }

    void ReturnClicked()
    {
        Debug.Log("CurrentStage " + GameStage.stage);
        Debug.Log("CurrentMode " + GameStage.mode);
        if (GameStage.mode == 0 && gameEnd.text == "Won Stage!") GameStage.stage = stage + 1;
        Debug.Log("UptStage " + GameStage.stage);
        SceneManager.LoadScene("Map");
        returnMain.gameObject.SetActive(false);
    }

    // Add money to pot if bet clicked
    void BetClicked()
    {
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));    
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
    }

    // Added Cheats
    void MagnifyingGlass()
    {
        if (addCheat) return;
        else
        {
            int cardValue = deckScript.TopDeck(topDeck.GetComponent<CardScript>());
            topDeck.GetComponent<Renderer>().enabled = true;
            topDeck.SetActive(true);
            magnifyingGlassN--;
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void SunGlass()
    {
        if (addCheat) return;
        else
        {
            hideCard.GetComponent<Renderer>().enabled = false;
            sunGlassN--;
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void Dagger()
    {
        if (addCheat) return;
        else
        {
            // Check that there is still room on the table
            if (playerScript.cardIndex <= 10)
            {
                playerScript.GetHalfCard();
                // scoreText.text = "Hand: " + playerScript.handValue.ToString();
                if (playerScript.handValue > 20)
                {
                    standBtnText.text = "Black Jack!";
                    if (playerScript.handValue > 21) standBtnText.text = "Busted!";
                    DisableCheat();
                    RoundOver();
                }
                topDeck.SetActive(false);
                daggerN--;
            }
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void Sword()
    {
        if (addCheat) return;
        else
        {
            DisableCheat();
            DisableButton();
            selectScreen.gameObject.SetActive(true);
            selectScreen.ChooseDestoryCard(playerScript.hand[0], playerScript.hand[1]);
            swordN--;
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void RustySword()
    {
        if (addCheat) return;
        else
        {
            DisableCheat();
            DisableButton();
            selectScreen.gameObject.SetActive(true);
            selectScreen.ChooseDestoryOppoCard(dealerScript.hand[0], dealerScript.hand[1]);
            topDeck.SetActive(false);
            rustySwordN--;
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void Hook()
    {
        if (addCheat) return;
        else
        {
            deckScript.currentIndex++;
            topDeck.SetActive(false);
            hookN--;
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void Cigar()
    {
        if (addCheat) return;
        else
        {
            playerScript.ResetAllHandAngle();
            playerScript.ResetHand();
            playerScript.StartHand();
            topDeck.SetActive(false);
            CheckBust();
            cigarN--;
        }
        CheckHaveItem();
        InitializeCheatText();
    }

    void Glove()
    {
        if (addCheat) return;
        else
        {
            DisableCheat();
            DisableButton();
            selectScreen.gameObject.SetActive(true);
            selectScreen.ChooseGlove(playerScript.hand[0], playerScript.hand[1]);
            gloveN--;
        }
        CheckHaveItem();
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
        // scoreText.text = "Hand: " + playerScript.handValue.ToString();
        if (playerScript.handValue > 20)
        {
            standBtnText.text = "Black Jack!";
            if (playerScript.handValue > 21) standBtnText.text = "Busted!";
            DisableCheat();
        }
        RoundOver();
    }

    public void CheckDealerBust()
    {
        // dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        if (dealerScript.handValue > 20)
        {
            DisableCheat();
        }
        RoundOver();
    }

    public GameObject itemDescription;
    public SpriteRenderer item1;
    public SpriteRenderer item2;
    public SpriteRenderer item3;
    public SpriteRenderer item4;
    public SpriteRenderer item5;
    public SpriteRenderer item6;
    public SpriteRenderer item7;
    public SpriteRenderer item8;

    void CheckHaveItem()
    {
        if (magnifyingGlassN == 0)
        {
            item1.enabled = false;
        }
        if (sunGlassN == 0)
        {
            item2.enabled = false;
        }
        if (gloveN == 0)
        {
            item3.enabled = false;
        }
        if (cigarN == 0)
        {
            item4.enabled = false;
        }
        if (hookN == 0)
        {
            item5.enabled = false;
        }
        if (swordN == 0)
        {
            item6.enabled = false;
        }
        if (rustySwordN == 0)
        {
            item7.enabled = false;
        }
        if (daggerN == 0)
        {
            item8.enabled = false;
        }
    }

    public void DisplayItemDescription(string itemName)
    {
        itemDescription.SetActive(true);

        if (itemName == "MagGlass")
        {
            item1.enabled = true;
        }
        if (itemName == "SunGlass")
        {
            item2.enabled = true;
        }
        if (itemName == "Glove")
        {
            item3.enabled = true;
        }
        if (itemName == "Cigar")
        {
            item4.enabled = true;
        }
        if (itemName == "Hook")
        {
            item5.enabled = true;
        }
        if (itemName == "Sword")
        {
            item6.enabled = true;
        }
        if (itemName == "RustySword")
        {
            item7.enabled = true;
        }
        if (itemName == "Dagger")
        {
            item8.enabled = true;
        }

    }

    public void RemoveItemDescription()
    {
        item1.enabled = false;
        item2.enabled = false;
        item3.enabled = false;
        item4.enabled = false;
        item5.enabled = false;
        item6.enabled = false;
        item7.enabled = false;
        item8.enabled = false;
    }
}
