using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

public struct UDPState
{
    public UdpClient socket;
    public IPEndPoint ipEndPoint; 
}

public class Connection : MonoBehaviour
{
    public Text ownerText;
    public Text otherText;

    UdpClient udpClient;
    UDPState udpState;

    public MovementController characterPref;
    public Transform spawnPoint;
    private MovementController myCharacter;
    private List<MovementController> characterList = new List<MovementController>();

    private List<string> keyTemp = new List<string>();
    public string username;
    public Text userText;
 

    private string receiveStr;
    private string sendStr;
    private bool isInit = false;
    private bool isHost;

    IPEndPoint endPointOtherConnection;

    public void CreateHost(int _port)
    {
   

        if (isInit)
            return;

        udpClient = new UdpClient(_port);

        isHost = true;
        isInit = true;

        udpState = new UDPState();
        udpState.socket = udpClient;
        udpState.ipEndPoint = new IPEndPoint(IPAddress.Any, _port);
        udpClient.BeginReceive(Callback, udpState);

        ownerText.text = "";

        CreateCharacter(username);
    }

    public void ConnectToHost(string ip, int port)
    {
        if (isInit)
            return;

        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        udpClient = new UdpClient(AddressFamily.InterNetwork);
        udpClient.Connect(ipEndPoint);
        isHost = false;
        isInit = true;

        udpState = new UDPState();
        udpState.socket = udpClient;
        udpState.ipEndPoint = ipEndPoint;

        SendTextMessage("connect");

        udpClient.BeginReceive(Callback, udpState);

        ownerText.text = "";

        CreateCharacter(username);

        string[] strData = new string[1];
        strData[0] = username;
        SendEvent("createCharacter", strData);

        userText.text = username;
    }

    public void SendTextMessage(string message)
    {
        if (!isInit)
            return;

        byte[] byteSend = Encoding.ASCII.GetBytes(message);

        receiveStr += "\n";

        if(message != "connect")
            sendStr  +=  message  +  "\n";

        if (!isHost)
        {
            udpClient.Send(byteSend, byteSend.Length);
        }
        else
        {
            if (endPointOtherConnection != null)
                udpClient.Send(byteSend, byteSend.Length, endPointOtherConnection);
        }
    }

    private void Update()
    {
        if(otherText.text != receiveStr)
        {
            otherText.text = receiveStr;
        }

        if(ownerText.text != sendStr)
        {
            ownerText.text = sendStr;
        }

        UpdateEvent();
        UpdateCharacter();
    }

    void Callback(IAsyncResult asyncResult)
    {
        UDPState newUDPState = (UDPState)asyncResult.AsyncState;

        byte[] receiveBytes = newUDPState.socket.EndReceive(asyncResult, ref newUDPState.ipEndPoint);

        string convertToString = Encoding.ASCII.GetString(receiveBytes);

        if(isHost && convertToString == "connect" && endPointOtherConnection == null)
        {
            endPointOtherConnection = newUDPState.ipEndPoint;
            string[] strData = new string[1];
            strData[0] = username;
            SendEvent("createCharacter", strData);
        }

        if(convertToString != "")
        {
            keyTemp.Add(convertToString);
        }

        if (convertToString != "connect")
        {
            receiveStr += convertToString + "\n";
            sendStr += "\n";
        }

        udpClient.BeginReceive(Callback, newUDPState);
    }

    void CreateCharacter(string id)
    {
        MovementController newCharacter = Instantiate<MovementController>(characterPref, spawnPoint.position, spawnPoint.rotation);

        newCharacter.id = id;
        newCharacter.isMine = id == username;
        if(newCharacter.isMine)
        {
            myCharacter = newCharacter;
            userText.text = username;
        }

        characterList.Add(newCharacter);

    }

    void SendEvent(string key, string[] values)
    {
        string totalStr = key;

        for(int i = 0; i < values.Length; i++)
        {
            totalStr += "|" + values[i];
        }

        byte[] byteSend = Encoding.ASCII.GetBytes(totalStr);

        if (isHost)
        {
            if (endPointOtherConnection == null)
                return;

            udpClient.Send(byteSend, byteSend.Length, endPointOtherConnection);
        }
        else
        {
            udpClient.Send(byteSend, byteSend.Length);
        }
    }

    void UpdateEvent()
    {
        for (int i = 0; i < keyTemp.Count; i++)
        {
            string[] splitStr = keyTemp[i].Split('|');

            if(splitStr[0] == "createCharacter")
            {
                CreateCharacter(splitStr[1]);
                keyTemp.RemoveAt(i);
                i = 0;
            }
            else if(splitStr[0] == "UpdatePos")
            {
                string _id = splitStr[1];
                Vector3 _pos;
                _pos.x = float.Parse(splitStr[2]);
                _pos.y = float.Parse(splitStr[3]);
                _pos.z = float.Parse(splitStr[4]);

               for(int j = 0; j < characterList.Count; j++)
                {
                    if(characterList[i].id == _id)
                    {
                        characterList[i].transform.position = _pos;
                    }
                }

                keyTemp.RemoveAt(i);
                i = 0;
            }
        }
    }

    void UpdateCharacter()
    {
        if (!isInit || !myCharacter)
            return;

        Vector3 pos = myCharacter.transform.position;

        string[] strData = new string[4];
        strData[0] = myCharacter.id;
        strData[1] = pos.x.ToString();
        strData[2] = pos.y.ToString();
        strData[3] = pos.z.ToString();

        SendEvent("UpdatePos", strData);
    } 
}
