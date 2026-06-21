using System.Reflection;
using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class MokousBlessing : KeineModCard, DualBlessing.IChoosable
{
    public MokousBlessing() : base(-1, CardType.Power, CardRarity.Event, TargetType.None)
    {
        WithPower<DupRekindlePower>(1);
    }

    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    public async Task OnChosen()
    {
        // 1. Check if MokouMod is actively loaded
        if (ModManager.GetLoadedMods().Any(mod => string.Equals(mod.manifest?.id, "MokouMod")))
            try
            {
                // 2. Locate Mokou's assembly
                var mokouAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => string.Equals(a.GetName().Name, "MokouMod"));

                if (mokouAssembly != null)
                {
                    // 3. Find the native RekindlePower type
                    var rekindlePowerType = mokouAssembly.GetTypes().FirstOrDefault(t => t.Name.Contains("RekindlePower"));

                    if (rekindlePowerType != null)
                    {
                        // 4. target the single-creature generic Apply method precisely (6 parameters, 2nd is a single Creature)
                        var applyMethod = typeof(PowerCmd).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "Apply" && m.IsGenericMethod && m.GetGenericArguments().Length == 1)
                            .FirstOrDefault(m =>
                            {
                                var p = m.GetParameters();
                                return p.Length == 6 && p[1].ParameterType == typeof(Creature);
                            });

                        if (applyMethod != null)
                        {
                            // 5. Transform the generic method definition into PowerCmd.Apply<RekindlePower>
                            var genericApply = applyMethod.MakeGenericMethod(rekindlePowerType);

                            // 6. Invoke the exact 6-parameter layout mapped from decompilation
                            var invokeResult = genericApply.Invoke(null, [new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["DupRekindlePower"].BaseValue, Owner.Creature, this, false]);

                            if (invokeResult is Task task)
                            {
                                await task;
                                return;
                            }
                        }
                        else
                        {
                            Log.Info("[KeineMod] Reflection Error: Could not locate the 6-parameter single-target Apply<T> method.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info($"[KeineMod] Reflection Exception: {ex.Message}\n{ex.StackTrace}");
            }

        await PowerCmd.Apply<DupRekindlePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["DupRekindlePower"].BaseValue, Owner.Creature, this);
    }
}