using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Read_Comp : MonoBehaviour
{
    public static Read_Comp Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    bool B_CloseDemo;
    public GameObject G_Hero;
    public GameObject G_Enemy;
    public GameObject G_HeroBullet;
    public GameObject G_EnemyBullet;
    public GameObject G_Game;
    public GameObject G_Transition;
    public GameObject G_coverPage;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;
    public string STR_Passage;

    [Header("Objects")]
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject G_Passage;
    public TextMeshProUGUI TMP_Passage;
    public GameObject[] GA_Options;
    int I_Dummmy;
    int I_Counter;
    bool B_CanClick;
    int I_Wrong;
    bool B_CloseReading;

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
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;
    public AudioSource AS_Wrong3;
    public AudioSource AS_Hit;

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
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA
        }
        else
        {
            URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
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
        G_Passage.SetActive(false);
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
    public void THI_Check()
    {
        if (B_CanClick)
        {
            // GameObject G_Selected = ;
            STR_currentSelectedAnswer = EventSystem.current.currentSelectedGameObject.name;

            EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<AudioSource>().Play();
            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
               // Selected.GetComponent<Rigidbody2D>().gravityScale = 1f;
                // Selected.GetComponent<Collider2D>().isTrigger = false;
                B_CanClick = false;
               // BUT_Option();
                THI_Correct();
                //I_Collect_count++;
            }
            else { B_CanClick = false; THI_Wrong(); }
        }

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
        BUT_instructionPage();
        G_Game.SetActive(true);
        I_Dummmy = 0;
        I_Counter = 0;
       
       
    }
    public void THI_Transition()
    {
       // G_Passage.SetActive(true);
        G_Question.transform.parent.gameObject.SetActive(false);
        G_Transition.SetActive(true);
       
        if(I_currentQuestionCount < STRL_questions.Count - 1)
        {
            BUT_PassageOn();
            B_CloseReading = true;
        }
        else
        {
            THI_Levelcompleted();
        }
        
    }

    public void BUT_PassageOn()
    {
        G_Passage.SetActive(true);
    }

    public void BUT_1passageclose()
    {
        if(B_CloseReading)
        {
            B_CloseReading = false;
            THI_NextQuestion();
        }
        G_Passage.SetActive(false);

    }
    public void THI_ShowQuestion()
    {
        B_CanClick = true;
        G_Question.transform.parent.gameObject.SetActive(true);
        G_Question.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

   

    public void THI_NextQuestion()
    {

        G_Transition.SetActive(false);
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            I_currentQuestionCount++;

            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
          
            G_Question.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            
            G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            
            I_Dummmy = I_Counter + IL_numbers[3];
            for (int i = 0; i < IL_numbers[3]; i++)
            {
                G_Options.transform.GetChild(i).name = STRL_options[i+ I_Counter];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i+ I_Counter];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA_optionClips[i+ I_Counter];
                G_Options.transform.GetChild(i).gameObject.SetActive(true);
            }
            I_Counter = I_Counter + IL_numbers[3];

            THI_ShowQuestion();
            I_wrongAnsCount = 0;
        }
        else
        {
            THI_Levelcompleted();
        }
    }



    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }

    public void THI_HitEffect(int hit)
    {
        AS_Hit.Play();
        if(hit!=1)
        {
            G_Hero.GetComponent<Animator>().Play("Hero_Hit");
        }
        else
        {
            G_Enemy.GetComponent<Animator>().Play("Enemy_Hit");
        }
       // Invoke(nameof(THI_Transition), 1f);
    }

    public void THI_Correct()
    {
        AS_crtans.Play();
        G_Hero.GetComponent<Animator>().Play("Hero_Attack");
        Invoke(nameof(H_FireBullet), 0.75f);

        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);

        THI_TrackGameData("1");
        Invoke(nameof(THI_Transition), 5f);
    }
    void H_FireBullet()
    {
        GameObject G_Dummy = Instantiate(G_HeroBullet);
       // G_Dummy.transform.SetParent(G_Hero.transform.GetChild(0).transform, false);
        G_Dummy.transform.position = G_Hero.transform.GetChild(0).transform.position;
    }
    void THI_Highlight()
    {
        for (int i = 0; i < G_Options.transform.childCount; i++)
        {
            G_Options.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < G_Options.transform.childCount; i++)
        {
            if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
            {
                I_Wrong = i;
                G_Options.transform.GetChild(i).gameObject.SetActive(true);

            }
        }


       /* G_Options.transform.GetChild(I_Wrong).GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Wrong).GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Wrong).GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(0.5f);
        G_Options.transform.GetChild(I_Wrong).GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(0.5f);*/


    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {
                AS_Wrong3.Play();
                THI_Highlight();
                Invoke(nameof(THI_Transition), 3f);
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
                AS_Wrong3.Play();
                THI_Highlight();
                B_CanClick = true;

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
            else
            {
                B_CanClick = true;
                AS_oops.Play();
            }

            //next question
        }
        else
        {
            B_CanClick = true;
            AS_oops.Play();
        }

        //  B_Fishspawn = true;
        // StartCoroutine(SpawnFish());
        // STR_currentSelectedAnswer = "";
        // B_Correct = false;
    }

    void THI_EnemyBullet()
    {
        GameObject G_Dummy = Instantiate(G_EnemyBullet);
      //  G_Dummy.transform.SetParent(G_Enemy.transform.GetChild(0).transform, false);
        G_Dummy.transform.position = G_Enemy.transform.GetChild(0).transform.position;
    }

    public void THI_Wrong()
    {
      //  Debug.Log("Wrong ans");
        
        G_Enemy.GetComponent<Animator>().Play("Enemy_Attack");
        Invoke(nameof(THI_EnemyBullet), 0.75f);

        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;



        //REDO the same question

        // wrong bird animation
        Invoke(nameof(THI_WrongEffect), 3f);

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
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_Cover_Img_link, STRL_passageDetail);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            if (STRL_passageDetail != null)
            {
                for (int i = 0; i < STRL_questions.Count; i++)
                {
                    STR_Passage = STRL_passageDetail[0];
                }
            }
            TMP_Passage.text = STR_Passage;
            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());

            if(IL_numbers[3]==2)
            {
                G_Options = GA_Options[0];
            }
            if (IL_numbers[3] == 3)
            {
                G_Options = GA_Options[1];
            }
            for(int i=0;i<GA_Options.Length;i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Options.SetActive(true);
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
        MyJSON json = new MyJSON();
        //  json.Helitemp(MainController.instance.STR_previewJsonAPI);
        json.Temp_type_2(MainController.instance.STR_previewJsonAPI, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_Cover_Img_link, STRL_passageDetail);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;


        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());

        if (STRL_passageDetail != null)
        {
            for (int i = 0; i < STRL_questions.Count; i++)
            {
                STR_Passage = STRL_passageDetail[0];
            }
        }
        TMP_Passage.text = STR_Passage;

        if (IL_numbers[3] == 2)
        {
            G_Options = GA_Options[0];
        }
        if (IL_numbers[3] == 3)
        {
            G_Options = GA_Options[1];
        }
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        G_Options.SetActive(true);
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
        TEXM_instruction.GetComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
        if(I_currentQuestionCount==-1)
        {
            THI_Transition();
        }
    }
}