using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Diguifi.Scene
{
	public class SceneControl : MonoBehaviour
	{
		public static SceneControl instance;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(this.gameObject);
			}
		}

		private void Start()
		{
			StartCoroutine(CustomSceneAwake(SceneManager.GetActiveScene().name));
		}

		public void LoadSceneWrapper(string scene)
		{
			StartCoroutine(LoadScene(scene));
		}

		public IEnumerator LoadScene(string scene)
		{
			//TODO: Loading panel in.

			var AO = SceneManager.LoadSceneAsync(scene);

			yield return null;
			do { yield return null; } while (!AO.isDone);
			yield return null;

			yield return CustomSceneAwake(scene);

			//TODO: Loading panel out.
		}

		public IEnumerator CustomSceneAwake(string scene)
		{

			if (scene == "InGame")
			{ }
			else if (scene == "MainMenu")
			{ }

			yield return null;
		}
	}
}