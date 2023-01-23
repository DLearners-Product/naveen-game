using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Tarzan_Main : MonoBehaviour
{
    public static Tarzan_Main Instance;
    public float F_quesAllocPer;
    public GameObject[] GA_mushrooms;
    public AllocatedQuestions[] AD_questionAllocated;
    public GameObject G_questionImage, G_optionImagePanel;
    public GameObject G_2OptionPanel,
                    G_3optionPanel,
                    G_4OptionPanel;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    public GameObject G_CurrentLevel;
    public GameObject[] GA_Levels;
    bool B_CloseDemo;
    public GameObject G_Game;
    public GameObject G_Transition;
    public GameObject G_coverPage;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    //public TextMeshProUGUI TEXM_instruction2;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public GameObject G_Question;
    public Sprite[] SPRA_Questions;
    GameObject G_Highlight;
    bool B_CanClick;
    public GameObject G_Player;
    int oldnumber;
    int index;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public int I_currentOptionStartingCount;
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public string[] STRA_AnsList;
    // public int I_Collect_count;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;
    public AudioSource AS_LevelOver;

    [Header("DB")]
    int I_optionCount;
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
    public Sprite[] SPRA_Options;
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

#region APP_FUNCTIONALITY

    public void THI_SpawnQuestion(){
        G_Question.SetActive(true);

        switch (I_optionCount)
        {
            case 2:
                G_optionImagePanel = G_2OptionPanel;
                break;
            case 3:
                G_optionImagePanel = G_3optionPanel;
                break;
            case 4:
                G_optionImagePanel = G_4OptionPanel;
                break;
        }

        G_optionImagePanel.SetActive(true);

        THI_NextQuestion();
    }

#endregion

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
       
        Frog_Follow.OBJ_followingCamera.B_canfollow = false;
        G_Player.SetActive(false);
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
        I_currentOptionStartingCount = 0;
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

    public void BUT_Submit()
    {
        STR_currentSelectedAnswer = G_Question.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_InputField>().text;
        THI_TrackGameData("");
        if(I_currentQuestionCount<STRL_questions.Count-1)
        {
            G_Question.SetActive(false);
            THI_CloneLevel();
            G_Player.GetComponent<Tarzan_Player>().THI_StartPos();
        }
        else
        {
            THI_Levelcompleted();
        }
        
    }

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

    public void DemoOver()
    {
        G_Game.SetActive(true);
      //  G_Player.SetActive(true);
        BUT_instructionPage();
       
    }

    void THI_CloneLevel()
    {
        THI_Transition();
        if (G_CurrentLevel!=null)
        {
            Destroy(G_CurrentLevel);
        }
        
        index = Random.Range(0, GA_Levels.Length);
        if(oldnumber !=index)
        {
            oldnumber = index;
        }
        else
        {
            if(index == GA_Levels.Length) { index--; }
            if(index == 0) { index++; }
            if(index == 1) { index--; }
        }
       
        G_CurrentLevel = Instantiate(GA_Levels[index]);
        G_CurrentLevel.transform.SetParent(G_Game.transform, false);

        G_Player.SetActive(true);
        G_Player.GetComponent<Tarzan_Player>().GA_Circles = new GameObject[0];

        Invoke(nameof(THI_DelayCall), 0.25f); 
    }

    void THI_DelayCall()
    {
        G_Player.GetComponent<Tarzan_Player>().THI_GetCircles();
    }
    void THI_Transition()
    {
       // this.GetComponent<N_SwipeControls>().enabled = true;
        G_Transition.SetActive(true);
        Invoke(nameof(THI_NewQuestion), 2f);
    }

    public void THI_ShowQuestion()
    {
       
       // this.GetComponent<N_SwipeControls>().enabled = false;
        B_CanClick = true;
        G_Question.SetActive(true);

        // TEXM_instruction2.gameObject.GetComponent<AudioSource>().Play();
        // Invoke(nameof(PlayQuestionAudio), TEXM_instruction2.gameObject.GetComponent<AudioSource>().clip.length);
        PlayQuestionAudio();
    }

    void PlayQuestionAudio()
    {
        if (G_Question.activeInHierarchy)
            G_Question.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    public void THI_NewQuestion()
    {
        G_Question.SetActive(false);
        THI_NextQuestion();
    }

    public void THI_NextQuestion()
    {

        // G_Transition.SetActive(false);
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            I_currentQuestionCount++;

            STRA_AnsList = null;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            G_questionImage.GetComponent<Image>().sprite = SPRA_Questions[I_currentQuestionCount];
            for(int i=0; i < G_optionImagePanel.transform.childCount; i++, I_currentOptionStartingCount++){

                G_optionImagePanel.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = SPRA_Options[I_currentOptionStartingCount];

            }

            // G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            // G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            // G_Question.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_InputField>().text = "";

            // G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Questions[I_currentQuestionCount];
            // G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            //  G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

          //  I_Dummmy = I_Counter + IL_numbers[3];


           // I_Counter = I_Counter + IL_numbers[3];

          //  I_wrongAnsCount = 0;
        }
        else
        {
           // THI_Levelcompleted();
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
        // I_Collect_count++;
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);

        // Release bird animation
        THI_TrackGameData("1");
       
        Invoke(nameof(THI_NewQuestion), 2f);

    }

    IEnumerator Highlight()
    {
        for (int i = 0; i < 5; i++)
        {
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.green;
            yield return new WaitForSeconds(0.5f);
            G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        G_Highlight.GetComponent<TextMeshProUGUI>().color = Color.green;
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {

            if (STR_difficulty == "assistive")
            {
                B_CanClick = false;



                Invoke(nameof(THI_Transition), 5f);
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
                B_CanClick = true;

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
                B_CanClick = false;
                Invoke(nameof(THI_Transition), 2f);
            }

            //next question
        }
        else
        {
            AS_oops.Play();
        }
    }

    public void THI_Wrong()
    {
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;

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

    public void THI_CoinCollect()
    {
        AS_collecting.Play();
        I_Points += 1;
        TEX_points.text = I_Points.ToString();
        TM_pointFx.text = "+1 points";
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
            List<string> STRL_Passagedetails = new List<string>();
            MyJSON json = new MyJSON();
            //json.Helitemp(www.downloadHandler.text);
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_cover_img_link, STRL_Passagedetails);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            I_optionCount = IL_numbers[3];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            AD_questionAllocated = new AllocatedQuestions[IL_numbers[0]];
            THI_AllocateQuestion();

            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());
            StartCoroutine(IMG_Question());
            StartCoroutine(IMG_Option());
        }
    }

    IEnumerator IMG_Option()
    {

        SPRA_Options = new Sprite[STRL_options.Count];
        // yield return new WaitForSeconds(0.01f);
        //  Q_Img();

        for (int i = 0; i < STRL_options.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_options[i]);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D downloadedTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                SPRA_Options[i] = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                string[] Names = (STRL_options[i].Split('/'));
                string[] Finalname = (Names[Names.Length - 1].Split('.'));

                SPRA_Options[i].name = Finalname[0];
            }
        }
    }

    IEnumerator IMG_Question()
    {

        SPRA_Questions = new Sprite[STRL_questions.Count];
        // yield return new WaitForSeconds(0.01f);
        //  Q_Img();

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

   
  /*public async void Q_Img()
    {
       // var tasks = new Task[STRL_questions.Count];
        for (int i = 0; i < STRL_questions.Count; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(STRL_questions[i]);

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
                await Task.CompletedTask;
            }
        }       
    }*/


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
            TEXM_instruction.text  = STR_instruction;
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
        StartCoroutine(IMG_Question());
        StartCoroutine(IMG_Option());

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
        StopAllCoroutines();
        Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STR_instruction;
        TEXM_instruction.gameObject.AddComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
        if(I_currentQuestionCount==-1)
        {
            G_Player.SetActive(true);
            Frog_Follow.OBJ_followingCamera.B_canfollow = true;
            // THI_CloneLevel();
            
        }
    }

    void THI_AllocateQuestion(){
        float percent;
        int questionCount = 0;
        while(questionCount < AD_questionAllocated.Length){
            Debug.Log("in while loop...!!");
            for(int i=0; i < GA_mushrooms.Length; i++){
                Debug.Log("In for loop..@@");
                percent = Random.Range(1, 100);
                if(percent >= F_quesAllocPer){
                    // Debug.Log(GA_mushrooms[i].gameObject.name, GA_mushrooms[i].gameObject);
                    AD_questionAllocated[questionCount] = new AllocatedQuestions();

                    AD_questionAllocated[questionCount].selectedMushroom = GA_mushrooms[i].gameObject;

                    for(int j=0; j < GA_mushrooms[i].gameObject.transform.childCount; j++){
                        // Debug.Log("Mushroom Name : "+GA_mushrooms[i].gameObject.transform.GetChild(j).gameObject.name, GA_mushrooms[i].gameObject.transform.GetChild(j).gameObject);

                        AD_questionAllocated[questionCount].mushrooms.Add(GA_mushrooms[i].gameObject.transform.GetChild(j).gameObject.GetComponent<Mushroom>());
                        GA_mushrooms[i].gameObject.transform.GetChild(j).gameObject.GetComponent<Mushroom>().B_questionALlocated = true;
                    }
                    AD_questionAllocated[questionCount].I_questionID = questionCount;
                    questionCount++;
                    // Debug.Log("Question Count : "+questionCount);
                    // Debug.Log("Current Ques Count : "+I_currentQuestionCount);
                    if(!(questionCount < AD_questionAllocated.Length))
                        break;
                }
            }
        }
        Debug.Log("Loop exited...");
    }

    public void THI_DeAllocateQuestion(GameObject gameObjectInstanceID){
        // Debug.Log("Jumped Mushroom : "+gameObjectInstanceID.gameObject.name+" "+gameObjectInstanceID.gameObject.GetInstanceID(), gameObjectInstanceID.gameObject);
        for(int i=0; i < AD_questionAllocated.Length; i++){
            // Debug.Log("Allocated Instance ID : "+AD_questionAllocated[i].selectedMushroom.GetInstanceID(), AD_questionAllocated[i].selectedMushroom);
            if(AD_questionAllocated[i].selectedMushroom.GetInstanceID() == gameObjectInstanceID.GetInstanceID()){
                for(int j=0; j < AD_questionAllocated[i].mushrooms.Count; j++){
                    AD_questionAllocated[i].mushrooms[j].B_questionALlocated = false;
                }
                break;
            }
        }
    }
}

[System.Serializable]
public class AllocatedQuestions{
    public int I_questionID;
    public GameObject selectedMushroom;
    public List<Mushroom> mushrooms;
    public AllocatedQuestions(){
        Debug.Log("Object instantiated");
        mushrooms = new List<Mushroom>();
    }
}