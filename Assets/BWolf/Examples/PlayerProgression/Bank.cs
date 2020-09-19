using BWolf.Utilities.PlayerProgression;
using BWolf.Utilities.PlayerProgression.PlayerProps;
using UnityEngine;

namespace BWolf.Examples.PlayerProgression
{
    public class Bank : MonoBehaviour
    {
        [Header("Quests")]
        [SerializeField]
        private BooleanQuest openAccountQuest = null;

        [SerializeField]
        private IntegerQuest depositCountQuest = null;

        [SerializeField]
        private FloatQuest depositAmmountQuest = null;

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
                PropertyManager.Instance.SetProperty("HasOpenedBankAccount", true);
                openAccountQuest.UpdateValue(true);
                hasOpenedBankAccount = true;
            }
        }

        public void DepositMoney()
        {
            if (hasOpenedBankAccount)
            {
                moneyDeposited += depositOnClick;
                depositCount++;

                PropertyManager.Instance.SetProperty("MoneyDeposited", moneyDeposited);
                PropertyManager.Instance.SetProperty("DepositCount", depositCount);

                depositCountQuest.UpdateValue(depositCount);
                depositAmmountQuest.UpdateValue(moneyDeposited);
            }
        }
    }
}