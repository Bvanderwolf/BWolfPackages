using BWolf.Utilities.PlayerProgression.PlayerProps;
using BWolf.Utilities.PlayerProgression.Quests;
using UnityEngine;

namespace BWolf.Examples.PlayerProgression
{
    public class Bank : MonoBehaviour
    {
        [Header("Quests")]
        [SerializeField]
        private Quest quest = null;

        [Header("PlayerProperties")]
        [SerializeField]
        private FloatProperty moneyDeposited = null;

        [SerializeField]
        private IntegerProperty depositCount = null;

        [SerializeField]
        private BooleanProperty openedBankAccount = null;

        [Header("Settings")]
        [SerializeField]
        private float depositOnClick = 20.0f;

        public void OnOpenBankAccount()
        {
            if (!openedBankAccount.IsTrue)
            {
                //set has opened bank account property using the property manager
                openedBankAccount.UpdateValue(true);

                //update the achieve financial independance quest if it is active and not completed
                if (quest.IsUpdatable)
                {
                    //get specific type of task to update its value
                    quest.GetTask<DoOnceTask>("OpenBankAccount").SetDoneOnce();
                }
            }
        }

        public void DepositMoney()
        {
            if (openedBankAccount.IsTrue)
            {
                //set money deposited and deposit count properties
                moneyDeposited.AddValue(depositOnClick);
                depositCount.AddValue(1);

                //check if we can update our achieve financial independance quest
                if (quest.IsUpdatable)
                {
                    //get specific types of task to update its value accordingly
                    quest.GetTask<IncrementTask>("MakeSomeDeposits").Increment();
                    quest.GetTask<MinimalValueTask>("DepositSomeMoney").AddValue(depositOnClick);
                }
            }
        }
    }
}