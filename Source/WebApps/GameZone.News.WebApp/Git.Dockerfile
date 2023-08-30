#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
EXPOSE 80
EXPOSE 443

FROM base AS debug
RUN tdnf install procps-ng -y

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /.
COPY ["./Source/00-Building Blocks/Core/GameZone.Core/GameZone.Core.csproj", "00-Building Blocks/Core/GameZone.Core/"]
COPY ["./Source/00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/GameZone.WebAPI.Core.csproj", "00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/"]
COPY ["./Source/WebApps/GameZone.News.WebApp/GameZone.News.WebApp.csproj", "WebApps/GameZone.News.WebApp/"]

RUN dotnet restore "Source/00-Building Blocks/Core/GameZone.Core/GameZone.Core.csproj"
COPY . .
WORKDIR "00-Building Blocks/Core/GameZone.Core"
RUN dotnet build "GameZone.Core.csproj" -c Release -o /app/build

RUN dotnet restore "Source/00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/GameZone.WebAPI.Core.csproj"
COPY . .
WORKDIR "00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core"
RUN dotnet build "GameZone.WebAPI.Core.csproj" -c Release -o /app/build

RUN dotnet restore "Source/WebApps/GameZone.News.WebApp/GameZone.News.WebApp.csproj"
COPY . .
WORKDIR "WebApps/GameZone.News.WebApp"
RUN dotnet build "GameZone.News.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameZone.News.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameZone.News.WebApp.dll"]