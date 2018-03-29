using Rocket.API.Collections;
using Rocket.API.Serialisation;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Core.Logging;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace TheCubicNoobik.JoinLeaveMessagesUI
{
    public class JoinLeaveMessagesUI : RocketPlugin<Config>
    {
        internal JoinLeaveMessagesUI Instance;

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"connected", "<size=15><color={1}>{0}</color><color=white> connected to the server</color></size>"},
                    {"disconnected", "<size=15><color={1}>{0}</color><color=white> disconnected from the server</color></size>"}
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            Logger.Log("Plugin created by TheCubicNoobik");
            if (Instance.Configuration.Instance.JoinMessageEnabled)
            {
                U.Events.OnPlayerConnected += EOnPlayerConnected;
                Logger.Log("Join message enabled!");
            }
            if (Instance.Configuration.Instance.LeaveMessageEnabled)
            {
                U.Events.OnPlayerDisconnected += EOnPlayerDisconnected;
                Logger.Log("Leave message enabled!");
            }
        }

        protected override void Unload()
        {
            if (Instance.Configuration.Instance.JoinMessageEnabled)
            {
                U.Events.OnPlayerConnected -= EOnPlayerConnected;
            }
            if (Instance.Configuration.Instance.LeaveMessageEnabled)
            {
                U.Events.OnPlayerDisconnected -= EOnPlayerDisconnected;
            }
        }

        private void EOnPlayerConnected(UnturnedPlayer player)
        {
            if (!HasPermission(player, "TCN.JoinLeaveMessageUI.Invulnerable"))
            {
                string hexCC = UnityEngine.ColorUtility.ToHtmlStringRGBA(player.Color);
                foreach (UnturnedPlayer currentplayer in Players())
                {
                    EffectManager.sendUIEffect(Instance.Configuration.Instance.JoinEffectId, short.Parse((player.CSteamID.m_SteamID / 9390480284090).ToString()), currentplayer.CSteamID, false, string.Format(Instance.Translate("connected"), player.CharacterName, "#" + hexCC));
                    Logger.Log(hexCC);
                }
            }
        }

        private void EOnPlayerDisconnected(UnturnedPlayer player)
        {
            if (!HasPermission(player, "TCN.JoinLeaveMessageUI.Invulnerable"))
            {
                string hexCC = UnityEngine.ColorUtility.ToHtmlStringRGBA(player.Color);
                foreach (UnturnedPlayer currentplayer in Players())
                {
                    EffectManager.sendUIEffect(Instance.Configuration.Instance.LeaveEffectId, short.Parse((player.CSteamID.m_SteamID / 9390480284091).ToString()), currentplayer.CSteamID, false, string.Format(Instance.Translate("disconnected"), player.CharacterName, "#" + hexCC));
                }
            }
        }

        private bool HasPermission(UnturnedPlayer player, string permissionName)
        {
            List<Permission> permissions = IRocketPlayerExtension.GetPermissions(player);

            foreach (Permission permission in permissions)
            {
                if(permission.Name == permissionName)
                {
                    return true;
                }
            }
            return false;
        }

        public List<UnturnedPlayer> Players()
        {
            List<UnturnedPlayer> list = new List<UnturnedPlayer>();
            foreach (SteamPlayer current in Provider.clients)
            {
                UnturnedPlayer item = UnturnedPlayer.FromSteamPlayer(current);
                list.Add(item);
            }
            return list;
        }
    }
}