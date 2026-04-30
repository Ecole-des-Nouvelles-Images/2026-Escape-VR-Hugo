using System;

namespace Core
{
    public static class EventBus
    {
        // ===== GAME =====
        public static Action GameStart;
        public static Action GamePause;
        public static Action GameResume;
        public static Action GameEnd;
    }
}