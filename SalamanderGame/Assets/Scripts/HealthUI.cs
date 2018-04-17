using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    public Sprite[] HeartSprites;

    public Image HeartsUI;

    private PlayerMovement player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        HeartsUI.sprite = HeartSprites[player.health];
    }
}
