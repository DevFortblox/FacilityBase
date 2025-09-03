using Exiled.API.Features;
using Exiled.Events;
using System;
using System.Numerics;

namespace FacilityBase
{
    public class KomiPlugin : Plugin<Config>
    {
        public override string Name => "MyFirstPlugin";
        public override string Author => "YourName";
        public override Version Version => new Version(1, 0, 0);

        public override void OnEnabled()
        {
            base.OnEnabled();
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Log.Info("Test plugin enabled");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.OnDisabled();
            Log.Info("test plugin disabled");
        }
       
        private void OnRoundStarted()
        {
            Round.IsLocked = true;
            Log.Warn("The round has been locked, and the script is working!");
            Map.Broadcast(10, "Test plugin is WORKING");
            foreach (Player player in Player.List)
            {
                
                player.Role.Set(PlayerRoles.RoleTypeId.Scientist, Exiled.API.Enums.SpawnReason.ForceClass);
                player.AddItem(ItemType.SCP1507Tape);
                player.AddAmmo(Exiled.API.Enums.AmmoType.Nato9, 300);
                player.AddItem(ItemType.Jailbird);
                Log.Warn("Set role to class D!");
            }
        }
    }
}