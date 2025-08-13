using BepInEx;
using HarmonyLib;

namespace FrisbeeBadgeFix;

[BepInPlugin("decalfree.frisbeebadgefix", "FrisbeeBadgeFix", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public Plugin() =>
        Harmony.CreateAndPatchAll(GetType().Assembly, "decalfree.frisbeebadgefix");
}

// The check "obj == this" always fails, using "obj == __instance.item" fixes this check.
[HarmonyPatch(typeof(Frisbee))]
public static class FrisbeePatches
{
    [HarmonyPatch("OnItemThrown"), HarmonyPrefix]
    public static bool FixFrisbeeCheck(Frisbee __instance, Item obj)
    {
        if (obj == __instance.item)
        {
            AccessTools.Field(typeof(Frisbee), "startedThrowPosition").SetValue(__instance, obj.Center());
            AccessTools.Field(typeof(Frisbee), "throwValidForAchievement").SetValue(__instance, true);
            return false;
        }

        return true;
    }
}