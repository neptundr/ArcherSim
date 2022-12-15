using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Process pythonVisionProcess;
    
    private void Start()
    {
        StartProcess();
    }

    private void OnApplicationQuit()
    {
        // ending python process
        pythonVisionProcess.Kill(); //close is not working properly
    }

    private async Task StartProcess()
    {
        // running python script
        pythonVisionProcess = new Process(); 
        pythonVisionProcess.StartInfo.FileName = "c:/ArcherSim/venv/Scripts/python.exe";
        pythonVisionProcess.StartInfo.RedirectStandardOutput = true;
        pythonVisionProcess.StartInfo.CreateNoWindow = true;
        pythonVisionProcess.StartInfo.UseShellExecute = false; 
        pythonVisionProcess.StartInfo.Arguments = "c:/ArcherSim/PythonVision/main.py"; 
        pythonVisionProcess.Start();
    }
}
