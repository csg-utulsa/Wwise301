using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using QuestSystem;

public class QuestUpdateHandler : MonoBehaviour
{

    [System.Serializable]
    public class questInfo {
        [Header("When to Invoke OnTrue")]
        public int questStart = 0;
        public string objectiveKey = "";
        [Header("When to Invoke OnFalse")]
        public int questEnd = 0;
        public bool activated = false;
    } public questInfo QuestRange;

    public QuestObjective activatedQuestObjective;

    private void Start()
    {
        QuestManager.PushQuestUpdate += CheckRangeAndInvoke;
    }

    void CheckRangeAndInvoke(int qNum, List<QuestObjective> OL, QuestObjective O) {
        // if not activated && inside range specified
        if (O != null && !QuestRange.activated && qNum >= QuestRange.questStart && qNum <= QuestRange.questEnd)
        {
            // and if same key
            if (QuestRange.objectiveKey == O.TitleKey ) {
                // invoke OnTrue events
                PushOnTrue(qNum, O);
            }
        } // if not already activated && side specified range
        else  {
            // if O is null (meaning it started a new quest) && there's already an objective active
            if (O == null && qNum == QuestRange.questStart && QuestRange.activated) {
                PushOnFalse();
            } else if (O != null && activatedQuestObjective != null)
            {
                if (qNum < QuestRange.questStart || qNum > QuestRange.questEnd)
                {
                    PushOnFalse();
                } else if (activatedQuestObjective.state == QuestObjectiveState.InProgress) {
                    PushOnFalse();
                }
                
            } // and if saved objective is NOT empty && but it's in progress
            else if (activatedQuestObjective != null && activatedQuestObjective.state == QuestObjectiveState.InProgress)
            {
                // invoke OnFalse Events
                PushOnFalse();
            }
        }       
    }

    void PushOnTrue(int qNum, QuestObjective O) {
        QuestRange.activated = true;
        OnTrue.Invoke();
        activatedQuestObjective = O;
    }

    void PushOnFalse() {
        QuestRange.activated = false;
        OnFalse.Invoke();
        activatedQuestObjective = null;
    }

    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

}
