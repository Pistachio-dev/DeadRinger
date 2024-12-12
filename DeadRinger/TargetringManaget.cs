using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;

namespace DeadRinger
{
    public class TargetringManager
    {
        private readonly ITargetManager targetManager;
        private readonly IChatOutput chatOutput;
        private IGameObject? lastTarget;

        public TargetringManager(ITargetManager targetManager, IChatOutput chatOutput)
        {
            this.targetManager = targetManager;
            this.chatOutput = chatOutput;
        }

        public void Attach(IFramework framework)
        {
            framework.Update += Tick;
        }

        private void Tick(IFramework framework)
        {
            if (targetManager.Target == null || targetManager.Target == lastTarget)
            {
                return;
            }

            lastTarget = targetManager.Target;
            if (lastTarget is IPlayerCharacter)
            {
                chatOutput.WriteCommand("/targetring off");
            }
            else
            {
                chatOutput.WriteCommand("/targetring on");
            }
        }
    }
}
