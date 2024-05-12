using UnityEngine;
using UnityEngine.SceneManagement;

namespace EmptyGame.Misc
{
    public class ForcePreload : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            if (FindObjectOfType<Preload>() == null)
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }
#endif
    }
}
