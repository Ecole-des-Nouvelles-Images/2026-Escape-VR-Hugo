using Unity.Android.Gradle.Manifest;

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
    }
}