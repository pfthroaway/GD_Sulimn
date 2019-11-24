using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Card;
using Sulimn.Classes.Enums;
using Sulimn.Classes.Extensions;
using System;
using System.Collections.Generic;

public class BlackjackScene : Control
{
    private bool _handOver = true;
    private Button BtnDealHand, BtnReturn, BtnHit, BtnStay, BtnConvertAce, BtnSplit, BtnHitSplit, BtnStaySplit, BtnConvertAceSplit;
    private Container playerHand, splitHand, dealerHand;
    private Hand PlayerHand, SplitHand, DealerHand;
    private int _index, _bet, _sidePot, _totalWins, _totalLosses, _totalDraws, _totalBetWinnings, _totalBetLosses;
    private LineEdit TxtBet;
    private List<Card> CardList = new List<Card>();
    private VBoxContainer SplitContainer;

    private void AssignControls()
    {
        BtnDealHand = (Button)FindNode("BtnDealHand");
        BtnHit = (Button)FindNode("BtnHit");
        BtnStay = (Button)FindNode("BtnStay");
        BtnConvertAce = (Button)FindNode("BtnConvertAce");
        BtnSplit = (Button)FindNode("BtnSplit");
        BtnHitSplit = (Button)FindNode("BtnHitSplit");
        BtnStaySplit = (Button)FindNode("BtnStaySplit");
        BtnConvertAceSplit = (Button)FindNode("BtnConvertAceSplit");
        BtnReturn = (Button)FindNode("BtnReturn");
        playerHand = (Container)GetNode("MainHand/Hand");
        splitHand = (Container)GetNode("SplitHand/Hand");
        dealerHand = (Container)GetNode("DealerHand/Hand");
        SplitContainer = (VBoxContainer)GetNode("SplitHand");
    }

    #region Display

    private void DisplayHands()
    {
        DisplayDealerHand();
        DisplaySplitHand();
        DisplayPlayerHand();
    }

    private void DisplayDealerHand()
    {
        dealerHand.GetChildren().Clear();
        foreach (Card card in DealerHand.CardList)
            dealerHand.AddChild(card);
    }

    private void DisplaySplitHand()
    {
        SplitContainer.Visible = SplitHand.CardList.Count > 0;

        splitHand.GetChildren().Clear();
        foreach (Card card in SplitHand.CardList)
            splitHand.AddChild(card);
    }

    private void DisplayPlayerHand()
    {
        playerHand.GetChildren().Clear();
        foreach (Card card in PlayerHand.CardList)
            playerHand.AddChild(card);
    }

    #endregion Display

    #region Card Management

    /// <summary>Converts the first Ace valued at eleven in the Hand to be valued at one.</summary>
    /// <param name="handConvert">Hand to be converted.</param>
    private void ConvertAce(Hand handConvert)
    {
        handConvert.ConvertAce();
        DisplayHands();
    }

    /// <summary>Creates a deck of Cards.</summary>
    /// <param name="numberOfDecks">Number of standard-sized decks to add to total deck.</param>
    private void CreateDeck(int numberOfDecks)
    {
        CardList.Clear();
        for (int h = 0; h < numberOfDecks; h++)
            for (int i = 1; i < 14; i++)
                for (int j = 0; j < 4; j++)
                {
                    string name;
                    CardSuit suit = CardSuit.Spades;
                    int value;

                    switch (j)
                    {
                        case 0:
                            suit = CardSuit.Spades;
                            break;

                        case 1:
                            suit = CardSuit.Hearts;
                            break;

                        case 2:
                            suit = CardSuit.Clubs;
                            break;

                        case 3:
                            suit = CardSuit.Diamonds;
                            break;
                    }

                    switch (i)
                    {
                        case 1:
                            name = "Ace";
                            value = 11;
                            break;

                        case 11:
                            name = "Jack";
                            value = 10;
                            break;

                        case 12:
                            name = "Queen";
                            value = 10;
                            break;

                        case 13:
                            name = "King";
                            value = 10;
                            break;

                        default:
                            name = i.ToString();
                            value = i;
                            break;
                    }

                    Card newCard = new Card(name, suit, value, false);
                    CardList.Add(newCard);
                }
    }

    /// <summary>Deals a Card to a specific Hand.</summary>
    /// <param name="handAdd">Hand where Card is to be added.</param>
    /// <param name="hidden">Should the Card be hidden?</param>
    private void DealCard(Hand handAdd, bool hidden = false)
    {
        handAdd.AddCard(new Card(CardList[_index], hidden));
        _index++;
    }

    /// <summary>Creates a new Hand for both the Player and the Dealer.</summary>
    private void DealHand()
    {
        PlayerHand = new Hand();
        DealerHand = new Hand();

        _handOver = false;
        TxtBet.Editable = false;
        //ToggleNewGameExitButtons(false);
        if (_index >= CardList.Count * 0.8)
        {
            _index = 0;
            CardList.Shuffle();
        }

        DealCard(PlayerHand);
        DealCard(DealerHand);
        DealCard(PlayerHand);
        DealCard(DealerHand, true);
        DisplayHands();
    }

    #endregion Card Management

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        Hand hand = new Hand();
        hand.AddCard(new Card("2", CardSuit.Spades, 2, false));
        hand.AddCard(new Card("Ace", CardSuit.Spades, 11, false));
        hand.AddCard(new Card("8", CardSuit.Spades, 8, false));
        hand.AddCard(new Card("8", CardSuit.Spades, 8, false));
        hand.AddCard(new Card("8", CardSuit.Spades, 8, false));
    }

    private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}