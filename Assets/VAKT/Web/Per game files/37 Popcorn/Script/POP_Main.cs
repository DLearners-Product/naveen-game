using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class POP_Main : MonoBehaviour
{
    public static POP_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;
    bool B_CanClick;

    [Header("Objects")]
    public bool B_Buttonclick;
    int I_Charcount;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_Answer;
    public GameObject G_coverPage;
    string Dummy;
    int IndexofChar;
    public GameObject G_Question;
    public GameObject G_Options;
    int I_Dummy;
    public GameObject G_Demo;
    public GameObject G_Game;
    bool B_CloseDemo;
    public GameObject G_Machine;
    public GameObject G_PopCorn;
    public GameObject[] G_POPCustomer;
    int I_CustomerIndex;
    public Sprite SPR_Correct, SPR_Wrong;
    public GameObject G_Speaker;

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
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
    void Start()
    {
        G_Game.SetActive(false);
        B_CloseDemo = true;
        I_Dummy = 0;
        G_levelComplete.SetActive(false);
        G_Transition.SetActive(false);
        G_instructionPage.SetActive(false);
        I_currentQuestionCount = -1;

        TEX_points.text = I_Points.ToString();

        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

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
        G_Game.SetActive(true);
        THI_Transition();
    }

    public void THI_Transition()
    {
        G_Transition.SetActive(true);
        THI_NextQuestion();
        Invoke(nameof(THI_OffTransition), 5f);
    }

    void THI_OffTransition()
    {
        G_Transition.SetActive(false);
        
    }

    public void THI_NextQuestion()
    {
        if (I_currentQuestionCount < STRL_questions.Count-1)
        {
            G_PopCorn.SetActive(false);
          //  B_CanClick = true;
            I_wrongAnsCount = 0;
            I_currentQuestionCount++;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            G_Question.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            G_Speaker.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            G_Speaker.GetComponent<AudioSource>().Play();
            Debug.Log(STRL_questions[I_currentQuestionCount]);
            for (int i=0;i<IL_numbers[3];i++)
            {
                I_Dummy++;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[I_Dummy-1];
            }

           // I_CustomerIndex = Random.Range(0, G_POPCustomer.Length);

            G_POPCustomer[1].GetComponent<Animator>().SetInteger("Cond", 1);
            G_Machine.GetComponent<Animator>().SetInteger("Cond", 0);
            Invoke(nameof(THI_CanClick), 4f);
        }
        else
        {

            G_levelComplete.SetActive(true);
            StartCoroutine(IN_sendDataToDB());
        }
    }

    void THI_CanClick()
    {
        B_CanClick = true;
    }
    public void BUT_Select()
    {  
        if(B_CanClick)
        {
            GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
            B_CanClick = false;

            STR_currentSelectedAnswer = G_Selected.GetComponent<TextMeshProUGUI>().text;


            string STR_Dummy = STRL_questions[I_currentQuestionCount];
            string[] STRA_splitUnderscore;

            STRA_splitUnderscore = STR_Dummy.Split('_');

            if (STRA_splitUnderscore.Length > 0 && STRA_splitUnderscore[0] == "" && STRA_splitUnderscore[STRA_splitUnderscore.Length - 1] != "") // ____ is in front of word or sentence
            {
                STR_Dummy = STR_currentSelectedAnswer + STRA_splitUnderscore[STRA_splitUnderscore.Length - 1];
            }



            if (STRA_splitUnderscore.Length > 0 && STRA_splitUnderscore[0] != "" && STRA_splitUnderscore[STRA_splitUnderscore.Length - 1] == "") // ____ is in back of word or sentence
            {
                STR_Dummy = STRA_splitUnderscore[0] + STR_currentSelectedAnswer;
            }



            if (STRA_splitUnderscore.Length > 0 && STRA_splitUnderscore[0] != "" && STRA_splitUnderscore[STRA_splitUnderscore.Length - 1] != "") // ____ is in middle of word or sentence
            {
                STR_Dummy = STRA_splitUnderscore[0] + STR_currentSelectedAnswer + STRA_splitUnderscore[STRA_splitUnderscore.Length - 1];
            }

            G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STR_Dummy;

            G_Machine.GetComponent<Animator>().SetInteger("Cond", int.Parse(G_Selected.transform.parent.name));
            
            if(STR_currentSelectedAnswer==STR_currentQuestionAnswer)
            {
               
                G_PopCorn.GetComponent<Image>().sprite = SPR_Correct;
                Invoke(nameof(ShowPopCorn), 4f) ;
                Invoke(nameof(PopCorncollected), 5f);
            }
            else
            {
                G_PopCorn.GetComponent<Image>().sprite = SPR_Wrong;
               
                Invoke(nameof(ShowPopCorn), 3f);
                Invoke(nameof(PopCornBurned), 4f);
            }
        }
    }

    void ShowPopCorn()
    {
       // Debug.Log("ShowOff");
        G_Machine.GetComponent<Animator>().SetInteger("Cond", 0);
       
        G_PopCorn.SetActive(true);
       
    }

    void PopCorncollected()
    {
        G_POPCustomer[I_CustomerIndex].GetComponent<Animator>().SetInteger("Cond", 2);
        G_PopCorn.SetActive(false);
        Invoke(nameof(THI_NextQuestion), 4f);
    }

    void PopCornBurned()
    {
        B_CanClick = true;
        G_POPCustomer[I_CustomerIndex].GetComponent<Animator>().SetInteger("Cond", 3);
        G_PopCorn.SetActive(false);
        Invoke(nameof(CustomerIdle), 3f);
    }

    void CustomerIdle()
    {
        G_POPCustomer[I_CustomerIndex].GetComponent<Animator>().SetInteger("Cond", 0);
    }
    public void THI_Wrong()
    {
        AS_Wrong.Play();
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;

       

      /*  if (I_wrongAnsCount == 7)
        {
            // Debug.Log("Out Off Live");

            if (STR_difficulty == "assistive")
            {
                //next question
            }

        }*/

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
            StartCoroutine(IN_CoverImage());

        }
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

       // THI_OffDemo();
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

    }
    void THI_playAudio()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
        Debug.Log("player clicked. so playing audio");
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
        StartCoroutine(IN_CoverImage());
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
