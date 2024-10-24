using Microsoft.AspNetCore.SignalR;

namespace HubServerVittalTec.Services
{
    public class PasswordHubService : Hub
    {
        public async Task EnviarSenha(string senhaGerada)
        {
            await Clients.All.SendAsync("ReceberSenha", senhaGerada);
        }
    }
}
