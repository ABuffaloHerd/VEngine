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
using VEngine.Animations;

namespace VEngine.Items
{
    public static class SpellRegistry
    {
        public static Spell ArcaneBlast = new(
            "Arcane Blast",
            "I'm gonna play arcane blast with efficiency at level 11 so that brings the cost down to zero then in response to my declaration i am going to pay one and ignite the soul",
            30, // do the above to make your opponent think that you think you know the rules but react to the resolution of their spellshield arcane with another ignite the soul so they get one enlighten counter and take 11 damage to the face.
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
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", list)
                    .AddField("source", spell)
                    .AddField("weapon", new object())
                    .Build();

                return ev;
            });

        public static Spell Fireball = new(
            "Fireball",
            "Magic!",
            10,
            10,
            new Pattern()
                .Mark(2, -1)
                .Mark(3, -1)
                .Mark(4, -1)
                .Mark(2, 0)
                .Mark(3, 0)
                .Mark(4, 0)
                .Mark(2, 1)
                .Mark(3, 1)
                .Mark(4, 1),
            (spell, targets, wielder, arena) =>
            {
                int finalDamage = spell.Damage;

                foreach(GameObject obj in targets)
                {
                    obj.TakeDamage(finalDamage, DamageType.MAGIC);
                }

                AnimatedEffect explode = AnimationPresets.ExplosionEffect(3, TimeSpan.FromSeconds(0.2));

                explode.Position = wielder.Facing switch
                {
                    Data.Direction.UP => (0, -3),
                    Data.Direction.DOWN => (0, 3),
                    Data.Direction.LEFT => (-3, 0),
                    Data.Direction.RIGHT => (3, 0),
                    _ => throw new Exception("Switch fucked up.")
                }
                + wielder.Position;

                arena.PlayAnimatedEffect(explode);

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", spell)
                    .AddField("weapon", new object())
                    .Build();

                return ev;
            });
                
    }
}
