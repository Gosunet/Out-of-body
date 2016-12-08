using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class used to communicate between Unity and the server
/// </summary>
public class SocketClient
{
    private static SocketClient _instance;

    private Thread _thread;

    private Socket _socket;

    private Boolean _stopThread = false;

    /// <summary>
    /// Contains the messages received from the socket
    /// </summary>
    private string _message;

    private IPEndPoint _remoteEP;

    private int _nbTryReconnection = 0;

    private System.Diagnostics.Process _process;

    private System.Diagnostics.Process _showIpProcess;

    /// <summary>
    /// Gets the instance of the class
    /// </summary>
    /// <returns>The instance</returns>
    public static SocketClient GetInstance()
    {
        if (_instance == null)
            _instance = new SocketClient();
        return _instance;
    }

    /// <summary>
    /// Constructor called only once (Singleton)
    /// </summary>
    private SocketClient()
    {

        //Recup�re sur quel r�seau on est connect�
        System.Diagnostics.Process process_0 = new System.Diagnostics.Process
        {
            StartInfo =
            {
                FileName = "netsh.exe",
                Arguments = "wlan show interface",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process_0.Start();
        String output_0 = process_0.StandardOutput.ReadToEnd();


        if (output_0.Contains(FilesConst.NAME_WIFI_NETWORK) == true)
        {
            //Deja connect� au r�seau out of body

        }
        else
        {
            //Pas encore connect� au r�seau
            //V�rifi� si le r�seau est actif
            System.Diagnostics.Process process_1 = new System.Diagnostics.Process
            {
                StartInfo =
            {
                FileName = "netsh.exe",
                Arguments = "wlan show network",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
            };
            process_1.Start();
            String output_1 = process_1.StandardOutput.ReadToEnd();


            //Si oui, se connect� dessus
            if (output_1.Contains(FilesConst.NAME_WIFI_NETWORK) == true)
            {
                Debug.Log("Reseau_OutOfBody - OK");

                System.Diagnostics.Process process_2 = new System.Diagnostics.Process
                {
                    StartInfo =
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = "/c netsh wlan add profile filename=configProfilWifi.xml && netsh wlan connect name="+FilesConst.NAME_WIFI_NETWORK
                }
                };
                process_2.Start();

                string output_2 = process_2.StandardOutput.ReadToEnd();

            }

            //pause pour laisser le temps � l'ordi de se connecter pour r�cuperer la bonne ip apr�s
            Thread.Sleep(5000);
        }




        IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
        _remoteEP = new IPEndPoint(ipAddress, Utils.SOCKET_PORT);

        // Run the node server
        _process = new System.Diagnostics.Process()
        {
            StartInfo =
            {
                FileName = "cmd.exe",
                Arguments = "/c node ..\\ApplicationMenu\\server\\server.js",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            }
        };

        // Connect the socket
        _socket = new Socket(_remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            _socket.Connect(_remoteEP);
            new Thread(() => Receive()).Start();
        }
        catch (Exception)
        {
            _socket.Close();
            new Thread(() => LaunchThreadConnect()).Start();
        }

        // Open a dialog showing the ip address
        _showIpProcess = System.Diagnostics.Process.Start("ShowIp");
    }

    /// <summary>
    /// Run the thread who read the content of the socket
    /// </summary>
    private void LaunchThreadConnect()
    {
        Connect();
        new Thread(() => Receive()).Start();
    }

    /// <summary>
    /// Connect the socket
    /// </summary>
    private void Connect()
    {
        _process.Start();
        while (!_socket.Connected && !_stopThread)
        {
            if (_nbTryReconnection > 2)
            {
                _process.Kill();
                StopNodeServer();
                Thread.Sleep(2000);
                _process.Start();
                _nbTryReconnection = 0;
            }
            try
            {
                _socket = new Socket(_remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_remoteEP);
                _nbTryReconnection = 0;
            }
            catch
            {
                _socket.Close();
                Thread.Sleep(1000);
                _nbTryReconnection++;
            }
        }
    }

    /// <summary>
    /// Stop the node server
    /// </summary>
    private void StopNodeServer()
    {
        foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("node"))
        {
            p.Kill();
        }
    }

    /// <summary>
    /// Stop every processes launched by Unity
    /// </summary>
    public void StopAllProcess()
    {
        StopNodeServer();
        if (_showIpProcess != null && !_showIpProcess.HasExited)
            _showIpProcess.Kill();
    }

    /// <summary>
    /// Read the datas in the socket
    /// </summary>
    private void Receive()
    {
        int bytes = 0;
        Byte[] bytesReceived = new Byte[256];
        do
        {
            try
            {
                bytes = _socket.Receive(bytesReceived, bytesReceived.Length, 0);
                message = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                if (message.Equals(Utils.SOCKET_EXIT))
                    _stopThread = true;
            }
            catch (SocketException)
            {
                _socket.Close();
                if (!_stopThread)
                {
                    Connect();
                }
            }
        }
        while (!_stopThread);
    }

    /// <summary>
    /// Write the specified message on the socket.
    /// </summary>
    /// <param name="message">Message.</param>
    public void Write(String message)
    {
        _socket.Send(System.Text.Encoding.ASCII.GetBytes(message));
    }

    public Socket socket
    {
        get
        {
            return _socket;
        }
        set
        {
            _socket = value;
        }
    }

    public Boolean stopThread
    {
        set
        {
            _stopThread = value;
        }
    }

    public String message
    {
        get
        {
            return _message;
        }
        set
        {
            _message = value;
        }
    }
}