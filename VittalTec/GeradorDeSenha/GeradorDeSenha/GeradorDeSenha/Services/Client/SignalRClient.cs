using Microsoft.AspNetCore.SignalR.Client;

namespace GeradorDeSenhas.Services.Client
{
    public class SignalRClient
    {
        private HubConnection _connection;

        public async Task IniciarConexaoAsync()
        {
            _connection = new HubConnectionBuilder().WithUrl("http://localhost:5233/password-generated").Build();

            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Conectado ao servidor SignalR.");
            }catch(Exception ex)
            {
                Console.WriteLine($"Erro ao realizar conexão: {ex.StackTrace}");
            }
        }

        public async Task EnviarSenha(string senha)
        {
            if (_connection.State == HubConnectionState.Connected)
                await _connection.SendAsync("EnviarSenha", senha);
        }
    }
}
