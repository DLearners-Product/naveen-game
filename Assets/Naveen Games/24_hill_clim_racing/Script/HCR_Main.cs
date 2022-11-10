using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class HCR_Main : MonoBehaviour
{
    public static HCR_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    bool B_CloseDemo;

    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    GameObject G_Selected;
    public GameObject G_instructionPage;
    public GameObject G_Options;
    public GameObject G_Question;
    public TextMeshProUGUI TEXM_Question;
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_Game;
    public GameObject G_Player;
    int k_dummy;
    public GameObject G_coverPage;
    public GameObject[] GA_Options;
    bool B_CanClick;
    public Button BUT_Arrow1;
    public Button BUT_Arrow2;
    GameObject G_Highlight;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;

    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_Correct;
    public AudioSource AS_Wrong;
    public AudioSource AS_Wrong2;
    public AudioSource AS_BGM;

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
              URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
              SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

          //  URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
          //  SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

        }

    }
    void Start()
    {
        B_CloseDemo = true;
        G_Player.SetActive(true);
        G_Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        // G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);
        G_Question.SetActive(false);
        G_instructionPage.SetActive(false);


        TEX_points.text = I_Points.ToString();

        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;
        I_Counter= I_Dummmy = 0;


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
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

    }
    public void DemoOver()
    {
        THI_Transition();
        //G_Game.SetActive(true);
        G_Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        AS_BGM.Play();
    }

    public void THI_ShowQuestion()
    {
        if(I_currentQuestionCount<STRL_questions.Count)
        {
            B_CanClick = true;
            G_Question.SetActive(true);
            G_Question.transform.GetChild(0).GetComponent<AudioSource>().Play();
        }
       
    }
    public void BUT_OptionSelected()
    {
        if(B_CanClick)
        {
            STR_currentSelectedAnswer = "";
            GameObject G_Selected = EventSystem.current.currentSelectedGameObject;

            STR_currentSelectedAnswer = G_Selected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                TEXM_Question.GetComponent<AudioSource>().Stop();
                G_Selected.GetComponent<AudioSource>().Play();
               // Debug.Log("correct");
                THI_Correct();
                B_CanClick = false;
            }
            else
            {
               // Debug.Log("Wrong");
                THI_Wrong();
            }
        }
       
    }

    void THI_Transition()
    {
        // Debug.Log("OffQuestion");
        HC_Controller.Instance.THI_ReFill();

        G_Question.SetActive(false);
        G_Transition.SetActive(true);

        TEXM_Question.text = STRL_questions[I_currentQuestionCount + 1];
        G_Question.transform.GetChild(0).gameObject.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount + 1];


        THI_NextQuestion();
        Invoke(nameof(THI_OffTransition), 6f);
    }


    public void THI_NextQuestion()
    {
        I_wrongAnsCount = 0;
        // G_Transition.SetActive(false);
        
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            I_currentQuestionCount++;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

             Debug.Log("Next que");
            I_Dummmy = IL_numbers[3] + I_Counter;


            for (int k = 0; k < G_Options.transform.childCount; k++)
            {
               // for (int i = I_Counter; i < I_Dummmy; i++)
               // {
                    G_Options.transform.GetChild(k).GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[k + I_Counter];
                    G_Options.transform.GetChild(k).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                    G_Options.transform.GetChild(k).GetChild(0).name = STRL_options[k + I_Counter];
                    G_Options.transform.GetChild(k).GetComponent<AudioSource>().clip = ACA_optionClips[k + I_Counter];

                // }
            }

            I_Counter = I_Counter + IL_numbers[3];
        }
        
    }

   
    void THI_OffTransition()
    {
        if (!G_Player.activeInHierarchy) { G_Player.SetActive(true); }
       
        G_Transition.SetActive(false);
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
       if(I_currentQuestionCount < STRL_questions.Count - 1)
       {
            Invoke(nameof(THI_Transition),3f);
       }
        else
        {
            Invoke(nameof(Invoke), 3f);
            //  Invoke(nameof(THI_Levelcompleted), 2f);
        }
        
    }

    void Invoke()
    {
        G_Question.SetActive(false);
        BUT_Arrow1.interactable = false;
        BUT_Arrow2.interactable = false;
        THI_ScoreIncrease();
    }
    public void THI_ScoreIncrease()
    {
        // float lerp = 0f, duration = 2f;
        float lerp = 0.2f;

        string value = HC_Controller.Instance.TEX_Distance.text;
       // Debug.Log("Distance ="+value);
        string[] value2 = value.Split(' ');
       // Debug.Log("Split =" + value2);
        int NewPoints = int.Parse(value2[0]);

        NewPoints = NewPoints + I_Points;
       // Debug.Log("Newpoint =" + NewPoints);

        I_Points = (int)Mathf.Lerp(I_Points, NewPoints, lerp);
        TEX_points.text = NewPoints.ToString();
       // Debug.Log("FInalpoint ="+ NewPoints);
        Invoke(nameof(THI_Levelcompleted),3f);

    }

    IEnumerator Highlight()
    {
        for(int i=0;i<3;i++)
        {
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.green;
            yield return new WaitForSeconds(1f);
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.black;
            yield return new WaitForSeconds(1f);
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.white;
            yield return new WaitForSeconds(0.75f);
        }
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {
                AS_Wrong2.Play();
                for(int i=0;i<G_Options.transform.childCount;i++)
                {
                    if (G_Options.transform.GetChild(i).transform.GetChild(0).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).transform.GetChild(0).gameObject;
                    }
                }
                StartCoroutine(Highlight());
                Invoke(nameof(THI_Transition), 8f);
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {

                AS_Wrong2.Play();
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    if (G_Options.transform.GetChild(i).transform.GetChild(0).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).transform.GetChild(0).gameObject;
                    }
                }
                StartCoroutine(Highlight());
                // THI_ReCreate();
                //Show answer and after click next question
            }

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                AS_Wrong2.Play();
                THI_Transition();
            }

            //next question
        }
        

    }

  
    public void THI_Wrong()
    {
        AS_Wrong.Play();
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;

        //REDO the same question

        // wrong bird animation
        THI_WrongEffect();

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
    public void THI_Collectcoins()
    {
        TM_pointFx.text = "+" + 2 + " points";
        I_Points += 2;
        TEX_points.text = I_Points.ToString();
        Invoke("THI_pointFxOff", 1f);
    }

    public void THI_pointReduce()
    {
        I_Points -= 10;
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

                  //  I_Points -= 2;
                  //  TEX_points.text = I_Points.ToString();
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
            List<string> STRL_Passagedetails = new List<string>();
            MyJSON json = new MyJSON();
            //json.Helitemp(www.downloadHandler.text);
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            for(int i=0;i<GA_Options.Length;i++)
            {
                GA_Options[i].SetActive(false);
            }
            if(IL_numbers[3]==2) {G_Options = GA_Options[0];}
            if(IL_numbers[3]==3) {G_Options = GA_Options[1];}
            if(IL_numbers[3]==4) {G_Options = GA_Options[2];}
            G_Options.SetActive(true);

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

       //  DemoOver();   //remove later
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
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        if (IL_numbers[3] == 2) { G_Options = GA_Options[0]; }
        if (IL_numbers[3] == 3) { G_Options = GA_Options[1]; }
        if (IL_numbers[3] == 4) { G_Options = GA_Options[2]; }
        G_Options.SetActive(true);

        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());
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
