using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    [SerializeField] private HandManager handManager;

    Thread receiveThread;
    UdpClient client; 
    public int port = 4444;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;
    
    private string previousData;

    public void Start()
    {
        // creating thread connection
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void Update()
    {
        if (data != null && previousData != data)
        {
            // sending data to hand manager
            handManager.UpdateHands(data);
            previousData = data;
        }
    }


    // receiving thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);
                    
                if (printToConsole) { print(data); }
            }
            catch (Exception err) // no data / wrong data
            {
                print(err.ToString());
            }
        }
    }
}
