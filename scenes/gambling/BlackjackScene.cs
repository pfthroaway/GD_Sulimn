using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Card;
using Sulimn.Classes.Enums;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.Extensions.DataTypeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

public class BlackjackScene : Control
{
    private bool _handOver = true;
    private Button BtnDealHand, BtnReturn, BtnHit, BtnStay, BtnConvertAce, BtnSplit, BtnHitSplit, BtnStaySplit, BtnConvertAceSplit;
    private Container playerHand, splitHand, dealerHand;
    private Hand MainHand, SplitHand, DealerHand;
    private int _index, _sidePot;
    private Label LblMainTotal, LblSplitTotal, LblDealerTotal, LblStatistics;
    private LineEdit TxtBet;
    private List<Card> CardList = new List<Card>();
    private RichTextLabel TxtBlackJack;
    private VBoxContainer SplitContainer;

    #region Properties

    /// <summary>Current bet.</summary>
    public int Bet { get; set; }

    /// <summary>Total wins for the player.</summary>
    public int TotalWins { get; set; }

    /// <summary>Total losses for the player.</summary>
    public int TotalLosses { get; set; }

    /// <summary>Total draws.</summary>
    public int TotalDraws { get; set; }

    /// <summary>Total bet winnings for the player.</summary>
    public int TotalBetWinnings { get; set; }

    /// <summary>Total bet losses for the player.</summary>
    public int TotalBetLosses { get; set; }

    /// <summary>Statistics about the player's games.</summary>
    public string Statistics => $"Wins: {TotalWins:N0}\n" +
    $"Losses: {TotalLosses:N0}\n" +
    $"Draws: {TotalDraws:N0}\n" +
    $"Gold Won: {TotalBetWinnings:N0}\n" +
    $"Gold Lost: {TotalBetLosses:N0}";

    #endregion Properties

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
        dealerHand = (Container)GetNode("DealerHand");
        SplitContainer = (VBoxContainer)GetNode("SplitHand");
        LblMainTotal = (Label)FindNode("LblMainTotal");
        LblSplitTotal = (Label)FindNode("LblSplitTotal");
        LblDealerTotal = (Label)GetNode("LblDealerTotal");
        LblStatistics = (Label)GetNode("LblStatistics");
        TxtBet = (LineEdit)GetNode("TxtBet");
        TxtBlackJack = (RichTextLabel)GetNode("TxtBlackjack");
    }

    /// <summary>Adds text to the TxtBattle RichTextLabel.</summary>
    /// <param name="newText">Text to be added</param>
    private void AddTextToTextBox(string newText)
    {
        TxtBlackJack.Text += TxtBlackJack.Text.Length > 0 ? "\n\n" + newText : newText;
        TxtBlackJack.ScrollFollowing = true;
    }

    /// <summary>Action taking place on the Dealer's turn.</summary>
    private void DealerAction()
    {
        bool keepGoing = true;

        while (keepGoing)
            if (DealerHand.ActualValue == 21)
                keepGoing = false;
            else if (DealerHand.ActualValue >= 17)
                if (DealerHand.ActualValue > 21 && CheckHasAceEleven(DealerHand))
                    ConvertAce(DealerHand);
                else
                    keepGoing = false;
            else
                DealCard(DealerHand);
    }

    #region Display

    private void DisplayHands()
    {
        if (!CheckBlackjack(MainHand) && !CheckBust(MainHand) && !_handOver) //Player can still play
            CheckButtons();
        else if (CheckBust(MainHand))
        {
            AddTextToTextBox("You bust!");
            LoseBlackjack(Bet);
        }
        else if (CheckBlackjack(MainHand))
        {
            if (MainHand.CardList.Count == 2)
            {
                AddTextToTextBox("You have a natural blackjack!");
                if (DealerHand.TotalValue != 21)
                    WinBlackjack(Int32Helper.Parse(Bet * 1.5));
                else
                {
                    AddTextToTextBox("You and the dealer both have natural blackjacks.");
                    DrawBlackjack();
                }
            }
            else
                Stay();
        }
        else if (!_handOver)
        {
            // FUTURE
        }

        DisplayDealerHand();
        DisplaySplitHand();
        DisplayPlayerHand();
    }

    private void DisplayDealerHand()
    {
        if (_handOver)
            DealerHand.ClearHidden();
        while (dealerHand.GetChildren().Count > 0)
            dealerHand.RemoveChild(dealerHand.GetChild(0));
        foreach (Card card in DealerHand.CardList)
        {
            GD.Print(card.CardToString);
            dealerHand.AddChild(card);
        }
        LblDealerTotal.Text = DealerHand.TotalValue > 0 ? DealerHand.Value : "";
    }

    private void DisplaySplitHand()
    {
        SplitContainer.Visible = SplitHand?.CardList?.Count > 0;
        if (SplitHand?.CardList?.Count > 0)
        {
            splitHand.GetChildren().Clear();
            foreach (Card card in SplitHand.CardList)
                splitHand.AddChild(card);
        }
        LblSplitTotal.Text = SplitHand?.TotalValue > 0 ? SplitHand?.Value : "";
    }

    private void DisplayPlayerHand()
    {
        while (playerHand.GetChildren().Count > 0)
            playerHand.RemoveChild(playerHand.GetChild(0));
        playerHand.GetChildren().Clear();
        foreach (Card card in MainHand.CardList)
            playerHand.AddChild(card);

        LblMainTotal.Text = MainHand.TotalValue > 0 ? MainHand.Value : "";
    }

    #endregion Display

    #region Check Logic

    /// <summary>Checks whether a Hand has reached Blackjack.</summary>
    /// <param name="checkHand">Hand to be checked</param>
    /// <returns>Returns true if the Hand's value is 21.</returns>
    private static bool CheckBlackjack(Hand checkHand) => checkHand.TotalValue == 21;

    /// <summary>Checks whether the current Hand has gone Bust.</summary>
    /// <param name="checkHand">Hand to be checked</param>
    /// <returns>Returns true if Hand has gone Bust.</returns>
    private static bool CheckBust(Hand checkHand) => !CheckHasAceEleven(checkHand) && checkHand.TotalValue > 21;

    /// <summary>Checks whether the Player can Double Down with Window Hand.</summary>
    /// <param name="checkHand">Hand to be checked</param>
    /// <param name="checkSplit">Hand the Hand could be split into</param>
    /// <returns>Returns true if Hand can Double Down.</returns>
    private bool CheckCanHandDoubleDown(Hand checkHand, Hand checkSplit) => checkHand.CardList.Count == 2 && checkSplit.CardList.Count == 0
        && (checkHand.TotalValue >= 9 && checkHand.TotalValue <= 11);

    /// <summary>Checks whether the Player can split a specific Hand.</summary>
    /// <param name="checkHand">Hand to be checked</param>
    /// <param name="checkSplit">Hand it could be spilt into</param>
    /// <returns>Returns true if Hand can be Split.</returns>
    private bool CheckCanHandSplit(Hand checkHand, Hand checkSplit) => checkHand.CardList.Count == 2 && checkSplit.CardList.Count == 0
        && checkHand.CardList[0].Value == checkHand.CardList[1].Value;

    /// <summary>Checks whether a Hand has an Ace valued at eleven in it.</summary>
    /// <param name="checkHand">Hand to be checked</param>
    /// <returns>Returns true if Hand has an Ace valued at eleven in it.</returns>
    private static bool CheckHasAceEleven(Hand checkHand) => checkHand.CardList.Any(card => card.Value == 11);

    /// <summary>Checks whether the Dealer has an Ace face up.</summary>
    /// <returns>Returns true if the Dealer has an Ace face up.</returns>
    private bool CheckInsurance() => !_handOver && DealerHand.CardList[0].Value == 11 && _sidePot == 0;

    #endregion Check Logic

    #region Button Management

    /// <summary>Checks which Buttons should be enabled.</summary>
    private void CheckButtons()
    {
        ToggleHitStay(MainHand.TotalValue >= 21);
        BtnConvertAce.Disabled = !CheckHasAceEleven(MainHand);
    }

    /// <summary>Disables all the buttons on the Page except for BtnDealHand.</summary>
    private void DisablePlayButtons()
    {
        ToggleNewGameExitButtons(false);
        ToggleHitStay(true);
        BtnConvertAce.Disabled = true;
    }

    private void ToggleNewGameExitButtons(bool disabled)
    {
        BtnDealHand.Disabled = disabled;
        BtnReturn.Disabled = disabled;
    }

    /// <summary>Toggles the Hit and Stay Buttons' Disabled property.</summary>
    /// <param name="disabled">Should the Buttons be disabled?</param>
    private void ToggleHitStay(bool disabled)
    {
        BtnHit.Disabled = disabled;
        BtnStay.Disabled = disabled;
    }

    #endregion Button Management

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
        MainHand = new Hand();
        SplitHand = new Hand();
        DealerHand = new Hand();

        _handOver = false;
        TxtBet.Editable = false;
        ToggleNewGameExitButtons(true);
        if (_index >= CardList.Count * 0.8)
        {
            _index = 0;
            CardList.Shuffle();
        }

        DealCard(MainHand);
        DealCard(DealerHand);
        DealCard(MainHand);
        DealCard(DealerHand, true);
        if (MainHand.CanSplit())
            BtnSplit.Disabled = false;
        DisplayHands();
    }

    #endregion Card Management

    #region Game Results

    /// <summary>The game ends in a draw.</summary>
    private void DrawBlackjack()
    {
        EndHand();
        AddTextToTextBox("You reach a draw.");
        TotalDraws++;
    }

    /// <summary>Ends the Hand.</summary>
    private void EndHand()
    {
        _handOver = true;
        TxtBet.Editable = true;
        DisplayDealerHand();
        DisablePlayButtons();
        DisplayStatistics();
    }

    /// <summary>Displays the game's statistics.</summary>
    private void DisplayStatistics()
    {
        LblStatistics.Text = Statistics;
        GameState.Info.DisplayStats();
    }

    /// <summary>The game ends with the Player losing.</summary>
    /// <param name="betAmount">Amount the Player bet</param>
    private void LoseBlackjack(int betAmount)
    {
        AddTextToTextBox($"You lose {betAmount:N0}.");
        GameState.CurrentHero.Gold -= betAmount;
        TotalLosses++;
        TotalBetLosses += betAmount;
        EndHand();
    }

    /// <summary>Player either chooses not to draw a card or has reached 21 with more than 2 cards.</summary>
    private void Stay()
    {
        _handOver = true;
        DealerAction();
        DisplayDealerHand();

        if (MainHand.TotalValue > DealerHand.TotalValue && !CheckBust(DealerHand))
            WinBlackjack(Bet);
        else if (CheckBust(DealerHand))
            WinBlackjack(Bet);
        else if (MainHand.TotalValue == DealerHand.TotalValue)
            DrawBlackjack();
        else if (MainHand.TotalValue < DealerHand.TotalValue)
            LoseBlackjack(Bet);
    }

    /// <summary>The game ends with the Player winning.</summary>
    /// <param name="betAmount">Amount the Player bet</param>
    private void WinBlackjack(int betAmount)
    {
        GameState.CurrentHero.Gold += betAmount;
        AddTextToTextBox($"You win {betAmount}!");
        TotalWins++;
        TotalBetWinnings += betAmount;
        EndHand();
    }

    #endregion Game Results

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        CreateDeck(6);
        CardList.Shuffle();
        DisplayStatistics();
        TxtBet.GrabFocus();
    }

    #region Button Click

    private void _on_BtnDealHand_pressed()
    {
        Bet = Int32Helper.Parse(TxtBet.Text);
        if (Bet > 0 && Bet <= GameState.CurrentHero.Gold)
            DealHand();
        else
            AddTextToTextBox(Bet > GameState.CurrentHero.Gold ? "You can't bet more gold than you have!" : "Please enter a valid bet.");
    }

    private void _on_BtnHit_pressed()
    {
        DealCard(MainHand);
        BtnSplit.Disabled = true;
        _index++;
        if (MainHand.CardList.Count < 5)
            DisplayHands();
        else
        {
            if (MainHand.TotalValue < 21 || (CheckHasAceEleven(MainHand) && MainHand.TotalValue <= 31))
            {
                AddTextToTextBox("Five Card Charlie!");
                DisplayPlayerHand();
                WinBlackjack(Bet);
            }
            else
                DisplayHands();
        }
    }

    private void _on_BtnStay_pressed() => Stay();

    private void _on_BtnConvertAce_pressed()
    {
        ConvertAce(MainHand);
        DisplayHands();
    }

    private void _on_BtnSplit_pressed()
    {
        // TODO Implement logic for split hand.
        Card moveCard = MainHand.CardList[1];
        MainHand.CardList.RemoveAt(1);
        playerHand.RemoveChild(moveCard);
        SplitHand.AddCard(moveCard);
        BtnSplit.Disabled = true;
        splitHand.Visible = true;
        DealCard(MainHand);
        DealCard(SplitHand);
        DisplayHands();
    }

    private void _on_BtnHitSplit_pressed()
    {
        // Replace with function body.
    }

    private void _on_BtnConvertAceSplit_pressed()
    {
        // Replace with function body.
    }

    private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    #endregion Button Click
}