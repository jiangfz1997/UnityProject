using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader : MonoBehaviour
{
    //public string filePath = "Assets/Resources/StoryContext.csv";  // CSV 文件路径
    public string fileName = "StoryContext";
    public RawImage backgroundImage; // 背景图片 UI
    public TextMeshProUGUI dialogText; // 显示对话文本 UI

    private List<string> backgroundList; // 背景图片路径列表
    private List<string> textList; // 文本列表
    private int currentBackgroundIndex = 0; // 当前背景索引
    private Mouse mouse;

    void Start()
    {
        mouse = Mouse.current;

        // 读取 CSV 文件并解析数据
        ReadCSV(fileName);

        // 如果背景数据为空，则输出错误信息
        if (backgroundList == null || backgroundList.Count == 0)
        {
            Debug.LogError("Failed to load background data!");
            return;
        }

        // 设置初始背景和文本
        SetBackground(backgroundList[currentBackgroundIndex]);
        SetDialogText(textList[currentBackgroundIndex]);
    }

    void Update()
    {
        // 点击左键切换到下一张背景和文本
        if (mouse.leftButton.wasPressedThisFrame)
        {
            NextBackground();
        }
    }

    // 读取 CSV 文件，并将背景图片路径和文本保存到列表
    void ReadCSV(string resourceName)
    {

        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);
        if (csvFile == null)
        {
            Debug.LogError($"CSV 文件未找到！请确保 {resourceName}.csv 放在 Assets/Resources 目录下");
            return;
        }
        backgroundList = new List<string>();
        textList = new List<string>();

        //if (!File.Exists(path))
        //{
        //    Debug.LogError("CSV file not found!");
        //    return;
        //}

        string[] lines = csvFile.text.Split('\n');
        Regex regex = new Regex(@"^""(.*?)"",(.*?)$|^([^""]+),(.*?)$", RegexOptions.Singleline);


        // 正则表达式解析每行
        // 处理带有引号和不带引号的文本，换行符也能正确处理
        //Regex regex = new Regex(@"^""(.*?)"",(.*?)$|^([^""]+),(.*?)$", RegexOptions.Singleline);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            Debug.Log($"Line {i + 1}: {line}");

            Match match = regex.Match(line);
            if (match.Success)
            {
                string text = string.Empty;
                string backgroundPic = string.Empty;

                // 处理带引号的文本
                if (match.Groups[1].Success)
                {
                    text = match.Groups[1].Value.Trim();
                    backgroundPic = match.Groups[2].Value.Trim();
                }
                // 处理没有引号的文本
                else if (match.Groups[3].Success)
                {
                    text = match.Groups[3].Value.Trim();
                    backgroundPic = match.Groups[4].Value.Trim();
                }

                // 处理带换行符的信件，先将换行符替换为一个特殊标识符
                text = text.Replace("\n", "<NEWLINE>");
                text = text.Replace("\"\"", "\"");  // 处理文本中的双引号，防止被误解析

                textList.Add(text);
                backgroundList.Add(backgroundPic);

                Debug.Log($"Parsed: Text = {text}, BackgroundPic = {backgroundPic}");
            }
            else
            {
                Debug.LogWarning($"Skipping invalid line {i + 1}: {line}");
            }
        }
    }

    // 切换到下一张背景和文本
    void NextBackground()
    {
        if (backgroundList == null || backgroundList.Count == 0)
        {
            Debug.LogError("Background list is empty!");
            return;
        }

        // 切换背景图片
        if (currentBackgroundIndex < backgroundList.Count - 1)
        {
            currentBackgroundIndex++;
            SetBackground(backgroundList[currentBackgroundIndex]);
            SetDialogText(textList[currentBackgroundIndex]);
        }
        else
        {
            Debug.Log("End of background scenes!");
            SceneManagerController.instance.LoadSceneAdditive("Level_1");
            SceneManagerController.instance.UnloadScene("Context");
        }
    }


    // 设置当前背景图片
    void SetBackground(string backgroundPic)
    {
        // 加载图片资源，去掉文件后缀
        Texture2D backgroundTexture = Resources.Load<Texture2D>(backgroundPic);

        // 如果背景图片加载成功，显示它
        if (backgroundTexture != null)
        {
            backgroundImage.texture = backgroundTexture;
            Debug.Log($"Background set to: {backgroundPic}");
        }
        else
        {
            Debug.LogError("Background image not found: " + backgroundPic);
        }
    }

    // 设置对话文本
    void SetDialogText(string text)
    {
        if (dialogText != null)
        {
            // 恢复文本中的换行符
            text = text.Replace("<NEWLINE>", "\n");
            dialogText.text = text;
            Debug.Log($"Displayed Text: {text}");
        }
        else
        {
            Debug.LogError("TextMeshPro UI component is not assigned!");
        }
    }
}