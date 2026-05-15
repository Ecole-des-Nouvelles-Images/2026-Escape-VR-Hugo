namespace _Branches.Hugo.Scripts.PuzzleFlower
{
    public class AlarmMechanismSocketHandler : SocketHandler
    {
        public override void OnSelectedEnter()
        {
            _conectedSocket.enabled = true;
        }
    }
}