using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState state = GameState.ChooseAttack;

    public GameState State { get => state; set => state = value; }

    public enum GameState
    {
        ChooseAttack,
        Attacks,
        Damages,
        Draw,
        GameOver,
    }
    // Start is called before the first frame update
    private void Start()
    {
        // Debug.Log(state == State.ChooseAttack);

        // state = State.Damages;
        // Debug.Log(state);


    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.ChooseAttack)
        {
            // ChooseAttack logic
        }
        else if (state == GameState.Attacks)
        {
            // Attacks logic
        }
        else if (state == GameState.Damages)
        {
            // Damages logic
        }
        else if (state == GameState.Draw)
        {
            // Draw logic
        }
    }
}
