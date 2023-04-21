using OkapifanUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMenuState : State<GameController>
{
    [SerializeField] PartyMenuScreen partyMenuUI;

    public int SelectedPokemon { get; set; }

    public static PartyMenuState i { get; private set; }
    GameController gc;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        gc = owner;

        partyMenuUI.gameObject.SetActive(true);
        partyMenuUI.OnSelected += OnActionSelected;
        partyMenuUI.OnBack += OnBack;

        partyMenuUI.UpdateSelectionInUI();
    }

    public override void Execute()
    {
        // Controlls

        partyMenuUI.HandleUpdate();
    }

    public override void Exit()
    {
        partyMenuUI.gameObject.SetActive(false);
        partyMenuUI.OnSelected -= OnActionSelected;
        partyMenuUI.OnBack -= OnBack;
    }

    void OnActionSelected(int selection)
    {
        switch (selection) {
            case 0: // Summary
                break;
            case 1: // Switch
                gc.StateMachine.ChangeState(SwitchPartySlotsState.i);
                break;
            default: // Item
                break;
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
