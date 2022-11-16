using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Attacks attacks;
    public Transform atkPosRef;
    public Player player;

    Vector2 startPosition;

    public void OnClick()
    {
        player.SetAttack(attacks);
    }

    private void Start() {
        startPosition = this.transform.position;
    }

    float timer = 0;

    private void Update() {
        if(timer <= 1)
        {
            timer += 1;
        }
        else
        {
            
        }
    }
}
