using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using System;

public class GitCommit : EditorWindow
{
    private string commitMessage = "Unity Auto Commit";
    private static string currentBranch = "Unknown";
    private static List<string> branches = new List<string>();
    private int selectedBranchIndex = 0;

    [MenuItem("Tools/Git Commit & Push")]
    static void Init()
    {
        GitCommit window = (GitCommit)GetWindow(typeof(GitCommit));
        window.titleContent = new GUIContent("Git Commit");
        window.Show();

        window.RefreshBranches();
    }

    void OnGUI()
    {
        GUILayout.Label("Git Commit & Push", EditorStyles.boldLabel);

        GUILayout.Label("Current Branch: " + currentBranch, EditorStyles.label);

        selectedBranchIndex = EditorGUILayout.Popup("Select Branch:", selectedBranchIndex, branches.ToArray());
        commitMessage = EditorGUILayout.TextField("Commit Message:", commitMessage);

        if (GUILayout.Button("Commit and Push"))
        {
            SwitchBranch(branches[selectedBranchIndex]);
            RunGitCommand(commitMessage);
        }

        if (GUILayout.Button("Refresh Branches"))
        {
            RefreshBranches();
        }
    }

    void RefreshBranches()
    {
        branches.Clear();
        string projectPath = Directory.GetParent(Application.dataPath).FullName;
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c git branch",
            WorkingDirectory = projectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // 设置环境变量（暂时硬编码）
        //TODO: 从系统环境变量中获取, 或者从配置文件中读取
        psi.EnvironmentVariables["PATH"] = @"C:\Program Files\Git\cmd;" + Environment.GetEnvironmentVariable("PATH");

        Process process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        UnityEngine.Debug.Log("Git branch output: " + output);
        UnityEngine.Debug.Log("Git branch errors: " + error);
        if (!string.IsNullOrEmpty(output))
        {
            string[] branchList = output.Split('\n');
            foreach (string branch in branchList)
            {
                if (!string.IsNullOrWhiteSpace(branch))
                {
                    string cleanBranch = branch.Replace("*", "").Trim();
                    branches.Add(cleanBranch);
                    if (branch.Contains("*"))
                    {
                        currentBranch = cleanBranch;
                        selectedBranchIndex = branches.IndexOf(cleanBranch);
                    }
                }
            }
        }
        else
        {
            currentBranch = "Unknown";
        }
    }

    static void SwitchBranch(string branchName)
    {
        string projectPath = Directory.GetParent(Application.dataPath).FullName;
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c git checkout {branchName}",
            WorkingDirectory = projectPath,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = Process.Start(psi);
        process.WaitForExit();
        UnityEngine.Debug.Log($"Switched to branch: {branchName}");
    }

    static void RunGitCommand(string message)
    {
        string projectPath = Directory.GetParent(Application.dataPath).FullName;

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c git add . && git commit -m \"{message}\" && git push",
            WorkingDirectory = projectPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogError($"Git Error: {error}");
        }
        else
        {
            UnityEngine.Debug.Log($"Git Output: {output}");
        }
    }
}
