using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    //0 =SelectRole
    //1 = InputName
    //2 = Host
    //3 = Client
    //4 = Chat
    //5 = Game

    public GameObject[] panels;

    public Button buttonHost;
    public Button buttonClient;
    public Button buttonNext;
    public Button buttonCreate;
    public Button buttonConnect;
    public Button buttonSend;

    public InputField inputName;
    public InputField inputHostPort;
    public InputField inputClientIP;
    public InputField inputClientPort;
    public InputField inputMessage;

    private bool isHost;
    private string userName;

    private Connection connection;

    // Start is called before the first frame update
    void Start()
    {
        connection = GetComponent<Connection>();

        buttonHost.onClick.AddListener(OnButtonHost);
        buttonClient.onClick.AddListener(OnButtonClient);
        buttonNext.onClick.AddListener(OnButtonNext);
        buttonCreate.onClick.AddListener(OnButtonCreate);
        buttonConnect.onClick.AddListener(OnButtonConnect);
        buttonSend.onClick.AddListener(OnButtonSend);

        OpenPanal(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonHost()
    {
        isHost = true;
        OpenPanal(1);
    }

    void OnButtonClient()
    {
        isHost = false;
        OpenPanal(1);
    }

    void OnButtonNext()
    {
        userName = inputName.text;
        connection.username = userName;

        if(isHost)
        {
            OpenPanal(2);
        }
        else
        {
            OpenPanal(3);
        }
    }

    void OnButtonConnect()
    {
        connection.ConnectToHost(inputClientIP.text, int.Parse(inputClientPort.text));
        //OpenPanal(4);
        OpenPanal(5);
    }

    void OnButtonCreate()
    {
        connection.CreateHost(int.Parse (inputHostPort.text));
        //OpenPanal(4);
        OpenPanal(5);
    }

    void OnButtonSend()
    {
        connection.SendTextMessage(userName  +  ":"  +  inputMessage.text);
    }

    void OpenPanal(int index)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panels[index].SetActive(true);
    }
}
