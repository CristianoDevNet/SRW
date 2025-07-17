# SRW

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

## Como executar

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