using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OkapifanUtils.StateMachine;

public class PartyScreenState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;

    public static PartyScreenState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        partyScreen.Init();

        partyScreen.gameObject.SetActive(true);
        partyScreen.OnSelected += OnPokemonSelected;
        partyScreen.OnBack += OnBack;
    }

    public override void Execute()
    {
        if (gc.StateMachine.GetPreviousState() == GameMenuState.i)
        {
            if (PlayerController.playerControls.Game.RightTrigger.triggered)
                gc.StateMachine.Push(StorageScreenState.i);

            if (PlayerController.playerControls.Game.KeyItem.triggered)
                if (PokemonParty.GetPlayerParty().Pokemons.Count > 1)
                    PokemonParty.GetPlayerParty().SendToBox(partyScreen.SelectedMember, StorageScreen.CurrentBox);

            if (PlayerController.playerControls.Game.SelectKey.triggered)
                partyScreen.EnterKeyboardMode();
        } else if (gc.StateMachine.GetPreviousState() == InventoryState.i)
        {
            if (InventoryState.i.SelectedItem is TmItem)
                partyScreen.ShowIfTmIsUsable(InventoryState.i.SelectedItem as TmItem);
            else if (InventoryState.i.SelectedItem is EvolutionItem)
                partyScreen.ShowIfEvoItemIsUsable(InventoryState.i.SelectedItem as EvolutionItem);
        }

        partyScreen.HandleUpdate();
    }

    public override void Exit()
    {
        partyScreen.gameObject.SetActive(false);
        partyScreen.OnSelected -= OnPokemonSelected;
        partyScreen.OnBack -= OnBack;
    }

    void OnPokemonSelected(int selection)
    {
        if (gc.StateMachine.GetPreviousState() == InventoryState.i)
        {
            // Use Item
            StartCoroutine(GoToUseItemState());
        } else if (gc.StateMachine.GetPreviousState() == BattleState.i)
        {
            Pokemon selectedMember = partyScreen.SelectedMember;
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon");
                return;
            }

            var battleState = gc.StateMachine.GetPreviousState() as BattleState;
            if (selectedMember == battleState.bc.PlayerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You can't switch with the same pokemon");
                return;
            }

            gc.StateMachine.Pop();
        }
        else
        {
            PartyMenuState.i.SelectedPokemon = selection;
            gc.StateMachine.Push(PartyMenuState.i);
        }
    }

    IEnumerator GoToUseItemState()
    {
        yield return gc.StateMachine.PushAndWait(UseItemState.i);
        partyScreen.ClearMemberSlotMessages();
        gc.StateMachine.Pop();
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
        gc.StateMachine.CurrentState.ReEnter();
    }

    public override void ReEnter()
    {
        partyScreen.UpdateSelectionInUI();
    }

    public ButtonSlot SelectedUISlot => partyScreen.SelectedUISlot;
    public Pokemon SelectedPokemon => partyScreen.SelectedMember;
}
