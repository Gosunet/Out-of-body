using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketObjet : MonoBehaviour
{

    public Vector3 initPos;
    public Vector3 oldPosition = Vector3.zero;
    public Vector3 newPosition = Vector3.zero;
    private struct ReceiveOperationInfo { public UdpClient msgUDPClient; public IPEndPoint msgIPEndPoint; }
    Vector3 tmp = Vector3.zero;
    public Vector3 move = Vector3.zero;
    private Queue<float> X;
    private Queue<float> Y;
    private Queue<float> Z;
    Thread receiveData;
    UdpClient client;
    int port = 5005;
    public bool kinectIsLeft = true;

    void Start()
    {
        X = new Queue<float>();
        Y = new Queue<float>();
        Z = new Queue<float>();
        client = new UdpClient(port);
        ReceiveOperationInfo msgInfo = new ReceiveOperationInfo();
        msgInfo.msgUDPClient = client;
        msgInfo.msgIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        client.BeginReceive(new AsyncCallback(ReceiveDataAsync), msgInfo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (kinectIsLeft)
            {
                initPos = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(0.80f, 0.6f, 1.25f);
                gameObject.transform.position = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(0.80f, 0.6f, 1.25f);
            }
            else
            {
                initPos = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(-0.80f, 0.6f, -3.33f);
                gameObject.transform.position = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(-0.80f, 0.6f, -3.33f);
            }
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
        if (!tmp.Equals(newPosition))
        {
            oldPosition = newPosition;
            newPosition = tmp;

            float z = 1.0f / ((newPosition.x / 100.0f) * -0.0030711016f);
            float x = z * 2 * (float)Math.Tan(1.0821 / 2) * (newPosition.z / 640);
            float y = z * 2 * (float)Math.Tan(0.8423 / 2) * (newPosition.y / 480);

            if (kinectIsLeft)
            {
                gameObject.transform.position = initPos + new Vector3(x * 0.025f, y * 0.025f, 0.025f * z);
            }
            else
            {
                gameObject.transform.position = initPos + new Vector3(-x * 0.01f, y * 0.01f, 0.01f * z);
            }
        }
    }

    private void ReceiveDataAsync(IAsyncResult msgResult)
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        byte[] msg = client.EndReceive(msgResult, ref anyIP);
        String text = Encoding.UTF8.GetString(msg);
        String[] coordonees = text.Split(':');
        tmp = new Vector3(float.Parse(coordonees[2]), float.Parse(coordonees[1]), float.Parse(coordonees[0]));
        ReceiveOperationInfo msgInfo = new ReceiveOperationInfo();
        msgInfo.msgUDPClient = client;
        msgInfo.msgIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        client.BeginReceive(new AsyncCallback(ReceiveDataAsync), msgInfo);
    }

    private void moveStick(String position)
    {
        String[] coordonees = position.Split(':');
        tmp = new Vector3(int.Parse(coordonees[2]), int.Parse(coordonees[1]), int.Parse(coordonees[0]));

        if (!tmp.Equals(newPosition))
        {
            oldPosition = newPosition;
            newPosition = tmp;
            Vector3 mvt = newPosition - oldPosition;
            X.Enqueue(mvt.x);
            Y.Enqueue(mvt.y);
            Z.Enqueue(mvt.z);

            if (X.Count > 5)
            {
                X.Dequeue();
                Y.Dequeue();
                Z.Dequeue();
            }
            gameObject.transform.Translate(-mvt.x * 0.0000f, -mvt.y * 0.005f, -mvt.z * 0.005f);
        }
    }
    void OnApplicationQuit()
    {
        client.Close();
    }

    public float ExponentialMovingAverage(float[] data, double baseValue)
    {
        float numerator = 0;
        float denominator = 0;
        float average = sum(data);
        average /= data.Length;

        for (int i = 0; i < data.Length; ++i)
        {
            numerator += data[i] * (float)Math.Pow(baseValue, data.Length - i - 1);
            denominator += (float)Math.Pow(baseValue, data.Length - i - 1);
        }

        numerator += average * (float)Math.Pow(baseValue, data.Length);
        denominator += (float)Math.Pow(baseValue, data.Length);

        return (numerator / denominator);
    }

    public float sum(float[] data)
    {
        float sum = 0f;
        foreach (float value in data)
        {
            sum += value;
        }
        return sum;
    }
}