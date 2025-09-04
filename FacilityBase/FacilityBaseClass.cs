using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using System.Text;

namespace FacilityBase
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public byte SpeedBoostPerUse { get; set; } = 5;
        public byte MaxSpeedBoost { get; set; } = 255;
        public float EffectDisplayInterval { get; set; } = 1f; 
    }

    public class FacilityBaseClass : Plugin<Config>
    {
       
        public override string Name => "FacilityBase";
        public override string Author => "Komiynthoni";
        public override Version Version => new Version(0, 1, 0);

        private static FacilityBaseClass Instance;
        private Dictionary<Player, CoroutineHandle> EffectDisplayCoroutines;
        private Dictionary<Player, string> LastEffectDisplay;

        public override void OnEnabled()
        {
            Instance = this;
            EffectDisplayCoroutines = new Dictionary<Player, CoroutineHandle>();
            LastEffectDisplay = new Dictionary<Player, string>();
            
            Exiled.Events.Handlers.Player.Spawned += Spawned;
            Exiled.Events.Handlers.Player.FlippingCoin += FlippingCoin;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Log.Info("FacilityBase plugin enabled");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Spawned -= Spawned;
            Exiled.Events.Handlers.Player.FlippingCoin -= FlippingCoin;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            foreach (var coroutine in EffectDisplayCoroutines.Values)
                Timing.KillCoroutines(coroutine);
            EffectDisplayCoroutines.Clear();
            LastEffectDisplay.Clear();
           
            
            EffectDisplayCoroutines = null;
            LastEffectDisplay = null;
            Instance = null;
            Log.Info("FacilityBase plugin disabled");
            base.OnDisabled();
        }

        private void Spawned(SpawnedEventArgs ev)
        {
            ev.Player.AddItem(ItemType.Lantern);
          
            if (EffectDisplayCoroutines.ContainsKey(ev.Player))
                Timing.KillCoroutines(EffectDisplayCoroutines[ev.Player]);
            EffectDisplayCoroutines[ev.Player] = Timing.RunCoroutine(UpdateEffectDisplay(ev.Player));
        }

        private void FlippingCoin(FlippingCoinEventArgs ev)
        {
            Cassie.Clear();
            Map.Broadcast(10, "The funny coin has been flipped.", Broadcast.BroadcastFlags.Normal, true);
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

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
           
            foreach (Player player in Player.List)
            {
                player.DisableEffect(EffectType.MovementBoost);
                if (EffectDisplayCoroutines.ContainsKey(player))
                {
                    Timing.KillCoroutines(EffectDisplayCoroutines[player]);
                    EffectDisplayCoroutines.Remove(player);
                    LastEffectDisplay.Remove(player);
                }
                if (Instance.Config.Debug)
                    Log.Debug($"Reset MovementBoost and stopped effect display for {player.Nickname}.");
            }
        }

        private void OnPlayerLeft(LeftEventArgs ev)
        {
           
            if (EffectDisplayCoroutines.ContainsKey(ev.Player))
            {
                Timing.KillCoroutines(EffectDisplayCoroutines[ev.Player]);
                EffectDisplayCoroutines.Remove(ev.Player);
                LastEffectDisplay.Remove(ev.Player);
                if (Instance.Config.Debug)
                    Log.Debug($"Stopped effect display for {ev.Player.Nickname}.");
            }
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Painkillers)
            {
                Player player = ev.Player;
                if (Instance.Config.Debug)


                   
                    ApplySpeedBoost(player);
            }
        }

        private void ApplySpeedBoost(Player player)
        {
           
            var effect = player.GetEffect(EffectType.MovementBoost);

            byte currentBoost = 0;
            if (effect != null && effect.IsEnabled)
                currentBoost = effect.Intensity;

           
            currentBoost = (byte)Math.Min(currentBoost + Instance.Config.SpeedBoostPerUse, Instance.Config.MaxSpeedBoost);

           
            if (effect != null && effect.IsEnabled)
                player.DisableEffect(EffectType.MovementBoost);

           
            player.EnableEffect(EffectType.MovementBoost, currentBoost, float.MaxValue);

            if (Instance.Config.Debug)
                Log.Debug($"Applied MovementBoost intensity {currentBoost} to {player.Nickname}");

            player.ShowHint($"Movement speed increased to {currentBoost}%", 3f);
        }


        private IEnumerator<float> UpdateEffectDisplay(Player player)
        {
            while (player.IsAlive && player.IsConnected)
            {
                StringBuilder hint = new StringBuilder("<size=20><b>Active Effects:</b>\n");
                bool hasEffects = false;

                foreach (StatusEffectBase effect in player.ActiveEffects)
                {
                    if (effect.IsEnabled)
                    {
                        hasEffects = true;
                        string effectName = effect.name;
                        string duration = effect.Duration > 1000000 ? "Infinite" : $"{Math.Round(effect.Duration, 0)}s";
                        byte intensity = effect.Intensity;
                        hint.Append($"{effectName} (Intensity: {intensity}, Duration: {duration})\n");
                    }
                }

                if (!hasEffects)
                    hint.Append("<color=#FFFFFF>None</color>");

                string hintText = hint.ToString();

              
                player.ShowHint(hintText, Instance.Config.EffectDisplayInterval + 0.2f);

                yield return Timing.WaitForSeconds(Instance.Config.EffectDisplayInterval);
            }
        }


    }
}
