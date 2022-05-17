using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using _Project.Scripts;
using System.Collections;

namespace _Project.UI.Scripts.Tutorial
{
    /// <summary>
    /// Tutorial task UI class
    /// </summary>
    public class TutorialManager : MonoBehaviour
    {
        private static TutorialManager instance;
        private const string DEFAULT_REQUIRED_NAME = "You have completed all required tasks for level ";
        private const string DEFAULT_OPTIONAL_NAME = "You have completed all optional tasks for level ";
        private const string DEFAULT_REQUIRED_DESC = "You may press \"Next Level\" now to move onto the next level or click the button next to the progress bar to continue with additional tasks.";
        private const string DEFAULT_OPTIONAL_DESC = "You may press \"Next Level\" now to move onto the next level or do some more exploring.";

        [SerializeField]
        private GameObject contents;

        [SerializeField]
        private Button nextLevelButton;
        [SerializeField]
        private Button nextLevelButtonMainMenu;
        [SerializeField]
        private Button previousLevelButton;

        [SerializeField]
        private Button nextButton;
        [SerializeField]
        private Image nextImage;

        [SerializeField]
        private Button previousButton;
        [SerializeField]
        private Image previousImage;

        [SerializeField]
        private Button expandCollapseButton;
        [SerializeField]
        private Image expandCollapseImage;

        [SerializeField]
        private Sprite expandedIcon;
        [SerializeField]
        private Sprite collapsedIcon;

        [SerializeField]
        private GameObject progressBar;
        [SerializeField]
        private GameObject progressFill;


        [SerializeField]
        private TextMeshProUGUI taskName;
        [SerializeField]
        private TextMeshProUGUI taskDescription;
        [SerializeField]
        private TextMeshProUGUI taskProgress;

        private Tasks currentTasks;

        private Vector2 originalSize;

        private int lastScene;
        private int currentScene;

        /// <summary>
        /// Get the current <see cref="TutorialManager"/> instance.
        /// </summary>
        /// <returns> The current <see cref="TutorialManager"/> instance. </returns>
        public static TutorialManager Get()
        {
            return instance;
        }

        /// <summary>
        /// Load next level (if possible) and update the buttons.
        /// </summary>
        public void NextLevel()
        {
            if (nextLevelButton.interactable && currentScene < lastScene)
                SceneManager.LoadSceneAsync(++currentScene);
        }

        /// <summary>
        /// Load previous level (if possible) and update the buttons.
        /// </summary>
        public void PreviousLevel()
        {
            if (previousLevelButton.interactable && currentScene > 1)
                SceneManager.LoadSceneAsync(--currentScene);
        }

        /// <summary>
        /// Whether the level can be loaded.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Whether the level can be loaded></returns>
        public static bool CanLevelBeLoaded(int level)
        {
            // The first level can always be loaded
            if (level <= 1)
                return true;

            // Check whether the tasks of the previous level exist
            if (GlobalSettings.Get().TutorialTasks.Count <= level - 2)
                return false;

            // Check whether the tasks of the previous level are finished
            return GlobalSettings.Get().TutorialTasks[level - 2].AreRequiredTasksFinished();
        }

        /// <summary>
        /// Skip a tutorial task.
        /// </summary>
        public static void NextTask()
        {
            TutorialManager manager = Get();
            manager.NextTaskinternal();
        }

        /// <summary>
        /// Go to the previous task.
        /// </summary>
        public static void PreviousTask()
        {
            TutorialManager manager = Get();
            manager.PreviousTaskInternal();
        }

        /// <summary>
        /// Complete a tutorial task.
        /// </summary>
        /// <param name="identifier"></param>
        public static void CompleteTask(string identifier)
        {
            TutorialManager manager = Get();
            manager.StartCoroutine(manager.CompleteTaskInternal(identifier));
        }

        /// <summary>
        /// Complete a tutorial task and update the UI if necessary.
        /// </summary>
        /// <param name="identifier"></param>
        private IEnumerator CompleteTaskInternal(string identifier)
        {
            yield return new WaitForSeconds(.2f);
            if (currentTasks.CompleteTask(identifier)) UpdateTutorial();
        }

        /// <summary>
        /// Goes to the previous task and updates the UI if necessary.
        /// </summary>
        private void PreviousTaskInternal()
        {
            if (currentTasks.PreviousTask()) UpdateTutorial();
        }

        /// <summary>
        /// Skip a tutorial task and update the UI if necessary.
        /// </summary>
        private void NextTaskinternal()
        {
            if (currentTasks.NextTask()) UpdateTutorial();
        }

        /// <summary>
        /// Expand or collapse the tutorial tasks.
        /// </summary>
        private void ExpandCollapse()
        {
            GlobalSettings.TutorialExpanded = !GlobalSettings.TutorialExpanded;
            UpdateExpandCollapse();
        }

        /// <summary>
        /// Update the collapse/expand state.
        /// </summary>
        private void UpdateExpandCollapse()
        {
            if (GlobalSettings.TutorialExpanded)
            {
                expandCollapseImage.sprite = expandedIcon;
                GetComponent<LayoutElement>().preferredHeight = originalSize.y;
                contents.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSize.x, originalSize.y);
                progressBar.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
            }
            else
            {
                expandCollapseImage.sprite = collapsedIcon;
                GetComponent<LayoutElement>().preferredHeight = 38;
                contents.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSize.x, 38);
                progressBar.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Updates all UI elements.
        /// </summary>
        private void UpdateTutorial()
        {
            // Set the fill width/percentage
            progressFill.GetComponent<RectTransform>().sizeDelta = new Vector2(progressBar.GetComponent<RectTransform>().rect.width * 
                currentTasks.GetCompletedPercentage(), 0);

            // Set the task progression text
            taskProgress.text = currentTasks.GetCompletedTaskIndex() + "/" + (currentTasks.GetTotalTaskCount() - 1);

            // Set the name and description
            if (currentTasks.GetName() == "") taskName.text = (currentTasks.IsRequiredTask() ? DEFAULT_REQUIRED_NAME : DEFAULT_OPTIONAL_NAME) + currentScene + "/" + lastScene + "!";
            else taskName.text = currentScene + ". " + currentTasks.GetName();

            if (currentTasks.GetDescription() == "") taskDescription.text = currentTasks.IsRequiredTask() ? DEFAULT_REQUIRED_DESC : DEFAULT_OPTIONAL_DESC;
            else taskDescription.text = currentTasks.GetDescription();

            // Update the next/previous level buttons
            previousLevelButton.interactable = currentScene > 1;
            nextLevelButton.interactable = currentScene < lastScene && currentTasks.AreRequiredTasksFinished();
            nextLevelButtonMainMenu.interactable = nextLevelButton.interactable;

            // Update the next/previous buttons
            nextButton.interactable = currentTasks.IsSkippable();
            nextImage.color = nextButton.interactable ? Color.white : new Color(0.3f, 0.3f, 0.3f);
            previousButton.interactable = currentTasks.GetCurrentTaskindex() > 0;
            previousImage.color = previousButton.interactable ? Color.white : new Color(0.3f, 0.3f, 0.3f);
        }

        /// <summary>
        /// Initialize variables, update UI.
        /// </summary>
        private void Start()
        {
            lastScene = SceneManager.sceneCountInBuildSettings - 1;
            currentScene = SceneManager.GetActiveScene().buildIndex;
            originalSize = contents.GetComponent<RectTransform>().sizeDelta;
            expandCollapseButton.onClick.AddListener(ExpandCollapse);
            
            int level = SceneManager.GetActiveScene().buildIndex - 1;
            if (level < 0 || level >= GlobalSettings.Get().TutorialTasks.Count) currentTasks = new Tasks();
            else currentTasks = GlobalSettings.Get().TutorialTasks[level];

            UpdateTutorial();
            UpdateExpandCollapse();
        }

        private void Awake()
        {
            instance = this;
        }
    }
}

