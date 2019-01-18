using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Fading : MonoBehaviour {

        public Texture2D FadeOutTexture;
        public float FadeSpeed = 0.8f;

        private const int DrawDepth = -1000;
        private float _alpha = 1.0f;
        private int _fadeDir = -1;

        private void Start() {
            Input.simulateMouseWithTouches = false;
            Time.timeScale = 1f;
            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        private void OnGUI()
        {
            _alpha += _fadeDir * FadeSpeed * Time.deltaTime;
            _alpha = Mathf.Clamp01(_alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
            GUI.depth = DrawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeOutTexture);
        }

        // -1 and 1 
        public float BeginFade(int direction)
        {
            _fadeDir = direction;
            return FadeSpeed;
        }
        
        private void ChangedActiveScene(Scene current, Scene next)
        {
            BeginFade(-1);
        }

    }
}
