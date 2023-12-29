using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;
using VEngine.Data;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Factory;

namespace VEngine.Items
{
    public static class WeaponRegistry
    {
        public static Weapon Hands = new(
            "Bare Hands",
            "Give em' the hands! - Oskar", // Todo: Choose between this and "I COULD KILL YOU IN FIVE SECONDS WITH ME BARE HANDS. I'M TRAINED I AM" - Michael Rosen
            1, 
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = weapon.Damage;
                foreach (GameObject obj in targets)
                {
                    obj.TakeDamage(finalDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static Weapon WoodenSword = new(
            "Wooden sword",
            "A wooden stick that barely meets the criteria of being a weapon",
            3,
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = weapon.Damage;
                foreach(GameObject obj in targets)
                {
                    obj.TakeDamage(finalDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static RangedWeapon Rifle = new(
            "Makeshift rifle",
            "A pistol with a long rifled barrel and telescope taped to it.",
            5,
            2,
            new Pattern().Mark(2, 0).Mark(3, 0).Mark(4, 0).Mark(5, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = weapon.Damage;
                var target = Data.Algorithms.Closest(targets, wielder.Position);
                var list = new List<GameObject>();

                if (target != null)
                {
                    Logger.Report(null, "Found a single target to attack!");
                    target.TakeDamage(finalDamage, DamageType.PHYSICAL);
                    list.Add(target);
                }
                else
                    Logger.Report(null, "found no targets");

                CombatEvent ev = new CombatEventBuilder()
                    .AddField("amount", finalDamage)
                    .AddField("targets", list)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });
    }
}
