using UnityEditor;
using UnityEngine;

namespace Editor.PlaySaveMode
{
	public class PlaySaveModeMenuItems
	{
		[MenuItem("CONTEXT/Component/Play Save Mode/Save Now", false, 2000)]
		static void SaveNow(MenuCommand cmd)
		{
			var component = cmd.context as Component;
			PlaySaveModeManager.SaveNow(component);
		}
		
		[MenuItem("CONTEXT/Component/Play Save Mode/Save Now", true)]
		static bool SaveNowValidate(MenuCommand cmd)
		{
			return EditorApplication.isPlaying;
		}
		
		[MenuItem("CONTEXT/Component/Play Save Mode/Save When Exiting Play Mode", false, 2001)]
		static void SaveOnExit(MenuCommand cmd)
		{
			var component = cmd.context as Component;
			PlaySaveModeManager.RegisterSaveOnExit(component);
		}
		
		[MenuItem("CONTEXT/Component/Play Save Mode/Save When Exiting Play Mode", true)]
		static bool SaveOnExitValidate(MenuCommand cmd)
		{
			return EditorApplication.isPlaying;
		}
		
		[MenuItem("CONTEXT/Component/Play Save Mode/Always Save When Exiting Play Mode", false, 2002)]
		static void ToggleAlwaysSave(MenuCommand cmd)
		{
			var component = cmd.context as Component;
			PlaySaveModeManager.ToggleAlwaysSave(component);
		}
		
		[MenuItem("CONTEXT/Component/Play Save Mode/Always Save When Exiting Play Mode", true)]
		static bool ToggleAlwaysSaveValidate(MenuCommand cmd)
		{
			var component = cmd.context as Component;
			Menu.SetChecked(
				"CONTEXT/Component/Play Save Mode/Always Save When Exiting Play Mode",
				component is not null && PlaySaveModeManager.IsAlwaysSave(component)
			);
			return true;
		}
	}
}