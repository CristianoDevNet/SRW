# SRW

Baixe a [ultima versão do SRW](https://github.com/CristianoDevNet/SRW/releases).

Para rodar o programa, você precisa ter instalado a versão do .NET 9.0 Desktop Runtime - [Baixe aqui](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Clique na imagem abaixo para assistir ao vídeo tutorial de como usar o programa.
[![Tutorial do SRW](https://github.com/user-attachments/assets/56377291-a2a9-40ad-b6c6-f98815009734)](https://www.youtube.com/watch?v=zUy99pMKag8)

SRW é uma aplicação WPF para Windows, desenvolvida em .NET 9, que permite configurar horários e mensagens de aviso personalizadas.

## Funcionalidades

- Ativar ou desativar a funcionalidade principal.
- Adicionar múltiplos horários (hora e minuto) para notificações.
- Remover horários cadastrados facilmente.
- Definir quantos minutos antes do horário será exibida a mensagem de aviso.
- Personalizar a mensagem de aviso utilizando o placeholder `{minutos}`.

## Sobre o funcionamento

SRW é um aplicativo que fica minimizado na bandeja do sistema (systray/tryicon).  
É necessário que o aplicativo esteja rodando para que as notificações funcionem corretamente no SCUM.

## Pré-requisitos

- [SDK do .NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows 10 ou superior

## Como executar (para desenvolvedores)

1. Clone o repositório:
2. Abra a solução no Visual Studio 2022.
3. Restaure os pacotes NuGet.
4. Compile a solução.

Para rodar:
- Pressione `F5` no Visual Studio ou execute:

## Uso

- Acesse a janela de configurações para adicionar horários e definir a mensagem de aviso.
- Configure quantos minutos antes do horário deseja ser notificado.
- Personalize a mensagem conforme sua necessidade.

## Tecnologias

- WPF (.NET 9)
- [HandyControl](https://github.com/HandyOrg/HandyControl) para componentes visuais aprimorados

## Licença

Este projeto está licenciado sob a licença MIT.
