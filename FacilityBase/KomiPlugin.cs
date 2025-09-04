using CustomPlayerEffects;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using System;
using Utf8Json.Formatters;

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
            Cassie.MessageTranslated("xmas_bouncyballs", "<i><size=30><color=#B20BF4>B</color><color=#B00DF4>o</color><color=#AE0FF4>u</color><color=#AC11F4>n</color><color=#AA13F4>c</color><color=#A815F4>y</color> <color=#A419F4>b</color><color=#A21BF4>a</color><color=#A01DF4>l</color><color=#9E1FF4>l</color><color=#9C21F4>s</color><color=#9A23F4>,</color> <color=#9627F4>b</color><color=#9429F4>o</color><color=#922BF4>u</color><color=#902DF4>n</color><color=#8E2FF4>c</color><color=#8C31F4>y</color> <color=#8835F4>b</color><color=#8637F4>a</color><color=#8439F4>l</color><color=#823BF4>l</color><color=#803DF4>s</color><color=#7E3FF4>,</color> <color=#7A43F4>b</color><color=#7845F4>o</color><color=#7647F4>u</color><color=#7449F4>n</color><color=#724BF4>c</color><color=#704DF4>i</color><color=#6E4FF4>n</color><color=#6C51F4>g</color> <color=#6855F4>a</color><color=#6657F4>l</color><color=#6459F4>l</color> <color=#605DF4>a</color><color=#5E5FF4>w</color><color=#5C61F4>a</color><color=#5A63F4>y</color><color=#5865F4>,</color> <color=#5469F4>o</color><color=#526BF4>h</color> <color=#4E6FF4>w</color><color=#4C71F4>h</color><color=#4A73F4>a</color><color=#4875F4>t</color> <color=#4479F4>f</color><color=#427BF4>u</color><color=#407DF4>n</color> <color=#3C81F4>i</color><color=#3A83F4>t</color> <color=#3687F4>i</color><color=#3489F4>s</color> <color=#308DF4>t</color><color=#2E8FF4>o</color> <color=#2A93F4>d</color><color=#2895F4>i</color><color=#2697F4>e</color> <color=#229BF4>s</color><color=#209DF4>o</color> <color=#1CA1F4>h</color><color=#1AA3F4>o</color><color=#18A5F4>l</color><color=#16A7F4>d</color> <color=#12ABF4>y</color><color=#10ADF4>o</color><color=#0EAFF4>u</color><color=#0CB1F4>r</color> <color=#08B5F4>h</color><color=#06B7F4>e</color><color=#04B9F4>a</color><color=#02BBF4>d</color><color=#00BDF4>s</color> <color=#00C1F4>a</color><color=#00C3F4>n</color><color=#00C5F4>d</color> <color=#00C9F4>p</color><color=#00CBF4>r</color><color=#00CDF4>a</color><color=#00CFF4>y</color><color=#00D1F4>!</color></size></i>", true, false, true);
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


