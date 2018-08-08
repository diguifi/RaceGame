using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Diguifi.UI.MainMenu
{

	[System.Serializable]
	public class UIMainMenu
	{
		public Button buttonStart;

		public EventSystem eventSystem;
	}

	public class MainMenuUI : MonoBehaviour
	{

		public UIMainMenu mainMenu;

		public void ExitGame()
		{
			#if UNITY_EDITOR

			UnityEditor.EditorApplication.isPlaying = false;

			#elif UNITY_WEBPLAYER

			Application.OpenURL("https://diguifi.github.io");

			#else

			Application.Quit();

			#endif
		}
	}
}