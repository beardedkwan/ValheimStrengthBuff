using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrengthBuff
{
    public class PluginInfo
    {
        public const string Name = "Strength Buff";
        public const string Guid = "beardedkwan.StrengthBuff";
        public const string Version = "1.0.0";
    }

    public class StrengthBuffConfig
    {
        public static ConfigEntry<float> BaseCarryWeight { get; set; }
        public static ConfigEntry<float> BeltBuff { get; set; }
    }

    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    [BepInProcess("valheim.exe")]
    public class StrengthBuff : BaseUnityPlugin
    {
        void Awake()
        {
            // Initialize config
            StrengthBuffConfig.BaseCarryWeight = Config.Bind("General", "BaseCarryWeight", 300f, "Base carry weight.");
            StrengthBuffConfig.BeltBuff = Config.Bind("General", "BeltBuff", 150f, "Additional carry weight from Megingjord.");

            Harmony harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }

        // base
        [HarmonyPatch(typeof(Player), "Awake")]
        public static class BaseCarry_Patch
        {
            private static void Postfix(ref Player __instance)
            {
                __instance.m_maxCarryWeight = StrengthBuffConfig.BaseCarryWeight.Value;
            }
        }

        // megingjord
        [HarmonyPatch(typeof(SE_Stats), "Setup")]
        public static class Megingjord_Patch
        {
            private static void Postfix(ref SE_Stats __instance)
            {
                if (__instance.m_addMaxCarryWeight > 0)
                {
                    __instance.m_addMaxCarryWeight = (__instance.m_addMaxCarryWeight - 150) + StrengthBuffConfig.BeltBuff.Value;
                }
            }
        }
    }
}
