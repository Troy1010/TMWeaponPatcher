using Mutagen.Bethesda;
using Mutagen.Bethesda.Oblivion;
using Mutagen.Bethesda.Plugins.Exceptions;
using Mutagen.Bethesda.Synthesis;

namespace TMWeaponPatcher;

public class Program
{
    private static Lazy<Settings> _settings = null!;
    private static Settings Settings => _settings.Value;

    public static async Task<int> Main(string[] args)
    {
        return await SynthesisPipeline.Instance
            .AddPatch<IOblivionMod, IOblivionModGetter>(RunPatch)
            .SetAutogeneratedSettings("Settings", "settings.json", out _settings)
            .SetTypicalOpen(GameRelease.Oblivion, "TMWeaponPatcher.esp")
            .Run(args);
    }

    private static void RunPatch(IPatcherState<IOblivionMod, IOblivionModGetter> state)
    {
        // Mainly copied and edited from: https://github.com/Synthesis-Collective/speedandreachfixes/blob/master/SpeedandReachFixes/Program.cs
        Console.WriteLine("\n\nInitialization successful, beginning patcher process...\n");
        var count = 0;
        foreach (var oldWeapon in state.LoadOrder.PriorityOrder.WinningOverrides<IWeaponGetter>())
        {
            try
            {
                if (oldWeapon.Data == null || oldWeapon.EditorID == null)
                    continue;

                var newWeapon = oldWeapon.DeepCopy();
                
                if (newWeapon.Data?.Speed == null)
                    continue;

                newWeapon.Data.Speed *= newWeapon.Data.Type switch
                {
                    Weapon.WeaponType.Staff => Settings.StaffSpeedMultiplier,
                    Weapon.WeaponType.Bow => Settings.BowSpeedMultiplier,
                    _ => Settings.MeleeSpeedMultiplier
                };

                state.PatchMod.Weapons.Set(newWeapon);
                Console.WriteLine($"Successfully modified weapon: {oldWeapon.EditorID}");
                ++count;
                Console.WriteLine($"\tOldSpeed:{oldWeapon.Data.Speed} NewSpeed:{newWeapon.Data.Speed} Type:{oldWeapon.Data.Type}\n");
            }
            catch (Exception ex)
            {
                throw RecordException.Enrich(ex, oldWeapon);
            }
        }

        Console.WriteLine($"\nFinished patching {count} records.\n");
    }
}