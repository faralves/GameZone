#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS debug
RUN tdnf install procps-ng -y

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Source
COPY ["./00-Building Blocks/Core/GameZone.Core/GameZone.Core.csproj", "00-Building Blocks/Core/GameZone.Core/"]
COPY ["./00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/GameZone.WebAPI.Core.csproj", "00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/"]
COPY ["./Identidade/0-Presentation/API/GameZone.Identidade.API/GameZone.Identidade.API.csproj", "Identidade/0-Presentation/API/GameZone.Identidade.API/"]
COPY ["./Identidade/1-Application/GameZone.Identidade.Application/GameZone.Identidade.Application.csproj", "Identidade/1-Application/GameZone.Identidade.Application/"]
COPY ["./Identidade/2-Services/GameZone.Identidade.Services/GameZone.Identidade.Services.csproj", "Identidade/2-Services/GameZone.Identidade.Services/"]
COPY ["./Identidade/3-Domain/GameZone.Identidade.Domain/GameZone.Identidade.Domain.csproj", "Identidade/3-Domain/GameZone.Identidade.Domain/"]
COPY ["./Identidade/4-Infra/GameZone.Identidade.Infra/GameZone.Identidade.Infra.csproj", "Identidade/4-Infra/GameZone.Identidade.Infra/"]
COPY ["./Identidade/4-Infra/GameZone.Identidade.Infra/Configurations/InitialSetup-azure.sql", "/app/Configurations/InitialSetup-azure.sql"]

RUN dotnet restore "Identidade/0-Presentation/API/GameZone.Identidade.API/GameZone.Identidade.API.csproj"
COPY . .
WORKDIR "Identidade/0-Presentation/API/GameZone.Identidade.API"
RUN dotnet build "GameZone.Identidade.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameZone.Identidade.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameZone.Identidade.API.dll"]