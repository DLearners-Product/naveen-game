using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
public class SlingShot_Main : MonoBehaviour
{
    public static SlingShot_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    bool B_CloseDemo;

    public GameObject G_Game;
    public GameObject G_Transition;
    public GameObject G_coverPage;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject[] GA_Options;
    public GameObject G_Sling;
    public GameObject G_Sling_pos;
    GameObject G_Slingclone;
    int I_Dummy;
    public Vector3[] V3_OptPos;
    bool B_CanClick;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public Sprite[] SPRA_Question;
    public string[] STRA_Que;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;
    public AudioSource AS_Wrong3;

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
    public List<string> STRL_Cover_Img_link;
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
        
        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_2.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA
        }
        else
        {
            URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
            SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

        }
    }

    void Start()
    {
        B_CloseDemo = true;

        Instance = this;
        G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);

        G_instructionPage.SetActive(false);

        TEX_points.text = I_Points.ToString();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;
        
    }
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

    }
    public void THI_Check(GameObject Selected)
    {
        if (B_CanClick)
        {
            // GameObject G_Selected = ;
            STR_currentSelectedAnswer = Selected.name;

           
            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                Selected.GetComponent<Rigidbody2D>().gravityScale = 1f;
               // Selected.GetComponent<Collider2D>().isTrigger = false;
                   B_CanClick = false;
                BUT_Option();
                THI_Correct();
                //I_Collect_count++;
            }
            else { THI_Wrong(); }
        }

    }

    public void BUT_Option()
    {
      //  AS_crtans.Play();

       // G_Selected = EventSystem.current.currentSelectedGameObject;
        string[] STRA_splitUnderscore;

       // STR_currentSelectedAnswer = G_Selected.name;

        string STR_dummy = STRA_Que[I_currentQuestionCount];

        STRA_splitUnderscore = STR_dummy.Split('_');


        if (STRA_splitUnderscore.Length > 0 && STRA_splitUnderscore[0] == "" && STRA_splitUnderscore[STRA_splitUnderscore.Length - 1] != "") // ____ is in front of word or sentence
        {
            STR_dummy = STR_currentSelectedAnswer + STRA_splitUnderscore[STRA_splitUnderscore.Length - 1];
        }



        if (STRA_splitUnderscore.Length > 0 && STRA_splitUnderscore[0] != "" && STRA_splitUnderscore[STRA_splitUnderscore.Length - 1] == "") // ____ is in back of word or sentence
        {
            STR_dummy = STRA_splitUnderscore[0] + STR_currentSelectedAnswer;
        }



        if (STRA_splitUnderscore.Length > 0 && STRA_splitUnderscore[0] != "" && STRA_splitUnderscore[STRA_splitUnderscore.Length - 1] != "") // ____ is in middle of word or sentence
        {
            STR_dummy = STRA_splitUnderscore[0] + STR_currentSelectedAnswer + STRA_splitUnderscore[STRA_splitUnderscore.Length - 1];
        }

        G_Question.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STR_dummy;

    }


    void THI_gameData()
    {
          //THI_getPreviewData();
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
        G_Game.SetActive(true);
        THI_Transition();
    }
    void THI_Transition()
    {
        G_Question.SetActive(false);
        G_Transition.SetActive(true);
        Invoke(nameof(THI_NextQuestion), 2f);
    }

    public void THI_ShowQuestion()
    {
        B_CanClick = true;
        G_Question.SetActive(true);
        G_Question.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    public void THI_CloneSling()
    {
        if (G_Slingclone != null)
        {
            Destroy(G_Slingclone);
        }
        G_Slingclone = Instantiate(G_Sling, G_Sling_pos.transform);
        G_Slingclone.transform.position = G_Sling_pos.transform.position;
    }
   
    public void THI_NextQuestion()
    {

        G_Transition.SetActive(false);
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            I_currentQuestionCount++;

            THI_CloneSling();

            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Question[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];


            G_Question.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRA_Que[I_currentQuestionCount];
            // TEX_question.text = STRA_Que[I_currentQuestionCount];

            for (int i = 0; i < G_Options.transform.childCount; i++)
            {
                G_Options.transform.GetChild(i).name = STRL_options[i];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA_optionClips[i];
                G_Options.transform.GetChild(i).transform.position = V3_OptPos[i];
                G_Options.transform.GetChild(i).GetComponent<Rigidbody2D>().gravityScale = 0f;
               // G_Options.transform.GetChild(i).GetComponent<Collider2D>().isTrigger = true;
            }
           
            THI_ShowQuestion();
            I_wrongAnsCount = 0;
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


    public void THI_Correct()
    {
        AS_crtans.Play();
        
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);

        // Release bird animation
        THI_TrackGameData("1");
        Invoke(nameof(THI_Transition), 3f);


    }
    IEnumerator Highlight()
    {
        
        for (int i = 0; i < G_Options.transform.childCount; i++)
        {
            if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
            {
                I_Dummy = i;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }


        G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.green;
         yield return new WaitForSeconds(0.5f);
            G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Dummy).GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(0.5f);

        
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {
                AS_Wrong3.Play();
                StartCoroutine(Highlight());
                Invoke(nameof(THI_Transition), 10f);

                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
                AS_Wrong3.Play();
                StartCoroutine(Highlight());

               // Invoke(nameof(THI_Transition), 3f);

                //Show answer and after click next question
            }

        }
        else
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                AS_Wrong3.Play();
                Invoke(nameof(THI_Transition), 2f);
            }
               

            //next question
        }
        else
        {
            AS_oops.Play();
        }

        //  B_Fishspawn = true;
        // StartCoroutine(SpawnFish());
        // STR_currentSelectedAnswer = "";
        // B_Correct = false;
    }
    
    public void THI_Wrong()
    {
        Debug.Log("Wrong ans");

       
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;

        THI_CloneSling();
       
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
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_Cover_Img_link[0]);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (STRL_Cover_Img_link != null)
            {
                G_coverPage.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

        //SPRA_Options

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
               STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Img_link,STRL_passageDetail);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            StartCoroutine(IN_downloadlImg());
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());
            
            THI_FindOptioncount();
        }
    }

    void THI_FindOptioncount()
    {
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        if (STRL_options.Count == 2)
        {
            G_Options = GA_Options[0];
        }
        if (STRL_options.Count == 4)
        {
            G_Options = GA_Options[1];
        }
        
        V3_OptPos = new Vector3[STRL_options.Count];

        if (G_Options != null)
        {
            G_Options.SetActive(true);
            for(int i=0;i<STRL_options.Count;i++)
            {
                V3_OptPos[i] = G_Options.transform.GetChild(i).transform.position;
            }
        }
        if(STRL_passageDetail!=null)
        {
            for (int i = 0; i < STRL_questions.Count; i++)
            {
                STRA_Que = STRL_passageDetail[0].Split(',');
            }
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

      //  DemoOver();//remove later
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
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_1(MainController.instance.STR_previewJsonAPI, IL_numbers, STRL_difficulty, STRL_instruction, STRL_BG_img_link, STRL_instructionAudio, STRL_questions,
                STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Img_link, STRL_passageDetail);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        StartCoroutine(IN_downloadlImg());
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());
        
        THI_FindOptioncount();

        // THI_createOptions();
    }
    public IEnumerator IN_downloadlImg()
    {
        SPRA_Question = new Sprite[STRL_questions.Count];
        Debug.Log("Downloading Image");
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

                SPRA_Question[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = STRL_questions[i].Split('/');
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Question[i].name = Finalname[0];
            }
        }

        //  THI_ShowQuestion();
        // Invoke("THI_ShowQuestion",2f);
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
        G_Slingclone.SetActive(false);
        StopAllCoroutines();
        Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STR_instruction;
        TEXM_instruction.gameObject.AddComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
        G_Slingclone.SetActive(true);
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
       
    }
}
