using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.PlayModeSaver.Editor
{
	[InitializeOnLoad]
	public static class PlayModeSaveHierarchyIndicator
	{
		static GUIContent _iconContent;
		static GUIStyle _iconStyle;
		
		static HashSet<int> _registeredIDs = new();

		static PlayModeSaveHierarchyIndicator()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
			EditorApplication.playModeStateChanged += _ => RefreshCache();
			
			EditorApplication.update += OnEditorUpdate;
		}

		static double _lastRefresh;

		static void OnEditorUpdate()
		{
			if (!EditorApplication.isPlaying) return;
			if (EditorApplication.timeSinceStartup - _lastRefresh < 0.5) return;
			_lastRefresh = EditorApplication.timeSinceStartup;
			RefreshCache();
		}

		static void RefreshCache()
		{
			_registeredIDs = PlaySaveModeManager.GetRegisteredInstanceIDs();
			EditorApplication.RepaintHierarchyWindow();
		}

		static void OnHierarchyItemGUI(int instanceID, Rect selectionRect)
		{
			if (_registeredIDs == null || !_registeredIDs.Contains(instanceID))
				return;
			
			if (_iconContent == null)
			{
				string[] candidateIcons = new[]
				{
					"SaveActive",
					"d_SaveActive",
					"SaveAs",
					"d_SaveAs",
					"Favorite Icon",
					"d_Favorite Icon"
				};

				foreach (var name in candidateIcons)
				{
					var candidate = EditorGUIUtility.IconContent(name);
					if (candidate == null || candidate.image == null) continue;
					_iconContent = candidate;
					break;
				}
				
				if (_iconContent == null || _iconContent.image == null)
				{
					var tex = new Texture2D(1, 1);
					tex.SetPixel(0, 0, Color.white);
					tex.Apply();
					_iconContent = new GUIContent(tex);
				}
			}

			_iconStyle ??= new GUIStyle(GUIStyle.none)
			{
				alignment = TextAnchor.MiddleCenter,
				padding = new RectOffset(0, 0, 0, 0)
			};
			
			const float iconSize = 14f;
			Rect iconRect = new Rect(
				selectionRect.xMax - iconSize - 2f,
				selectionRect.y + (selectionRect.height - iconSize) / 2f,
				iconSize,
				iconSize
			);
			
			Color prevColor = GUI.color;
			GUI.color = new Color(1f, 0.75f, 0.2f, 0.9f);
			GUI.Label(iconRect, _iconContent, _iconStyle);
			GUI.color = prevColor;

			// Clicking the icon opens the Play Mode Save window
			if (Event.current.type != EventType.MouseDown || !iconRect.Contains(Event.current.mousePosition)) return;

			PlaySaveModeWindow.Open();
			Event.current.Use();
		}
	}
}