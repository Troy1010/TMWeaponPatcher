using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace TMWeaponPatcher;

public class Settings
{
    [MaintainOrder]

    [SettingName("Speed Multiplier")]
    [Tooltip("Multiplies each weapon's speed by this value.")]
    public float SpeedMultiplier = 0.75f;
}