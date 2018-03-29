using Rocket.API;

namespace TheCubicNoobik.JoinLeaveMessagesUI
{
    public class Config : IRocketPluginConfiguration
    {
        public bool JoinMessageEnabled;
        public bool LeaveMessageEnabled;
        public ushort JoinEffectId;
        public ushort LeaveEffectId;

        public void LoadDefaults()
        {
            JoinMessageEnabled = true;
            LeaveMessageEnabled = true;
            JoinEffectId = 44031;
            LeaveEffectId = 44031;
        }
    }
}
