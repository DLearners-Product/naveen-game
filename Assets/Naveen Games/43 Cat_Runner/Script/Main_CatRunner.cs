using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Main_CatRunner : MonoBehaviour
{
    public static Main_CatRunner Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    public GameObject G_Game;
    public GameObject G_Camera;
    public GameObject G_Cat;
    bool B_CloseDemo;
  //  public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public GameObject G_instructionPage;
    bool B_Answerhighlight;
    public GameObject G_Question;
   
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_coverPage;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;

    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_Correct;
    public AudioSource AS_Wrong;
    public AudioSource AS_Wrong_2;
    public AudioSource AS_Collect;

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
    public List<string> STRL_Cover_Image_link;
    public List<string> STRL_passageDetail;

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
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_2.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
            //  URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
            //  SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

            URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
            SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        G_Game.SetActive(false);
        B_CloseDemo = true;
        G_levelComplete.SetActive(false);
       // G_Transition.SetActive(false);
        G_instructionPage.SetActive(false);
        G_Question.SetActive(false);
        I_currentQuestionCount = -1;
        TEX_points.text = I_Points.ToString();
       
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke(nameof(THI_gameData), 1f);
      
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
        BUT_instructionPage();
        TEXM_instruction.gameObject.GetComponent<AudioSource>().Play();
        G_Game.SetActive(true);
        G_Camera.GetComponent<Cat_Follow>().B_canfollow = true;
        THI_NextQuestion();
    }

   

    
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

    }

    public void BUT_Options()
    {
        // Debug.Log("BUT_option");
        G_Cat.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        G_Cat.GetComponent<Rigidbody2D>().gravityScale = 1;
        G_Cat.GetComponent<Animator>().Play("CatDown");
        STR_currentSelectedAnswer = G_Question.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_InputField>().text;
        THI_TrackGameData("");
        G_Question.SetActive(false);
        THI_NextQuestion();
    }
   
    public void THI_NextQuestion()
    {
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            G_Cat.GetComponent<Animator>().enabled = true;
            G_Cat.GetComponent<CatRunnerMain>().B_CanChange = true;
            G_Cat.GetComponent<CatRunnerMain>().B_CanMove = true;
            I_currentQuestionCount++;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
           
            // TEXM_Question.text = STRL_questions[I_currentQuestionCount];
        }
        else
        {
            G_Cat.GetComponent<CatRunnerMain>().B_LevelCompleted = true;
            G_Cat.GetComponent<CatRunnerMain>().B_CanChange = false;
            G_Cat.GetComponent<CatRunnerMain>().B_CanMove = false;
            G_Cat.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            // CatRunnerMain.B_CanMove
            Invoke(nameof(THI_Levelcompleted), 1f);
        }
    }
    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }

  /*  public void THI_Transition()
    {
        G_Transition.SetActive(true);
        G_Transition.GetComponent<Animator>().Play("Todark");
        Invoke("OffTransition", 2f);
    }*/

    void OffTransition()
    {
       // G_Transition.SetActive(false);
        THI_ShowQuestion();
    }

    public void THI_ShowQuestion()
    {

        G_Question.SetActive(true);
      //  Debug.Log(G_Question.transform.GetChild(0).transform.GetChild(0).name);
        G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
        G_Question.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_InputField>().text = "";
        G_Question.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);


        //TEXM_instruction2.gameObject.GetComponent<AudioSource>().Play();
        I_wrongAnsCount = 0;
        
       // Invoke(nameof(PlayQueAudio), TEXM_instruction2.gameObject.GetComponent<AudioSource>().clip.length);
    }

   
    public void THI_Correct()
    {
       // B_CanClick = false;
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
        AS_Correct.Play();
       
        THI_TrackGameData("1");
        
    }
    

    public void THI_Wrong()
    {

        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;
        if (I_wrongAnsCount == 3)
        {
            AS_Wrong_2.Play(); 
            if (STR_difficulty == "assistive")
            {
                

                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
               
                //Show answer and after click next question
            }

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                //next question
            }
        }
        else
        {
            AS_Wrong.Play();
        }

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

    public void THI_pointCoinFxOn(bool plus)
    {
        if (plus)
        {
            AS_Collect.Play();
            TM_pointFx.text = "+ 1 point";
           
            I_Points += 1;
        }
        else
        {
            if (I_Points > 3)
            {
                AS_Wrong.Play();
                TM_pointFx.text = "- 3 point";
                I_Points -= 3;
            }
            else
            {
                I_Points = 0;
            }
            
        }
        TEX_points.text = I_Points.ToString();
        Invoke(nameof(THI_pointFxOff), 1f);
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
        Invoke(nameof(THI_pointFxOff), 1f);
    }
    public void THI_pointFxOff()
    {
        TM_pointFx.text = "";
    }
    public IEnumerator IN_CoverImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_Cover_Image_link[0]);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (STRL_Cover_Image_link != null)
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
            json.Temp_type_1(www.downloadHandler.text, IL_numbers, STRL_difficulty, STRL_instruction, STRL_BG_img_link, STRL_instructionAudio, STRL_questions,
                STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Image_link, STRL_passageDetail);
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
    public void BUT_Speaker()
    {
        // Debug.Log("Playing Audio");
        G_Question.transform.GetChild(1).GetComponent<AudioSource>().Play();


    }
    void THI_assignAudioClips()
    {
        if (ACA_instructionClips.Length > 0)
        {
            TEXM_instruction.text = STR_instruction;

            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);


            
        }

        // DemoOver();   //remove later

    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
    public void THI_getPreviewData()
    {
        MyJSON json = new MyJSON();
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_1(MainController.instance.STR_previewJsonAPI, IL_numbers, STRL_difficulty, STRL_instruction, STRL_BG_img_link, STRL_instructionAudio, STRL_questions,
                STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Image_link, STRL_passageDetail);

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
        TEXM_instruction.gameObject.AddComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
    }
}
