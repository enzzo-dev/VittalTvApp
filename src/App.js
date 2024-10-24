import React, { useEffect, useState } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import './App.css';


function App() {
    const [senhaGerada, setSenhaGerada] = useState('');
    const [especialidade, setEspecialidade] = useState('');
    const [senhasAnteriores, setSenhasAnteriores] = useState([]);

    useEffect(() => {
        // Conexão com o hub SignalR
        const connection = new HubConnectionBuilder()
            .withUrl("https://qqf5fzmk-5233.brs.devtunnels.ms/password-generated")
            .withAutomaticReconnect()
            .build();
    
        connection.start()
            .then(() => console.log("Conectado ao Hub SignalR"))
            .catch(err => console.error("Erro ao conectar ao SignalR", err));

        receberSenha(connection)

        return () => {
            connection.stop();
        };
    }, [senhaGerada]);
    
    function receberSenha(connection) 
    {
        connection.on("ReceberSenha", (senha) => {
            const senhaFormatada = senha.split(' ');

            if (senhaGerada > 0) {
                setSenhasAnteriores((prevSenhas) => [
                    ...prevSenhas,
                    {
                        senha: senhaGerada,
                        especialidade: especialidade
                    }
                ]);
            }

            setSenhaGerada(senhaFormatada[2]);
            setEspecialidade(senhaFormatada[1]);
        });
    }

    return (
        <body>      
        <div>
            <div class="container">
                <div class="header">
                    <div class="header-left">
                        <img src="../public/logo-conahp23-topo" alt="CONAHP 2024 Logo" />
                        <h1>Jornada da Saúde</h1>
                    </div>
                    <div class="header-right">
                        <img src="logo-vittaltec.svg" alt="VittalTec Logo" />
                        <img src="logo-vision" alt="Vision Logo" />
                    </div>
                </div>

            <table>
                <thead>
                    <tr>
                        <th>Senha</th>
                        <th>Especialidade</th>
                    </tr>
                </thead>
                <tbody id="senhaTableBody">
                    <td>{senhaGerada}</td>
                    <td>{especialidade}</td>
                </tbody>
                <thead>
                    <tr>
                        <th>Senhas Anteriores</th>
                        <th>Especialidades Anteriores</th>
                    </tr>
                </thead>
                <tbody id="senhaTableBody">
                    {
                        senhasAnteriores.reverse().map(senhaAnterior => {
                            return (
                                <tr>
                                    <td>{senhaAnterior.senha}</td>
                                    <td>{senhaAnterior.especialidade}</td>
                                </tr>
                                )
                        })
                    }
                </tbody>
            </table>
            </div>
      </div>
    </body>
    );
}

export default App;
