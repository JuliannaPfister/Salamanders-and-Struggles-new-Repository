using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    public Sprite[] HeartSprites;

    public Image HeartsUI;

    private PlayerMovementALTERNATE player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementALTERNATE>();

    }

    private void Update()
    {
        HeartsUI.sprite = HeartSprites[player.health];
    }
}
