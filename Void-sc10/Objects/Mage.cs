using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Events;
using VEngine.Factory;

namespace VEngine.Objects
{
    /// <summary>
    /// Mages get a mana boost for every magic circle owned by them
    /// </summary>
    public class Mage : ControllableGameObject
    {
        public Mage(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {

        }

        public override ICollection<ControlBase> GetControls()
        {
            Button b = new("Summon")
            {
                Position = (0, 0)
            };
            b.Click += (s, e) =>
            {
                // summon a magic circle
                CombatEvent combat = new CombatEventBuilder()
                    .SetEventType(CombatEventType.SUMMON)
                    .AddField("summon", "magic_circle")
                    .Build();

                if ((MP - 10) < 0) return;
                this.MP -= 10; // costs 10 mana

                GameManager.Instance.SendGameEvent(this, combat);
            };

            List<ControlBase> controls = new()
            {
                b
            };

            return controls;
        }
    }
}
