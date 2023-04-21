using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OkapifanUtils.GenericSelectionUI;
using System.Linq;
using TMPro;

public class PartyScreen : SelectionUI<ButtonSlot>
{
    [SerializeField] TMP_Text messageText;
    [SerializeField] PartyScreenPokemonInfo infoBox;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    PokemonParty party;

    //selectedItem is protected property from SelectionUI
    public Pokemon SelectedMember => pokemons[selectedItem];
    public ButtonSlot SelectedUISlot => GetComponentsInChildren<ButtonSlot>()[selectedItem];

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);

        party = PokemonParty.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    public void SetPartyData()
    {
        pokemons = party.Pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(pokemons[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        var bSlots = memberSlots.Select(m => m.GetComponent<ButtonSlot>());
        SetItems(bSlots.Take(pokemons.Count).ToList());

        messageText.text = "Choose a Pokemon";
    }

    public void ShowIfTmIsUsable(TmItem tmItem)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            string message = tmItem.CanBeTaught(pokemons[i]) ? "Can Learn" : "Cannot Learn";
            memberSlots[i].ShowAdditionalMessage(message);
        }
    }

    public void ShowIfEvoItemIsUsable(EvolutionItem evoItem)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            string message = evoItem.CanBeUsed(pokemons[i]) ? "Can Use" : "Cannot Use";
            memberSlots[i].ShowAdditionalMessage(message);
        }
    }

    public void ClearMemberSlotMessages()
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            memberSlots[i].ShowAdditionalMessage("");
        }
    }

    public void SetMessageText(string message)
    {
        //messageText.text = message;
    }

    public override void UpdateSelectionInUI()
    {
        base.UpdateSelectionInUI();

        infoBox.UpdateInfoPanel(SelectedMember);
    }

    public void EnterKeyboardMode() => infoBox.EnterKeyboardMode();
}
