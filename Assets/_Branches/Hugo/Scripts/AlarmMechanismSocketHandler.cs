namespace _Branches.Hugo.Scripts
{
    public class AlarmMechanismSocketHandler : SocketHandler
    {
        public override void OnSelectedEnter()
        {
            _conectedSocket.enabled = true;
        }
    }
}