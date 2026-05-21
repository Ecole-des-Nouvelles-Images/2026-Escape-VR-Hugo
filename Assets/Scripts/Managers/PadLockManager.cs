using Core.Singletons;

namespace Managers
{
    public class PadLockManager : MonoBehaviourSingleton<PadLockManager>
    {
        public CodePadLockHandler CurrentPadLock;

        public void SetCurrentPadLock(CodePadLockHandler newCurrentPadLock, bool bigVisualActivated)
        {
            if (CurrentPadLock == null)
            {
                CurrentPadLock = newCurrentPadLock;
                CurrentPadLock.SpawnBigPadLock();
            }
            else if (newCurrentPadLock != CurrentPadLock)
            {
                CurrentPadLock.DespawnBigPadLock();
                CurrentPadLock = newCurrentPadLock;
                CurrentPadLock.SpawnBigPadLock();
            }
            else if (newCurrentPadLock == CurrentPadLock)
            {
                if (bigVisualActivated)
                {
                    CurrentPadLock.DespawnBigPadLock();
                }
                else
                {
                    CurrentPadLock.SpawnBigPadLock();
                }
            }
        }
    }
}