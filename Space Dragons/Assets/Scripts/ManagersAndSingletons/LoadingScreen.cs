using TMPro;
using UnityEngine;
using UnityEngine.UI;



//Code Credits: https://www.windykeep.com/2018/02/15/make-loading-screen-unity/

public class LoadingScreen : Singleton<LoadingScreen>
{
    public bool IsLoadingOpen;
    // Make sure the loading screen shows for at least 1 second:
    private const float MIN_TIME_TO_SHOW = 1.0f;
    // The reference to the current loading operation running in the background:
    private AsyncOperation currentLoadingOperation;
    // A flag to tell whether a scene is being loaded or not:
    private bool isLoading;
    // The rect transform of the bar fill game object:
    [SerializeField]
    private Slider progressBar;
    // The text that shows how much has been loaded:
    [SerializeField]
    private TextMeshProUGUI percentLoadedText;
    // The elapsed time since the new scene started loading:
    private float timeElapsed;
    // Set to true to hide the progress bar:
    [SerializeField]
    private bool hideProgressBar;
    // Set to true to hide the percentage text:
    [SerializeField]
    private bool hidePercentageText;
    // Flag whether the fade out animation was triggered.
    private bool didTriggerFadeOutAnimation;

    [SerializeField]
    private GameObject LoadingPanel;
    [SerializeField]
    private Animator anim;

    public override void Awake()
    {
        base.Awake();
        Configure();
        Hide();
    }
    private void Configure()
    {
        // Enable/disable the progress bar based on configuration:
        progressBar.gameObject.SetActive(!hideProgressBar);
        // Enable/disable the percentage text based on configuration:
        percentLoadedText.gameObject.SetActive(!hidePercentageText);

    }
    private void Update()
    {
        if (isLoading)
        {
            // Get the progress and update the UI. Goes from 0 (start) to 1 (end):
            SetProgress(currentLoadingOperation.progress);
            // If the loading is complete and the fade out animation has not been triggered yet, trigger it:
            if (currentLoadingOperation.isDone && !didTriggerFadeOutAnimation)
            {
                anim.SetTrigger("FadeOut");
                didTriggerFadeOutAnimation = true;
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= MIN_TIME_TO_SHOW)
                {
                    // The loading screen has been showing for the minimum time required.
                    // Allow the loading operation to formally finish:
                    currentLoadingOperation.allowSceneActivation = true;
                }
            }
        }
    }

    private float slowProgress = 0.0f;

    // Updates the UI based on the progress:
    private void SetProgress(float progress)
    {
        // Update the fill's scale based on how far the game has loaded:
        //progressBar.value = progress;
        //Slow fill instead of autoset
        if (slowProgress < progress)
        {
            slowProgress += Time.deltaTime;
            progressBar.value = slowProgress;
            if (slowProgress > 1.0f) slowProgress = 1.0f;
        }
        // Set the percent loaded text:
        // percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
        percentLoadedText.text = Mathf.CeilToInt(slowProgress * 100).ToString() + "%";
    }
    // Call this to show the loading screen.
    // We can determine the loading's progress when needed from the AsyncOperation param:
    public void Show(AsyncOperation loadingOperation)
    {
        // Enable the loading screen:
        if (LoadingPanel)  LoadingPanel.SetActive(true);
        // Store the reference:
        currentLoadingOperation = loadingOperation;
        // Stop the loading operation from finishing, even if it technically did:
        currentLoadingOperation.allowSceneActivation = false;
        // Reset the UI:
        SetProgress(0f);
        // Reset the time elapsed:
        timeElapsed = 0f;
        // Reset the fade out animation flag:
        didTriggerFadeOutAnimation = false;
        // Reset slow load
        slowProgress = 0.0f;

        Time.timeScale = 1;
        isLoading = true;
        IsLoadingOpen = true;
    }
    // Call this to hide it:
    public void Hide()
    {
        // Disable the loading screen:
        currentLoadingOperation = null;
        if (LoadingPanel) LoadingPanel.SetActive(false);
        isLoading = false;
        IsLoadingOpen = false;
    }
}