using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class sendGuessNumber : MonoBehaviour
{
    static SocketIOComponent socket;

    public string inputText = "Hello World";
    public string displayText = "";

    //public Text textshow;


    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);

        socket.On("message.send", OnListenerMessage);
        
        socket.On("winner",OnWinner);
    }

    void Update()
    {
        //socket.On("winner",OnWinner);       
    }

    private void testBC(SocketIOEvent obj)
    {
        Debug.Log("Hey");
        //Debug.Log("serverBroadCast : "+obj.data["name"].str);
        //textshow.text = obj.data.ToString();
    }

    private void OnListenerMessage(SocketIOEvent obj)
    {
        string msg = obj.data["message"].str;

        //displayText += msg + System.Environment.NewLine;

        Debug.Log(msg);
    }


    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    public void ClickSendData()//testSendDataByClickButton
    {
        string playerName = "newlusifer";
        int playerScore = 999;     

        JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
        jSONObject.AddField("name",playerName);
        jSONObject.AddField("score",playerScore);//"name" หรือ "score" คือชื่อ key ของ Json

      //  Debug.Log(jSONObject);              
      // socket.Emit("message.send", jSONObject);
    }

    void OnWinner(SocketIOEvent e)
    {
        //textshow.text = "The winner is...." + e.data.ToString() + "\n" + "Let's start new Guess";
    }



    /* void OnGUI()
     {
         displayText = GUI.TextArea(new Rect(10, 10, 500, 100), displayText, 25);


         inputText = GUI.TextField(new Rect(10, 120, 500, 20), inputText, 25);



         if (GUI.Button(new Rect(10, 160, 50, 30), "Send"))
         {
             JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
             jSONObject.AddField("message", inputText);
             socket.Emit("message.send", jSONObject);


         }



     }*/

}//end
