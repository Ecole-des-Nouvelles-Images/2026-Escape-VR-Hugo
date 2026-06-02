using System;

namespace Core
{
    public static class EventBus
    {
        // ===== GAME =====
        public static Action OnGameStart;
        public static Action OnGamePause;
        public static Action OnGameResume;
        public static Action OnGameEnd;
        
        // ===== CLOCK =====
        public static Action OnClockTimeChanged;
        
        // ===== PUZZLE CANDLE =====
        public static Action OnFirstKeyUnlocked;
        public static Action OnBriefcaseOpened;
        public static Action OnCuckooClockRepaired;
        public static Action OnCandleKeyUnlocked;
        
        // ===== PUZZLE LIGHT =====
        public static Action OnStaticStatuetteEnlightened;
        public static Action OnSecondStatuetteEnlightened;
        public static Action OnMechanismUnlocked;
        public static Action OnFirstElementActivated;
        public static Action OnSecondElementActivated;
        public static Action OnThirdElementActivated;
        public static Action OnLightKeyUnlocked;
        
        // ===== PUZZLE FLOWER =====
        public static Action OnFirstDrawerOpened;
        public static Action OnSecondDrawerOpened;
        public static Action OnThirdDrawerOpened;
        public static Action OnAlarmRepaired;
        public static Action OnFlowerKeyUnlocked;
        
        // ===== GENERAL PUZZLE =====
        public static Action OnCandleKeyInserted;
        public static Action OnLightKeyInserted;
        public static Action OnFlowerKeyInserted;
        public static Action OnLetterRecovered;
        
        // ===== NARRATION AUDIO =====
        public static Action OnNarrationEvent02;
        public static Action OnNarrationEvent03;
        public static Action OnNarrationEvent04;
    }
}