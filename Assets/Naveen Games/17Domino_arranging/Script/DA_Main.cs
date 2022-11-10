using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
public class DA_Main : MonoBehaviour
{
    public static DA_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    bool B_CloseDemo;
    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;
    public AnimationClip AC_CorrectClip,AC_WrongClip, AC_FinalAnim;

    [Header("Objects")]
   
    GameObject G_Selected;
    public GameObject G_instructionPage;
    public GameObject G_Options;
    public GameObject G_Dropping;
    public TextMeshProUGUI TEXM_Question;
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_Game;
    public int I_DroppedCount;
    public GameObject G_Optionpos;
    public GameObject G_Optionclone;
    public GameObject G_Answer;
    int k_dummy;
    public GameObject[] GA_Domino;
    public Sprite[] SPR_Dominos;
    public GameObject G_coverPage;
    public GameObject G_Check, G_Restart;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    int I_Counter;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_Correct;
    public AudioSource AS_Wrong;
   //public AudioSource AS_Effect;

    /* [Header("Dummy_Audios")]
     public List<AudioClip> AC_Options;
     public List<AudioClip> AC_Questions;
     public AudioClip AC_InstructionAudio;*/

    [Header("DB")]
    public List<string> STRL_difficulty;
    public string STR_difficulty;
    public List<int> IL_numbers;
    public int I_correctPoints;
    public int I_wrongPoints;
    public List<string> STRL_instruction;
    public string STR_instruction;
    public string STR_video_link;
    public List<string> STRL_options;
    public List<string> STRL_questions;
    public List<string> STRL_answers;
    public List<string> STRL_quesitonAudios;
    public List<string> STRL_optionAudios;
    public List<string> STRL_instructionAudio;
    // public string STR_instructionAudio;
    public List<string> STRL_questionID;
    public string STR_customizationKey;
    //Dummy values only for helicopter game
    public List<string> STRL_BG_img_link;
    public List<string> STRL_avatar_Color;
    public List<string> STRL_Panel_Img_link;
    public List<string> STRL_cover_img_link;

    [Header("GAME DATA")]
    public List<string> STRL_gameData;
    public string STR_Data;

    [Header("LEVEL COMPLETE")]
    public GameObject G_levelComplete;

    [Header("AUDIO ASSIGN")]
    public AudioClip[] ACA__questionClips;
    public AudioClip[] ACA_optionClips;
    public AudioClip[] ACA_instructionClips;
    private void Awake()
    {

        Instance = this;

        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
          //  URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
          //  SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        B_CloseDemo = true;
        G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);
        G_Answer.SetActive(false);
        G_instructionPage.SetActive(false);
        
        
        TEX_points.text = I_Points.ToString();
        
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;
        I_Counter = -1;
        

    }

    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

    }
    void THI_gameData()
    {
        //  THI_getPreviewData();
        if (MainController.instance.mode == "live")
        {
            StartCoroutine(EN_getValues()); // live game in portal
        }
        if (MainController.instance.mode == "preview")
        {
            // preview data in html game generator

            Debug.Log("PREVIEW MODE RAKESH");
            THI_getPreviewData();
        }
    }

    public void DemoOver()
    {
        THI_Transition();
        G_Game.SetActive(true);
    }

    public void THI_Counter()
    {
        I_DroppedCount++;
       // Debug.Log("Count =" + I_DroppedCount);

        if (I_DroppedCount == IL_numbers[3])
        {
           // Debug.Log("Count =" + I_DroppedCount);
            G_Check.GetComponent<Button>().interactable = true;
            // Invoke(nameof(THI_Check),2f);
        }
        else
        {
            G_Check.GetComponent<Button>().interactable = false;
        }
    }
    public void THI_Check()
    {
        G_Check.GetComponent<Button>().interactable = false;
        G_Restart.GetComponent<Button>().interactable = false;

        STR_currentSelectedAnswer = "";
        k_dummy=0;
        for (int i = G_Dropping.transform.GetChild(0).transform.childCount-1; i > 0; i--)
        {
            if (G_Dropping.transform.GetChild(0).transform.GetChild(i).gameObject.activeInHierarchy)
            {
                k_dummy++;
                if (k_dummy == 1) { STR_currentSelectedAnswer = G_Dropping.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).name; }
                else
                {
                    STR_currentSelectedAnswer = STR_currentSelectedAnswer + "," + G_Dropping.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).name;
                }

                //Debug.Log("STR_Selected = " + STR_currentSelectedAnswer + G_Dropping.transform.GetChild(0).transform.GetChild(i).name);
            }
        }
      

        // if(I_currentQuestionCount<STRL_questions.Count-1)

        if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
        {
           // Debug.Log("correct");
              THI_Correct();
        }
        else
        {
          //  Debug.Log("Wrong");
            THI_Wrong();
        }
    }

    void THI_Transition()
    {
        G_Answer.SetActive(false);
        I_DroppedCount = 0;
        G_Transition.SetActive(true);
        
        if (G_Transition.activeInHierarchy) // { Debug.Log("Show Black"); }
        
        TEXM_Question.text = STRL_questions[I_currentQuestionCount+1];
        TEXM_Question.gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount + 1];
        Invoke(nameof(PlayQue_Audio), 1f);

        if(G_Dropping.activeInHierarchy)
        {
            G_Dropping.transform.GetChild(0).GetComponent<Animator>().Play("New State");
        }
        THI_NextQuestion();
       // Invoke(nameof(THI_OffTransition), ACA__questionClips[I_currentQuestionCount + 1].length);
       // Invoke(nameof(THI_OffTransition), 8f);
    }
   
    void PlayQue_Audio()
    {
        TEXM_Question.gameObject.GetComponent<AudioSource>().Play();
       // Debug.Log("Q audio playing");
    }

    public void THI_NextQuestion()
    {
        I_wrongAnsCount = 0;
        G_Check.GetComponent<Button>().interactable = false;
        G_Restart.GetComponent<Button>().interactable = true;
        // G_Transition.SetActive(false);
        for (int i=0;i< GA_Domino.Length;i++)
       {
            GA_Domino[i].SetActive(false);
       }
        for (int i = 0; i < IL_numbers[3]; i++)
        {
            GA_Domino[i].SetActive(true);
            int index = Random.Range(0, SPR_Dominos.Length);
            GA_Domino[i].GetComponent<Image>().sprite = SPR_Dominos[index];
        }

        for (int i = 1; i < G_Dropping.transform.GetChild(0).transform.childCount; i++)
        {
            G_Dropping.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);

            if(G_Dropping.transform.GetChild(0).transform.GetChild(i).transform.childCount!=0)
            {
                Destroy(G_Dropping.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).gameObject);
            }
            
        }
        if(G_Optionpos.transform.childCount!=0)
        {
            for(int i=0;i<G_Optionpos.transform.childCount;i++)
            {
                Destroy(G_Optionpos.transform.GetChild(i).gameObject);
            }
        }

        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            G_Optionpos.GetComponent<HorizontalLayoutGroup>().enabled = true;
            I_currentQuestionCount++;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

           // Debug.Log("Next que");

            for (int i = 0; i < IL_numbers[3]; i++)
            {
              //  Debug.Log("Option assign");
                I_Counter++;
                GameObject G_Dummy = Instantiate(G_Optionclone);
                G_Dummy.transform.SetParent(G_Optionpos.transform, false);

                G_Dummy.GetComponent<AudioSource>().clip=ACA_optionClips[I_Counter];
                G_Dummy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[I_Counter];
                G_Dummy.name = STRL_options[I_Counter];
                G_Dummy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
              //  G_Dropping.transform.GetChild(0).GetChild(i)
            }

           string[] Answers = STR_currentQuestionAnswer.Split(',');
            int j = IL_numbers[3];
           for(int i=0;i<IL_numbers[3];i++)
           {
                G_Dropping.transform.GetChild(0).transform.GetChild(i+1).gameObject.SetActive(true);
                j = j - 1;
                G_Dropping.transform.GetChild(0).transform.GetChild(i + 1).name = Answers[j];
                G_Dropping.transform.GetChild(0).transform.GetChild(i + 1).GetComponent<Collider2D>().enabled = true;
            }
        }
       /* else
        {
            Invoke(nameof(THI_Levelcompleted), 8f);
        }*/
    }

    public void THI_REDO ()
    {
        G_Answer.SetActive(false);
        I_DroppedCount = 0;
        if (G_Dropping.activeInHierarchy)
        {
            G_Dropping.transform.GetChild(0).GetComponent<Animator>().Play("New State");
        }
        G_Transition.SetActive(true);
        REDO_SameQuestion();
       // Invoke(nameof(THI_OffTransition), 6f);
    }
    public void THI_OffTransition()
    {
        // Debug.Log("Off Black");
        Invoke(nameof(INVOKE), 3f);
    }

    void INVOKE()
    {
        G_Transition.SetActive(false);
    }
    void REDO_SameQuestion()
    {
        Debug.Log("redoSame Question");
        for (int i = 1; i < G_Dropping.transform.GetChild(0).transform.childCount; i++)
        {
            G_Dropping.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);

            if (G_Dropping.transform.GetChild(0).transform.GetChild(i).transform.childCount != 0)
            {
                Destroy(G_Dropping.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).gameObject);
            }

        }
        if (G_Optionpos.transform.childCount != 0)
        {
            for (int i = 0; i < G_Optionpos.transform.childCount; i++)
            {
                Destroy(G_Optionpos.transform.GetChild(i).gameObject);
            }
        }
        if (I_currentQuestionCount < STRL_questions.Count)
        {
            G_Optionpos.GetComponent<HorizontalLayoutGroup>().enabled = true;

            G_Restart.GetComponent<Button>().interactable = true;
            G_Check.GetComponent<Button>().interactable = false;

            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            I_Counter = I_Counter - IL_numbers[3];     //sameoptions to appear

            for (int i = 0; i < IL_numbers[3]; i++)
            {
                //  Debug.Log("Option assign");
                I_Counter++;
                GameObject G_Dummy = Instantiate(G_Optionclone);
                G_Dummy.transform.SetParent(G_Optionpos.transform, false);

                G_Dummy.GetComponent<AudioSource>().clip = ACA_optionClips[I_Counter];
                G_Dummy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[I_Counter];
                G_Dummy.name = STRL_options[I_Counter];
                G_Dummy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                //  G_Dropping.transform.GetChild(0).GetChild(i)
            }

            string[] Answers = STR_currentQuestionAnswer.Split(',');
            int j = IL_numbers[3];
            for (int i = 0; i < IL_numbers[3]; i++)
            {
                G_Dropping.transform.GetChild(0).transform.GetChild(i + 1).gameObject.SetActive(true);
                j = j - 1;
                G_Dropping.transform.GetChild(0).transform.GetChild(i + 1).name = Answers[j];
                G_Dropping.transform.GetChild(0).transform.GetChild(i + 1).GetComponent<Collider2D>().enabled = true;
            }
            
        }
    }


    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }

   

   

    public void THI_Correct()
    {
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
        AS_Correct.Play();
       // Release bird animation
        THI_TrackGameData("1");

        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            G_Dropping.transform.GetChild(0).GetComponent<Animator>().Play("Domino_falling");
            Invoke(nameof(THI_Transition), AC_CorrectClip.length + 1f);
        }
        else
        {
            G_Dropping.transform.GetChild(0).GetComponent<Animator>().Play("Domino_birdescape");
            G_Optionpos.SetActive(false);
            
            Invoke(nameof(THI_Levelcompleted), AC_FinalAnim.length);
        }
    }
   
    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {
                G_Answer.SetActive(true);
                G_Answer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Correct answer = " + STR_currentQuestionAnswer;
                Invoke(nameof(THI_Transition), 3f);
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {

                G_Answer.SetActive(true);
                G_Answer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Correct answer = " + STR_currentQuestionAnswer;

                Invoke(nameof(THI_REDO), 3f);
                //Show answer and after click next question
            }

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if(STR_difficulty== "independent")
            {
                Invoke(nameof(THI_Transition), 3f);
            }
            else
            {
                Invoke(nameof(THI_REDO), 3f);
            }

            //next question
        }
        else
        {
           // Debug.Log("redo1");
            THI_REDO();
        }
        
    }

    public void THI_Wrong()
    {
        AS_Wrong.Play();
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;
        G_Dropping.transform.GetChild(0).GetComponent<Animator>().Play("Wrong_Domino");
        Invoke(nameof(THI_WrongEffect), AC_WrongClip.length+1f);           //REDO the same question

        // wrong bird animation
        

        if (I_Points > I_wrongPoints)
        {
            I_Points -= I_wrongPoints;
        }
        else
        {
            if (I_Points > 0)
            {
                I_Points = 0;
            }
        }
        TEX_points.text = I_Points.ToString();
    }
    public void THI_pointFxOn(bool plus)
    {
        if (plus)
        {
            if (I_correctPoints != 1)
            {
                TM_pointFx.text = "+" + I_correctPoints + " points";
            }
            else
            {
                TM_pointFx.text = "+" + I_correctPoints + " point";
            }
        }
        else
        {
            if (I_Points > 0)
            {
                if (I_wrongPoints != 0)
                {
                    if (I_wrongPoints != 1)
                    {
                        TM_pointFx.text = "-" + I_wrongPoints + " points";
                    }
                    else
                    {
                        TM_pointFx.text = "-" + I_wrongPoints + " point";
                    }
                }
            }
        }
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_pointFxOff()
    {
        TM_pointFx.text = "";
    }
    public IEnumerator IN_CoverImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_cover_img_link[0]);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (STRL_cover_img_link != null)
            {
                G_coverPage.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

    }
    public IEnumerator EN_getValues()
    {
        WWWForm form = new WWWForm();
        form.AddField("game_id", MainController.instance.STR_GameID);
        // Debug.Log("GAME ID : " + MainController.instance.STR_GameID);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {

            MyJSON json = new MyJSON();
            //json.Helitemp(www.downloadHandler.text);
            List<string> STRL_Passagedetails=new List<string>();

            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;



            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());
        }
    }
    public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_quesitonAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudio.Count];

        for (int i = 0; i < STRL_quesitonAudios.Count; i++)
        {
            UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(STRL_quesitonAudios[i], AudioType.MPEG);
            yield return www1.SendWebRequest();
            if (www1.result == UnityWebRequest.Result.ConnectionError || www1.isHttpError || www1.isNetworkError)
            {
                Debug.Log(www1.error);
            }
            else
            {
                ACA__questionClips[i] = DownloadHandlerAudioClip.GetContent(www1);
            }
        }

        for (int i = 0; i < STRL_optionAudios.Count; i++)
        {
            UnityWebRequest www2 = UnityWebRequestMultimedia.GetAudioClip(STRL_optionAudios[i], AudioType.MPEG);
            yield return www2.SendWebRequest();
            if (www2.result == UnityWebRequest.Result.ConnectionError || www2.isHttpError || www2.isNetworkError)
            {
                Debug.Log(www2.error);
            }
            else
            {
                ACA_optionClips[i] = DownloadHandlerAudioClip.GetContent(www2);
            }
        }


        for (int i = 0; i < STRL_instructionAudio.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(STRL_instructionAudio[i], AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {

                ACA_instructionClips[i] = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("audio clips fetched instruction");

            }
        }
        THI_assignAudioClips();
    }
  
    void THI_assignAudioClips()
    {
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }

        // DemoOver();   //remove later
       // THI_Transition();
    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        List<string> STRL_Passagedetails = new List<string>();

        json.Temp_type_2(MainController.instance.STR_previewJsonAPI,STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options,STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        StartCoroutine(EN_getAudioClips());
        // THI_createOptions();
    }
    public void THI_TrackGameData(string analysis)
    {
        DBmanager TrainSortingDB = new DBmanager();
        TrainSortingDB.question_id = STR_currentQuestionID;
        TrainSortingDB.answer = STR_currentSelectedAnswer;
        TrainSortingDB.analysis = analysis;
        string toJson = JsonUtility.ToJson(TrainSortingDB);
        STRL_gameData.Add(toJson);
        STR_Data = string.Join(",", STRL_gameData);
    }

    public IEnumerator IN_sendDataToDB()
    {
        WWWForm form = new WWWForm();
        form.AddField("child_id", MainController.instance.STR_childID);
        form.AddField("game_id", MainController.instance.STR_GameID);
        form.AddField("game_details", "[" + STR_Data + "]");


        Debug.Log("child id : " + MainController.instance.STR_childID);
        Debug.Log("game_id  : " + MainController.instance.STR_GameID);
        Debug.Log("game_details: " + "[" + STR_Data + "]");

        UnityWebRequest www = UnityWebRequest.Post(SendValueURL, form);
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Sending data to DB failed : " + www.error);
        }
        else
        {
            MyJSON json = new MyJSON();
            json.THI_onGameComplete(www.downloadHandler.text);

            Debug.Log("Sending data to DB success : " + www.downloadHandler.text);
        }
    }
    public void BUT_playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BUT_instructionPage()
    {
        Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STR_instruction;
    }

    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
    }
}
