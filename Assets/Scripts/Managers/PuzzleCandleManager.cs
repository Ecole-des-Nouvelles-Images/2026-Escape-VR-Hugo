using Core;
using UnityEngine;

namespace Managers
{
    public class PuzzleCandleManager : MonoBehaviour
    {
        [SerializeField] private bool _onFirstKeyUnlocked;
        [SerializeField] private bool _onBriefcaseOpened;
        [SerializeField] private bool _onCuckooClockRepaired;
        [SerializeField] private bool _onCandleKeyUnlocked;

        #region MyRegion

        private void OnEnable()
        {
            EventBus.OnFirstKeyUnlocked += () => {_onFirstKeyUnlocked = true;};
            EventBus.OnBriefcaseOpened += () => {_onBriefcaseOpened = true;};
            EventBus.OnCuckooClockRepaired += () => {_onCuckooClockRepaired = true;};
            EventBus.OnCandleKeyUnlocked += () => {_onCandleKeyUnlocked = true;};
        }

        private void OnDisable()
        {
            EventBus.OnFirstKeyUnlocked -= () => {_onFirstKeyUnlocked = true;};
            EventBus.OnBriefcaseOpened -= () => {_onBriefcaseOpened = true;};
            EventBus.OnCuckooClockRepaired -= () => {_onCuckooClockRepaired = true;};
            EventBus.OnCandleKeyUnlocked -= () => {_onCandleKeyUnlocked = true;};
        }

        #endregion
    }
}
