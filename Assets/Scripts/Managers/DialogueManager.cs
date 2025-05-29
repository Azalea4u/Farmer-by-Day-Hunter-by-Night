using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f; // smaller the number, faster the typing

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displaySpeakerText;
    private Animator layoutAnimator;
    public bool dialogueIsPlaying { get; private set; }

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Shop UI")]
    [SerializeField] private GameObject buyPanel;

    [Header("Audio")]
    [SerializeField] private DialogueAudio_Data defaultAudioInfo;
    [SerializeField] private DialogueAudio_Data[] audioInfos;
    [SerializeField] private bool makePredicatable;
    private DialogueAudio_Data currentAudioInfo;
    private Dictionary<string, DialogueAudio_Data> audioInfoDictionary;
    private AudioSource audioSource;

    private Story currentStory;

    public static DialogueManager instance;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    private bool canSkip = false;
    private bool submitSkip = false;

    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";
    private const string AUDIO_TAG = "audio";

    private Player currentTargetPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            audioSource = this.gameObject.AddComponent<AudioSource>();
            currentAudioInfo = defaultAudioInfo;
        }
        else { Destroy(gameObject); }
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;

        dialoguePanel.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Shop")
            buyPanel.SetActive(false);

        // get layout animator
        //layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        InitializeAudioInfoDictionary();
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudio_Data>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach (DialogueAudio_Data audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudio_Data audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if (audioInfo != null)
        {
            currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogError("Audio info not found for id: " + id);
            currentAudioInfo = defaultAudioInfo;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            submitSkip = true;
        }

        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine && currentStory.currentChoices.Count == 0
            && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit")))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, Player targetPlayer)
    {
        currentStory = new Story(inkJSON.text);
        currentTargetPlayer = targetPlayer;

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        /* Old Code, but kept for reference for when using Methods in INK
        //if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            currentStory.BindExternalFunction("ShowBuyMenu", ShowBuyMenu);
            currentStory.BindExternalFunction("CloseBuyMenu", CloseBuyMenu);
        }
        //else if (SceneManager.GetActiveScene().name == "Test_Level")
        {
            //currentStory.BindExternalFunction("Load_StartMenu", StartMenu);
            //currentStory.BindExternalFunction("QuitGame", QuitGame);
        }
        */

        // Will set an if statement to check if the scene is the shop later
        // Bind the functions to the INK script
        currentStory.BindExternalFunction("Open_Buy", Open_Buy);
        currentStory.BindExternalFunction("Open_Sell", Open_Sell);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        /* Old Code, but kept for reference for when using Methods in INK
        //if (SceneManager.GetActiveScene().name == "Test_Level")
        {
            //currentStory.UnbindExternalFunction("Load_StartMenu");
            //currentStory.UnbindExternalFunction("QuitGame");
        }
        //else if (SceneManager.GetActiveScene().name == "Rest_Level")
        {
            currentStory.UnbindExternalFunction("ShowBuyMenu");
            currentStory.UnbindExternalFunction("CloseBuyMenu");
        }
        */

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        // go back to default audio info
        SetCurrentAudioInfo(defaultAudioInfo.id);
        //GameManager.instance.ResumeGame();
    }

    // Continues to the next line of text
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            // handle tags
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    // Checks if the player can skip the current line
    // i.e. if you have already read the line before
    private IEnumerator CanSkip()
    {
        canSkip = false; //Making sure the variable is false.
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }

    // shows the current line of dialogue
    private IEnumerator DisplayLine(string line)
    {
        // empty the dialogue text
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        continueIcon.SetActive(false);
        HideChoices();

        canSkip = false;
        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            if (canSkip && submitSkip)
            {
                submitSkip = false;
                dialogueText.maxVisibleCharacters = line.Length;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>') isAddingRichTextTag = false;
            }
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // Continue icon will show to indicate that all letters in the line has been shown & the player can continue
        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
        canSkip = false;
    }

    // As each letter is displayed, it plays a sound
    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        if (currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            AudioClip soundClip = null;

            // create predicatable audio from hasing
            if (makePredicatable)
            {
                //int hashCode = currentCharacter.GetHashCode();
                // get random hash code
                int hashCode = Random.Range(0, 1000);
                // sound clip
                int predicableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predicableIndex];

                // pitch
                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                // cannot be zero
                if (pitchRangeInt != 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100.0f;
                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            else // randomize audio
            {
                int randomInt = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomInt];

                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }
            audioSource.PlayOneShot(soundClip);

        }
    }

    // Handles the tags in the INK script
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogError("Invalid tag format: " + tag);
                continue;
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                // SPEAKER_TAG = who is currently speaking
                case SPEAKER_TAG:
                    displaySpeakerText.text = tagValue;
                    break;
                // LAYOUT_TAG = checks if Left or Right Layout will be used
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                // AUDIO_TAG = sees which kind of text audio will be played
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue); // This should now be called
                    break;
                default:
                    Debug.LogWarning("Unrecognized tag: " + tagKey);
                    break;
            }
        }
    }

    #region CHOICES
    // After a choice is made, hide all the choices
    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    // When the player needs to make a choice, display the choices
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // check if the number of choices is greater than the number of choices the UI can display
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can display! Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            if (currentChoices.Count == 2)
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = choice.text;
                index++;
            }
        }

        // go through the remaining choicces the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    // Automatically sets the first choice as the selected object
    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    // When the player presses on a Choice BTN
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
        else
        {
            Debug.LogError("Choice index out of range: " + choiceIndex);
        }
    }
    #endregion

    // When the player reaches the end of all dialogues in the INK file
    private void EndConversation()
    {
        StartCoroutine(ExitDialogueMode());
    }

    // These are old code, but kept for reference for when using Methods in INK
    #region STORE_MENU
    private void ShowBuyMenu()
    {
        Debug.Log("Showing buy menu");

        //dialoguePanel.SetActive(false);  // Optionally hide dialogue while shopping
        //InventoryMenu_UI.GetInstance().ToggleStore();
        buyPanel.SetActive(true);
    }

    private void ShowSellMenu()
    {
        Debug.Log("Showing sell menu");
        //dialoguePanel.SetActive(false);  // Optionally hide dialogue while shopping
        //sellPanel.SetActive(true);
    }

    // Add methods to handle closing shop UI and returning to dialogue
    public void CloseBuyMenu()
    {
        //InventoryMenu_UI.GetInstance().ToggleStore();
        buyPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        //ContinueStory();
    }
    #endregion

    #region Open_SHOP

    public void Open_Buy()
    {
        ShopManager.instance.OpenShop(currentTargetPlayer);
        ExitDialogueMode();
    }

    public void Open_Sell()
    {
        ShopManager.instance.OpenShop(currentTargetPlayer, true);
        ExitDialogueMode();
    }
    #endregion

    public void QuitGame()
    {
        Debug.Log("Quiting...");
        GameManager.instance.QuitGame();
    }
}