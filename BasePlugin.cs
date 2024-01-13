using BepInEx;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace BBFieldTripAllFLoors.Plugin
{
	[BepInPlugin(ModInfo.GUID, ModInfo.Name, ModInfo.Version)]
	public class BasePlugin : BaseUnityPlugin
	{
		void Awake()
		{
			Harmony harmony = new(ModInfo.GUID);
			harmony.PatchAll();
		}

		public static bool KeepModEnabled = true; // Since the mod is quite invasive, any other mod can modify this field to disable it

	}


	internal static class ModInfo
	{
		internal const string GUID = "pixelguy.pixelmodding.baldiplus.bbfieldtripallfloors";
		internal const string Name = "BB+ Field Trip in All Floors";
		internal const string Version = "1.0.0";
	}

	[HarmonyPatch(typeof(NameManager), "Awake")]
	internal class GetTheFieldTripPrefab
	{
		private static void Postfix()
		{
			var levelObject = Resources.FindObjectsOfTypeAll<LevelObject>().FirstOrDefault(x => x.name == "Endless1"); // Endless have field trip prefabs, straight copy paste of F2 lol
			if (levelObject == null) return;

			ld = levelObject;
		}

		internal static LevelObject ld;
	}

	[HarmonyPatch(typeof(LevelGenerator), "StartGenerate")]
	internal class FieldTripsEverywhere
	{
		private static void Prefix(LevelGenerator __instance)
		{
			if (!BasePlugin.KeepModEnabled || GetTheFieldTripPrefab.ld == null) return; // Cancel if this is disabled

			__instance.ld.tripEntrancePre = GetTheFieldTripPrefab.ld.tripEntrancePre;
			__instance.ld.fieldTrip = true;
			__instance.ld.fieldTripItems = GetTheFieldTripPrefab.ld.fieldTripItems;
			__instance.ld.fieldTrips = GetTheFieldTripPrefab.ld.fieldTrips;
		}
	}

}
