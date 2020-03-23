using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class GameLog : MonoBehaviour
{
    private string PostLogURL = "yourwebsi.te/GameLog/post.php";
    public string secretKey;    //Key to Game

    //To post, follow this format. 
    //Put whereevery you want in any class, just get the reference to GameLogObject correct
    //string[][] postAttr = new string[2][];
    //postAttr[0] = new string[] { column1, column2, ... };  //Column header
    //postAttr[1] = new string[] { value1, value2, ... };  //Column values
    //GameLogObject('this', if this script is in the same object).GetComponent<GameLog>().LOG("t4b1e1Dxxx", postAttr);

    //We start by just getting the HighScores, this should be removed, when you are done setting up.
    void Start() { }
    private void Update() { }

	bool notNull(string s){return (s!=null&&s!="");}
    public void LOG(string TableID, string[][] Attr) {
        StartCoroutine(PostLog(TableID, Attr));
    }
    //This is where we post
    //allow for posting multiple values. In an array or whatnot
    IEnumerator PostLog(string TableID, string[][] Attr) {
		if(notNull(TableID)&&notNull(secretKey)&&Attr.Length!=0){
			WWWForm form = new WWWForm();
            form.AddField("TableID", TableID);
			form.AddField("Key", secretKey);
            form.AddField("rowSize", Attr[0].Length);
            for (int i = 0; i < Attr[0].Length; i++) {
                form.AddField("Attr[]", Attr[0][i]);
                form.AddField("Val[]", Attr[1][i]);
            }
            using (UnityWebRequest www = UnityWebRequest.Post(PostLogURL, form)) {
				yield return www.SendWebRequest();
				if (www.isNetworkError || www.isHttpError)
					Debug.Log(www.error);
				else
					Debug.Log(www.downloadHandler.text);
			}
		}else{
			Debug.Log("Enter TableID and secretKey to upload!\n");
		}
    }
    //when application closes
    void OnApplicationQuit() {
        Debug.Log("Bye bye! ");
    }

}
