using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class guessNumber : MonoBehaviour
{
    static SocketIOComponent socket;

    public string inputText = "Hello World";
    public string displayText = "";

    public Text textShow;
    public InputField replyInput;
    public InputField nameInput;
    public GameObject warningName;
    public GameObject warningReply;
    public GameObject warningReplyValid;
    public Text lastReplyShow;
    public Text showLDB1;
    public Text showLDB2;
    public Text showLDB3;

    private int countSendReply = 0;
    int refreashLDB = 0;

    // Start is called before the first frame update
    void Start()
    {       
        warningName.SetActive(false);
        warningReply.SetActive(false);
        nameInput.interactable = true;
        warningReplyValid.SetActive(false);
        refreashLDB = 1;

        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        /*socket.On("test",test);*/
        socket.On("startGuess",startGuess);
        socket.On("than",statusThan);
        socket.On("less", statusLess);
        socket.On("winner", statusWinner);        

        socket.On("sendLeaderBoard1",showLeaderBoard1);
        socket.On("sendLeaderBoard2", showLeaderBoard2);
        socket.On("sendLeaderBoard3", showLeaderBoard3);


    }

    void Update()
    {
        if (refreashLDB==1)
        {
            socket.Emit("wantLeaderBoard1");
            socket.Emit("wantLeaderBoard2");
            socket.Emit("wantLeaderBoard3");            
        }
       
    }

    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    private void startGuess(SocketIOEvent e)
    {
        textShow.text = "Let's start Guess!!!";
    }

    private void statusThan(SocketIOEvent e)
    {
        textShow.text = e.data["status"].str;
    }

    private void statusLess(SocketIOEvent e)
    {
        textShow.text = e.data["status"].str;
    }

    private void statusWinner(SocketIOEvent e)
    {
        textShow.text = "The winner is "+e.data["name"].str;
        countSendReply = 0;
    }

    public void resetName()
    {
        nameInput.interactable = true;
        countSendReply = 0;
        replyInput.text = "";
        nameInput.text = "";
        textShow.text = "Let's start Guess!!!";
    }

    private void showLeaderBoard1(SocketIOEvent e)
    {

        showLDB1.text = "Number 1 is " + e.data["name"].str + " With " + e.data["countReply"].ToString()+" reply";
        refreashLDB = 0;
    }

    private void showLeaderBoard2(SocketIOEvent e)
    {

        showLDB2.text = "Number 2 is " + e.data["name"].str + " With " + e.data["countReply"].ToString() + " reply";
        refreashLDB = 0;
    }

    private void showLeaderBoard3(SocketIOEvent e)
    {

        showLDB3.text = "Number 3 is " + e.data["name"].str + " With " + e.data["countReply"].ToString() + " reply";
        refreashLDB = 0;
    }

    public void ClickSendData()//testSendDataByClickButton
    {
        if (nameInput.text=="")
        {
            //still
            warningName.SetActive(true);
        }

        if (nameInput.text != "")
        {
            warningName.SetActive(false);

            if (replyInput.text=="")
            {
                //still
                warningReply.SetActive(true);
            }

            if (replyInput.text != "")
            {

                socket.Emit("wantLeaderBoard1");
                socket.Emit("wantLeaderBoard2");
                socket.Emit("wantLeaderBoard3");

                //still
                warningReply.SetActive(false);
                warningReplyValid.SetActive(false);

                try
                {
                    int checkReply = int.Parse(replyInput.text);

                    countSendReply++;

                    JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
                    jSONObject.AddField("name", nameInput.text);
                    jSONObject.AddField("countReply", countSendReply);
                    jSONObject.AddField("reply", replyInput.text);

                    socket.Emit("reply", jSONObject);
                    Debug.Log("Send Reply....");

                    lastReplyShow.text = replyInput.text;
                    replyInput.text="";

                    nameInput.interactable = false;
                    
                }
                catch
                {
                    warningReplyValid.SetActive(true);
                }
                
            }          
           
        }
        
    }//end void clickSendData



    // Update is called once per frame

}
