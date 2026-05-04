using Core;
using UnityEngine;

namespace _Branches.Hugo.Scripts
{
    public class PuzzleFlowerManager : MonoBehaviour
    {
        [Header("===== DEBUGS =====")]
        [SerializeField] private bool _isFirstDrawerOpened;
        [SerializeField] private bool _isSecondDrawerOpened;
        [SerializeField] private bool _isThirdDrawerOpened;
        [SerializeField] private bool _isAlarmRepaired;
        [SerializeField] private bool _isFlowerKeyUnlocked;
        
        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnFirstDrawerOpened += OnFirstDrawerOpened;
            EventBus.OnSecondDrawerOpened += OnSecondDrawerOpened;
            EventBus.OnThirdDrawerOpened += OnThirdDrawerOpened;
            EventBus.OnAlarmRepaired += OnAlarmRepaired;
            EventBus.OnFlowerKeyUnlocked += OnFlowerKeyUnlocked;
        }
        
        private void OnDisable()
        {
            EventBus.OnFirstDrawerOpened -= OnFirstDrawerOpened;
            EventBus.OnSecondDrawerOpened -= OnSecondDrawerOpened;
            EventBus.OnThirdDrawerOpened -= OnThirdDrawerOpened;
            EventBus.OnAlarmRepaired -= OnAlarmRepaired;
            EventBus.OnFlowerKeyUnlocked -= OnFlowerKeyUnlocked;
        }

        private void OnFirstDrawerOpened()
        {
            _isFirstDrawerOpened = true;
        }

        private void OnSecondDrawerOpened()
        {
            _isSecondDrawerOpened = true;
        }

        private void OnThirdDrawerOpened()
        {
            _isThirdDrawerOpened = true;
        }

        private void OnAlarmRepaired()
        {
            _isAlarmRepaired = true;
        }

        private void OnFlowerKeyUnlocked()
        {
            _isFlowerKeyUnlocked = true;
        }

        #endregion
    }
}
