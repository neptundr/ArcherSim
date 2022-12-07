using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartProcess();
    }
    
    private async Task StartProcess()
    {
        // running python script
        Process p = new Process(); // create process (i.e., the python program
        p.StartInfo.FileName = "c:/ArcherSim/venv/Scripts/python.exe";
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.UseShellExecute = false; // make sure we can read the output from stdout
        p.StartInfo.Arguments = "c:/ArcherSim/PythonVision/main.py"; // start the python program with two parameters
        p.Start(); // start the process (the python program)
    }
    
    // private void RunCmd(string cmd, string args)
    // {
    //     ProcessStartInfo start = new ProcessStartInfo();
    //     start.FileName = "C:/ArcherSim/PythonVision/main.py";
    //     start.Arguments = string.Format("{0} {1}", cmd, args);
    //     start.UseShellExecute = false;
    //     start.RedirectStandardOutput = true;
    //     using(Process process = Process.Start(start))
    //     {
    //         print("python process executed");
    //     }
    // }
}
