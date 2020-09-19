using BWolf.Utilities.PlayerProgression;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PlayerProgression
{
    public class PropertyUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject propertyTextPrefab = null;

        private Dictionary<string, Text> quests = new Dictionary<string, Text>();

        private void Start()
        {
            foreach (IProgressInfo info in QuestManager.Instance.QuestInfo)
            {
                Text text = Instantiate(propertyTextPrefab, transform).GetComponent<Text>();
                text.text = $"{info.Name} : {info.Progress}";
                quests.Add(info.Name, text);
            }

            QuestManager.Instance.QuestCompleted += OnQuestCompleted;
        }

        private void OnQuestCompleted(IProgressInfo info)
        {
            quests[info.Name].text = $"{info.Name} : {info.Progress}";
        }
    }
}