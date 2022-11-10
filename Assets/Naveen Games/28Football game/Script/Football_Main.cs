using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
public class Football_Main : MonoBehaviour
{
    public static Football_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public Camera Cam;
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
    public Sprite SPR_Catch;
    public Sprite [] SPR_L, SPR_R;
    bool B_Goal;
    

    [Header("Objects")]
    public GameObject G_Question;
    public GameObject G_Options;
    public GameObject[] GA_Options;
    public GameObject G_Ball, G_Keeper;
    Vector3 V3_Ball;
    Vector3 V3_keeper;
    Vector3 V3_BallIntPos;
    public Transform[] LT3,RT3;
    public GameObject G_Goal;
    public GameObject G_BallIndicator;
    GameObject G_Highlight;
   // public Sprite SPR_Catched;
    // public List<string> Lstr_ans, Lstr_wrng;
    // public List<AudioClip> AC_ans, AC_wrg;
    bool B_CanClick;
    bool B_CallOnce;
    bool B_CatchSlerp;
    bool B_GoalSlerp;
    int index;
    float timeCount;
    public GameObject G_Kicker;


    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Counter, I_Dummmy;
    public string[] STRA_AnsList;
    public int I_GoalTry;
    


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_collecting;
    public AudioSource AS_oops;
    public AudioSource AS_crtans;
    public AudioSource AS_Wrong3;
    public AudioSource AS_Horn;
    public AudioSource AS_Goal;
    public AudioSource AS_BGM;
    public AudioSource AS_Kick;

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
            URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
            SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }
    void Start()
    {
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
      //  G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = false;
        V3_BallIntPos = G_Ball.transform.position;
        B_CloseDemo = true;
        B_CallOnce = true;
        G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);
        G_Kicker.SetActive(false);
        G_instructionPage.SetActive(false);

        TEX_points.text = I_Points.ToString();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);

        I_currentQuestionCount = -1;
        I_Dummmy = 0;
        I_Counter = 0;
    }
    private void Update()
    {
        if(!G_Demo.activeInHierarchy && B_CloseDemo)
        {
            B_CloseDemo = false;
            DemoOver();
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() )
        {
            Vector2 worldpoint = Cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D Hit = Physics2D.Raycast(worldpoint, Vector2.zero);
            if (Hit.collider != null)
            {
                if (!Football_Main.Instance.G_instructionPage.activeInHierarchy)
                {
                    if (Hit.collider.name == "Ball")
                    {
                        if (B_CallOnce)
                        {
                           // G_Ball.transform.GetChild(1).gameObject.SetActive(false);
                            B_CallOnce = false;
                            G_Goal.transform.GetChild(G_Goal.transform.childCount - 1).gameObject.GetComponent<Collider2D>().enabled = false;
                            //  V3_Ball = G_Ball.transform.GetChild(0).transform.position;
                            //Debug.Log("Ball = " + V3_Ball);

                              G_Keeper.GetComponent<Animator>().enabled = false;
                              G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                              G_Ball.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            //  G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = false;
                              V3_keeper = G_Keeper.transform.position;


                            // float distance = Vector3.Distance(V3_Ball, V3_keeper);
                          //  Debug.Log("Rotation =" + G_Ball.transform.GetChild(0).transform.rotation.z);
                           
                            if(G_Ball.transform.GetChild(0).transform.rotation.z<0.75)
                            {
                               // Debug.Log("ball less than 90");
                                if(V3_keeper.x > 0)
                                {
                                  //  Debug.Log("hit keeper catch 90");
                                    B_Goal = false;
                                }
                                else
                                {
                                   // Debug.Log("hit goal  90");
                                    B_Goal = true;
                                }

                            }
                            if (G_Ball.transform.GetChild(0).transform.rotation.z> 0.75)
                            {
                              //  Debug.Log("ball greater than 90");
                                if (V3_keeper.x > 0)
                                {
                                   // Debug.Log("hit goal greater 90");
                                    B_Goal = true;
                                }
                                else
                                {
                                  //  Debug.Log("hit keeper catch greater 90");
                                    B_Goal = false;
                                }
                            }



                           // if (distance > 4.5)
                            if(B_Goal)
                            {

                                G_Ball.GetComponent<SpriteRenderer>().sortingOrder = 3;
                                Vector3 temp = G_Ball.transform.position;
                                temp.x += 2;
                                temp.y += 1.5f;
                                
                                G_Kicker.transform.position = temp;
                                G_Kicker.SetActive(true);
                                G_Kicker.GetComponent<SpriteRenderer>().enabled = true;
                                 Debug.Log("Goal");
                                Invoke(nameof(THI_GoalHIt), 2f);
                            }
                            else
                            {
                                G_Ball.GetComponent<SpriteRenderer>().sortingOrder = 5;
                                Vector3 temp = G_Ball.transform.position;
                                temp.x += 2;
                                temp.y += 1.5f;
                                Debug.Log("Catched");
                                G_Kicker.transform.position = temp;
                                G_Kicker.SetActive(true);
                                G_Kicker.GetComponent<SpriteRenderer>().enabled = true;
                                Invoke("THI_KIck", 2f);
                            }
                        }
                    }
                }
            }
        }
        

        if (B_GoalSlerp)
        {
            G_Ball.GetComponent<TrailRenderer>().enabled = true;
            
            if(V3_keeper.x>0)
            {
                index = Random.Range(0, LT3.Length);
                G_Ball.transform.position = Vector3.Slerp(G_Ball.transform.position, LT3[index].position, 2f * Time.deltaTime);
                G_Ball.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                Quaternion Target = Quaternion.Euler(0, 0, 360);
                G_Ball.transform.rotation = Quaternion.Slerp(G_Ball.transform.rotation, LT3[index].rotation, timeCount);
                timeCount = timeCount + Time.deltaTime * 0.1f;
            }
            else
            {
                index = Random.Range(0, RT3.Length);
                G_Ball.transform.position = Vector3.Slerp(G_Ball.transform.position, RT3[index].position, 2f * Time.deltaTime);
                G_Ball.transform.localScale = new Vector3(0.2f, 0.2f, 0);
               
                G_Ball.transform.rotation = Quaternion.Slerp(G_Ball.transform.rotation, RT3[index].rotation, timeCount);
                timeCount = timeCount + Time.deltaTime*0.1f;
            }
             // Invoke(nameof(THI_Goal), 3f);
           // B_GoalSlerp = false;
        }

        if (B_CatchSlerp)
        {
            G_Ball.GetComponent<TrailRenderer>().enabled = true;
            G_Ball.transform.position = Vector3.Slerp(G_Ball.transform.position, G_Keeper.transform.GetChild(0).transform.position, 2f * Time.deltaTime);
            G_Ball.transform.localScale = new Vector3(0.2f, 0.2f, 0);
           // Quaternion Target = Quaternion.Euler(0, 0, 360);
            G_Ball.transform.rotation = Quaternion.Slerp(G_Ball.transform.rotation, G_Keeper.transform.GetChild(0).transform.rotation, Time.deltaTime * 4f);
           
        }
    }

    IEnumerator SpriteChange(bool Left)
    {
        Vector3 temp = V3_keeper;
        
       
        for (int i=0;i<2;i++)
        {
            if(Left)
            {
                temp.x -= 2;
                temp.y -= 0.5f;
                G_Keeper.GetComponent<SpriteRenderer>().sprite = SPR_L[i];
                G_Keeper.transform.position = temp;
            }
            else
            {
                temp.x += 2;
                temp.y -= 0.5f;
                G_Keeper.GetComponent<SpriteRenderer>().sprite = SPR_R[i];
                G_Keeper.transform.position = temp;
            }
            
            yield return new WaitForSeconds(1f);
        }
    }
    void THI_GoalHIt()
    {
        AS_Kick.Play();
        AS_Goal.Play();
       // G_Kicker.SetActive(false);
        if (V3_keeper.x > 0)
        { StartCoroutine(SpriteChange(true)); }
        else
        {
            StartCoroutine(SpriteChange(false));
        }
        B_GoalSlerp = true;
        Invoke(nameof(THI_Goal), 2f);
    }
    void THI_KIck()
    {
        
        B_CatchSlerp = true;
        
        //Debug.Log("Missed");
        Invoke(nameof(THI_BallGravity), 3f);
        Invoke(nameof(THI_Missed), 3f);
    }

    void THI_Goal()
    {
        G_Kicker.SetActive(false);
        AS_Horn.Play();
        THI_Collect_Out(true);
        //  Debug.Log("Goal hit");
        B_GoalSlerp = false;
        B_CallOnce = false;
        G_Goal.transform.GetChild(G_Goal.transform.childCount - 1).gameObject.GetComponent<Collider2D>().enabled = true;
        G_Ball.GetComponent<Rigidbody2D>().gravityScale = 1f;
        G_BallIndicator.transform.GetChild(I_GoalTry).GetComponent<Image>().color = Color.green;
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false; 
       // G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = false; 
        
        Invoke(nameof(THI_Transition), 1f);

    }
    void THI_Missed()
    {
        B_CatchSlerp = false;
        G_Keeper.GetComponent<SpriteRenderer>().sprite = SPR_Catch;
        G_BallIndicator.transform.GetChild(I_GoalTry).GetComponent<Image>().color = Color.red;
        THI_Collect_Out(false);
        //G_Keeper.GetComponent<Animator>().Play("Aftercatch");
        // G_Keeper.GetComponent<Animator>().enabled = true;
        // G_Keeper.transform.position = V3_keeper;
      


        I_GoalTry++;
       // Debug.Log("again Missed = "+ I_GoalTry);
        if (I_GoalTry==3)
        {
            G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            THI_Transition();
        }
    }
    void THI_BallGravity()
    {
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        G_Kicker.SetActive(false);
        G_Goal.transform.GetChild(G_Goal.transform.childCount-1).gameObject.GetComponent<Collider2D>().enabled = true;
        B_GoalSlerp = B_CatchSlerp = false;
        G_Ball.GetComponent<Rigidbody2D>().gravityScale = 1f;
        Invoke(nameof(THI_NextGoal), 1f);
    }
    void THI_NextGoal()
    {
        G_Ball.transform.position = V3_BallIntPos;
        B_CallOnce = true;
        G_Ball.transform.rotation = Quaternion.EulerAngles(0, 0, 0);
        G_Ball.GetComponent<Rigidbody2D>().gravityScale = 0f;
        G_Ball.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        G_Keeper.GetComponent<Animator>().enabled = true;
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        G_Ball.GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    public void THI_Check()
    {
        if (B_CanClick)
        {
            // GameObject G_Selected = ;
            STR_currentSelectedAnswer = EventSystem.current.currentSelectedGameObject.name;


            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                B_CanClick = false;
                B_CallOnce = true;
                THI_Correct();
            }
            else { THI_Wrong(); }
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
    }
    void THI_Transition()
    {

        G_Question.SetActive(false);
        G_Transition.SetActive(true);
        G_Ball.transform.rotation = Quaternion.EulerAngles(0, 0, 0);
        G_Ball.GetComponent<Rigidbody2D>().gravityScale = 0f;
        G_Ball.transform.position = V3_BallIntPos;
        G_Keeper.GetComponent<Animator>().enabled = true;
        
        Invoke(nameof(THI_NextQuestion), 2f);
    }

    public void THI_ShowQuestion()
    {
        G_BallIndicator.SetActive(false);
        B_CanClick = true;
        G_Question.SetActive(true);
        G_Question.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

   
    public void THI_NextQuestion()
    {
       
        G_Ball.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        G_Ball.GetComponent<TrailRenderer>().enabled = false;
        G_Ball.transform.position = V3_BallIntPos;
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
       // G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = false;
        G_Transition.SetActive(false);
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
           
            I_currentQuestionCount++;


            STRA_AnsList = null;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            G_Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_questions[I_currentQuestionCount];
            G_Question.transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];

            I_Dummmy = I_Counter + IL_numbers[3];

            for (int i = 0; i < G_Options.transform.childCount; i++)
            {
                G_Options.transform.GetChild(i).name = STRL_options[i + I_Counter];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STRL_options[i + I_Counter];
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                G_Options.transform.GetChild(i).GetComponent<AudioSource>().clip = ACA_optionClips[i + I_Counter];
            }
            THI_ShowQuestion();
            G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
           // G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = false;

            I_Counter = I_Counter + IL_numbers[3];

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
        
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = false;
       // G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = false;

        // Release bird animation
        THI_TrackGameData("1");
        THI_GoforShoot();
        // Invoke(nameof(THI_Transition), 3f);
        

    }

    void THI_GoforShoot()
    {
       // G_Ball.transform.GetChild(1).gameObject.SetActive(true);
        G_BallIndicator.SetActive(true);
        for (int i = 0; i < G_BallIndicator.transform.childCount; i++)
        {
            G_BallIndicator.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        I_GoalTry = 0;
        G_Question.SetActive(false);
        G_Ball.transform.GetChild(0).GetComponent<Animator>().enabled = true;
      //  G_Ball.transform.GetChild(1).GetComponent<Animator>().enabled = true;
        G_Ball.GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    IEnumerator Highlight()
    {
        for(int i=0;i<3;i++)
        {
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {
            AS_Wrong3.Play();
            if (STR_difficulty == "assistive")
            {
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).gameObject;
                        StartCoroutine(Highlight());
                    }
                }


                Invoke(nameof(THI_Transition), 8f);
                //Show answer and move to next question
            }
            if (STR_difficulty == "intuitive")
            {
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {

                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).gameObject;
                        StartCoroutine(Highlight());
                    }
                }

               
                //Show answer and after click next question
            }
        }
        else
        if (I_wrongAnsCount == 2)
        {
            AS_Wrong3.Play();
            if (STR_difficulty == "independent")
            {
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
    public void THI_Collect_Out(bool plus)
    {

        if (plus)
        {
            AS_collecting.Play();
            TM_pointFx.text = "+" + 8 + " points";
            I_Points += 8;
        }
        else
        {
            AS_oops.Play();
            if (I_Points > 10)
            {
                TM_pointFx.text = "-" + 3 + " point";
                I_Points -= 3;
            }
            else
            {
                if (I_Points > 0)
                {
                    I_Points = 0;
                }
            }
        }
        TEX_points.text = I_Points.ToString();
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_Wrong()
    {
        Debug.Log("Wrong ans");

        
        THI_pointFxOn(false);
        THI_TrackGameData("0");
        I_wrongAnsCount++;


        if (I_wrongAnsCount == 5)
        {
            Debug.Log("Restart or use coins");
        }
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

            for (int i = 0; i < GA_Options.Length; i++)
            {
                GA_Options[i].SetActive(false);
            }
            if (IL_numbers[3] == 2)
            {
                G_Options = GA_Options[0];
            }
            if (IL_numbers[3] == 3)
            {
                G_Options = GA_Options[1];
            }
           
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
        if (IL_numbers[3] == 2)
        {
            G_Options = GA_Options[0];
        }
        if (IL_numbers[3] == 3)
        {
            G_Options = GA_Options[1];
        }
       
        G_Options.SetActive(true);
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
       
    }
}
