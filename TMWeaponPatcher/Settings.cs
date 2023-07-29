using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace TMWeaponPatcher;

public class Settings
{
    [MaintainOrder]

    [SettingName("Melee Speed Multiplier")]
    [Tooltip("Multiplies each melee weapon's speed by this value.")]
    public float MeleeSpeedMultiplier = 0.75f;

    [SettingName("Bow Speed Multiplier")]
    [Tooltip("Multiplies each bow weapon's speed by this value.")]
    public float BowSpeedMultiplier = 1.0f;

    [SettingName("Staff Speed Multiplier")]
    [Tooltip("Multiplies each staff weapon's speed by this value.")]
    public float StaffSpeedMultiplier = 1.0f;
}