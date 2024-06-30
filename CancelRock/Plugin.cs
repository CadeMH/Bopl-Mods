using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace CancelRock
{
	[BepInPlugin($"dev.cmax.{PluginInfo.PLUGIN_NAME}", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInProcess("BoplBattle.exe")]
	public class Plugin : BaseUnityPlugin
	{
		internal static Harmony harmony;
		internal static ManualLogSource logger;
		internal static ConfigFile config;

        private void Awake()
		{
			harmony = new(Info.Metadata.GUID);
			logger = Logger;
			config = Config;

			Logger.LogMessage($"guid: {Info.Metadata.GUID}, name: {Info.Metadata.Name}, version: {Info.Metadata.Version}");

            harmony.Patch(
                AccessTools.Method(typeof(BounceBall), nameof(BounceBall.LateUpdateSim)),
                prefix: new(typeof(Patches), nameof(Patches.BallUpdate_Prefix))
            );
        }
	}

	class Patches
	{
        internal static void BallUpdate_Prefix(BounceBall __instance)
        {
            __instance.IsCancellable = true;
        }
	}
}