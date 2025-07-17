# SRW

SRW � uma aplica��o WPF para Windows, desenvolvida em .NET 9, que permite configurar hor�rios e mensagens de aviso personalizadas.

## Funcionalidades

- Ativar ou desativar a funcionalidade principal.
- Adicionar m�ltiplos hor�rios (hora e minuto) para notifica��es.
- Remover hor�rios cadastrados facilmente.
- Definir quantos minutos antes do hor�rio ser� exibida a mensagem de aviso.
- Personalizar a mensagem de aviso utilizando o placeholder `{minutos}`.

## Sobre o funcionamento

SRW � um aplicativo que fica minimizado na bandeja do sistema (systray/tryicon).  
� necess�rio que o aplicativo esteja rodando para que as notifica��es funcionem corretamente no SCUM.

## Pr�-requisitos

- [SDK do .NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows 10 ou superior

## Como executar

1. Clone o reposit�rio:
2. Abra a solu��o no Visual Studio 2022.
3. Restaure os pacotes NuGet.
4. Compile a solu��o.

Para rodar:
- Pressione `F5` no Visual Studio ou execute:

## Uso

- Acesse a janela de configura��es para adicionar hor�rios e definir a mensagem de aviso.
- Configure quantos minutos antes do hor�rio deseja ser notificado.
- Personalize a mensagem conforme sua necessidade.

## Tecnologias

- WPF (.NET 9)
- [HandyControl](https://github.com/HandyOrg/HandyControl) para componentes visuais aprimorados

## Licen�a

Este projeto est� licenciado sob a licen�a MIT.