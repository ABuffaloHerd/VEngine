using SadConsole.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Data;
using VEngine.Items;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    public class Ranger : ControllableGameObject
    {
        public Stat Ammo { get; set; } = 2;
        public Ranger(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            weapon = WeaponRegistry.Rifle.Clone() as Weapon;
        }

        public override ICollection<ControlBase> GetHudElements()
        {
            ICollection<ControlBase> list = base.GetHudElements();

            ProgressBar ammobar = new(20, 1, HorizontalAlignment.Left)
            {
                Progress = (float)Ammo.Current / (float)Ammo.Max,
                Position = (6, 10),
                DisplayText = $"{Ammo.Current} / {Ammo.Max}",
                BarColor = Color.Brown,
                DisplayTextColor = Color.Black
            };
            Label ammolabel = new("AMMO: ");
            ammolabel.Position = (0, 10);

            list.Add(ammobar);
            list.Add(ammolabel);

            return list;
        }

        public override void Attack(IEnumerable<GameObject> targets, Arena arena)
        {
            base.Attack(targets, arena);

            Ammo--;
        }
    }
}
