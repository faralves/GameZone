# <h1 align="center"> Tech Challenge - Grupo 31 - Fase 3</h1>
<h3 align="center">Desenvolvimento do projeto GameZone</h3>

## 📚 Sobre o projeto

O projeto tem como objetivo criar uma solução para gestão de notícias para o portal da GameZone, manipulando informações em uma base de dados SQL Server.
O projeto está sendo desenvolvido em grupo, com o objetivo de compartilhar conhecimentos e experiências e atender os requisitos avaliativos do Tech Challenge FIAP do curso postech ARQUITETURA DE SISTEMAS .NET COM AZURE na fase 3.

## 📝 Conteúdo

- [Sobre o projeto](#-sobre-o-projeto)

## Configuração do ambiente

### 📋 Pré-requisitos

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Sql Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)

### 🎲 Banco de dados
A configuração do banco de dados é feita através do arquivo appsettings.json, que fica na raiz do projeto da GameZone.Blog.API. 
O arquivo já está configurado para o banco de dados **Sql Server** local, mas caso queira utilizar outro banco de dados, basta alterar a string de conexão. Você pode configurar também
a váriavel `ConnectionLocal` e `Connection` que pode conter o endereço do banco remoto, no caso deste projeto ele será publicado no Azure. 

```json
 "ConnectionStrings": {
    "ConnectionLocal": "Server=gamezone.sqlserver;Database=GameZoneDB;User Id=<SEU_USUARIO>;Password=<SUA_SENHA>;MultipleActiveResultSets=true;TrustServerCertificate=true;",
    "Connection": "Server=tcp:GameZoneServer.database.windows.net,1433;Initial Catalog=gamezoneDB;Persist Security Info=False;User ID=<SEU_USUARIO>;Password=<SUA_SENHA>;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;"
  }
```

## 🚀 Como configurar e executar o projeto
Para esta etapad do projeto contamos com um script com a sequencia a ser executada, estamos utilizando o portal do Azure.


### Efetuando o login
az login

### Definindo a subscription
az account set --subscription <insira sua subscription>

### Criando um resorce group
az group create --name <rg-GameZone-trocar-o-nome> --location westus

az ad sp create-for-rbac --name "GameZone" --role contributor --scopes /subscriptions/<insira sua subscription>/resourceGroups/<rg-GameZone-trocar-o-nome> --sdk-auth

### Irá gerar um retorno como esse abaixo, inserir ele nas suas secrets no gitHub
### Retorno
{
{  "clientId": "",
{  "clientSecret": "",
{  "subscriptionId": "",
{  "tenantId": "",
{  "activeDirectoryEndpointUrl": "",
{  "resourceManagerEndpointUrl": "",
{  "activeDirectoryGraphResourceId": "",
{  "sqlManagementEndpointUrl": "",
{  "galleryEndpointUrl": "",
{  "managementEndpointUrl": ""
{

### Criando um storage account
az storage account create --name <gamezonetech-trocar-o-nome> --resource-group <rg-GameZone-trocar-o-nome> --location westus --sku Standard_LRS 

### Criando um container blob
az storage container create --account-name <gamezonetech-trocar-o-nome> --name images --public-access blob

### Criando o ACI
#### Criando um volume no Azure File Share
az storage share create --account-name <gamezonetech-trocar-o-nome> --name https

### Definir o caminho dos certificados
cd C:/Projetos/TechChalenge/Fase2/GameZone/certs
az storage file upload --source ./gamezone-certificate.pfx --share-name https --account-name <gamezonetech-trocar-o-nome>
az storage file upload --source ./gamezone-identidade-certificate.pfx --share-name https --account-name <gamezonetech-trocar-o-nome>


### Criando um Server
az sql server create --name <GameZoneServer-trocar-o-nome> --resource-group <rg-GameZone-trocar-o-nome> --location westus --admin-user <insira seu usuário> --admin-password <insira sua senha>

### Criando SQL DataBase
az sql db create --resource-group <rg-GameZone-trocar-o-nome> --server <GameZoneServer-trocar-o-nome> --name <gamezoneDB-trocar-o-nome> --backup-storage-redundancy Local --sample-name AdventureWorksLT --edition GeneralPurpose --compute-model Serverless --family Gen5 --capacity 2

### Liberando acesso do ip para ao BD para acesso local
az sql server firewall-rule create --resource-group <rg-GameZone-trocar-o-nome> --server <GameZoneServer-trocar-o-nome> -n PermissaoFaixaIpLocal --start-ip-address <SEU_IP> --end-ip-address <SEU_IP>

### Liberando acesso do ip para ao BD para acesso da API
az sql server firewall-rule create --resource-group <rg-GameZone-trocar-o-nome> --server <GameZoneServer-trocar-o-nome> --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

### Obtendo a connection string do blob storage
az storage account show-connection-string --resource-group <rg-GameZone-trocar-o-nome> --name <gamezonetech-trocar-o-nome>

#### Irá retornar a connection string do blob storage
{
 "connectionString": ""
}

### Obtendo a connection string sql server
az sql db show-connection-string --server GameZoneServer --name gamezoneDB --client ado.net
#"Server=tcp:GameZoneServer.database.windows.net,1433;Initial Catalog=gamezoneDB;Persist Security Info=False;User ID=<username>;Password=<password>;MultipleActiveResultSets=False;Encrypt=true;TrustServerCertificate=False;Connection Timeout=30;"

### Criando o Azure Container Registry:
az acr create --name <gamezoneacr-trocar-o-nome> --resource-group <rg-GameZone-trocar-o-nome> --location westus --sku Basic --admin-enabled true

### Retorno das credenciais do ACR:
az acr credential show --name <gamezoneacr-trocar-o-nome>

### Login no seu ACR
az acr login --name gamezoneacr.azurecr.io -u <SEU_USUARIO> -p <SUA_SENHA>

az acr login --name gamezoneacr.azurecr.io -u gamezoneacr -p <SUA_SENHA>

## 🏗️ DEVOPS
Para implementação desta solução no que tange ao DEVOPS estamos utilizando o portal do Azure para provisonar os recursos utilizados e integramos o app com a ferramenta de CI/CD [GitHub Actions](https://github.com/faralves/GameZone/actions).


## 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [C#](https://docs.microsoft.com/pt-br/dotnet/csharp/) - Linguagem
- [.NET](https://docs.microsoft.com/pt-br/dotnet/) - Framework
- [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet/apis) - Asp.NET Core WebAPI
- [ADO.NET](https://learn.microsoft.com/en-us/ef/core/) - EntityFramework Core
- [Swagger](https://swagger.io/) - Documentação da API

## ✒️ Colaborador(as/es)

- **Fernando Augusto Ribeiro Alves** - _Desenvolvedor_  - [Faralves](https://github.com/faralves)
- **André Leão da Silva** - _Desenvolvedor_ - [andreleaos](https://github.com/andreleaos)
- **André Bessa da Silva** - _Desenvolvedor_  - [bessax](https://github.com/bessax)
- **Liandro Freire dos Anjos** - _Desenvolvedor_  - [liandro](oliverliandro@gmail.com)
- **Diogo da Franca Rodrigues** - _Desenvolvedor_  - [diogo](diogo_f.rodrigues@hotmail.com)

