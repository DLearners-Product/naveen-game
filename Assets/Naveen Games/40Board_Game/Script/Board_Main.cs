using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
public class Board_Main : MonoBehaviour
{
    public static Board_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public GameObject G_Game;
    public GameObject G_Demo;
    bool B_CloseDemo;
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public GameObject G_coverPage;
    public GameObject[] G_waypoints;
    public int I_count, I_dummy;
    public GameObject G_display, G_dice;
    public Sprite[] SPR_images;
    public AnimationClip AC_dice;
    public AudioSource AS_button;
    public bool B_canroll;
    public GameObject G_Hero;
    GameObject G_Jumphere;
    public bool B_Slerp;
    public float journeyTime = 1.0f;
    private float startTime;
    public GameObject G_Options;
    public GameObject[] GA_Options;

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
    public AudioSource AS_falling;
    public AudioSource AS_Collect;


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

        Instance = this;


        if (B_production)
        {
            URL = "https://dlearners.in/template_and_games/Game_template_api-s/game_template_1.php"; // PRODUCTION FETCH DATA
            SendValueURL = "https://dlearners.in/template_and_games/Game_template_api-s/save_child_questions.php"; // PRODUCTION SEND DATA

        }
        else
        {
           // URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
           // SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA

             URL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
              SendValueURL = "http://20.120.84.12/Test/template_and_games/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        B_CloseDemo = true;
        G_Hero.GetComponent<Player_Slerp>().enabled = false;
        G_Game.SetActive(false);
        G_levelComplete.SetActive(false);
        G_Transition.SetActive(false);
        G_instructionPage.SetActive(false);
        I_currentQuestionCount = -1;
        G_display.SetActive(false);
        TEX_points.text = I_Points.ToString();

        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

       // I_count = -1;
        I_dummy = 0;
        B_canroll = true;
        startTime = Time.deltaTime;


        for (int i = 0; i < G_waypoints.Length; i++)
        {
            G_waypoints[i].transform.GetChild(0).gameObject.SetActive(false);
            G_waypoints[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        G_waypoints[I_dummy].transform.GetChild(0).gameObject.SetActive(true);
        G_waypoints[I_dummy].transform.GetChild(1).gameObject.SetActive(true);
        G_waypoints[G_waypoints.Length-1].transform.GetChild(1).gameObject.SetActive(true);
    }

    public void BUT_dice()
    {
        if (B_canroll)
        {
          //  I_count++;
            B_canroll = false;
            G_dice.GetComponent<Animator>().enabled = true;
            G_dice.GetComponent<Animator>().Play("dice");
            // for (int i = 0; i < G_display.transform.childCount; i++)
            // {
            //     G_display.transform.GetChild(i).gameObject.SetActive(false);
            // }

            G_display.gameObject.SetActive(false);
           
            if (I_dummy == 17)
            {
                I_count = 3;
                Debug.Log(I_dummy + " =" + I_count);
            }
            else
            if (I_dummy == 18)
            {
                I_count = 2;
                Debug.Log(I_dummy + " = "+I_count);
            }
            else
            if (I_dummy == 19)
            {
                I_count = 1;
                Debug.Log(I_dummy + " = " + I_count);
            }
            else
            if (I_dummy == 20)
            {
                I_count = 0;
                Debug.Log(I_dummy + " = " + I_count);
            }
            else
            {
                I_count = Random.Range(0, 3);
                Debug.Log("I_count = "+I_count);
            }
           
            Debug.Log(I_count+1);
            Invoke("showdice", AC_dice.length);
        }


    }
    public void showdice()
    {
        G_dice.GetComponent<Animator>().enabled = false;

        switch(I_count)
        {
            case 0: G_dice.GetComponent<Image>().sprite = SPR_images[0];
                StartCoroutine(waypoint(1));
                break;
            case 1:
                G_dice.GetComponent<Image>().sprite = SPR_images[1];
                StartCoroutine(waypoint(2));
                break;
            case 2:
                G_dice.GetComponent<Image>().sprite = SPR_images[2];
                StartCoroutine(waypoint(3));
                break;
            case 3:
                G_dice.GetComponent<Image>().sprite = SPR_images[3];
                StartCoroutine(waypoint(4));
                break;
            case 4:
                G_dice.GetComponent<Image>().sprite = SPR_images[4];
                StartCoroutine(waypoint(5));
                break;
            case 5:
                G_dice.GetComponent<Image>().sprite = SPR_images[5];
                StartCoroutine(waypoint(6));
                break;
            
        }

    }
    public IEnumerator waypoint(int index)
    {
        // yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < index; i++)
        {
            B_Slerp = false;
            yield return new WaitForSeconds(0.5f);

            AS_button.Play();
          //  Debug.Log("Selected =" + I_dummy);

            G_waypoints[I_dummy].transform.GetChild(0).gameObject.SetActive(false);
           // G_Hero.GetComponent<Player_Slerp>().sunrise = G_waypoints[I_dummy].transform;
           
            I_dummy++;

            G_Hero.GetComponent<Player_Slerp>().sunset = G_waypoints[I_dummy].transform;
            yield return new WaitForSeconds(0.25f);
            G_Hero.GetComponent<Player_Slerp>().enabled = true;

            if (I_dummy > 7) { G_Hero.transform.localScale = new Vector3(-1, 1, 1); }
            if (I_dummy > 14) { G_Hero.transform.localScale = new Vector3(1, 1, 1); }
           // B_Slerp = true;
            yield return new WaitForSeconds(1.25f);
            G_waypoints[I_dummy].transform.GetChild(0).gameObject.SetActive(true);
            G_Hero.GetComponent<Player_Slerp>().enabled = false;
        }
       
        if(G_waypoints[I_dummy].transform.childCount==3)
        {
            G_waypoints[I_dummy].transform.GetChild(2).gameObject.SetActive(false);
            THI_pointScoreFxOn(true);
        }

        if (I_dummy < 21)
        {
           Invoke(nameof(showposition),1f);
        }
        if(I_dummy==21)
        {
            Debug.Log("Won the match");
        }
      
    }
    public void showposition()
    {
        G_display.SetActive(true);

        I_wrongAnsCount = 0;
        I_currentQuestionCount = I_dummy-1;
        STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
       // int currentquesCount = I_currentQuestionCount + 1;
       // TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
        STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
        Debug.Log(STR_currentQuestionAnswer);


       // G_display.transform.GetChild(0).GetComponent<AudioSource>().clip = G_waypoints[I_dummy+1].GetComponent<AudioSource>().clip;
        G_display.transform.GetChild(0).GetComponent<AudioSource>().clip = G_waypoints[I_dummy].GetComponent<AudioSource>().clip;
        G_display.transform.GetChild(0).GetComponent<AudioSource>().Play();
       
       // G_display.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j-2];
       // G_display.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 1];

        if(IL_numbers[3]==2)
        {
            Debug.Log("2 OPtions");
            int j = I_dummy * 2;
            Debug.Log("OptCount =" + (j - 2));
            Debug.Log("OptCount2 =" + (j - 1));
            G_Options.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 2];
            G_Options.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 1];
        }

        if (IL_numbers[3] == 3)
        {
            Debug.Log("3 OPtions");
            int j = I_dummy * 3;
            Debug.Log("OptCount =" + (j - 3));
            Debug.Log("OptCount1 =" + (j - 2));
            Debug.Log("OptCount2 =" + (j - 1));
            G_Options.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 3];
            G_Options.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 2];
            G_Options.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 1];
        }

        if (IL_numbers[3] == 4)
        {
            Debug.Log("4 OPtions");
            int j = I_dummy * 4;
            Debug.Log("OptCount =" + (j - 4));
            Debug.Log("OptCount =" + (j - 3));
            Debug.Log("OptCount1 =" + (j - 2));
            Debug.Log("OptCount2 =" + (j - 1));
            G_Options.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 4];
            G_Options.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 3];
            G_Options.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 2];
            G_Options.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[j - 1];
        }

        // B_canroll = true;
    }

    public void BUT_Clicking()
    {
        if(!B_canroll)
        {
            STR_currentSelectedAnswer = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
           
            if(STR_currentSelectedAnswer==STR_currentQuestionAnswer)
            {
                THI_Correct();
                Invoke(nameof(THI_OffDisplay),1f);
            }
            else
            {
                THI_Wrong();
               // AS_Wrong.Play();
            }
        }
    }
    void THI_OffDisplay()
    {
        G_display.SetActive(false);
    }
    public void THI_Correct()
    {
        B_canroll = true;
        AS_Correct.Play();
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
        THI_TrackGameData("1");
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
      //  if(B_Slerp)
      //  {
          //   G_Hero.transform.position = Vector3.Slerp(G_Hero.transform.position, G_Jumphere.transform.position, 0.05f);

           /* Vector3 center = (G_Hero.transform.position + G_Jumphere.transform.position) * 0.5f;
            center -= new Vector3(0, 1, 0);

            Vector3 riseRelCenter = G_Hero.transform.position - center;
            Vector3 setRelCenter = G_Jumphere.transform.position - center;

            float fracComplete = (Time.time - startTime) / journeyTime;



            G_Hero.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, 0.05f);
            transform.position += center;*/
       // }
    }
    public void DemoOver()
    {
        G_Game.SetActive(true);
        THI_Transition();
        THI_NextQuestion();
    }

    public void THI_Transition()
    {
        G_Transition.SetActive(true);

       // Invoke(nameof(THI_NextQuestion), 1f);
        Invoke(nameof(THI_OffTransition), 5f);
    }

    void THI_OffTransition()
    {

        G_Transition.SetActive(false);
        
    }

   
    public void THI_NextQuestion()
    {
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            

          for(int i=1;i< G_waypoints.Length-1;i++)
          {
                G_waypoints[i].name = STRL_questions[i-1];
                G_waypoints[i].GetComponent<AudioSource>().clip = ACA__questionClips[i-1];
          }


        }
        else
        {
            MainController.instance.I_TotalPoints = I_Points;
            G_levelComplete.SetActive(true);
            StartCoroutine(IN_sendDataToDB());
        }
    }

   
    public void THI_pointScoreFxOn(bool plus)
    {
        AS_Collect.Play();
        if (plus)
        {
            if (I_correctPoints != 1)
            {
                TM_pointFx.text = "+2 points";
            }
            else
            {
                TM_pointFx.text = "+2 point";
            }
        }
        I_Points += 2;
        TEX_points.text = I_Points.ToString();
        Invoke("THI_pointFxOff", 1f);
    }



    public void THI_Wrong()
    {
        
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;
       
       // AS_falling.Play();
        

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
        THI_WrongEffect();
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 2)
        {
            if (STR_difficulty == "independent")
            {
                B_canroll = true;
                AS_falling.Play();
                Invoke(nameof(THI_OffDisplay), 1f);
            }
        }
        else
        {
            AS_Wrong.Play(); AS_Wrong.Play();
        }
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

            MyJSON json = new MyJSON();
            //json.Helitemp(www.downloadHandler.text);
            List<string> STRL_Passagedetails = new List<string>();
            json.Temp_type_2(www.downloadHandler.text, STRL_difficulty, IL_numbers, STRL_questions, STRL_answers, STRL_options, STRL_questionID, STRL_instruction, STRL_quesitonAudios, STRL_optionAudios,
            STRL_instructionAudio, STRL_Cover_Img_link, STRL_Passagedetails);
            //        Debug.Log("GAME DATA : " + www.downloadHandler.text);

            STR_difficulty = STRL_difficulty[0];

            STR_instruction = STRL_instruction[0];
            MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
            I_wrongPoints = IL_numbers[2];
            MainController.instance.I_TotalQuestions = STRL_questions.Count;

            Debug.Log("Option count -"+IL_numbers[3]);

            StartCoroutine(EN_getAudioClips());
            StartCoroutine(IN_CoverImage());

            if (IL_numbers[3] == 2)
            {
                G_Options = GA_Options[0];
            }
            if (IL_numbers[3] == 3)
            {
                G_Options = GA_Options[1];
            }
            if (IL_numbers[3] == 4)
            {
                G_Options = GA_Options[2];
            }
            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            G_Options.SetActive(true);

        }
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
            STRL_instructionAudio, STRL_Cover_Img_link, STRL_Passagedetails);

        STR_difficulty = STRL_difficulty[0];
        STR_instruction = STRL_instruction[0];
        MainController.instance.I_correctPoints = I_correctPoints = IL_numbers[1];
        I_wrongPoints = IL_numbers[2];
        MainController.instance.I_TotalQuestions = STRL_questions.Count;

        StartCoroutine(EN_getAudioClips());
        StartCoroutine(IN_CoverImage());

        Debug.Log("Option count -" + IL_numbers[3]);

        if (IL_numbers[3]==2)
        {
           G_Options= GA_Options[0];
        }
        if (IL_numbers[3] == 3)
        {
            G_Options = GA_Options[1];
        }
        if (IL_numbers[3] == 4)
        {
            G_Options = GA_Options[2];
        }
        for(int i=0;i<GA_Options.Length;i++)
        {
            GA_Options[i].SetActive(false);
        }
        G_Options.SetActive(true);
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