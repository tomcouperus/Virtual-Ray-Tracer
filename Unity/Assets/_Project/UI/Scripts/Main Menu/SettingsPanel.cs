using _Project.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI.Scripts.Main_Menu
{
    public class SettingsPanel : MonoBehaviour
    {
    
        [SerializeField]
        private Button exitButton;
        [SerializeField] 
        private Toggle fullScreenToggle;
        [SerializeField] 
        private Toggle fpsCounterToggle;
        [SerializeField] 
        private Toggle cheatModeToggle;
    
        [SerializeField] 
        private FPSCounter fpsCounter;
    
        /// <summary>
        /// Show the settings panel.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            UIManager.Get().AddEscapable(Hide);
        }

        /// <summary>
        /// Hide the settings panel.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            UIManager.Get().RemoveEscapable(Hide);
        }

        /// <summary>
        /// Toggle the settings panel. If the settings panel is hidden it will now be shown and vice versa.
        /// </summary>
        public void Toggle()
        {
            if (gameObject.activeSelf) Hide();
            else Show();
        }

        private void ToggleFpsCounter(bool isOn)
        {
            fpsCounter.gameObject.SetActive(isOn);
            GlobalSettings.Get().FPSEnabled = isOn;
        }

        private void ToggleFullScreen(bool isOn)
        {
            Screen.fullScreen = isOn;
        }

        private void ToggleCheatMode(bool isOn)
        {
            GlobalSettings.Get().CheatMode = isOn;
            Tutorial.TutorialManager.Get().UpdateTutorial();
        }
    
        private void Awake()
        {
            exitButton.onClick.AddListener(Hide);
            fpsCounterToggle.onValueChanged.AddListener(ToggleFpsCounter);
            fullScreenToggle.onValueChanged.AddListener(ToggleFullScreen);
            cheatModeToggle.onValueChanged.AddListener(ToggleCheatMode);
        }

        private void OnEnable()
        {
            fpsCounterToggle.isOn = GlobalSettings.Get().FPSEnabled;
            fullScreenToggle.isOn = Screen.fullScreen;
            cheatModeToggle.isOn = GlobalSettings.Get().CheatMode;
        }
    }
}
