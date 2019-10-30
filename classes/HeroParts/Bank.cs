using Newtonsoft.Json;

namespace Sulimn.Classes.HeroParts
{
    /// <summary>Represents an account at the Bank.</summary>
    public class Bank
    {
        #region Modifying Properties

        /// <summary>Gold the Hero has in the bank.</summary>
        [JsonProperty(Order = 1)]
        public int GoldInBank { get; set; }

        /// <summary>Gold the Hero has available on loan.</summary>
        [JsonProperty(Order = 2)]
        public int LoanAvailable { get; set; }

        /// <summary>Gold the Hero has taken out on loan.</summary>
        [JsonProperty(Order = 3)]
        public int LoanTaken { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Gold the Hero has in the bank, formatted.</summary>
        [JsonIgnore]
        public string GoldInBankToString => GoldInBank.ToString("N0");

        /// <summary>Gold the Hero has available on loan, formatted.</summary>
        [JsonIgnore]
        public string LoanAvailableToString => LoanAvailable.ToString("N0");

        /// <summary>Gold the Hero has taken out on loan, formatted.</summary>
        [JsonIgnore]
        public string LoanTakenToString => LoanTaken.ToString("N0");

        #endregion Helper Properties

        #region Constructors

        /// <summary>Initializes a default instance of Bank.</summary>
        public Bank()
        {
            GoldInBank = 0;
            LoanAvailable = 0;
            LoanTaken = 0;
        }

        /// <summary>Initializes an instance of Bank by assigning Properties.</summary>
        /// <param name="goldInBank">Gold in the bank</param>
        /// <param name="loanTaken">Loan already taken out</param>
        /// <param name="loanAvailable">Loan available to be taken out</param>
        public Bank(int goldInBank, int loanTaken, int loanAvailable)
        {
            GoldInBank = goldInBank;
            LoanTaken = loanTaken;
            LoanAvailable = loanAvailable;
        }

        /// <summary>Replaces this instance of Bank with another instance.</summary>
        /// <param name="other">Instance of Bank to replace this instance</param>
        public Bank(Bank other) : this(other.GoldInBank, other.LoanTaken, other.LoanAvailable)
        {
        }

        #endregion Constructors
    }
}