using SuperWebSocket; // Nuget package: SuperWebSocketNETServer v0.8.0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteWebSocket
{
    class Program
    {
        private static WebSocketServer wsServer;

        /// <summary>
        /// Testar no endereço: https://www.websocket.org/echo.html
        /// Location: ws://localhost:8088
        /// Message: Hello server
        /// </summary>
        static void Main(string[] args)
        {
            wsServer = new WebSocketServer();
            int port = 8088;
            wsServer.Setup(port);
            wsServer.NewSessionConnected += WsServer_NewSessionConnected;
            wsServer.NewMessageReceived += WsServer_NewMessageReceived;
            wsServer.NewDataReceived += WsServer_NewDataReceived;
            wsServer.SessionClosed += WsServer_SessionClosed;

            wsServer.Start();
            Console.WriteLine($"Servidor rodando na porta {port}. Pressione ENTER para fechar...");
            Console.ReadKey();
            wsServer.Stop();

        }

        private static void WsServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine("Session Closed");
        }

        private static void WsServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("New Data Received");
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine($"New Message Received: {value}");

            if (value == "Hello server")
            {
                session.Send("Hello client");
                Log(value);
            }
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("New Session Connected");
        }

        private static void Log(string value)
        {
            string nomeArquivo = @"C:\Log" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

            // Caso ocorra exception de acesso não autorizado, executar o VS como administrador
            using (StreamWriter writer = new StreamWriter(nomeArquivo)){

                try
                {
                    writer.WriteLine(value);
                    Console.WriteLine("Log gravado com sucesso");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao gravar o log: " + ex);
                }
            }
        }
    }
}
