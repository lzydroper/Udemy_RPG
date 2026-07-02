using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.PlayModeSaver.Editor
{
	public class PlaySaveModeWindow : EditorWindow
	{
		Vector2 _scrollPos;
		double _lastRepaintTime;
		const double RepaintInterval = 0.5; // refresh every 500ms
		
		[MenuItem("Tools/Play Save Mode")]
		public static void Open()
		{
			var window = GetWindow<PlaySaveModeWindow>("Play Save Mode");
			window.minSize = new Vector2(380, 300);
			window.Show();
		}

		void OnEnable()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
		}

		void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeChanged;
		}

		void OnPlayModeChanged(PlayModeStateChange _) => Repaint();
		
		void Update()
		{
			if (EditorApplication.isPlaying && EditorApplication.timeSinceStartup - _lastRepaintTime > RepaintInterval)
			{
				_lastRepaintTime = EditorApplication.timeSinceStartup;
				Repaint();
			}
		}
		
		void OnGUI()
		{
			DrawHeader();
			DrawStatusBanner();
			EditorGUILayout.Space(4);
			DrawPendingSavesList();
			EditorGUILayout.Space(4);
			DrawFooterActions();
		}

		void DrawHeader()
		{
			EditorGUILayout.Space(8);
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label("Play Save Mode", new GUIStyle(EditorStyles.boldLabel)
				{
					fontSize = 14
				});
				GUILayout.FlexibleSpace();

				// Small help label
				GUIStyle mutedStyle = new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = Color.gray } };
				GUILayout.Label("Right-click any component to register", mutedStyle);
			}

			// Divider
			Rect rect = EditorGUILayout.GetControlRect(false, 1);
			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.3f));
			EditorGUILayout.Space(4);
		}

		void DrawStatusBanner()
		{
			bool isPlaying = EditorApplication.isPlaying;
			Color bannerColor = isPlaying
				? new Color(0.15f, 0.5f, 0.15f, 0.25f)
				: new Color(0.4f, 0.4f, 0.4f, 0.15f);

			string bannerText = isPlaying
				? "● PLAY MODE  —  Modify components then right-click to register them"
				: "○ EDIT MODE  —  Changes will be applied here when you exit play mode";

			GUIStyle bannerStyle = new GUIStyle(EditorStyles.helpBox)
			{
				fontSize = 11,
				fontStyle = FontStyle.Normal,
				alignment = TextAnchor.MiddleLeft,
				padding = new RectOffset(10, 10, 6, 6)
			};

			Rect bannerRect = EditorGUILayout.GetControlRect(false, 30);
			EditorGUI.DrawRect(bannerRect, bannerColor);
			GUI.Label(bannerRect, bannerText, bannerStyle);
		}

		void DrawPendingSavesList()
		{
			var pendingSaves = PlaySaveModeManager.GetPendingSaves();
			int alwaysSaveCount = PlaySaveModeManager.AlwaysSaveCount;

			// Section header
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label(
					$"Pending Saves  ({pendingSaves.Count})",
					EditorStyles.boldLabel
				);
				GUILayout.FlexibleSpace();
				if (alwaysSaveCount > 0)
				{
					GUIStyle tagStyle = new GUIStyle(EditorStyles.miniLabel)
					{
						normal = { textColor = new Color(0.4f, 0.8f, 0.4f) }
					};
					GUILayout.Label($"Always-Save: {alwaysSaveCount} component(s)", tagStyle);
				}
			}

			EditorGUILayout.Space(2);

			if (pendingSaves.Count == 0)
			{
				DrawEmptyState();
				return;
			}

			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));
			
			bool alternate = false;
			List<Component> toRemove = new();

			foreach (var snapshot in pendingSaves)
			{
				if (snapshot.component is null)
				{
					toRemove.Add(null);
					continue;
				}

				Color rowColor = alternate
					? new Color(0.5f, 0.5f, 0.5f, 0.05f)
					: new Color(0f, 0f, 0f, 0f);
				alternate = !alternate;

				Rect rowRect = EditorGUILayout.GetControlRect(false, 28);
				EditorGUI.DrawRect(rowRect, rowColor);

				float padding = 8f;
				float removeButtonW = 60f;
				float pingButtonW = 22f;

				// GameObject name
				Rect gameObjRect = new Rect(rowRect.x + padding, rowRect.y + 5, 140, 18);
				GUIStyle goStyle = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };
				GUI.Label(gameObjRect, snapshot.component.gameObject.name, goStyle);

				// Component type
				Rect compRect = new Rect(rowRect.x + padding + 148, rowRect.y + 5, 130, 18);
				GUIStyle compStyle = new GUIStyle(EditorStyles.miniLabel)
					{ normal = { textColor = new Color(0.6f, 0.8f, 1f) } };
				GUI.Label(compRect, snapshot.component.GetType().Name, compStyle);

				// Always-save indicator badge
				bool isAlways = PlaySaveModeManager.IsAlwaysSave(snapshot.component);
				if (isAlways)
				{
					Rect badgeRect = new Rect(rowRect.x + padding + 285, rowRect.y + 6, 50, 16);
					EditorGUI.DrawRect(badgeRect, new Color(0.2f, 0.6f, 0.2f, 0.4f));
					GUI.Label(badgeRect, " always", new GUIStyle(EditorStyles.miniLabel)
					{
						normal = { textColor = new Color(0.4f, 1f, 0.4f) }
					});
				}

				// Ping button (locate in hierarchy)
				Rect pingRect = new Rect(rowRect.xMax - removeButtonW - pingButtonW - padding - 4, rowRect.y + 5, pingButtonW,
					18);
				// AFTER
				GUIContent pingIcon = GetPingIcon();
				if (GUI.Button(pingRect, pingIcon, EditorStyles.iconButton))
				{
					EditorGUIUtility.PingObject(snapshot.component.gameObject);
					Selection.activeGameObject = snapshot.component.gameObject;
				}

				// Remove button
				Rect removeRect = new Rect(rowRect.xMax - removeButtonW - padding, rowRect.y + 5, removeButtonW, 18);
				GUIStyle removeStyle = new GUIStyle(EditorStyles.miniButton)
				{
					normal = { textColor = new Color(1f, 0.4f, 0.4f) }
				};
				if (GUI.Button(removeRect, "Remove", removeStyle))
				{
					toRemove.Add(snapshot.component);
				}
			}

			EditorGUILayout.EndScrollView();
			
			foreach (var c in toRemove)
				PlaySaveModeManager.RemovePendingSave(c);
		}

		void DrawEmptyState()
		{
			GUILayout.FlexibleSpace();
			using (new EditorGUILayout.VerticalScope())
			{
				GUIStyle centeredMuted = new GUIStyle(EditorStyles.label)
				{
					alignment = TextAnchor.MiddleCenter,
					normal = { textColor = Color.gray },
					wordWrap = true
				};
				GUILayout.Label("No components registered yet.", centeredMuted);
				EditorGUILayout.Space(4);
				GUILayout.Label(
					"In play mode, right-click any component header\nand choose \"Play Save Mode > Save Now\" or\n\"Save When Exiting Play Mode\".",
					centeredMuted
				);
			}

			GUILayout.FlexibleSpace();
		}

		void DrawFooterActions()
		{
			// Divider
			Rect rect = EditorGUILayout.GetControlRect(false, 1);
			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.3f));
			EditorGUILayout.Space(6);

			using (new EditorGUILayout.HorizontalScope())
			{
				var pendingSaves = PlaySaveModeManager.GetPendingSaves();
				bool hasAnything = pendingSaves.Count > 0;

				// Apply All button
				using (new EditorGUI.DisabledGroupScope(EditorApplication.isPlaying || !hasAnything))
				{
					GUIStyle applyStyle = new GUIStyle(GUI.skin.button)
					{
						fontStyle = FontStyle.Bold,
						fixedHeight = 28
					};
					if (GUILayout.Button("✓  Apply All Changes", applyStyle, GUILayout.ExpandWidth(true)))
					{
						PlaySaveModeManager.ApplyAllSnapshots();
						Repaint();
					}
				}

				GUILayout.Space(6);

				// Clear All button
				using (new EditorGUI.DisabledGroupScope(!hasAnything))
				{
					GUIStyle clearStyle = new GUIStyle(GUI.skin.button)
					{
						fixedHeight = 28,
						normal = { textColor = new Color(1f, 0.5f, 0.5f) }
					};
					if (GUILayout.Button("✕  Clear All", clearStyle, GUILayout.Width(100)))
					{
						if (EditorUtility.DisplayDialog(
							    "Clear All Saves",
							    "Remove all pending saves? This does NOT revert your scene.",
							    "Clear", "Cancel"))
						{
							foreach (var s in new List<ComponentSnapshot>(pendingSaves))
								PlaySaveModeManager.RemovePendingSave(s.component);
							Repaint();
						}
					}
				}
			}

			EditorGUILayout.Space(6);
		}

		static GUIContent _pingIcon;

		static GUIContent GetPingIcon()
		{
			if (_pingIcon != null) return _pingIcon;

			string[] candidates = new[]
			{
				"d_pick_uielements",
				"d_Pick",
				"scenepicking_pickable-mixed_hover",
				"d_UnityEditor.SceneHierarchyWindow",
				"UnityEditor.SceneHierarchyWindow"
			};

			foreach (var name in candidates)
			{
				var c = EditorGUIUtility.IconContent(name);
				if (c?.image is null) continue;
				_pingIcon = c;
				return _pingIcon;
			}

			_pingIcon = new GUIContent("⌖");
			return _pingIcon;
		}
	}
}