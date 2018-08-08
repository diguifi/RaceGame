using UnityEngine;
using Diguifi.Scene;

namespace Diguifi.UI
{
	public class UIFunctions : MonoBehaviour
	{
		public void LoadScene(string scene)
		{
			SceneControl.instance.LoadSceneWrapper(scene);
		}

		public static void PauseGame()
		{

		}

		public static void UnpauseGame()
		{

		}
	}
}
