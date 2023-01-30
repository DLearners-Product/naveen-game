using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Fish_sorting_main : MonoBehaviour
{
    public static Fish_sorting_main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")] 
    public GameObject G_wrgeffect;
    public GameObject G_Answer;
    public GameObject G_Demo;
    bool B_CloseDemo;

    public GameObject G_instructionPage;
    public GameObject[] G_Options;
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_Game;
    int k_dummy;
    GameObject G_Selected;
    public GameObject G_coverPage;
    bool B_CanClick;
    public GameObject[] G_Fishes;
    public GameObject G_FishPrefab;
    public GameObject[] G_FishSpawnpoints;
    public bool B_Fishspawn;
    public GameObject G_Question;
    public GameObject G_hook;
    public GameObject G_BGlayer;
    public GameObject G_BGlayerHz;
    public GameObject G_Bucket;
    public GameObject G_Lerppos;
    public GameObject G_FishingRope;
    public GameObject G_Bear;
    public GameObject G_ClickedFish;
    public bool B_Down, B_Up, B_Lerp, B_Correct, B_Left, B_Right;
    Vector3 tmpPos;
    string animname;
    int I_Attempt;
    public TextMeshProUGUI Tap_Text;
    public float F_Timer;
    public bool B_CanCount;
    public Image IMG_Progress;
    float F_Progess;
    public bool B_CanCatched;
    public bool B_FishClicked;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public string[] STRA_AnsList;
    public int I_Collect_count;
    public int I_NeedtoCollect;


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
    void Start()
    {
        G_Game.SetActive(false);
        B_CloseDemo = true;
        B_Up = B_Down = B_Left = B_Right = false;
        
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);
        G_Answer.SetActive(false);
        G_instructionPage.SetActive(false);
        Tap_Text.gameObject.SetActive(false);
        TEX_points.text = I_Points.ToString();
        G_BGlayer.SetActive(false);
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 0f);

        I_currentQuestionCount = -1;
        I_Dummmy = 0;
        I_Counter = 0;
        I_Attempt = 0;
        

    }

    private void Update()
    {
        if (B_CanCount)
        {

            F_Timer += 1 * Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            F_Timer = 0;
            B_CanCount = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            B_CanCount = true;
        }

        if (F_Timer > 60)   //60 secs
        {
            F_Timer = 0;
            G_Question.GetComponent<AudioSource>().Play();
        }




        if (!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

        if (B_Fishspawn && B_CanCatched)
        {
         
            /*
            if (B_Down)
            {
                //Debug.Log("Move Up");
                G_hook.transform.Translate(Vector3.down * 10f * Time.deltaTime);
                // G_FishingRope.GetComponent<Rope_sim>().segmentLength--;
                B_Up = false;
                //Debug.Log("Move Down " + B_Up + "  " + G_hook.transform.position);

            }
             if (B_Up)
             {
                 //Debug.Log("Move Up");
                 G_hook.transform.Translate(Vector3.up * 10f * Time.deltaTime);
                 // G_FishingRope.GetComponent<Rope_sim>().segmentLength--;
                 B_Up = false;
                 //Debug.Log("Move Down " + B_Up + "  " + G_hook.transform.position);

             }
             if (B_Left)
             {
                 //Debug.Log("Move Left");
                 G_hook.transform.Translate(Vector3.left * 5f * Time.deltaTime);
                 B_Left = false;
             }
             if (B_Right)
             {
                 //Debug.Log("Move Right");
                 G_hook.transform.Translate(Vector3.right * 5f * Time.deltaTime);
                 B_Right = false;
             }*/
        }

       
          /*  if(Input.GetKey(KeyCode.UpArrow))
            {
                B_Up = true;
            }
            if(Input.GetKey(KeyCode.DownArrow))
            {
                B_Down = true;
            }
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                B_Left = true;
            }
           if (Input.GetKey(KeyCode.RightArrow))
           {
                B_Right = true;
           }*/
      
        



        if (B_Lerp)
        {
            
            G_hook.transform.position = Vector3.Lerp(G_hook.transform.position, G_Lerppos.transform.position, 0.01f);
            // G_hook.transform.GetChild(0).transform.rotation;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (B_CanClick)
            {
                
                OffLerp();
                B_CanClick = false;
            }
        }
    }

    private void FixedUpdate()
    {
        THI_CatchFish();
    }

    public void THI_CatchFish(){
        bool fishCatchCheck = (G_ClickedFish != null && B_Fishspawn && B_CanCatched);

        if(G_ClickedFish == null)
            B_FishClicked = false;

        if(fishCatchCheck && (Vector3.Distance(transform.position, G_ClickedFish.transform.position) > 1f))
        {
            Debug.Log($"Going towards fish : "+G_ClickedFish.name, G_ClickedFish);
            G_hook.transform.position = Vector3.MoveTowards(G_hook.transform.position, G_ClickedFish.transform.position, 10 * Time.deltaTime);
            // yield return new WaitForSeconds(0.1f);
            // G_FishingRope.GetComponent<Rope_sim>().segmentLength++;
            //B_Down = false;
            //Debug.Log("Move Down "+ B_Down + "  "+G_hook.transform.position);
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
        G_Game.SetActive(true);
        THI_Transition();
        B_Fishspawn = true;
        StartCoroutine(SpawnFish());
       // B_CanCatched = true;
    }
    void THI_Transition()
    {
        G_BGlayer.SetActive(false);
        G_Transition.SetActive(true);
        Invoke(nameof(THI_NextQuestion), 2f);
    }

    public void THI_Catched()
    {
        B_CanCatched = false;
        // Debug.Log("B_CanCatched " + B_CanCatched+ G_hook.transform.position);
        B_FishClicked = false;
        StopCoroutine(SpawnFish());
        B_Lerp = true;
        for (int i=0;i<STRA_AnsList.Length;i++)
        {
            if (STRA_AnsList[i]==STR_currentSelectedAnswer)
            {
                B_Correct = true;
            }
        }

        Invoke(nameof(THI_LateClick), 2f);

        
        
       // Invoke("OffLerp", 2f);        
    }
    
  
    void THI_LateClick()
    {
        Tap_Text.gameObject.SetActive(true);
        B_CanClick = true;
    }
    void OffLerp()
    {
        Tap_Text.gameObject.SetActive(false);
        B_Lerp = false;
        if (B_Correct)
        {
            THI_Correct();
            G_Bear.GetComponent<Animator>().Play("bear_anim");
            THI_CatchAnim();
            // animname = "Fish_catched";
            I_Collect_count++;

            IMG_Progress.fillAmount = (float)I_Collect_count / I_NeedtoCollect;
        }
        else
        {
           // Debug.Log("Wrong ans");
            THI_Wrong();
            // animname = "Fish_Escaped";
           // I_wrongAnsCount++;
        }
    }

    

    void THI_CatchAnim()
    {
       // B_Fishspawn = true;
        G_BGlayer.GetComponent<Collider2D>().enabled = false;
        G_hook.GetComponent<Animator>().enabled = true;
        G_hook.GetComponent<Animator>().Play("Fish_catched");
        G_hook.transform.parent.GetComponent<Animator>().Play("bear_anim");
        Invoke("THI_Bucketanim", 4f);
    }
    void THI_Bucketanim()
    {
        G_Bucket.GetComponent<Animator>().Play("Bucketfish");
       // B_CanCatched = true;

        if (I_Collect_count == 3)
        {
            THI_Transition();
        }else
        {
            Invoke("THI_Throwhook", 2f);
        }
    }

    void THI_Throwhook()
    {
        if(G_hook.transform.GetChild(0).transform.childCount!=0)
        {
            for (int i = 0; i < G_hook.transform.GetChild(0).transform.childCount;i++)
            {
                Destroy(G_hook.transform.GetChild(0).transform.GetChild(i).gameObject);
            }
               
        }
       // G_hook.GetComponent<Animator>().enabled = true;
        G_hook.GetComponent<Animator>().Play("Throwhook");
       
        if(I_currentQuestionCount>=0)
        {
            G_Bear.GetComponent<Animator>().Play("Catchout");
        }

        

       
        Invoke(nameof(Offanim), 3f);
    }
    void Offanim()
    {
        B_CanCatched = true;
        G_BGlayer.GetComponent<Collider2D>().enabled = true;
        G_hook.GetComponent<Animator>().enabled = false;
        B_Correct = false;
        if(I_currentQuestionCount==0)
      // B_Fishspawn = true;
      //  StartCoroutine(SpawnFish());
        STR_currentSelectedAnswer = "";
        G_ClickedFish = null;
    }

    public void THI_NextQuestion()
    {
        
        G_Transition.SetActive(false);
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            THI_Throwhook();
            I_currentQuestionCount++;
            if (I_currentQuestionCount != 0)
            {
                I_Counter = I_Counter + IL_numbers[3];
            }
            I_Collect_count = 0;
            G_BGlayer.SetActive(true);
            STRA_AnsList = null;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            G_Question.GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            G_Question.GetComponent<AudioSource>().Play();
            I_Dummmy = I_Counter + IL_numbers[3];

            IMG_Progress.fillAmount = 0 / 5;

            I_wrongAnsCount = 0;

            for (int i = 0; i < G_wrgeffect.transform.childCount; i++)
            {
                G_wrgeffect.transform.GetChild(i).gameObject.SetActive(false);
            }
            G_Answer.SetActive(false);
            G_Answer.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The answers are : " + STRL_answers[I_currentQuestionCount];

            STRA_AnsList = STRL_answers[I_currentQuestionCount].Split(',');
            B_CanCount = true;
        }
        else
        {
            THI_Levelcompleted();
            // Invoke(nameof(THI_Levelcompleted), 3f);
        }
    }

    IEnumerator SpawnFish()
    {
        do
        {
            int I_Fish = Random.Range(0, G_Fishes.Length);
            G_FishPrefab = G_Fishes[I_Fish];


            GameObject G_Fish = Instantiate(G_FishPrefab);
            int index = Random.Range(0, G_FishSpawnpoints.Length);
            G_Fish.transform.SetParent(G_FishSpawnpoints[index].transform, false);
            G_Fish.transform.position = G_FishSpawnpoints[index].transform.position;
           

            int I_OptText = Random.Range(I_Counter, I_Dummmy);
            G_Fish.transform.GetChild(0).GetComponent<Text>().text = STRL_options[I_OptText];
            G_Fish.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA_optionClips[I_OptText];
            //G_Fish.transform.localScale = new Vector2(2, 2);
            float Delay = Random.Range(3f, 5f);
            yield return new WaitForSeconds(Delay);

        } while (B_Fishspawn);

       
    }

    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }


    public void THI_Correct()
    {
        B_CanClick = false;
        // B_FishClicked = false;
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
       // I_Collect_count++;
        // Release bird animation
        THI_TrackGameData("1");
     
    }

    IEnumerator Highlight()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
        }
    }

    void THI_WrongEffect()
    {
        G_Answer.SetActive(true);
        G_BGlayer.SetActive(false);

        if (STR_difficulty == "assistive" || STR_difficulty == "independent")
        {
            Invoke(nameof(THI_Transition), 5f);

            //Show answer and move to next question
        }
        if (STR_difficulty == "intuitive")
        {
            I_Attempt++;
            I_wrongAnsCount = I_wrongAnsCount - 3;

            for (int i = 0; i < G_wrgeffect.transform.childCount; i++)
            {
                G_wrgeffect.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < I_wrongAnsCount; i++)
            {
                G_wrgeffect.transform.GetChild(i).gameObject.SetActive(true);
            }
            if (I_Attempt == 2)
            {
                Invoke(nameof(THI_Transition), 5f);

            }
            else
            {
                Invoke(nameof(THI_Continue), 3f);
            }

            //Show answer and after click next question
        }

    }
    void THI_Continue()
    {
      //  G_hook.GetComponent<Animator>().enabled = false;
        G_Answer.SetActive(false);
        G_BGlayer.SetActive(true);
        B_CanCatched = true;
        Invoke(nameof(THI_OFFAnim), 5f);
        //  StartCoroutine(SpawnFish());
    }
    
    void THI_OFFAnim()
    {
        G_hook.GetComponent<Animator>().enabled = false;
    }
    public void THI_Wrong()
    {
        // Debug.Log("Wrong ans");
        // B_CanCatched = true;
        // B_FishClicked = false;
        G_hook.GetComponent<Animator>().enabled = true;
        G_hook.GetComponent<Animator>().Play("Fish_Escaped");
        AS_Wrong.Play();
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;
        G_wrgeffect.transform.GetChild(I_wrongAnsCount-1).gameObject.SetActive(true);

        if (I_wrongAnsCount==I_NeedtoCollect)
        {
            THI_WrongEffect();
        }
        else
        {
            Invoke(nameof(Offanim), 6f);
        }
        

        //REDO the same question

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
        G_FishingRope.SetActive(false);
        G_BGlayer.SetActive(false);
        Time.timeScale = 0;
        G_instructionPage.SetActive(true);
        TEXM_instruction.text = STR_instruction;
        B_CanCount = false;
    }

    public void BUT_closeInstruction()
    {
        G_FishingRope.SetActive(true);
        G_BGlayer.SetActive(true);
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
        B_CanCount = true;
    }
}