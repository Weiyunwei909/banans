using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    private TMP_InputField _input;

    private Button _loginButton;

    private Button _desButton;
    // Start is called before the first frame update
    void Start()
    {
        _input = transform.Find("_input").GetComponent<TMP_InputField>();
        _loginButton = transform.Find("_loginButton").GetComponent<Button>();
        _desButton = transform.Find("_desButton").GetComponent<Button>();
        _loginButton.onClick.AddListener(GameLogin);
    }

    void GameLogin()
    {
        //todo 应该根据服务器返回的消息决定登录
        SceneManager.LoadScene(1);
    }
    
}
