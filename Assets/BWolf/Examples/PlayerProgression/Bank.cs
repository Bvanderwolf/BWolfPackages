using BWolf.Utilities.PlayerProgression.PlayerProps;
using BWolf.Utilities.PlayerProgression.Quests;
using UnityEngine;

namespace BWolf.Examples.PlayerProgression
{
    public class Bank : QuestGiver
    {
        [Header("Settings")]
        [SerializeField]
        private float moneyDeposited = 0.0f;

        [SerializeField]
        private float depositOnClick = 20.0f;

        [SerializeField]
        private int depositCount = 0;

        private bool hasOpenedBankAccount;

        public void OnOpenBankAccount()
        {
            if (!hasOpenedBankAccount)
            {
                //set has opened bank account property using the property manager
                PropertyManager.Instance.SetProperty("HasOpenedBankAccount", true);

                //update the achieve financial independance quest if it is active and not completed
                Quest quest = GetQuest("AchieveFinancialIndependance");
                if (quest.IsActive && !quest.IsCompleted)
                {
                    //get specific type of task to update its value
                    quest.GetTask<DoOnceTask>("OpenBankAccount").SetDoneOnce();
                }

                hasOpenedBankAccount = true;
            }
        }

        public void DepositMoney()
        {
            if (hasOpenedBankAccount)
            {
                moneyDeposited += depositOnClick;
                depositCount++;

                //set money deposited and deposit count properties
                PropertyManager.Instance.SetProperty("MoneyDeposited", moneyDeposited);
                PropertyManager.Instance.SetProperty("DepositCount", depositCount);

                //check if we can update our achieve financial independance quest
                Quest quest = GetQuest("AchieveFinancialIndependance");
                if (quest.IsActive && !quest.IsCompleted)
                {
                    //get specific types of task to update its value accordingly
                    quest.GetTask<IncrementTask>("MakeSomeDeposits").Increment();
                    quest.GetTask<MinimalValueTask>("DepositSomeMoney").UpdateCurrentValue(moneyDeposited);
                }
            }
        }
    }
}