﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BoplTranslator
{
	internal static class LanguagePatch
	{
		private static readonly Dictionary<string, string> _translationLookUp = new()
		{
			{ "menu_language", "en" },
			{ "menu_play", "play" },
			{ "play_start", "start!" },
			{ "menu_online", "online" },
			{ "menu_settings", "settings" },
			{ "menu_exit", "exit" },
			{ "settings_sfx_vol", "sfx\nvol" },
			{ "settings_music_vol", "music\nvol" },
			{ "settings_abilities", "abilities" },
			{ "settings_screen_shake", "screen shake" },
			{ "settings_rumble", "rumble" },
			{ "settings_resolution", "resolution" },
			{ "settings_save", "save" },
			{ "general_on", "on" },
			{ "general_off", "off" },
			{ "general_high", "high" },
			{ "screen_fullscreen", "fullscreen" },
			{ "screen_windowed", "windowed" },
			{ "screen_borderless", "borderless" },
			{ "settings_screen", "screen" },
			{ "play_click", "click to join!" },
			{ "play_ready", "ready!" },
			{ "play_color", "color" },
			{ "play_team", "team" },
			{ "rebind_keys", "rebind keys" },
			{ "rebind_jump", "click jump" },
			{ "rebind_ability_left", "click ability_left" },
			{ "rebind_ability_right", "click ability_right" },
			{ "rebind_ability_top", "click ability_top" },
			{ "rebind_move_left", "click move_left" },
			{ "rebind_move_down", "click move_down" },
			{ "rebind_move_right", "click move_right" },
			{ "rebind_move_up", "click move_up" },
			{ "settings_vsync", "vsync" },
			{ "hide_nothing", "nothing" },
			{ "settings_hide", "hide" },
			{ "hide_names", "names" },
			{ "hide_names_avatars", "names and avatars" },
			{ "undefined_mouse_only", "mouse only" },
			{ "play_local_game", "local game" },
			{ "undefined_click_start", "click to start!" },
			{ "end_next_level", "next level" },
			{ "end_ability_select", "ability select" },
			{ "end_winner", "winner!!" },
			{ "end_winners", "winners!!" },
			{ "end_draw", "draw!" },
			{ "undefined_whishlist", "wishlist bopl battle!" },
			{ "play_choosing", "choosing..." },
			{ "pause_leave", "leave game?" },
			{ "menu_invite", "invite friend" },
			{ "undefined_practice", "practice" },
			{ "tutorial_hold_dow", "hold down" },
			{ "tutorial_aim", "to aim" },
			{ "tutorial_throw_greneade", "to throw grenade" },
			{ "tutorial_dash", "to dash" },
			{ "tutorial_click", "click" },
			{ "menu_credits", "credits" },
			{ "credits_back", "back" },
			{ "menu_tutorial", "tutorial" },
			{ "play_empty_lobby", "your lobby is empty" },
			{ "play_invite", "invite a friend to play online" },
			{ "play_not_abailable_demo", "not available in demo" },
			{ "play_find_players", "find players" },
			{ "play_stop_player_search", "stop search" },
			{ "item_bow", "bow" },
			{ "item_tesla_coil", "tesla coil" },
			{ "item_engine", "engine" },
			{ "item_smoke", "smoke" },
			{ "item_invisibility", "invisibility" },
			{ "item_platform", "platform" },
			{ "item_meteor", "meteor" },
			{ "item_random", "random" },
			{ "item_missile", "missile" },
			{ "item_black_hole", "black hole" },
			{ "item_rock", "rock" },
			{ "item_push", "push" },
			{ "item_dash", "dash" },
			{ "item_grenade", "grenade" },
			{ "item_roll", "roll" },
			{ "item_time_stop", "time stop" },
			{ "item_blink_gun", "blink gun" },
			{ "item_gust", "gust" },
			{ "item_mine", "mine" },
			{ "item_revival", "revival" },
			{ "item_spike", "spike" },
			{ "item_shrink_ray", "shrink ray" },
			{ "item_growth_ray", "growth ray" },
			{ "item_chain", "chain" },
			{ "item_time_lock", "time lock" },
			{ "item_throw", "throw" },
			{ "item_teleport", "teleport" },
			{ "item_grappling_hook", "grappling hook" },
			{ "item_drill", "drill" },
			{ "item_beam", "beam" },
		};

		public static readonly List<CustomLanguage> languages = [];
		public static int OGLanguagesCount { get; private set; }

		private static readonly MethodInfo getTextMethod = typeof(LocalizationTable).GetMethod("getText", AccessTools.all);

		public static void Init()
		{
			Plugin.harmony.Patch(
				AccessTools.Method(typeof(LocalizationTable), nameof(LocalizationTable.GetText)),
				prefix: new(typeof(LanguagePatch), nameof(GetTextPatch))
			);

			Plugin.harmony.Patch(
				AccessTools.Method(typeof(LocalizationTable), nameof(LocalizationTable.GetFont)),
				prefix: new(typeof(LanguagePatch), nameof(GetFont_Prefix))
			);

			OGLanguagesCount = Utils.MaxOfEnum<Language>();

			string[] translationKeys = _translationLookUp.Keys.ToArray();

			// read languages
			foreach (FileInfo file in Plugin.translationsDir.EnumerateFiles())
			{
				string[] words = new string[translationKeys.Length];
				CustomLanguage language = new(words);
				languages.Add(language);

				foreach (string line in File.ReadLines(file.FullName))
				{
					string[] splitted = line.Split(['='], 2);
					if (splitted.Length == 1) continue;

					string key = splitted[0].Trim();
					string value = splitted[1].Trim().Replace("\\n", "\n");
					
					int index = Array.FindIndex(translationKeys, e => e.Equals(key));
					if (index == -1) continue;

					words[index] = value;
				}

				for (int i = 0; i < words.Length; i++)
				{
					string word = words[i];
					if (word != null) continue;

					string key = translationKeys[i];
					words[i] = _translationLookUp.GetValueSafe(key);

					if (!key.StartsWith("undefined"))
						Plugin.logger.LogWarning($"No translation for \"{translationKeys[i]}\" in \"{file.Name}\"");
				}
			}
		}

		internal static bool GetTextPatch(LocalizationTable __instance, ref string __result, string enText, Language lang)
		{
			if (!IsCustomLanguage(lang)) return true;

			// run orignal getText with custom langauges
			__result = getTextMethod.Invoke(__instance, [enText, GetCustomLanguage(lang).translations]) as string;
			
			return false;
		}

		internal static void GetFont_Prefix(ref Language lang, ref bool useFontWithStroke)
		{
			if (!IsCustomLanguage(lang)) return;

			CustomLanguage customLanguage = GetCustomLanguage(lang);

			switch (customLanguage.font)
			{
				case BopLTranslator.Font.English: lang = Language.EN; break;
				case BopLTranslator.Font.Japan: lang = Language.JP; break;
				case BopLTranslator.Font.Korean: lang = Language.KO; break;
				case BopLTranslator.Font.Russian: lang = Language.RU; break;
				case BopLTranslator.Font.Chinese: lang = Language.ZHCN; break;
				case BopLTranslator.Font.Poland: lang = Language.PL; break;
			}

			useFontWithStroke = customLanguage.stroke;
		}

		private static bool IsCustomLanguage(Language lang) => (int)lang > OGLanguagesCount;

		private static CustomLanguage GetCustomLanguage(Language lang) => languages[(int)lang - OGLanguagesCount - 1];
	}

	class CustomLanguage
	{
		internal string[] translations;
		internal BopLTranslator.Font font;
		internal bool stroke;

		internal CustomLanguage(string[] translations, BopLTranslator.Font font = BopLTranslator.Font.English, bool stroke = false)
		{
			this.translations = translations;
			this.font = font;
			this.stroke = stroke;
		}
	}
}