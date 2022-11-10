using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Cake_Main : MonoBehaviour
{
    public static Cake_Main Instance;
    public bool B_production;

    [Header("Screens and UI elements")]
    public GameObject G_Demo;
    bool B_CloseDemo;
    public GameObject G_Transition;
    public Text TEX_points;
    public Text TEX_questionCount;
    public TextMeshProUGUI TM_pointFx;

    [Header("Objects")]
    public GameObject G_instructionPage;
    public TextMeshProUGUI TEXM_instruction;
    public TextMeshProUGUI TEXM_instruction_2;
    public GameObject G_Game;
    int k_dummy;
    GameObject G_Selected,G_Clone;
    public Transform T_Pos;
    public GameObject G_Cake;
    public GameObject Prefab_cake;
    public GameObject G_TVScreen;
    public GameObject G_coverPage;
    public GameObject G_Question;
    int I_OptionStart, I_OptionEnd;
    public Sprite[] SPRA_Cream, SPRA_Topping;
    public GameObject[] GA_Options;
    public GameObject G_Options;
    public Button BUTN_Create;
   // public Sprite[] SPR_Entry, SPR_Mistake, SPR_Wrong;
    public GameObject[] G_Customer;
    GameObject G_Highlight;
    public Button[] BT_Buttons;

    [Header("Values")]
    public string STR_currentQuestionAnswer;
    public string STR_currentSelectedAnswer;
    public int I_currentQuestionCount; // question number current
    public string STR_currentQuestionID;
    public int I_Points;
    public int I_wrongAnsCount;
    public int I_Cream , I_Topping;
    public int Rand_Cream , Rand_Topping;
    public int I_CustomerID;
    bool B_CanClick;
    Vector3 CakePrefab_Initialpos;


    [Header("URL")]
    public string URL;
    public string SendValueURL;

    [Header("Audios")]
    public AudioSource AS_Correct;
    public AudioSource AS_Wrong;
    public AudioSource AS_CakeMistake;
    public AudioSource AS_Cancelorder;
    public AudioSource AS_Button;
    public AudioSource AS_Deliverontime;


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
    //public Sprite[] SPRA_Options;
    public Sprite[] SPRA_Questions;
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

          //   URL = "http://103.117.180.121:8000/test/Game_template_api-s/game_template_1.php"; // UAT FETCH DATA
          //   SendValueURL = "http://103.117.180.121:8000/test/Game_template_api-s/save_child_questions.php"; // UAT SEND DATA
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        B_CloseDemo = true;
        // G_Game.SetActive(false);
        G_Transition.SetActive(false);
        G_levelComplete.SetActive(false);
        G_instructionPage.SetActive(false);
        G_TVScreen.SetActive(false);
        TEX_points.text = I_Points.ToString();
        Hide_Customer();
        STRL_questions = new List<string>();
        STRL_answers = new List<string>();
        STRL_options = new List<string>();
        Invoke("THI_gameData", 1f);
        CakePrefab_Initialpos = T_Pos.position;
        I_currentQuestionCount = -1;
      
        I_OptionStart = I_OptionEnd = 0;
    }

    void Hide_Customer()
    {
        for(int i=0;i<G_Customer.Length;i++)
        {
            G_Customer[i].SetActive(false);
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
    void THI_Packcake()
    {
        AS_Deliverontime.Play();
        // G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 1);   //1 reverse
        Destroy(G_Clone.transform.transform.parent.GetChild(0).gameObject);
        G_Clone.transform.transform.parent.GetChild(1).gameObject.SetActive(true);

       
        G_Question.SetActive(false);
        I_Points += I_correctPoints;
        TEX_points.text = I_Points.ToString();
        THI_pointFxOn(true);
       // THI_Createnewcake();
        Invoke(nameof(THI_Createnewcake), 3f);
        Invoke(nameof(INvokeEnd), 3f);
        Invoke(nameof(THI_Transition), 7f);
    }

    void INvokeEnd()
    {
        G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 4);
    }
    void THI_Transition()
    {
        Hide_Customer();
        G_TVScreen.SetActive(false);
        G_Question.SetActive(false);
        G_Transition.SetActive(true);
        I_OptionStart = I_OptionEnd;
        Invoke(nameof(THI_NextQuestion), 2f);
    }

    public void THI_ShowQuestion()
    {
        B_CanClick = true;
        G_Cake.SetActive(false);
        G_Cake.transform.GetChild(1).GetComponent<Image>().enabled = false;
        G_Cake.transform.GetChild(2).GetComponent<Image>().enabled = false;
        G_Question.SetActive(true);

        TEXM_instruction_2.gameObject.GetComponent<AudioSource>().Play();

        Invoke(nameof(PlayQue_Audio), TEXM_instruction_2.gameObject.GetComponent<AudioSource>().clip.length);
        
       // Destroy(G_Clone);
    }

    void PlayQue_Audio()
    {
        Debug.Log("Question audio playing");
        G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
    
    void THI_NextQuestion()
    {

        G_Transition.SetActive(false);
        
        if (I_currentQuestionCount < STRL_questions.Count - 1)
        {
            THI_Createnewcake();
            I_currentQuestionCount++;
            STR_currentQuestionID = STRL_questionID[I_currentQuestionCount];
            int currentquesCount = I_currentQuestionCount + 1;
            TEX_questionCount.text = currentquesCount + "/" + STRL_questions.Count;
            STR_currentQuestionAnswer = STRL_answers[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = SPRA_Questions[I_currentQuestionCount];
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            G_Question.transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>().clip = ACA__questionClips[I_currentQuestionCount];
            StopCoroutine(Highlight());

            STR_currentSelectedAnswer = "";

            I_OptionEnd = I_OptionStart + IL_numbers[3];

            I_wrongAnsCount = 0;

            for(int i=0;i< IL_numbers[3]; i++)
            {
                G_Options.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = STRL_options[i+ I_OptionStart];
                G_Options.transform.GetChild(i).name = STRL_options[i+ I_OptionStart];
            }

            Rand_Cream = Random.Range(0, 2);
            G_TVScreen.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = SPRA_Cream[Rand_Cream];
            Rand_Topping = Random.Range(0, 2);
            G_TVScreen.transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().sprite = SPRA_Topping[Rand_Topping];
            
            BUTN_Create.interactable = false;
           

            I_CustomerID = Random.Range(0, G_Customer.Length);

            G_Customer[I_CustomerID].SetActive(true);
            G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 1);
          
            T_Pos.position = CakePrefab_Initialpos ;


            Invoke(nameof(INvokeTV), 3f);

            G_Cake.SetActive(true);
        }
        else
        {
            THI_Levelcompleted();

        }
    }
   

    void INvokeTV()
    {
        G_TVScreen.SetActive(true);
    }

    public void THI_OptionCollect()
    {   
        if(B_CanClick)
        {
            G_Selected = EventSystem.current.currentSelectedGameObject;

            STR_currentSelectedAnswer = G_Selected.name;
            if (STR_currentSelectedAnswer == STR_currentQuestionAnswer)
            {
                B_CanClick = false;
                // G_Clone.GetComponent<Image>().sprite = SPR_Gift;
               // Debug.Log("Correct");
                THI_Correct();
            }
            else
            {
               
               // Debug.Log("Wrong");
                THI_Wrong();
            }
        }
       
    }

    public void THI_OffQuestion()
    {
        G_Question.SetActive(false);
        for (int i = 0; i < BT_Buttons.Length; i++)
        {
            BT_Buttons[i].interactable = true;
        }
    }


    public void BUT_Create()
    {
        AS_Button.Play();
        for (int i=0;i<BT_Buttons.Length;i++)
        {
            BT_Buttons[i].interactable = false;
        }
        BUTN_Create.interactable = false;
        G_Clone = Instantiate(Prefab_cake);
        G_Clone.transform.SetParent(T_Pos, false);
        G_Clone.transform.position = T_Pos.position;
        G_Clone.transform.GetChild(1).GetComponent<Image>().sprite = SPRA_Cream[I_Cream];
        G_Clone.transform.transform.GetChild(2).GetComponent<Image>().sprite = SPRA_Topping[I_Topping];
        G_Clone.transform.transform.SetAsFirstSibling();
        G_Clone.transform.transform.parent.GetChild(1).gameObject.SetActive(false);
        G_Clone.transform.transform.parent.GetChild(2).gameObject.SetActive(false);
        T_Pos.gameObject.GetComponent<Animator>().Play("Cakemoving");

        if (I_Cream == Rand_Cream && I_Topping == Rand_Topping)
        {
            G_TVScreen.GetComponent<Animator>().SetInteger("Cond", 1);
            Invoke(nameof(THI_ShowQuestion),3f);
           // Debug.Log("Correct cake");
        }
        else
        {
            AS_CakeMistake.Play();
            G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 2);   //2
            Invoke(nameof(OFFCOND), 1f);
            StartCoroutine(wrongcake());
            // dustpin concept
           // Debug.Log("Wrong cake");
        }
        
    }
    IEnumerator wrongcake()
    {
        for(int i=0;i<3;i++)
        {
            G_Clone.transform.transform.parent.GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            G_Clone.transform.transform.parent.GetChild(2).gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        THI_Createnewcake();
    }

    void THI_Createnewcake()
    {
        for (int i = 0; i < BT_Buttons.Length; i++)
        {
            BT_Buttons[i].interactable = true;
        }
        G_Cake.transform.GetChild(1).GetComponent<Image>().enabled = false;
        G_Cake.transform.GetChild(2).GetComponent<Image>().enabled = false;
        if(G_Clone!=null)
        {
            Destroy(G_Clone);
        }

        T_Pos.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        T_Pos.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        T_Pos.gameObject.GetComponent<Animator>().Play("New State");
    }

   public void BUT_Cakecream(int cream)
   {
        AS_Button.Play();
        I_Cream = cream;
        G_Cake.transform.GetChild(1).GetComponent<Image>().sprite = SPRA_Cream[I_Cream];
        G_Cake.transform.GetChild(1).GetComponent<Image>().enabled = true;
        BUTN_Create.interactable = true;
    }
    public void BUT_Caketopping( int topping)
    {
        AS_Button.Play();
        I_Topping = topping;
        G_Cake.transform.GetChild(2).GetComponent<Image>().sprite = SPRA_Topping[I_Topping];
        G_Cake.transform.GetChild(2).GetComponent<Image>().enabled = true;
        BUTN_Create.interactable = true;
    }


    void THI_Levelcompleted()
    {
        MainController.instance.I_TotalPoints = I_Points;
        G_levelComplete.SetActive(true);
        StartCoroutine(IN_sendDataToDB());
    }

    public void THI_Addpoints()
    {
        I_Points += 1;
        TEX_points.text = I_Points.ToString();
        THI_AddpointFxOn(true);
    }
    public void THI_AddpointFxOn(bool plus)
    {
        if (plus)
        {
            if (I_correctPoints != 1)
            {
                TM_pointFx.text = "+" + 1 + " points";
            }
            else
            {
                TM_pointFx.text = "+" + 1 + " point";
            }
        }
        Invoke("THI_pointFxOff", 1f);
    }
    public void THI_Correct()
    {
        // Release bird animation
        THI_TrackGameData("1");

        THI_Packcake();
        // Invoke(nameof(THI_Transition),3f);
      
    }
    IEnumerator Highlight()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            yield return new WaitForSeconds(1f);
            G_Highlight.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }


    }
    void OFFCOND()
    {
        G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 0);
    }

    void assistive()
    {
        G_Question.SetActive(false);
        AS_Cancelorder.Play();
        G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 3);   //3
        Invoke(nameof(OFFCOND), 1f);
        T_Pos.gameObject.GetComponent<Animator>().Play("CakeRejected");
        Invoke(nameof(THI_Transition), 7f);
    }


    void THI_WrongEffect()
    {
        if (I_wrongAnsCount == 3)
        {
            
            // B_CanClick = false;
            if (STR_difficulty == "assistive")
            {
                AS_Cancelorder.Play();
                B_CanClick = false;
                for (int i = 0; i < G_Options.transform.childCount; i++)
                {
                    if (G_Options.transform.GetChild(i).name == STR_currentQuestionAnswer)
                    {
                        G_Highlight = G_Options.transform.GetChild(i).gameObject;
                        StartCoroutine(Highlight());
                    }
                }
                Invoke(nameof(assistive), 5f);
               
                //Show answer and move to next question
            }
            else
            if (STR_difficulty == "intuitive")
            {
                AS_Cancelorder.Play();
                B_CanClick = true;
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
            if (STR_difficulty == "independent")
            {
                AS_Cancelorder.Play();
                B_CanClick = false;
                THI_OffQuestion();
                G_Customer[I_CustomerID].GetComponent<Animator>().SetInteger("Cond", 3);     //3
                Invoke(nameof(OFFCOND), 1f);
                T_Pos.gameObject.GetComponent<Animator>().Play("CakeRejected");
                Invoke(nameof(THI_Transition), 8f);
                //dustbin concept
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
    public IEnumerator IMG_Options()
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
            StartCoroutine(IMG_Options());
            THI_SetOptions();
        }
    }

    void THI_SetOptions()
    {
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        if(IL_numbers[3]==2)
        {
            G_Options = GA_Options[0];
        }
        if (IL_numbers[3] == 3)
        {
            G_Options = GA_Options[1];
        }
        if (IL_numbers[3] ==4)
        {
            G_Options = GA_Options[2];
        }
        G_Options.SetActive(true);
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
            TEXM_instruction.text = TEXM_instruction_2.text= STR_instruction;
            TEXM_instruction.gameObject.AddComponent<AudioSource>();
            TEXM_instruction.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction.gameObject.AddComponent<Button>();
            TEXM_instruction.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);

            TEXM_instruction_2.gameObject.AddComponent<AudioSource>();
            TEXM_instruction_2.gameObject.GetComponent<AudioSource>().playOnAwake = false;
            TEXM_instruction_2.gameObject.GetComponent<AudioSource>().clip = ACA_instructionClips[0];
            TEXM_instruction_2.gameObject.AddComponent<Button>();
            TEXM_instruction_2.gameObject.GetComponent<Button>().onClick.AddListener(THI_playAudio);

        }

        //remove later
        // THI_Transition();
        // DemoOver();
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
        StartCoroutine(IMG_Options());
        THI_SetOptions();
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
        TEXM_instruction.GetComponent<AudioSource>().Play();
    }

    public void BUT_closeInstruction()
    {
        Time.timeScale = 1;
        G_instructionPage.SetActive(false);
    }
}
