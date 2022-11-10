using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Main_trainsorting : MonoBehaviour
{
    public static Main_trainsorting OBJ_Main_trainsorting;

    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Game;
    public GameObject G_Demo;
    public GameObject G_SkipButton;
    bool B_CloseDemo;
    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public bool B_MoveBG;
    public bool B_Spawn;
    public GameObject G_Option_SpawnPoint;
    public GameObject G_Plane,G_Train; 
    public GameObject[] GA_Trains;
   // public GameObject
    GameObject G_Dummy;
    public GameObject G_instructionPage;
    bool B_Answerhighlight;
   // public GameObject G_Demo;
   // public AnimationClip Demo_animclip;
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_Cover_Image;


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

        OBJ_Main_trainsorting = this;

        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_2.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
            URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
            SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

           // URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_2.php"; // UAT FETCH DATA
           // SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
   
    // Start is called before the first frame update
    void Start()
    {
        B_CloseDemo = true;
        G_levelComplete.SetActive(false);
        G_Transition.SetActive(false);
        G_instructionPage.SetActive(false);

        for(int i=0;i<GA_Trains.Length;i++)
        {
            GA_Trains[i].SetActive(false);
        }
       // G_Demo.SetActive(true);
        TEX_points.text = I_Points.ToString();
       
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);
       // Invoke("THI_OffDemo",2f);
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
        //  G_Demo.SetActive(false);
        G_SkipButton.SetActive(false);
       // Debug.Log("DemoOver");
        G_Train.SetActive(true);
        I_currentQuestionCount = -1;
        B_MoveBG = false;
        THI_Transition();
    }

    public void THI_Transition()
    {
        B_Spawn = false;
        G_Transition.SetActive(true);
        Invoke("THI_OffTransition", 5f);
    }

    void THI_OffTransition()
    {
        G_Transition.SetActive(false);
        train_engine.OBJ_train_Engine.B_TrainIn = true;
        train_engine.OBJ_train_Engine.speed = 3f;

        THI_NextQuestion();
    }
    private void Update()
    {
        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D Hit = Physics2D.Raycast(worldpoint, Vector2.zero);
            if (Hit.collider != null)
            {
                if (!Main_trainsorting.OBJ_Main_trainsorting.G_instructionPage.activeInHierarchy)
                {
                    if (Hit.collider.name == "TS_Text")
                    {
                        Hit.collider.GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
    }

    public void THI_PlaneStart()
    {
        B_MoveBG = true;
        B_Spawn = true;
        StartCoroutine("SpawnPlane");
    }
    public void THI_NextQuestion()
    {
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            I_currentQuestionCount++;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];

            for (int i = 0; i < STRL_options.Count; i++)
            {
                THI_Changenames(i);
            }

            //Gap between Questions
            Invoke("THI_ShowQuestion",3f);
        }
        else
        {
            MainController.instance.I_TotalPoints = I_Points;
            G_Train.SetActive(false);                                        //off game
            G_levelComplete.SetActive(true);
            StartCoroutine(IN_sendDataToDB());
        }
    }
    void THI_ShowQuestion()
    {
        I_wrongAnsCount = 0;
        B_Answerhighlight = false;
       
       // THI_Changenames(0);
       // THI_Changenames(1);
        train_engine.OBJ_train_Engine.THI_MoveTrain();
    }

    void THI_Changenames(int index)
    {
        G_Train.transform.GetChild(0).transform.GetChild(index).transform.GetChild(0).name = STRL_options[index];
        G_Train.transform.GetChild(0).transform.GetChild(index).transform.GetChild(0).transform.GetChild(0)
           .GetComponent<TextMesh>().color = Color.black;
        G_Train.transform.GetChild(0).transform.GetChild(index).transform.GetChild(0).transform.GetChild(0)
            .GetComponent<TextMesh>().text = STRL_options[index];
        G_Train.transform.GetChild(0).transform.GetChild(index).transform.GetChild(0).transform.GetChild(0)
           .GetComponent<AudioSource>().clip = ACA_optionClips[index];
    }

    IEnumerator SpawnPlane()
    {
        do
        {
            G_Dummy = Instantiate(G_Plane);
            G_Dummy.transform.SetParent(G_Option_SpawnPoint.transform, false);
            G_Dummy.transform.position = G_Option_SpawnPoint.transform.position;
           

           // int k = Random.Range(0, STRL_questions.Count);
            G_Dummy.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMesh>().text = STRL_questions[I_currentQuestionCount];
            G_Dummy.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            Invoke(nameof(PlayQAudio), 2f);

            if (B_Answerhighlight)
            {
                G_Dummy.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
            }

            float Speed = Random.Range(2, 3);
            G_Dummy.GetComponent<sorting_plane>().F_Speed = Speed;

            float Movement = Random.Range(0.3f, 0.8f);
            G_Dummy.GetComponent<sorting_plane>().F_Movement = Movement;

            yield return new WaitForSeconds(8f);
        } while (B_Spawn);
       
    }


    void PlayQAudio()
    {
        G_Dummy.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
   public void THI_Correct()
    {
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
        AS_Correct.Play();
        B_MoveBG = false;
        train_engine.OBJ_train_Engine.B_TrainIn = true;
        train_engine.OBJ_train_Engine.speed = 5f;
        B_Spawn = false;
        StopAllCoroutines();
        THI_TrackGameData("1");
   }
    void THI_wrongmovenext()
    {
        if(G_Dummy!=null)
        {
            Destroy(G_Dummy);
        }
        B_Spawn = false;
        train_engine.OBJ_train_Engine.B_TrainIn = true;
    }
    public void THI_Wrong()
    {
        AS_Wrong.Play();
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;
        if (I_wrongAnsCount == 3)
        {
            if (STR_difficulty == "assistive")
            {
                for (int i = 0; i < G_Train.transform.GetChild(0).transform.childCount; i++)
                {
                    if (G_Train.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(0)
            .GetComponent<TextMesh>().text == STRL_answers[I_currentQuestionCount])
                    {
                        G_Train.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(0)
            .GetComponent<TextMesh>().color = Color.green;

                    }
                }
                if(!B_Answerhighlight)
                {
                    if (G_Dummy != null)
                    {
                        Destroy(G_Dummy);
                    }
                    B_Answerhighlight = true;
                    Invoke("THI_wrongmovenext", 10f);
                }
                
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
                for (int i = 0; i < G_Train.transform.GetChild(0).transform.childCount; i++)
                {
                    if (G_Train.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(0)
            .GetComponent<TextMesh>().text == STRL_answers[I_currentQuestionCount])
                    {
                        G_Train.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).transform.GetChild(0)
            .GetComponent<TextMesh>().color = Color.green;
                    }
                }
                if (!B_Answerhighlight)
                {
                    B_Answerhighlight = true;
                }
                  
                //after click next question
            }
            
        }
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                if (G_Dummy != null)
                {
                    Destroy(G_Dummy);
                }
                B_Spawn = false;
                
                THI_wrongmovenext();
               // Invoke("THI_Transition", 5f);
                //next question
            }
        }

        if (I_Points > I_wrongPoints)
        {
            I_Points -= I_wrongPoints;
        }
        else
        {
            if (I_Points < 0)
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
                G_Cover_Image.GetComponent<Image>().sprite = Sprite.Create(downloadedTexture, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
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
            json.Temp_type_1(www.downloadHandler.text, IL_numbers, STRL_difficulty,STRL_instruction, STRL_BG_img_link, STRL_instructionAudio, STRL_questions, 
                STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Img_link, STRL_passageDetail);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints= IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;



            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());

            if (STRL_options.Count == 2)
            {
                G_Train = GA_Trains[0];
               // Debug.Log("Opt 2");
            }
            if (STRL_options.Count == 3)
            {
                G_Train = GA_Trains[1];
            }
            if (STRL_options.Count == 4)
            {
                G_Train = GA_Trains[2];
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
                STRL_answers, STRL_quesitonAudios, STRL_questionID, STRL_options, STRL_optionAudios, STRL_avatar_Color, STRL_Panel_Img_link, STRL_Cover_Img_link,STRL_passageDetail);
        
        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;
       
        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());

        if (STRL_options.Count == 2)
        {
            G_Train = GA_Trains[0];
          //  Debug.Log("Opt 2");
        }
        if (STRL_options.Count == 3)
        {
            G_Train = GA_Trains[1];
        }
        if (STRL_options.Count == 4)
        {
            G_Train = GA_Trains[2];
        }

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
