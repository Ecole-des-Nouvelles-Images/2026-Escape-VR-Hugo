namespace Handlers
{
    public class AlarmMechanismSocketHandler : SocketHandler
    {
        public override void OnSelectedEnter()
        {
            _conectedSocket.enabled = true;
        }
    }
}