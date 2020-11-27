using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace RCIM_OP10
{
  public class TcpPrint
  {
    public byte Disconnected = 0;
    public byte Connected = 1;
    private int skTimeout = 250;  //Socket (send / receive timeout)

    private string p_wsIPAddress;
    private int p_wsPort;
    private IPEndPoint p_wsIPEndPoint;
    private Socket p_wsSocket;
    private byte p_wsStatus;
    private int p_wsErrors;
    private StringBuilder p_wsLastError= new StringBuilder("");

    public string wsIPAddress 
    {
      set {p_wsIPAddress = value;}
    }

    public int wsPort
    {
      set { p_wsPort = value; }
    }

    public Socket wsSocket
    {
      get {return p_wsSocket;}
    }

    public byte wsStatus
    {
      get { return p_wsStatus; }
    }

    public long wsErrors
    {
      get { return p_wsErrors; }
    }

    public StringBuilder wsLastError 
    {
      get { return p_wsLastError; }
    }

    public void ConnectToServer()
    {
      p_wsIPEndPoint = new IPEndPoint(IPAddress.Parse(p_wsIPAddress), p_wsPort);
      Socket skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        p_wsStatus = Disconnected;
        p_wsSocket = skt;   // save this socket handle

        AsyncCallback onCon = new AsyncCallback(OnConnect);
        skt.BeginConnect(p_wsIPEndPoint, onCon, skt);
      }
      catch
      {
        p_wsStatus = Disconnected;
        p_wsSocket = null;
        p_wsErrors++;
        //return ConnectError;
      }
    }

    private void OnConnect(IAsyncResult ar)
    {
      Socket sock = (Socket)ar.AsyncState;
      p_wsStatus = Disconnected;

      try
      {
        if (sock.Connected)
        {
          p_wsStatus = Connected;
        }
        else
        {
          sock.Close();
        }
      }
      catch
      {
        // Unusual error during Connect!
      }
    }

    public void CloseSockect()
      {
      if (p_wsSocket != null)
        p_wsSocket.Close();
      p_wsStatus = Disconnected;
      p_wsSocket = null;
      }

    public bool WritePrinterPort(string msg)
    {
      int nBytes;
      char ch;

      byte[] ar = new byte[msg.Length];

      for (int N = 0; N < msg.Length; N++)
      {
        ch = Convert.ToChar(msg.Substring(N, 1));
        ar[N] = Convert.ToByte(ch);
      }
      try
      {
        p_wsSocket.ReceiveTimeout = skTimeout;
        p_wsSocket.SendTimeout = skTimeout;

        nBytes = p_wsSocket.Send(ar);
        return true;
      }
      catch
      {
        if (p_wsSocket != null) p_wsSocket.Close();
        p_wsStatus = Disconnected;
        return false;
      }
    }
  }
}
