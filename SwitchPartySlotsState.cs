using OkapifanUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPartySlotsState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;

    public static SwitchPartySlotsState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        partyScreen.OnSelected += OnPokemonToSwitch;
        partyScreen.OnBack += OnBack;
    }

    public override void Execute()
    {
        partyScreen.HandleUpdate();
    }

    public override void Exit()
    {
        partyScreen.OnSelected -= OnPokemonToSwitch;
        partyScreen.OnBack -= OnBack;
    }

    void OnPokemonToSwitch(int selection)
    {
        Debug.Log($"Switch slot {PartyMenuState.i.SelectedPokemon} with slot {selection}");
        if (PartyMenuState.i.SelectedPokemon != selection)
        {
            // Switching Around is Allowed
            Debug.Log($"Switch slot {PartyMenuState.i.SelectedPokemon} with slot {selection}");
            PokemonParty.GetPlayerParty().SwitchAroundPokemon(PartyMenuState.i.SelectedPokemon, selection);
            gc.StateMachine.Pop();
        }
        else
        {
            Debug.Log("U can't select the same Pokémon");
            gc.StateMachine.Pop();
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
