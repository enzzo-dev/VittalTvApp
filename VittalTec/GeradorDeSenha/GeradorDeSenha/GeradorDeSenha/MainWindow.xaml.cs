using GeradorDeSenhas.Services.Client;
using Quobject.EngineIoClientDotNet.Client;
using System.Printing;
using System.Windows;
using System.Windows.Controls;

namespace GeradorDeSenhas
{
    public partial class MainWindow : Window
    {
        private int senhaPreferencial = 1;
        private int senhaTelemedicina = 1;
        private int senhaClinico = 1;
        private int senhaExameLab = 1;
        private int senhaExameImg = 1;
        private int senhaOrto = 1;
        private string senhaGerada;
        private SignalRClient _signalRClient;
        private SocketClient _socketClient;
        private Socket _socket;

        public MainWindow()
        {
            InitializeComponent();
            _signalRClient = new SignalRClient();
            _signalRClient.IniciarConexaoAsync().GetAwaiter();
        }

        // Método para gerar a senha e imprimir
        private void GerarSenha(string tipoSenha, ref int contador)
        {
            // Gera a senha e a armazena na variável
            senhaGerada = $"Especialidade: {tipoSenha} {contador++}";

            _signalRClient.EnviarSenha(senhaGerada).GetAwaiter().GetResult();

            // Chama o método de impressão após gerar a senha
            ImprimirSenha(senhaGerada);

        }

        // Métodos de geração de senhas para os diferentes tipos
        private void GerarSenha_Senha_Preferencial(object sender, RoutedEventArgs e) => GerarSenha("PREFERENCIAL", ref senhaPreferencial);
        private void GerarSenhaTeleMedicina(object sender, RoutedEventArgs e) => GerarSenha("TELEMEDICINA", ref senhaTelemedicina);
        private void GerarSenhaClinico(object sender, RoutedEventArgs e) => GerarSenha("TOTEM TRIAGEM", ref senhaClinico);
        private void GerarSenhaExameLab(object sender, RoutedEventArgs e) => GerarSenha("TOTEM IA", ref senhaExameLab);
        private void GerarSenhaExameIMG(object sender, RoutedEventArgs e) => GerarSenha("EXAME DE IMAGEM", ref senhaExameImg);
        private void GerarSenhaOrto(object sender, RoutedEventArgs e) => GerarSenha("ORTOPEDISTA", ref senhaOrto);

        //// Método para iniciar o servidor WebSocket
        //private async void IniciarServidorWebSocket()
        //{
        //    HttpListener httpListener = new HttpListener();
        //    httpListener.Prefixes.Add("http://localhost:9090/");
        //    httpListener.Start();
        //    Console.WriteLine("Servidor WebSocket rodando em ws://localhost:9090/");

        //    await Task.Run(async () =>
        //    {
        //        while (true)
        //        {
        //            HttpListenerContext context = await httpListener.GetContextAsync();
        //            if (context.Request.IsWebSocketRequest)
        //            {
        //                HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
        //                WebSocket webSocket = wsContext.WebSocket;
        //                clientesWebSocket.Add(webSocket);
        //                Console.WriteLine("Cliente WebSocket conectado");
        //            }
        //            else
        //            {
        //                context.Response.StatusCode = 400;
        //                context.Response.Close();
        //            }
        //        }
        //    });
        //}

        //// Método para enviar a senha gerada via WebSocket
        //private async void EnviarSenhaViaWebSocket(string senha)
        //{
        //    foreach (var cliente in clientesWebSocket)
        //    {
        //        if (cliente.State == WebSocketState.Open)
        //        {
        //            var mensagem = Encoding.UTF8.GetBytes(senha);
        //            await cliente.SendAsync(new ArraySegment<byte>(mensagem), WebSocketMessageType.Text, true, CancellationToken.None);
        //        }
        //    }
        //}

        // Método para imprimir a senha diretamente

        private void ImprimirSenha(string senha)
        {
            string nomeImpressora = "CUSTOM VKP80III"; // Nome da impressora

            PrintQueue printQueue = null;
            var printServer = new PrintServer();

            foreach (PrintQueue pq in printServer.GetPrintQueues())
            {
                if (pq.Name.Equals(nomeImpressora, StringComparison.OrdinalIgnoreCase))
                {
                    printQueue = pq;
                    break;
                }
            }

            if (printQueue == null)
            {
                MessageBox.Show($"Impressora '{nomeImpressora}' não encontrada.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Criar o visual para impressão usando o layout desejado
            var visual = CriarVisualParaImpressao(senha);

            // Cria um diálogo de impressão e define a fila de impressão
            PrintDialog printDialog = new PrintDialog
            {
                PrintQueue = printQueue
            };

            printDialog.PrintTicket.PageMediaSize = new PageMediaSize(300, 600); // Tamanho de recibo (80mm x 297mm)
            printDialog.PrintTicket.PageOrientation = PageOrientation.Portrait; // Orientação da página

            // Executar a impressão
            printDialog.PrintVisual(visual, "Impressão de Senha");
        }

        // Método que cria um visual para impressão
        private UIElement CriarVisualParaImpressao(string senha)
        {
            var impressaoLayout = new ImpressaoLayout();

            string[] partesSenha = senha.Split(' ');
            string nomeSenha = string.Join(" ", partesSenha.Take(partesSenha.Length - 1)); // Nome da senha
            string numeroSenha = partesSenha.Last(); // Última parte é o número da senha

            if (impressaoLayout.TextBlockNomeSenha != null && impressaoLayout.TextBlockNumeroSenha != null)
            {
                impressaoLayout.TextBlockNomeSenha.Text = nomeSenha;
                impressaoLayout.TextBlockNumeroSenha.Text = numeroSenha;
            }
            else
            {
                MessageBox.Show("Erro: O TextBlock para exibir a senha não foi encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return impressaoLayout;
        }

    }
}
