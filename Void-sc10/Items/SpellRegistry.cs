using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VEngine.Data;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Logging;
using VEngine.Objects;

namespace VEngine.Items
{
    public static class SpellRegistry
    {
        static Spell ArcaneBlast = new(
            "Arcane Blast",
            "I'm gonna play arcane blast with efficiency at level 11 so that brings the cost down to zero then in response to my declaration i am going to pay one and ignite the soul",
            30,
            20,
            new Pattern()
                .Mark(7, 0)
                .Mark(8, 0)
                .Mark(9, 0),
            (spell, targets, wielder, arena) =>
            {
                int finalDamage = spell.Damage;
                var target = Data.Algorithms.Closest(targets, wielder.Position);
                var list = new List<GameObject>();

                if (target != null)
                {
                    target.TakeDamage(finalDamage, DamageType.MAGIC);
                    list.Add(target);
                }
                else
                    Logger.Report(null, "found no targets");

                CombatEvent ev = new CombatEventBuilder()
                    .AddField("amount", finalDamage)
                    .AddField("targets", list)
                    .AddField("source", spell)
                    .AddField("weapon", new object())
                    .Build();

                return ev;
            });
    }
}
