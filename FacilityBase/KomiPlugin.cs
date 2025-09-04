using CustomPlayerEffects;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using System;

namespace FacilityBase
{
    public class KomiPlugin : Plugin<Config>
    {
        public override string Name => "FacilityBase";
        public override string Author => "Komiynthoni";
        public override Version Version => new Version(0, 0, 2);

        public override void OnEnabled()
        {

            base.OnEnabled();
            Exiled.Events.Handlers.Player.Spawned += Spawned;
            Exiled.Events.Handlers.Player.FlippingCoin += FlippingCoin;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Log.Info("Test plugin enabled");
        }
        
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Spawned -= Spawned;
            Exiled.Events.Handlers.Player.FlippingCoin -= FlippingCoin;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            base.OnDisabled();
            Log.Info("test plugin disabled");
        }
        private void Spawned(SpawnedEventArgs ev)
        {
            ev.Player.AddItem(ItemType.Lantern);
        }

        private void FlippingCoin(FlippingCoinEventArgs ev)
        {
            Cassie.Clear();
            Map.Broadcast(10, "The funny coin has been flipped.",Broadcast.BroadcastFlags.Normal,true);
            Cassie.MessageTranslated(".g7", "REEEEEEE", true, false, true);
            foreach (Player player in Player.List)
            {
                player.ApplyRandomEffect(Exiled.API.Enums.EffectCategory.None, 15, true);
                player.ShowHint("You have received an effect from a funny coin."); 
            }
        }



        private void OnRoundStarted()
            {
                 Map.Broadcast(10, "New round has begun!");
            }
        }
    }


