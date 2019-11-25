using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Card;
using Sulimn.Classes.Enums;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.Extensions.DataTypeHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Scenes.Gambling
{
    public class BlackjackScene : Control
    {
        private bool previousStateOfBtnReturn;
        private Button BtnDealHand, BtnReturn, BtnHit, BtnStay, BtnConvertAce, BtnSplit, BtnHitSplit, BtnStaySplit, BtnConvertAceSplit, BtnInsurance, BtnDoubleDown, BtnDoubleDownSplit;
        private Container mainHandContainer, splitHandContainer, dealerHandContainer;
        private Hand MainHand, SplitHand, DealerHand;
        private int _index;
        private Label LblMainTotal, LblSplitTotal, LblDealerTotal, LblStatistics;
        private LineEdit TxtBet, TxtInsurance;
        private readonly List<Card> CardList = new List<Card>();
        private Panel PnlHelp;
        private RichTextLabel TxtBlackJack;
        private VBoxContainer SplitContainer;

        #region Properties

        /// <summary>Is the Main Hand over?</summary>
        public bool MainHandOver { get; set; } = true;

        /// <summary>Is the Split Hand over?</summary>
        public bool SplitHandOver { get; set; } = true;

        /// <summary>Is the round over?</summary>
        public bool RoundOver => MainHandOver && SplitHandOver;

        /// <summary>Current bet for the main hand.</summary>
        public int MainBet { get; set; }

        /// <summary>Current bet for the split hand.</summary>
        public int SplitBet { get; set; }

        /// <summary>Current insurance bet.</summary>
        public int SidePot { get; set; }

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

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
            {
                if (PnlHelp.Visible)
                    _on_BtnHideHelp_pressed();
                else if (BtnReturn.Disabled == false)
                    _on_BtnReturn_pressed();
            }
        }

        private void AssignControls()
        {
            BtnDealHand = (Button)FindNode("BtnDealHand");
            BtnHit = (Button)FindNode("BtnHit");
            BtnStay = (Button)FindNode("BtnStay");
            BtnConvertAce = (Button)FindNode("BtnConvertAce");
            BtnDoubleDown = (Button)FindNode("BtnDoubleDown");
            BtnSplit = (Button)FindNode("BtnSplit");
            BtnHitSplit = (Button)FindNode("BtnHitSplit");
            BtnStaySplit = (Button)FindNode("BtnStaySplit");
            BtnConvertAceSplit = (Button)FindNode("BtnConvertAceSplit");
            BtnDoubleDownSplit = (Button)FindNode("BtnDoubleDownSplit");
            BtnInsurance = (Button)FindNode("BtnInsurance");
            BtnReturn = (Button)FindNode("BtnReturn");
            mainHandContainer = (Container)GetNode("MainHand/MainHand");
            splitHandContainer = (Container)GetNode("SplitHand/SplitHand");
            dealerHandContainer = (Container)GetNode("DealerHand");
            SplitContainer = (VBoxContainer)GetNode("SplitHand");
            LblMainTotal = (Label)FindNode("LblMainTotal");
            LblSplitTotal = (Label)FindNode("LblSplitTotal");
            LblDealerTotal = (Label)GetNode("LblDealerTotal");
            LblStatistics = (Label)GetNode("LblStatistics");
            TxtBet = (LineEdit)GetNode("TxtBet");
            TxtInsurance = (LineEdit)GetNode("TxtInsurance");
            TxtBlackJack = (RichTextLabel)GetNode("TxtBlackjack");
            PnlHelp = (Panel)GetNode("PnlHelp");
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
                { keepGoing = false; }
                else if (DealerHand.ActualValue >= 17)
                {
                    if (DealerHand.ActualValue > 21 && DealerHand.HasAceEleven())
                        ConvertAce(DealerHand);
                    else
                        keepGoing = false;
                }
                else
                    DealCard(DealerHand);
        }

        #region Display

        private void DisplayHands()
        {
            DisplayMainHand();
            DisplaySplitHand();
            DisplayDealerHand();
        }

        private void DisplayMainHand()
        {
            while (mainHandContainer.GetChildren().Count > 0)
                mainHandContainer.RemoveChild(mainHandContainer.GetChild(0));

            foreach (Card card in MainHand.CardList)
                mainHandContainer.AddChild(card);

            LblMainTotal.Text = MainHand.TotalValue > 0 ? MainHand.Value : "";
        }

        private void DisplaySplitHand()
        {
            SplitContainer.Visible = SplitHand.Count > 0;
            if (SplitHand.Count > 0)
            {
                while (splitHandContainer.GetChildren().Count > 0)
                    splitHandContainer.RemoveChild(splitHandContainer.GetChild(0));
                foreach (Card card in SplitHand.CardList)
                    splitHandContainer.AddChild(card);
            }
            LblSplitTotal.Text = SplitHand.TotalValue > 0 ? SplitHand.Value : "";
        }

        private void DisplayDealerHand()
        {
            if (MainHandOver && SplitHandOver)
                DealerHand.ClearHidden();
            while (dealerHandContainer.GetChildren().Count > 0)
                dealerHandContainer.RemoveChild(dealerHandContainer.GetChild(0));
            foreach (Card card in DealerHand.CardList)
                dealerHandContainer.AddChild(card);

            LblDealerTotal.Text = DealerHand.TotalValue > 0 ? DealerHand.Value : "";
        }

        #endregion Display

        #region Check Logic

        /// <summary>Checks whether the Dealer has an Ace face up.</summary>
        /// <returns>Returns true if the Dealer has an Ace face up.</returns>
        private bool CheckInsurance() => !MainHandOver && DealerHand.CardList[0].Value == 11 && SidePot == 0;

        #endregion Check Logic

        #region Button Management

        /// <summary>Checks which Buttons should be enabled.</summary>
        private void CheckButtons()
        {
            ToggleMainHandButtons(MainHand.TotalValue >= 21 || MainHandOver);
            BtnConvertAce.Disabled = !MainHand.HasAceEleven();
            ToggleSplitHandButtons(SplitHand.TotalValue >= 21 || SplitHandOver);
            BtnConvertAceSplit.Disabled = !SplitHand.HasAceEleven();
        }

        /// <summary>Disables all the buttons on the Page except for BtnDealHand.</summary>
        private void DisablePlayButtons()
        {
            ToggleNewGameExitButtons(false);
            ToggleMainHandButtons(true);
            ToggleSplitHandButtons(true);
            BtnConvertAce.Disabled = true;
            BtnConvertAceSplit.Disabled = true;
        }

        private void ToggleNewGameExitButtons(bool disabled)
        {
            BtnDealHand.Disabled = disabled;
            BtnReturn.Disabled = disabled;
            GameState.Info.ToggleButtons(disabled);
        }

        /// <summary>Toggles the Hit and Stay Buttons' Disabled property.</summary>
        /// <param name="disabled">Should the Buttons be disabled?</param>
        private void ToggleMainHandButtons(bool disabled)
        {
            BtnHit.Disabled = disabled;
            BtnStay.Disabled = disabled;
            BtnDoubleDown.Disabled = !MainHand.CanDoubleDown();
        }

        /// <summary>Toggles the Split Hand's Hit and Stay Buttons' Disabled property.</summary>
        /// <param name="disabled">Should the Buttons be disabled?</param>
        private void ToggleSplitHandButtons(bool disabled)
        {
            BtnHitSplit.Disabled = disabled;
            BtnStaySplit.Disabled = disabled;
            BtnDoubleDownSplit.Disabled = !SplitHand.CanDoubleDown();
        }

        #endregion Button Management

        #region Card Management

        /// <summary>Converts the first Ace valued at eleven in the Hand to be valued at one.</summary>
        /// <param name="handConvert">Hand to be converted.</param>
        private void ConvertAce(Hand handConvert)
        {
            handConvert.ConvertAce();
            CheckButtons();
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
            // make all hands empty.
            // reset handOver and DoubledDown
            // make the bet LineEdit uneditable.
            // Check insurance
            // disable exit buttons
            // if current spot in deck is at 20% of total cards, reshuffle
            // deal cards
            // check can split
            // display cards, enable play buttons

            MainHand = new Hand();
            SplitHand = new Hand();
            DealerHand = new Hand();

            SplitBet = 0;
            SidePot = 0;

            MainHandOver = false;
            SplitHandOver = true;

            TxtBet.Editable = false;
            TxtInsurance.Text = "";
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
            CheckWinConditions();
            ToggleInsurance(!CheckInsurance());
        }

        /// <summary>Toggles the Insurance controls.</summary>
        /// <param name="disabled">Should the controls be disabled?</param>
        private void ToggleInsurance(bool disabled)
        {
            TxtInsurance.Editable = !disabled;
            BtnInsurance.Disabled = disabled;
        }

        #endregion Card Management

        #region Game Results

        /// <summary>The game ends in a draw.</summary>
        private string DrawBlackjack()
        {
            EndHand();
            TotalDraws++;
            return " You reach a draw.";
        }

        /// <summary>Ends the Hand.</summary>
        private void EndHand()
        {
            TxtBet.Editable = true;
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
        private string LoseBlackjack(int betAmount)
        {
            GameState.CurrentHero.Gold -= betAmount;
            TotalLosses++;
            TotalBetLosses += betAmount;
            EndHand();
            return $" You lose {betAmount:N0}.";
        }

        /// <summary>The game ends with the Player winning.</summary>
        /// <param name="betAmount">Amount the Player bet</param>
        private string WinBlackjack(int betAmount)
        {
            GameState.CurrentHero.Gold += betAmount;
            TotalWins++;
            TotalBetWinnings += betAmount;
            EndHand();
            return $" You win {betAmount}!";
        }

        #endregion Game Results

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // assign controls, create the deck, shuffle the deck, display statistics, focus on TxtBet
            AssignControls();
            CreateDeck(6);
            CardList.Shuffle();
            DisplayStatistics();
            TxtBet.GrabFocus();
        }

        /// <summary>Determines whether the round is over.</summary>
        private void CheckWinConditions()
        {
            if (MainHandOver && SplitHandOver)
            {
                ToggleMainHandButtons(true);
                ToggleSplitHandButtons(true);
                ToggleInsurance(true);
                // check whether or not the dealer needs to take any action
                // I do not recognize the dealer winning with Five Card Charlie, as it's a house rule, anyway. Only the player can win with it.
                // when I try to merge or invert these if statements, it doesn't work right. VS must not have the right operator precendence.
                if (MainHand.HasBlackjack() && SplitHand.HasBlackjack())
                { }
                else if (MainHand.IsBust() && SplitHand.IsBust())
                { }
                else if (SplitHand.Count == 0 && (MainHand.HasBlackjack() || MainHand.IsBust() || MainHand.HasFiveCardCharlie()))
                { }
                else if ((MainHand.HasBlackjack() && SplitHand.IsBust()) || (MainHand.IsBust() && SplitHand.HasBlackjack()))
                { }
                else if (MainHand.HasFiveCardCharlie() && (SplitHand.IsBust() || SplitHand.HasBlackjack()))
                { }
                else
                    DealerAction();
                DisplayDealerHand();

                GD.Print($"Main Hand: {MainHand.TotalValue}\n" +
                    $"Split Hand: {SplitHand.TotalValue}\n" +
                    $"Dealer Hand: {DealerHand.TotalValue}");
                if (MainHand.IsBust())
                    AddTextToTextBox("Your main hand busts!" + LoseBlackjack(MainBet));
                else if (MainHand.HasBlackjack() && MainHand.Count == 2)
                {
                    if (DealerHand.TotalValue != 21)
                        AddTextToTextBox("Your main hand is a natural blackjack!" + WinBlackjack(Int32Helper.Parse(MainBet * 1.5)));
                    else
                    {
                        AddTextToTextBox("Your main hand and the dealer both have natural blackjacks." + DrawBlackjack());
                    }
                }
                else if (DealerHand.IsBust())
                    AddTextToTextBox("Dealer busts!" + WinBlackjack(MainBet));
                else if (MainHand.HasBlackjack() && (DealerHand.IsBust() || DealerHand.TotalValue < 21))
                    AddTextToTextBox("Your main hand is 21!" + WinBlackjack(MainBet));
                else if (MainHand.HasFiveCardCharlie())
                {
                    if (!DealerHand.HasBlackjack())
                    {
                        AddTextToTextBox("Your main hand is a Five Card Charlie!" + WinBlackjack(MainBet));
                    }
                    else
                        AddTextToTextBox("Your main hand is a Five Card Charlie, but the dealer had a natural blackjack." + DrawBlackjack());
                }
                else if (MainHand.TotalValue > DealerHand.TotalValue && !DealerHand.IsBust())
                    AddTextToTextBox("Your main hand's cards are worth more than the dealer's!" + WinBlackjack(MainBet));
                else if (MainHand.TotalValue == DealerHand.TotalValue)
                    AddTextToTextBox("Your main hand's cards are worth the same as the dealer's." + DrawBlackjack());
                else if (MainHand.TotalValue < DealerHand.TotalValue)
                    AddTextToTextBox("Your main hand's cards are worth less than the dealer's." + LoseBlackjack(MainBet));

                //check split hand
                if (SplitHand.Count != 0)
                {
                    if (SplitHand.IsBust())
                        AddTextToTextBox("Your split hand busts!" + LoseBlackjack(SplitBet));
                    else if (SplitHand.HasBlackjack() && SplitHand.Count == 2)
                    {
                        if (DealerHand.TotalValue != 21)
                            AddTextToTextBox("Your split hand is a natural blackjack!" + WinBlackjack(Int32Helper.Parse(SplitBet * 1.5)));
                        else
                        {
                            AddTextToTextBox("Your split hand and the dealer both have natural blackjacks." + DrawBlackjack());
                        }
                    }
                    else if (DealerHand.IsBust())
                        AddTextToTextBox("Dealer busts!" + WinBlackjack(SplitBet));
                    else if (SplitHand.HasBlackjack() && (DealerHand.IsBust() || DealerHand.TotalValue < 21))
                        AddTextToTextBox("Your split hand is 21!" + WinBlackjack(SplitBet));
                    else if (SplitHand.HasFiveCardCharlie())
                    {
                        if (!DealerHand.HasBlackjack())
                        {
                            AddTextToTextBox("Your split hand is a Five Card Charlie!" + WinBlackjack(SplitBet));
                        }
                        else
                            AddTextToTextBox("Your split hand is a Five Card Charlie, but the dealer had a natural blackjack." + DrawBlackjack());
                    }
                    else if (SplitHand.TotalValue > DealerHand.TotalValue && !DealerHand.IsBust())
                        AddTextToTextBox("Your split hand's cards are worth more than the dealer's!" + WinBlackjack(SplitBet));
                    else if (SplitHand.TotalValue == DealerHand.TotalValue)
                        AddTextToTextBox("Your split hand's cards are worth the same as the dealer's." + DrawBlackjack());
                    else if (SplitHand.TotalValue < DealerHand.TotalValue)
                        AddTextToTextBox("Your split hand's cards are worth less than the dealer's." + LoseBlackjack(SplitBet));
                }

                //check side pot
                if (SidePot > 0 && DealerHand.Count == 2 && DealerHand.HasBlackjack())
                    AddTextToTextBox("Your insurance paid off!" + WinBlackjack(SidePot));
                else if (SidePot > 0)
                    AddTextToTextBox("Your insurance didn't pay off." + LoseBlackjack(SidePot));
            }
            else if (!MainHandOver)
            {
                if (MainHand.HasBlackjack() || MainHand.IsBust() || (MainHand.Count == 5 && (MainHand.TotalValue < 21 || (MainHand.HasAceEleven() && MainHand.TotalValue <= 31))))
                {
                    MainHandOver = true;
                    CheckWinConditions();
                }
                else
                    CheckButtons();
            }
            else if (!SplitHandOver)
            {
                if (SplitHand.HasBlackjack() || SplitHand.IsBust() || (SplitHand.Count == 5 && (SplitHand.TotalValue < 21 || (SplitHand.HasAceEleven() && SplitHand.TotalValue <= 31))))
                {
                    SplitHandOver = true;
                    CheckWinConditions();
                }
                else
                    CheckButtons();
            }

            DisplayHands();
        }

        #region Button Click

        private void _on_BtnDealHand_pressed()
        {
            // if can afford bet, set the bet, deal the hand
            // else can't afford
            MainBet = Int32Helper.Parse(TxtBet.Text);
            if (MainBet > 0 && MainBet <= GameState.CurrentHero.Gold)
                DealHand();
            else
                AddTextToTextBox(MainBet > GameState.CurrentHero.Gold ? "You can't bet more gold than you have!" : "Please enter a valid bet.");
        }

        private void _on_BtnHelp_pressed()
        {
            PnlHelp.Visible = true;
            previousStateOfBtnReturn = BtnReturn.Disabled;
            BtnReturn.Disabled = true;
        }

        private void _on_BtnHideHelp_pressed()
        {
            PnlHelp.Visible = false;
            BtnReturn.Disabled = previousStateOfBtnReturn;
        }

        private void _on_BtnInsurance_pressed()
        {
            int sidePot = Int32Helper.Parse(TxtInsurance.Text);

            if (sidePot <= MainBet / 2 && GameState.CurrentHero.Gold >= MainBet + SplitBet + sidePot)
            {
                SidePot = sidePot;
                ToggleInsurance(true);
            }
            else
                AddTextToTextBox("Your insurance bet must be less than or equal to half your main bet.");
        }

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        #region Main Hand Buttons

        private void _on_BtnHit_pressed()
        {
            DealCard(MainHand);
            BtnSplit.Disabled = true;
            TxtInsurance.Editable = false;
            BtnInsurance.Disabled = true;
            CheckWinConditions();
        }

        private void _on_BtnStay_pressed()
        {
            MainHandOver = true;
            ToggleMainHandButtons(true);
            CheckWinConditions();
        }

        private void _on_BtnConvertAce_pressed() => ConvertAce(MainHand);

        private void _on_BtnSplit_pressed()
        {
            if (GameState.CurrentHero.Gold >= (MainBet * 2))
            {
                AddTextToTextBox("You split your hand.");
                SplitBet = MainBet;
                Card moveCard = new Card(MainHand.CardList[1]);
                MainHand.CardList.RemoveAt(1);
                mainHandContainer.GetChild(1).Free();
                SplitHand.AddCard(moveCard);
                BtnSplit.Disabled = true;
                splitHandContainer.Visible = true;
                SplitHandOver = false;
                DealCard(MainHand);
                DealCard(SplitHand);
                CheckWinConditions();
            }
            else
                AddTextToTextBox("You cannot afford to double your wager to split your hands.");
        }

        #endregion Main Hand Buttons

        private void _on_BtnDoubleDown_pressed()
        {
            if (GameState.CurrentHero.Gold >= (MainBet * 2) + SplitBet + SidePot)
            {
                AddTextToTextBox("You double down on your main hand.");
                MainBet *= 2;
                DealCard(MainHand);
                MainHandOver = true;
                CheckWinConditions();
            }
            else
                AddTextToTextBox("You cannot afford to double down this hand.");
        }

        #region Split Hand Buttons

        private void _on_BtnHitSplit_pressed()
        {
            DealCard(SplitHand);
            CheckWinConditions();
        }

        private void _on_BtnConvertAceSplit_pressed() => ConvertAce(SplitHand);

        private void _on_BtnDoubleDownSplit_pressed()
        {
            if (GameState.CurrentHero.Gold >= MainBet + (SplitBet * 2) + SidePot)
            {
                AddTextToTextBox("You double down on your split hand.");
                SplitBet *= 2;
                DealCard(SplitHand);
                SplitHandOver = true;
                ToggleSplitHandButtons(true);
                CheckWinConditions();
            }
            else
                AddTextToTextBox("You cannot afford to double down this hand.");
        }

        private void _on_BtnStaySplit_pressed()
        {
            SplitHandOver = true;
            ToggleSplitHandButtons(true);
            CheckWinConditions();
        }

        #endregion Split Hand Buttons

        #endregion Button Click

        #region Text Changed

        private void _on_TxtBet_text_changed(string new_text) => BtnDealHand.Disabled = TxtBet.Text.Length == 0;

        private void _on_TxtInsurance_text_changed(string new_text) => BtnInsurance.Disabled = TxtBet.Text.Length == 0;

        #endregion Text Changed
    }
}