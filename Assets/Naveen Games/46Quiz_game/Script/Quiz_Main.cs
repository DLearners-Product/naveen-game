using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Quiz_Main : MonoBehaviour
{
    public static Quiz_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    bool B_CloseDemo;

    public GameObject G_Game;
   // public GameObject G_Transition;
    public GameObject G_coverPage;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;
    public Color[] PanelColors;
    public GameObject[] G_ColorChange;
    public GameObject G_CircleImages;
    public GameObject G_WhitePanel;
    public GameObject G_Correct,G_Wrong;

    [Header("Objects")]
    public GameObject G_Question;
    public Image IMG_QSprite;
    public GameObject[] GA_Options;
    public Sprite[] SPRA_Questions;
    GameObject G_Highlight;
    bool B_CanClick;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public int I_QueDummy;
    //public string[] STRA_AnsList;

    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
   // public AudioSource AS_oops;
   // public AudioSource AS_crtans;

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

    // Start is called before the first frame update
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
            /*  URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
               SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA*/

            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
    void Start()
    {
        B_CloseDemo = true;
       // 

        G_Game.SetActive(false);
       
        G_levelComplete.SetActive(false);

        G_instructionPage.SetActive(false);

        TEX_points.text = I_Points.ToString();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
       
        Invoke(nameof(THI_gameData), 1f);
       
        G_Correct.SetActive(false);
        G_Wrong.SetActive(false);
        I_currentQuestionCount = -1;
        I_Dummmy = 0;
        I_Counter = 0;
    }
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

    }
    public void THI_Check()
    {
        if (B_CanClick)
        {
            GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
            STR_currentSelectedAnswer = G_Selected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            G_Question.GetComponent<AudioSource>().Stop();
            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                
                B_CanClick = false;
               // G_Selected.transform.GetChild(0).GetComponent<AudioSource>()
               
                for (int i = 0; i < GA_Options.Length; i++)
                {
                    GA_Options[i].gameObject.SetActive(false);
                }
                G_Selected.SetActive(true);

                if (G_Selected.transform.GetChild(0).GetComponent<AudioSource>().clip != null)
                {
                    G_Selected.transform.GetChild(0).GetComponent<AudioSource>().Play();
                    Invoke(nameof(THI_Correct), G_Selected.transform.GetChild(0).GetComponent<AudioSource>().clip.length + 1f);
                }
                else
                {
                    THI_Correct();
                }
            }
            else
            {
                B_CanClick = false;
                if (G_Selected.transform.GetChild(0).GetComponent<AudioSource>().clip != null)
                {
                    G_Selected.transform.GetChild(0).GetComponent<AudioSource>().Play();
                    Invoke(nameof(THI_Wrong), G_Selected.transform.GetChild(0).GetComponent<AudioSource>().clip.length + 1f);
                }
                else
                {
                    THI_Wrong();
                }
            }
        }
    }
   
    public void DemoOver()
    {
        G_WhitePanel.GetComponent<Animator>().Play("Que_Disappear");
        
        G_Game.SetActive(true);
       // THI_Transition();
        for (int i = 0; i < G_CircleImages.transform.childCount; i++)
        {
            G_CircleImages.transform.GetChild(i).GetComponent<Image>().color = PanelColors[i];
            G_CircleImages.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            G_CircleImages.transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i=0;i<STRL_questions.Count;i++)
        {
            G_CircleImages.transform.GetChild(i).gameObject.SetActive(true);
        }
        BUT_instructionPage();
    }

    void THI_Transition()
    {
        G_WhitePanel.GetComponent<Animator>().Play("Que_Disappear");
        THI_NewQuestion();
    }

    public void THI_ShowQuestion()
    {
        G_Correct.SetActive(false);
        G_Wrong.SetActive(false);
        B_CanClick = true;
        G_WhitePanel.GetComponent<Animator>().Play("Que_Appear");
        G_Question.SetActive(true);
        G_Question.GetComponent<AudioSource>().Play();
    }

    public void BUT_QuestionImage()
    {
        I_QueDummy++;
        if (I_QueDummy%2==0)
        {
            G_ColorChange[1].GetComponent<Animator>().Play("Hide_Que_Image");
        }
        else
        {
            G_ColorChange[1].GetComponent<Animator>().Play("Show_Que_Image");
        }
    }

    public void THI_NewQuestion()
    {
        
        Invoke(nameof(THI_NextQuestion),2f);
    }

    public void THI_NextQuestion()
    {
       
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {

            I_currentQuestionCount++;

            
            for (int i = 0; i < G_ColorChange.Length; i++)
            {
                G_ColorChange[i].GetComponent<Image>().color = PanelColors[I_currentQuestionCount];
            }


            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            G_Question.GetComponent<TextMeshProUGUI>().text = SPRA_Questions[I_currentQuestionCount].name;
            G_Question.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

            IMG_QSprite.sprite = SPRA_Questions[I_currentQuestionCount];
            IMG_QSprite.preserveAspect = true;
            // G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

            I_Dummmy = I_Counter + IL_numbers[3];

            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].transform.GetChild(0).name = STRL_options[i + I_Counter];
                GA_Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i + I_Counter];
                //GA_Options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                GA_Options[i].transform.GetChild(0).GetComponent<AudioSource>().clip = ACA_optionClips[i + I_Counter];
                GA_Options[i].SetActive(true);
            }



            I_Counter = I_Counter + IL_numbers[3];

            I_wrongAnsCount = 0;

            THI_ShowQuestion();
        }
        else
        {
            THI_Levelcompleted();
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }



    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }


    /* public Text TEX_MapCount;
     public int I_MapCount;
     public TextMeshProUGUI TM_MapCountFx;*/


   
    public void THI_pointCoinFxOn()
    {
        AS_collecting.Play();
        TM_pointFx.text = "+ 1 points";
        I_Points += 1;
        TEX_points.text = I_Points.ToString();
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_Correct()
    {
        G_Question.SetActive(false);
        G_Correct.SetActive(true);
        G_CircleImages.transform.GetChild(I_currentQuestionCount).GetChild(0).gameObject.SetActive(true);
       // AS_crtans.Play();
        // I_Collect_count++;
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);

        // Release bird animation
        THI_TrackGameData("1");

        Invoke(nameof(THI_Transition), 2f);
    }




    void Highlight()
    {
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }

        for (int i = 0; i < GA_Options.Length; i++)
        {
            if (GA_Options[i].transform.GetChild(0).name == STR_currentQuestionAnswer)
            {
                GA_Options[i].SetActive(true);
                Debug.Log(GA_Options[i].transform.GetChild(0).name);
            }
        }
        THI_ShowQuestion();
        Debug.Log("On Question");
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {
            if (STR_difficulty == "assistive")
            {
                B_CanClick = false;

                // EXP_Player.OBJ_exp_Player.B_CanMove = true;
                Invoke(nameof(Highlight), 2f);
                Invoke(nameof(THI_Transition), 8f);

                //Show answer and move to next question --------------Easy
            }
            if (STR_difficulty == "intuitive")
            {
                //  EXP_Player.OBJ_exp_Player.B_CanMove = true;
                B_CanClick = true;
                Invoke(nameof(Highlight), 2f);
                // Invoke(nameof(THI_Transition), 3f);

                //Show answer and after click next question -------------Medium
            }
           

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                B_CanClick = false;
                Invoke(nameof(THI_Transition), 1f);
            }
            else
            {
                Invoke(nameof(THI_ShowQuestion), 2f);
                B_CanClick = true;
            }

            //next question                        --------------   Hard
        }
        else
        {
            B_CanClick = true;
            Invoke(nameof(THI_ShowQuestion),2f);
            
        }
    }

    public void THI_Wrong()
    {
        G_WhitePanel.GetComponent<Animator>().Play("Que_Disappear");
        G_Wrong.SetActive(true);
        G_Question.SetActive(false);

        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;


        /*  if (I_wrongAnsCount == 5)
          {
              Debug.Log("Restart or use coins");
          }*/
        //REDO the same question

        // wrong bird animation
        Invoke(nameof(THI_WrongEffect),2f);

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

    #region
    void THI_gameData()
    {
        // THI_getPreviewData();
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

            StartCoroutine(EN_getAudioClips());
            StartCoroutine(EN_getAudioClips1());
            StartCoroutine(EN_getAudioClips2());
            StartCoroutine(IN_CoverImage());
            StartCoroutine(IMG_Question());

        }
    }
    public void THI_getPreviewData()
    {
        List<string> STRL_Passagedetails = new List<string>();
        MyJSON json = new MyJSON();
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;


        StartCoroutine(EN_getAudioClips());
        StartCoroutine(EN_getAudioClips1());
        StartCoroutine(EN_getAudioClips2());
        StartCoroutine(IN_CoverImage());
        StartCoroutine(IMG_Question());

        // THI_createOptions();
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

        //SPRA_Options

    }
    public IEnumerator IMG_Question()
    {

        SPRA_Questions = new Sprite[STRL_questions.Count];

        for (int i = 0; i < STRL_questions.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_questions[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                SPRA_Questions[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_questions[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Questions[i].name = Finalname[0];


            }
        }
    }
    public IEnumerator EN_getAudioClips()
    {
        ACA__questionClips = new AudioClip[STRL_quesitonAudios.Count];
        

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

    }
    public IEnumerator EN_getAudioClips1()
    {
       // ACA__questionClips = new AudioClip[STRL_quesitonAudios.Count];
        ACA_optionClips = new AudioClip[STRL_optionAudios.Count];
      //  ACA_instructionClips = new AudioClip[STRL_instructionAudio.Count];


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


    }
    public IEnumerator EN_getAudioClips2()
    {
       // ACA__questionClips = new AudioClip[STRL_quesitonAudios.Count];
       // ACA_optionClips = new AudioClip[STRL_optionAudios.Count];
        ACA_instructionClips = new AudioClip[STRL_instructionAudio.Count];

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
            TEXM_instruction.text = STR_instruction;
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);
        }

        // DemoOver();//remove later
        // THI_Transition();
    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
    }
   
    public void THI_TrackGameData(string analysis)
    {
        DBmanager NewDBManager = new DBmanager();
        NewDBManager.question_id = STR_currentQuestionID;
        NewDBManager.answer = STR_currentSelectedAnswer;
        NewDBManager.analysis = analysis;
        string toJson = JsonUtility.ToJson(NewDBManager);
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

    #endregion
    public void BUT_playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BUT_instructionPage()
    {
        StopAllCoroutines();
       // Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STR_instruction;
        TEXM_instruction.gameObject.GetComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
       // Time.timeScale = 1;
        G_instructionPage.SetActive(false);
        if (I_currentQuestionCount == -1)
        {
            THI_Transition();
        }
    }
}
