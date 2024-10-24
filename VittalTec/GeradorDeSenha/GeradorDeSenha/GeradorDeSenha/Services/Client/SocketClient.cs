using Quobject.EngineIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeSenhas.Services.Client
{
    public class SocketClient
    {
        public void EnviarSenha(string senhaGerada, Socket socket)
        {
            socket.Emit("enviarSenha", senhaGerada);
        }
    }
}
