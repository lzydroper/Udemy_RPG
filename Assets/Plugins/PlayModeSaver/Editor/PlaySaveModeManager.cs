using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.PlaySaveMode
{
	[Serializable]
	public class ComponentSnapshot {
		public string json;
		public Component component;
	}

	[InitializeOnLoad]
	public static class PlaySaveModeManager
	{
		// Components registered for "Save When Exiting Play Mode"
		private static readonly List<ComponentSnapshot> _pendingSaves = new();
		// Components registered for "Always Save" (persists between play sessions)
		// Key = GlobalObjectId string so it survives domain reload
		private static readonly HashSet<string> _alwaysSaveIds = new();
		/// Returns a read-only view of all pending saves (for the window)
		public static IReadOnlyList<ComponentSnapshot> GetPendingSaves() => _pendingSaves.AsReadOnly();
		/// Returns how many components are in the always-save list
		public static int AlwaysSaveCount => _alwaysSaveIds.Count;
		
		static PlaySaveModeManager()
		{
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		public static void RegisterSaveOnExit(Component c)
		{
			if (c == null) return;
			
			_pendingSaves.RemoveAll(s => s.component == c);
			_pendingSaves.Add(new ComponentSnapshot
			{
				component = c,
				json = EditorJsonUtility.ToJson(c)
			});
			//Debug.Log($"[PlayModeSave] Will save {c.GetType().Name} on {c.gameObject.name} when exiting play mode.");
		}

		public static void SaveNow(Component c)
		{
			if (c == null) return;

			if (EditorApplication.isPlaying)
			{
				RegisterSaveOnExit(c);
				//Debug.Log($"[PlayModeSave] Snapshot taken for {c.GetType().Name} on {c.gameObject.name}. Will apply on exit.");
			}
		}

		public static void ToggleAlwaysSave(Component c)
		{
			if (c == null) return;

			var id = GlobalObjectId.GetGlobalObjectIdSlow(c).ToString();
			if (!_alwaysSaveIds.Add(id))
			{
				_alwaysSaveIds.Remove(id);
				//Debug.Log($"[PlayModeSave] Removed {c.GetType().Name} from always-save list.");
			}
			else
			{
				_alwaysSaveIds.Add(id);
				//Debug.Log($"[PlayModeSave] Added {c.GetType().Name} to always-save list.");
			}
		}
		
		public static bool IsAlwaysSave(Component c)
		{
			return c is not null && _alwaysSaveIds.Contains(GlobalObjectId.GetGlobalObjectIdSlow(c).ToString());
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			switch (state)
			{
				// When entering play mode, snapshot all "always save" components
				case PlayModeStateChange.EnteredPlayMode:
					SnapshotAlwaysSaveComponents();
					break;
				// When exiting play mode, apply all pending snapshots
				case PlayModeStateChange.EnteredEditMode:
					ApplyAllSnapshots();
					break;
			}
		}

		static void SnapshotAlwaysSaveComponents()
		{
			var allComponents = Object.FindObjectsByType<Component>(FindObjectsSortMode.None);
			foreach (var c in allComponents)
			{
				if (c == null) continue;
				var id = GlobalObjectId.GetGlobalObjectIdSlow(c).ToString();
				if(_alwaysSaveIds.Contains(id))
					RegisterSaveOnExit(c);
			}
		}

		public static void ApplyAllSnapshots()
		{
			if (_pendingSaves.Count == 0) return;
			
			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName("Play Save Mode - Apply Changes");
			int undoGroup = Undo.GetCurrentGroup();

			foreach (var snapshot in _pendingSaves)
			{
				if (snapshot.component is null) continue;
				Undo.RecordObject(snapshot.component, "Play Save Mode");
				EditorJsonUtility.FromJsonOverwrite(snapshot.json, snapshot.component);
				EditorUtility.SetDirty(snapshot.component);
				//Debug.Log($"[PlayModeSave] Applied saved values to {snapshot.component.GetType().Name} on {snapshot.component.gameObject.name}");
			}
			
			Undo.CollapseUndoOperations(undoGroup);
			_pendingSaves.Clear();

			// Save the scene so changes persist
			UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
		}
		
		/// Remove a specific component from the pending saves list
		public static void RemovePendingSave(Component c)
		{
			_pendingSaves.RemoveAll(s => s.component == c);
		}
		
		/// Manually trigger apply (called from the window button)
		public static void ApplyNow() => ApplyAllSnapshots();
		
		public static HashSet<int> GetRegisteredInstanceIDs()
		{
			var ids = new HashSet<int>();
			foreach (var s in _pendingSaves)
			{
				if (s.component != null)
					ids.Add(s.component.gameObject.GetInstanceID());
			}
			return ids;
		}
	}
}
