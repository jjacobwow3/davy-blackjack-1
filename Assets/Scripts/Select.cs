using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public Button card1;
    public Button card2;
    public Button cardo1;
    public Button cardo2;
    public Button cardg1;
    public Button cardg2;
    public Button cardg3;
    public Button cardg4;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public GameManager gameManager;

    public int choice = 0;

    public void OnDisable()
    {
        card1.gameObject.SetActive(false);
        card2.gameObject.SetActive(false);

        cardo1.gameObject.SetActive(false);
        cardo2.gameObject.SetActive(false);

        cardg1.gameObject.SetActive(false);
        cardg2.gameObject.SetActive(false);
        cardg3.gameObject.SetActive(false);
        cardg4.gameObject.SetActive(false);
    }

    public void Start()
    {
        card1.onClick.AddListener(() => DesCard1());
        card2.onClick.AddListener(() => DesCard2());
        cardo1.onClick.AddListener(() => DesOppoCard1());
        cardo2.onClick.AddListener(() => DesOppoCard2());

        cardg1.onClick.AddListener(() => Glove1());
        cardg2.onClick.AddListener(() => Glove2());
        cardg3.onClick.AddListener(() => Glove3());
        cardg4.onClick.AddListener(() => Glove4());
    }

    public void ChooseDestoryCard(GameObject choice1, GameObject choice2)
    {
        if (playerScript.hand[0].GetComponent<CardScript>().value != 0)
        {
            card1.gameObject.SetActive(true);
            card1.image.sprite = choice1.GetComponent<SpriteRenderer>().sprite;
        }
        if (playerScript.hand[1].GetComponent<CardScript>().value != 0)
        {
            card2.gameObject.SetActive(true);
            card2.image.sprite = choice2.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void DesCard1()
    {
        playerScript.RemoveCard(0);
        gameManager.topDeck.SetActive(false);
        gameManager.CheckBust();
        gameManager.DisableSelect();
    }

    public void DesCard2()
    {
        playerScript.RemoveCard(1);
        gameManager.topDeck.SetActive(false);
        gameManager.CheckBust();
        gameManager.DisableSelect();
    }
    public void ChooseDestoryOppoCard(GameObject choice1, GameObject choice2)
    {
        if (dealerScript.hand[0].GetComponent<CardScript>().value != 0)
        {
            cardo1.gameObject.SetActive(true);
        }
        if (dealerScript.hand[1].GetComponent<CardScript>().value != 0)
        {
            cardo2.gameObject.SetActive(true);
            cardo2.image.sprite = choice2.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void DesOppoCard1()
    {
        dealerScript.RemoveCard(0);
        gameManager.topDeck.SetActive(false);
        gameManager.hideCard.GetComponent<Renderer>().enabled = false;
        gameManager.CheckBust();
        gameManager.DisableSelect();
    }

    public void DesOppoCard2()
    {
        dealerScript.RemoveCard(1);
        gameManager.topDeck.SetActive(false);
        gameManager.CheckBust();
        gameManager.DisableSelect();
    }

    public void ChooseGlove(GameObject choice1, GameObject choice2)
    {
        if (playerScript.hand[0].GetComponent<CardScript>().value != 0)
        {
            cardg1.gameObject.SetActive(true);
            cardg1.image.sprite = choice1.GetComponent<SpriteRenderer>().sprite;
        }
        if (playerScript.hand[1].GetComponent<CardScript>().value != 0)
        {
            cardg2.gameObject.SetActive(true);
            cardg2.image.sprite = choice2.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private int choice1 = 0;
    public void Glove1()
    {
        ChooseGloveOppo(playerScript.hand[0], dealerScript.hand[1]);
        choice1 = 1;
    }

    public void Glove2()
    {
        ChooseGloveOppo(playerScript.hand[1], dealerScript.hand[1]);
        choice1 = 2;
    }

    public void ChooseGloveOppo(GameObject choiced, GameObject choice2)
    {
        cardg1.gameObject.SetActive(false);
        cardg2.gameObject.SetActive(false);
        if (dealerScript.hand[0].GetComponent<CardScript>().value != 0)
        {
            cardg3.gameObject.SetActive(true);
        }
        if (dealerScript.hand[1].GetComponent<CardScript>().value != 0)
        {
            cardg4.gameObject.SetActive(true);
            cardg4.image.sprite = choice2.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void Glove3()
    {
        if (choice1 == 1)
        {
            int temp1 = playerScript.hand[0].GetComponent<CardScript>().value;
            int temp2 = dealerScript.hand[0].GetComponent<CardScript>().value;
            Sprite newSprite1 = playerScript.hand[0].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            Sprite newSprite2 = dealerScript.hand[0].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            playerScript.ChangeCard(0, temp2, newSprite2);
            dealerScript.ChangeCard(0, temp1, newSprite1);
        }
        else if (choice1 == 2)
        {
            int temp1 = playerScript.hand[1].GetComponent<CardScript>().value;
            int temp2 = dealerScript.hand[0].GetComponent<CardScript>().value;
            Sprite newSprite1 = playerScript.hand[1].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            Sprite newSprite2 = dealerScript.hand[0].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            playerScript.ChangeCard(1, temp2, newSprite2);
            dealerScript.ChangeCard(0, temp1, newSprite1);
        }
        gameManager.CheckBust();
        gameManager.DisableSelect();
    }

    public void Glove4()
    {
        if (choice1 == 1)
        {
            int temp1 = playerScript.hand[0].GetComponent<CardScript>().value;
            int temp2 = dealerScript.hand[1].GetComponent<CardScript>().value;
            Sprite newSprite1 = playerScript.hand[0].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            Sprite newSprite2 = dealerScript.hand[1].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            playerScript.ChangeCard(0, temp2, newSprite2);
            dealerScript.ChangeCard(1, temp1, newSprite1);
        }
        else if (choice1 == 2)
        {
            int temp1 = playerScript.hand[1].GetComponent<CardScript>().value;
            int temp2 = dealerScript.hand[1].GetComponent<CardScript>().value;
            Sprite newSprite1 = playerScript.hand[1].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            Sprite newSprite2 = dealerScript.hand[1].
                GetComponent<CardScript>().GetComponent<SpriteRenderer>().sprite;
            playerScript.ChangeCard(1, temp2, newSprite2);
            dealerScript.ChangeCard(1, temp1, newSprite1);
        }
        gameManager.CheckBust();
        gameManager.DisableSelect();
    }
}
