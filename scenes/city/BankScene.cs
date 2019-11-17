using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Extensions.DataTypeHelpers;

namespace Sulimn.Scenes.City
{
    public class BankScene : Control
    {
        private Button BtnReturn, BtnDeposit, BtnWithdraw, BtnTakeOutLoan, BtnRepayLoan;
        private Label LblGoldOnHand, LblGoldInBank, LblLoanAvailable, LblLoanOwed, LblError;
        private LineEdit TxtGold;
        private RichTextLabel TxtBank;
        private int Gold;

        #region Load

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.Pressed && @event.Scancode == (int)KeyList.Escape)
                _on_BtnReturn_pressed();
        }

        /// <summary>Assigns all controls.</summary>
        private void AssignControls()
        {
            LblError = (Label)GetNode("LblError");
            LblGoldOnHand = (Label)GetNode("Gold/LblGoldOnHand");
            LblGoldInBank = (Label)GetNode("Gold/LblGoldInBank");
            LblLoanAvailable = (Label)GetNode("Gold/LblLoanAvailable");
            LblLoanOwed = (Label)GetNode("Gold/LblLoanOwed");
            BtnReturn = (Button)GetNode("BtnReturn");
            BtnDeposit = (Button)GetNode("Buttons/BtnDeposit");
            BtnWithdraw = (Button)GetNode("Buttons/BtnWithdraw");
            BtnTakeOutLoan = (Button)GetNode("Buttons/BtnTakeOutLoan");
            BtnRepayLoan = (Button)GetNode("Buttons/BtnRepayLoan");
            TxtGold = (LineEdit)GetNode("TxtGold");
            TxtBank = (RichTextLabel)GetNode("TxtBank");
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
            TxtBank.Text =
                "You enter the Bank. A teller beckons to you and you approach him. You tell him your name, and he rummages through a few papers. He finds one, and pulls it out.\n\n" +
                $"You have {GameState.CurrentHero.Bank.GoldInBankToString} gold available to withdraw. You also have an open credit line of { GameState.CurrentHero.Bank.LoanAvailableToString} gold.";
            DisplayGold();
        }

        #endregion Load

        /// <summary>Displays current gold values.</summary>
        private void DisplayGold()
        {
            LblGoldOnHand.Text = GameState.CurrentHero.GoldToStringWithText;
            LblGoldInBank.Text = GameState.CurrentHero.Bank.GoldInBankToStringWithText;
            LblLoanAvailable.Text = GameState.CurrentHero.Bank.LoanAvailableToStringWithText;
            LblLoanOwed.Text = GameState.CurrentHero.Bank.LoanTakenToStringWithText;
        }

        #region Transaction Methods

        /// <summary>Deposit money into the bank.</summary>
        private void Deposit()
        {
            if (GameState.CurrentHero.Gold >= Gold)
            {
                GameState.CurrentHero.Bank.GoldInBank += Gold;
                GameState.CurrentHero.Gold -= Gold;
                AddTextToTextBox($"You deposit {Gold:N0} gold.");
                DisplayGold();
            }
            else
                LblError.Text = "You have insufficient gold on hand to deposit that much.";
        }

        /// <summary>Repay the loan.</summary>
        private void RepayLoan()
        {
            if (GameState.CurrentHero.Bank.LoanTaken >= Gold)
            {
                GameState.CurrentHero.Bank.LoanTaken -= Gold;
                GameState.CurrentHero.Bank.LoanAvailable += Gold;
                GameState.CurrentHero.Gold -= Gold;
                AddTextToTextBox($"You repay {Gold:N0} gold on your loan.");
                DisplayGold();
            }
            else
                LblError.Text = "You're attempting to pay back more gold than you owe.";
        }

        /// <summary>Take out a loan.</summary>
        private void TakeOutLoan()
        {
            if (GameState.CurrentHero.Bank.LoanAvailable >= Gold)
            {
                GameState.CurrentHero.Bank.LoanTaken += Gold + (Gold / 20);
                GameState.CurrentHero.Bank.LoanAvailable -= Gold + (Gold / 20);
                GameState.CurrentHero.Gold += Gold;
                AddTextToTextBox($"You take out a loan for {Gold:N0} gold.");
                DisplayGold();
            }
            else
                LblError.Text = "You do not have sufficient credit.";
        }

        /// <summary>Withdraw money from the bank account.</summary>
        private void Withdrawal()
        {
            if (GameState.CurrentHero.Bank.GoldInBank >= Gold)
            {
                GameState.CurrentHero.Bank.GoldInBank -= Gold;
                GameState.CurrentHero.Gold += Gold;
                AddTextToTextBox($"You withdraw {Gold:N0} gold from your account.");
                DisplayGold();
            }
            else
                LblError.Text = "You do not have sufficient gold in your account.";
        }

        #endregion Transaction Methods

        #region Control Manipulation

        /// <summary>Toggles the bank buttons.</summary>
        /// <param name="disabled">Should the buttons be disabled?</param>
        private void ToggleButtons(bool disabled)
        {
            BtnDeposit.Disabled = disabled;
            BtnWithdraw.Disabled = disabled;
            BtnTakeOutLoan.Disabled = disabled;
            BtnRepayLoan.Disabled = disabled;
        }

        /// <summary>Adds text to the TxtBank RichTextLabel.</summary>
        /// <param name="newText">Text to be added</param>
        private void AddTextToTextBox(string newText)
        {
            TxtBank.Text += TxtBank.Text.Length > 0 ? "\n\n" + newText : newText;
            TxtBank.ScrollFollowing = true;
        }

        #endregion Control Manipulation

        #region Button Click

        private void _on_BtnDeposit_pressed() => Deposit();

        private void _on_BtnTakeOutLoan_pressed() => TakeOutLoan();

        private void _on_BtnRepayLoan_pressed() => RepayLoan();

        private void _on_BtnWithdraw_pressed() => Withdrawal();

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        #endregion Button Click

        private void _on_TxtGold_text_changed(string new_text)
        {
            LblError.Text = "";
            Gold = Int32Helper.Parse(TxtGold.Text);
            ToggleButtons(Gold == 0);
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.

        //  public override void _Process(float delta)
        //  {
        //
        //  }
    }
}