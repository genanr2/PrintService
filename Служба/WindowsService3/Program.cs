using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;using System.IO;
using System.Net;

using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Printing;
using System.Data.Linq;
using System.Windows.Forms;
//using System.Drawing.Printing;
//namespace WindowsService3

namespace ServiceGleb1
{
    //  static class Program
    class Program
    {
      /// <summary>
      /// Главная точка входа для приложения.
      /// </summary>
      //    static void Main()
      static private Font printFont;
      static private StreamReader streamToPrint;
      static public NetworkStream stream;
      static public TcpListener server = null;
			static public EventLog myLog;
			static public Socket listenerSocket;
			public static ManualResetEvent tcpClientConnected=new ManualResetEvent(false);
      public static Int32 port;
			static void Main()
      {
        ServiceBase[] ServicesToRun;
//        ServicesToRun = new ServiceBase[]{new Service1() };
				ServicesToRun = new ServiceBase[] { new ServiceGleb1() };
				//      Service1 ss = WindowsService3.Service1;
				ServiceBase.Run(ServicesToRun);
				if (!EventLog.SourceExists("ServiceGleb")){EventLog.CreateEventSource("Печать заявок", "ServiceGleb");}
				myLog = new EventLog();myLog.Source = "Печать заявок";myLog.WriteEntry("Запуск службы.", EventLogEntryType.Warning, 1, 1);
/*
				try
				{
					// Set the TcpListener on port 13000.
					port = 12000;
//        IPAddress localAddr = IPAddress.Parse("192.168.1.8");
        IPAddress localAddr = IPAddress.Any;
					// TcpListener server = new TcpListener(port);
          server = new TcpListener(localAddr, port);
//					listenerSocket = server.Server;
//					LingerOption lingerOption = new LingerOption(true, 10);listenerSocket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.Linger,lingerOption);
          myLog.WriteEntry("Сервер прослушивания создан. IP: " + localAddr + " Порт: " + port, EventLogEntryType.Warning, 1, 1);
          server.Start();
          myLog.WriteEntry("Сервер прослушивания запущен.", EventLogEntryType.Warning, 1, 1);
          Byte[] bytes = new Byte[1024];String data = null;
          TcpClient client;
          while (true)
          {
//            myLog.WriteEntry("Сервер прослушивания ожидает соединение с клиентом.", EventLogEntryType.Warning, 1, 1);
            Application.DoEvents();
            // Perform a blocking call to accept requests. You could also user server.AcceptSocket() here.
						if (!server.Pending())
						{
//							myLog.WriteEntry("Нет никаких запросов на связь .", EventLogEntryType.Warning, 1, 1);
						}
						else
						{
							myLog.WriteEntry("Есть запрос на соединение.", EventLogEntryType.Warning, 1, 1);
							client = server.AcceptTcpClient();
							//Socket socket = server.AcceptSocket();
							//DoBeginAcceptTcpClient(server);
							myLog.WriteEntry("Соединено с клиентом.", EventLogEntryType.Warning, 1, 1);
							myLog.WriteEntry("Слушаю соединение с ." + 
										IPAddress.Parse(((IPEndPoint)server.LocalEndpoint).Address.ToString())+" по порту "+
										((IPEndPoint)server.LocalEndpoint).Port.ToString(),EventLogEntryType.Warning, 1, 1);
							stream = client.GetStream();
							client.ReceiveBufferSize = 2048;
							int i;
							while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
							{
								data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
								myLog.WriteEntry("Получены сведения ." + data, EventLogEntryType.Warning, 1, 1);
								data = data.ToUpper();
								Byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);
								stream.Write(msg, 0, msg.Length);
								myLog.WriteEntry("Отправлены сведения ." + data, EventLogEntryType.Warning, 1, 1);
								//using (StreamWriter sw = new StreamWriter("c:\\CDriveDirs.txt")){sw.Write("Заявка на пропуск\n");sw.Write(data);}
								try
								{
									//string logFile = DateTime.Now.ToShortDateString().Replace(@"/", @"-").Replace(@"\", @"-") + ".log";
									FileStream fs = null;
									try
									{
										fs = new FileStream("c:\\CDrive.txt", FileMode.Create, FileAccess.Write, FileShare.None);
									}
									catch (Exception ex)
									{
										myLog.WriteEntry("Не могу создать файл c:\\CDrive.txt " + ex.Message, EventLogEntryType.Warning, 1, 1);
									}
									StreamWriter sw = new StreamWriter(fs);
									try { sw.Write("Заявка на пропуск\n"); sw.Write(data); }finally { sw.Flush(); sw.Close(); }
								}
								catch (Exception ex)
								{
									myLog.WriteEntry("Не могу записать на диск. " + ex.Message, EventLogEntryType.Warning, 1, 1);
								}
								myLog.WriteEntry("Начало печати на принтер.", EventLogEntryType.Warning, 1, 1);
								printMessage(stream);
								myLog.WriteEntry("Конец печати на принтер.", EventLogEntryType.Warning, 1, 1);
							}
							stream.Flush();stream.Dispose();stream.Close();client.Close();
						}

          }
        }
        catch (SocketException e)
        {
          //        Console.WriteLine("SocketException: {0}", e);
          myLog.WriteEntry("SocketException: " + e.Message, EventLogEntryType.Error, 1, 1);
        }
//        finally{server.Stop();}
*/ 
       }

			public static void DoBeginAcceptTcpClient(TcpListener listener)
			{
				// Set the event to nonsignaled state.
				myLog.WriteEntry("tcpClientConnected.Reset().", EventLogEntryType.Warning, 1, 1);
				tcpClientConnected.Reset();
				// Start to listen for connections from a client.
//				Console.WriteLine("Waiting for a connection...");
				// Accept the connection. BeginAcceptSocket() creates the accepted socket.
				myLog.WriteEntry("BeginAcceptTcpClient.", EventLogEntryType.Warning, 1, 1);
				listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listener);
				// Wait until a connection is made and processed before continuing.
				myLog.WriteEntry("tcpClientConnected.WaitOne().", EventLogEntryType.Warning, 1, 1);
				tcpClientConnected.WaitOne();
				myLog.WriteEntry("tcpClientConnected.WaitOne2().", EventLogEntryType.Warning, 1, 1);
			}
			// Process the client connection.
			public static void DoAcceptTcpClientCallback(IAsyncResult ar)
			{
				// Get the listener that handles the client request.
				TcpListener listener = (TcpListener)ar.AsyncState;
				myLog.WriteEntry("(TcpListener)ar.AsyncState.", EventLogEntryType.Warning, 1, 1);
				// End the operation and display the received data on the console.
				TcpClient client = listener.EndAcceptTcpClient(ar);
				// Process the connection here. (Add the client to a server table, read data, etc.)
				myLog.WriteEntry("Client connected completed.", EventLogEntryType.Warning, 1, 1);
//				Console.WriteLine("Client connected completed");
				// Signal the calling thread to continue.
				tcpClientConnected.Set();
				myLog.WriteEntry("tcpClientConnected.Set().", EventLogEntryType.Warning, 1, 1);
			}


      static public void printMessage(NetworkStream str1)
      {
        try
        {
//					myLog.WriteEntry("Печать на принтер1.", EventLogEntryType.Warning, 1, 1);
					streamToPrint = new StreamReader("C:\\CDrive.txt");
//					myLog.WriteEntry("Печать на принтер2.", EventLogEntryType.Warning, 1, 1);
					try
          {
            printFont = new Font("Arial", 10);
            PrintDocument pd = new PrintDocument();
						//listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listener);
//						PrintPageEventHandler eh = new PrintPageEventHandler(pd_PrintPage);

            
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);


//						pd.PrintPage += eh;
//						myLog.WriteEntry("PrintPageEventHandler: " + eh.ToString(), EventLogEntryType.Warning, 1, 1);
//						myLog.WriteEntry("Печать на принтер3: " + pd.DocumentName, EventLogEntryType.Warning, 1, 1);
//						pd.PrintController = new myControllerImplementation();
//						if (wantsStatusDialog == true)
//						{
//						pd.PrintController=new PrintControllerWithStatusDialog(pd.PrintController);
            //pd.PrinterSettings.PrinterName = "HP LaserJet P1505";
            pd.PrinterSettings.PrinterName = "Xerox WC M123 PCL";
						myLog.WriteEntry("PrinterName: " + pd.PrinterSettings.PrinterName, EventLogEntryType.Warning, 1, 1);
//						myLog.WriteEntry("PrintFileName: " + pd.PrinterSettings.PrintFileName.ToString(), EventLogEntryType.Warning, 1, 1);
//						myLog.WriteEntry("PrintToFile? " + pd.PrinterSettings.PrintToFile, EventLogEntryType.Warning, 1, 1);
//						myLog.WriteEntry("PrinterSettings: " + pd.PrinterSettings.ToString(), EventLogEntryType.Warning, 1, 1);
						try
						{
//							pd.Print();
//							PrintEventArgs ev = new PrintEventArgs();
//							ev.PrintAction = PrintAction.PrintToFile;
//							pd.PrintController.OnStartPrint(pd, ev);
							pd.Print();
//							pd.PrintController
						}
						catch (Exception ex)
						{
							myLog.WriteEntry("Ошибка начала печати: " + ex.Message, EventLogEntryType.Error, 1, 1);
						}
//						myLog.WriteEntry("Печать на принтер4.", EventLogEntryType.Warning, 1, 1);
					}
          finally { streamToPrint.Close(); }
        }
				catch (Exception ex)
				{/*MessageBox.Show(ex.Message);*/
					myLog.WriteEntry("Ошибка печати: " + ex.Message, EventLogEntryType.Error, 1, 1);
				}
//				myLog.WriteEntry("Конец печати printMessage");

			}
      static public void pd_PrintPage(object sender, PrintPageEventArgs ev)
      {
        float linesPerPage = 0;
        float yPos = 0;
        int count = 0;
        float leftMargin = ev.MarginBounds.Left;
        float topMargin = ev.MarginBounds.Top;
        string line = null;
        // Calculate the number of lines per page.
				myLog.WriteEntry("Печать на принтер55.", EventLogEntryType.Warning, 1, 1);
				linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
        // Print each line of the file.
        while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
        {
          yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
          ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
          count++;
        }
        // If more lines exist, print another page.
        if (line != null) ev.HasMorePages = true; else ev.HasMorePages = false;
      }
 
     }
}







public class PrintingExample // : System.Windows.Forms.Form
{
//  private System.ComponentModel.Container components;
//  private System.Windows.Forms.Button printButton;
  private Font printFont;
  private StreamReader streamToPrint;
  public PrintingExample():base()
  {
    // The Windows Forms Designer requires the following call.
//    InitializeComponent();
  }
  // The Click event is raised when the user clicks the Print button.
  private void printButton_Click(object sender, EventArgs e)
  {
    try
    {
      streamToPrint = new StreamReader("C:\\My Documents\\MyFile.txt");
      try
      {
        printFont = new Font("Arial", 10);
        PrintDocument pd = new PrintDocument();
        pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
        pd.Print();
      }
      finally{streamToPrint.Close();}
    }
    catch (Exception ex)
    {
//      MessageBox.Show(ex.Message);
    }
  }

  // The PrintPage event is raised for each page to be printed.
  private void pd_PrintPage(object sender, PrintPageEventArgs ev)
  {
    float linesPerPage = 0;
    float yPos = 0;
    int count = 0;
    float leftMargin = ev.MarginBounds.Left;
    float topMargin = ev.MarginBounds.Top;
    string line = null;

    // Calculate the number of lines per page.
    linesPerPage = ev.MarginBounds.Height/printFont.GetHeight(ev.Graphics);
    // Print each line of the file.
    while(count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
    {
      yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
      ev.Graphics.DrawString(line, printFont, Brushes.Black,leftMargin, yPos, new StringFormat());
      count++;
    }
    // If more lines exist, print another page.
    if (line != null)ev.HasMorePages = true; else ev.HasMorePages = false;
  }

/*
  // The Windows Forms Designer requires the following procedure.
  private void InitializeComponent()
  {
    this.components = new System.ComponentModel.Container();
    this.printButton = new System.Windows.Forms.Button();

    this.ClientSize = new System.Drawing.Size(504, 381);
    this.Text = "Print Example";

    printButton.ImageAlign =
       System.Drawing.ContentAlignment.MiddleLeft;
    printButton.Location = new System.Drawing.Point(32, 110);
    printButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
    printButton.TabIndex = 0;
    printButton.Text = "Print the file.";
    printButton.Size = new System.Drawing.Size(136, 40);
    printButton.Click += new System.EventHandler(printButton_Click);

    this.Controls.Add(printButton);
  }

  // This is the main entry point for the application.
  public static void Main(string[] args)
  {
    Application.Run(new PrintingExample());
  }
 */
}











