using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityScrollMouseCommand : AltUnityCommand
    {
        float scrollValue;
        float duration;

        public AltUnityScrollMouseCommand(params string[] parameters) : base(parameters, 4)
        {
            this.scrollValue = JsonConvert.DeserializeObject<float>(parameters[2]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            LogMessage("scrollMouse with: " + scrollValue);
            Input.Scroll(scrollValue, duration);
            return "Ok";
#else
            return null;
#endif
        }
    }
}
