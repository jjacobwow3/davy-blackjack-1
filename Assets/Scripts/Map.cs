using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{

    public Button stage1;
    public Button stage2;
    public Button stage3;
    public Button endgame;

    public Button unlockStage;
    public Text unlockText;

    public SpriteRenderer flag1;
    public SpriteRenderer flag2;
    public SpriteRenderer flag3;

    public SpriteRenderer cross1;
    public SpriteRenderer cross2;

    public static int stage = 0;

    public void Start()
    {
        Debug.Log("MapInit " + GameStage.stage);
        stage = GameStage.stage;

        unlockStage.onClick.AddListener(() => UnlockStage());
        CheckStage();
        Debug.Log("MapMode " + GameStage.mode);
    }

    void UnlockStage()
    {
        if (GameStage.mode == 1) GameStage.mode = 0; 
        else GameStage.mode = 1;
        CheckStage();
    }

    public void UnlockAll()
    {
        stage = 0;
    }

    public void CheckStage()
    {
        stage1.gameObject.SetActive(false);
        stage2.gameObject.SetActive(false);
        stage3.gameObject.SetActive(false);
        endgame.gameObject.SetActive(false);
        flag1.enabled = false;
        flag2.enabled = false;
        flag3.enabled = false;
        cross1.enabled = false;
        cross2.enabled = false;

        if (GameStage.mode == 1)
        {
            unlockText.color = Color.green;
            unlockText.text = "Free Roam";
            stage1.gameObject.SetActive(true);
            stage2.gameObject.SetActive(true);
            stage3.gameObject.SetActive(true);
        }
        else {
            unlockText.color = Color.red;
            unlockText.text = "Conquest";
            if (stage == 1)
            {
                stage1.gameObject.SetActive(true);
                cross1.enabled = true;
                cross2.enabled = true;
            }
            if (stage == 2)
            {
                stage2.gameObject.SetActive(true);
                flag1.enabled = true;
                cross2.enabled = true;
            }
            if (stage == 3)
            {
                stage3.gameObject.SetActive(true);
                flag1.enabled = true;
                flag2.enabled = true;
            }
            if (stage == 4)
            {
                flag1.enabled = true;
                flag2.enabled = true;
                flag3.enabled = true;
                endgame.gameObject.SetActive(true);
            }
        }
    }
}
