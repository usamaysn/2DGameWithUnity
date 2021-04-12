namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetTimeScale : AltBaseCommand
    {
        float timeScale;

        public AltUnitySetTimeScale(SocketSettings socketSettings, float timescale) : base(socketSettings)
        {
            this.timeScale = timescale;
        }
        public void Execute()
        {
            SendCommand("setTimeScale", Newtonsoft.Json.JsonConvert.SerializeObject(timeScale));
            var data = Recvall();
            if (data.Equals("Ok"))
                return;
            HandleErrors(data);

        }
    }
}