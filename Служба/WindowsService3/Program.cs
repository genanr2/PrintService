using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography; 
using System.Drawing;
using System.Drawing.Printing;
namespace WindowsService3
{
//  namespace Service1
//  {
    //  static class Program
    class Program
    {
      /// <summary>
      /// Главная точка входа для приложения.
      /// </summary>
      //    static void Main()
      private Font printFont;
      private StreamReader streamToPrint;
      static public NetworkStream stream;
      static public TcpListener server = null;
      static void Main()
      {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
			{ 
				new Service1() 
			};
//        Service1 ss = WindowsService3.Service1;
      ServiceBase.Run(ServicesToRun);
//      ServicesToRun
      if (!EventLog.SourceExists("ServiceGleb"))
      {
          //An event log source should not be created and immediately used.
          //There is a latency time to enable the source, it should be created
          //prior to executing the application that uses the source.
          //Execute this sample a second time to use the new source.
          EventLog.CreateEventSource("Печать заявок", "ServiceGleb");
      }
      EventLog myLog = new EventLog();
      myLog.Source = "Печать заявок";
      myLog.WriteEntry("Запуск службы.", EventLogEntryType.Warning, 1, 1);
      try
      {
        // Set the TcpListener on port 13000.
        Int32 port = 12000;
//        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
//          IPAddress localAddr = IPAddress.Any;
//        IPAddress localAddr = IPAddress.Parse("192.168.1.8");
        IPAddress localAddr = IPAddress.Any;
        // TcpListener server = new TcpListener(port);
          server = new TcpListener(localAddr, port);
          myLog.WriteEntry("Сервер прослушивания создан. IP: " + localAddr + " Порт: " + port, EventLogEntryType.Warning, 1, 1);
          // Start listening for client requests.
          server.Start();
          myLog.WriteEntry("Сервер прослушивания запущен.", EventLogEntryType.Warning, 1, 1);
          // Buffer for reading data
          Byte[] bytes = new Byte[1024];
          String data = null;
          //        byte[] msg = null;
          //        String msg = null;
          TcpClient client;
//          NetworkStream stream;
//          PrintingExample prt = new PrintingExample();

          // Enter the listening loop.
          while (true)
          {
            myLog.WriteEntry("Сервер прослушивания ожидает соединение с клиентом.", EventLogEntryType.Warning, 1, 1);
            // Perform a blocking call to accept requests. You could also user server.AcceptSocket() here.
            client = server.AcceptTcpClient();
            //          Socket socket = server.AcceptSocket();
            myLog.WriteEntry("Соединено с клиентом.", EventLogEntryType.Warning, 1, 1);
            //        data = null;
            // Get a stream object for reading and writing
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
//              using (StreamWriter sw = new StreamWriter("c:\\CDriveDirs.txt"))
//              {
//                sw.Write("Заявка на пропуск\n");
//                sw.Write(data);
//              }

              try
              {
                StreamWriter sw = new StreamWriter("c:\\CDriveDirs.txt");
                //        streamToPrint = str1;
                try
                {
                  sw.Write("Заявка на пропуск\n");
                }
                finally { sw.Flush();sw.Close(); }
              }
              catch (Exception ex) 
              {
                /*MessageBox.Show(ex.Message);*/
                myLog.WriteEntry("Не могу записать на диск ." + ex.Message, EventLogEntryType.Warning, 1, 1);

              }
              //            socket.Send(msg);
            }
            stream.Flush();
            stream.Dispose();
            stream.Close();
            client.Close();
          }
        }
        catch (SocketException e)
        {
          //        Console.WriteLine("SocketException: {0}", e);
          myLog.WriteEntry("SocketException: " + e.Message, EventLogEntryType.Error, 1, 1);
        }
        /*
              finally
              {
                // Stop listening for new clients.
                server.Stop();
              }
         */
        //    Console.WriteLine("\nHit enter to continue...");
        //      Console.Read();
       }
      private void printMessage(NetworkStream streamToPrint)
      {
        try
        {
//          streamToPrint = new StreamReader("C:\\My Documents\\MyFile.txt");
          try
          {
            printFont = new Font("Arial", 10);
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
            pd.Print();
          }
          finally { streamToPrint.Close(); }
        }catch (Exception ex){/*MessageBox.Show(ex.Message);*/}
      }
      void pd_PrintPage(object sender, PrintPageEventArgs ev)
      {
        float linesPerPage = 0;
        float yPos = 0;
        int count = 0;
        float leftMargin = ev.MarginBounds.Left;
        float topMargin = ev.MarginBounds.Top;
        string line = null;
        // Calculate the number of lines per page.
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











