using UnityEngine;

namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class AlarmFaceSocketHandler : SocketHandler
    {
        [Header("===== PRECONDITIONS =====")]
        [SerializeField] private AlarmClockHandler _alarmClockHandler;
        
        public override void OnSelectedEnter()
        {
            if (_alarmClockHandler && _alarmClockHandler.GetAdvancement() > 0)
            {
                _conectedSocket.enabled = true;
            }
        }
    }
}