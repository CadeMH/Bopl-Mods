﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BoplModSyncer.Utils
{
	internal class PanelMaker
	{
		private static GameObject CopyGeneric(GameObject genericPanel)
		{
			GameObject panel = Object.Instantiate(genericPanel);
			panel.hideFlags = HideFlags.HideInHierarchy;
			Object.DontDestroyOnLoad(panel);
			return panel;
		}
		
		public static TextMeshProUGUI GetTitleText(GameObject panel) =>
			panel.transform.Find("Title").GetComponent<TextMeshProUGUI>();

		public static TextMeshProUGUI GetInfoText(GameObject panel) =>
			panel.transform.Find("Info").GetComponent<TextMeshProUGUI>();

		public static GameObject GetTextArea(GameObject panel) =>
			panel.transform.Find("Text").gameObject;

		public static GameObject GetProgressBar(GameObject panel) =>
			panel.transform.Find("ProgressBar").gameObject;

		public static GameObject GetModList(GameObject panel) =>
			panel.transform.Find("NeededModList").gameObject;

		public static Button GetOkButtonComp(GameObject panel) =>
			panel.transform.Find("OK Button").GetComponent<Button>();

		public static Button GetCancelButtonComp(GameObject panel) =>
			panel.transform.Find("Cancel Button").GetComponent<Button>();

		public static void SetupCloseButton(GameObject panel)
		{
			void close() => Object.Destroy(panel);
			GetOkButtonComp(panel).onClick.AddListener(close);
			Button.ButtonClickedEvent cancelClicked = GetCancelButtonComp(panel).onClick;
			cancelClicked.AddListener(close);
			cancelClicked.AddListener(() => GameUtils.CancelSyncing());
		}

		private static TMP_FontAsset GetDefaultFont() =>
			LocalizedText.localizationTable.GetFont(Language.EN, false);

		public static void MakeGenericPanel(ref GameObject genericPanel)
		{
			// add text components into panel
			TextMeshProUGUI title = genericPanel.transform.Find("Title").gameObject.AddComponent<TextMeshProUGUI>();
			TextMeshProUGUI info = genericPanel.transform.Find("Info").gameObject.AddComponent<TextMeshProUGUI>();
			TextMeshProUGUI ok = genericPanel.transform.Find("OK Button/Text (TMP)").gameObject.AddComponent<TextMeshProUGUI>();
			TextMeshProUGUI cancel = genericPanel.transform.Find("Cancel Button/Text (TMP)").gameObject.AddComponent<TextMeshProUGUI>();
			TextMeshProUGUI text = genericPanel.transform.Find("Text").gameObject.AddComponent<TextMeshProUGUI>();

			// text mesh pro settings
			title.fontSize = 56;
			title.color = Color.black;
			title.font = GetDefaultFont();
			title.alignment = TextAlignmentOptions.Left;
			title.fontStyle = FontStyles.Bold;

			cancel.fontSize = ok.fontSize = 50;
			cancel.color = ok.color = Color.black;
			cancel.font = ok.font = GetDefaultFont();
			cancel.alignment = ok.alignment = TextAlignmentOptions.Center;
			cancel.fontStyle = ok.fontStyle = FontStyles.Bold;
			ok.text = "OK";
			cancel.text = "Cancel";

			info.fontSize = 50;
			info.color = Color.black;
			info.font = GetDefaultFont();
			info.alignment = TextAlignmentOptions.Left;
			info.fontStyle = FontStyles.Normal;

			text.fontSize = 50;
			text.color = Color.black;
			text.font = GetDefaultFont();
			text.alignment = TextAlignmentOptions.Left;
			text.fontStyle = FontStyles.Normal;
		}

		public static GameObject MakeMissingModsPanel(GameObject genericPanel)
		{
			GameObject panel = CopyGeneric(genericPanel);

			Object.Destroy(GetProgressBar(panel));
			Object.Destroy(GetTextArea(panel));

			TextMeshProUGUI rowLabel = panel.transform.Find("NeededModList/Viewport/Content/tmprow/Background/Label").gameObject.AddComponent<TextMeshProUGUI>();
			rowLabel.fontSize = 50;
			rowLabel.color = new Color32(50, 50, 50, 255);
			rowLabel.font = GetDefaultFont();
			rowLabel.alignment = TextAlignmentOptions.Left;
			rowLabel.raycastTarget = false;
	
			GetTitleText(panel).text = "Missing mods";
			GetInfoText(panel).text = "Install | Mod data";

			return panel;
		}

		public static GameObject MakeNoSyncerPanel(GameObject genericPanel)
		{
			GameObject panel = CopyGeneric(genericPanel);

			Object.Destroy(GetProgressBar(panel));
			Object.Destroy(GetTextArea(panel));
			Object.Destroy(GetModList(panel));

			GetTitleText(panel).text = "Warning!";
			GetInfoText(panel).text = "Host's missing BoplModSyncer";

			return panel;
		}

		public static GameObject MakeInstallingPanel(GameObject genericPanel)
		{
			GameObject panel = CopyGeneric(genericPanel);

			Object.Destroy(GetModList(panel));

			TextMeshProUGUI percentage = panel.transform.Find("ProgressBar/Percentage").gameObject.AddComponent<TextMeshProUGUI>();
			percentage.fontSize = 50;
			percentage.color = Color.black;
			percentage.font = GetDefaultFont();
			percentage.alignment = TextAlignmentOptions.Left;

			GetTitleText(panel).text = "Downloading!";

			GameObject continueButtonObject = Object.Instantiate(GetOkButtonComp(panel).gameObject, panel.transform);
			Button continueButton = continueButtonObject.GetComponent<Button>();

			continueButton.onClick.AddListener(() => continueButton.gameObject.SetActive(false));
			continueButtonObject.SetActive(false);
			continueButtonObject.name = "Continue Button";
			continueButtonObject.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Continue";

			return panel;
		}

		public static GameObject MakeRestartPanel(GameObject genericPanel)
		{
			GameObject panel = CopyGeneric(genericPanel);

			Object.Destroy(GetProgressBar(panel));
			Object.Destroy(GetTextArea(panel));
			Object.Destroy(GetModList(panel));

			GetTitleText(panel).text = "Warning!";
			GetInfoText(panel).text = "Game will be restarted!";

			return panel;
		}
	}

	internal class PanelUtils
	{
		public static Transform GetCanvas()
		{
			string sceneName = SceneManager.GetActiveScene().name;
			if (sceneName == "MainMenu") return GameObject.Find("Canvas (1)").transform;
			else if (sceneName.Contains("Select")) return GameObject.Find("Canvas").transform;

			GameObject go = GameObject.Find("Canvas");
			if(go != null) return go.transform;
			go = new GameObject("Canvas");
			go.AddComponent<Canvas>();
			return go.transform;
		}
	}
}