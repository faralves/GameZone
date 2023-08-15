#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Source/0-Presentation/APIs/GameZone.Blog.API/GameZone.Blog.API.csproj", "Source/0-Presentation/APIs/GameZone.Blog.API/"]
RUN dotnet restore "Source/0-Presentation/APIs/GameZone.Blog.API/GameZone.Blog.API.csproj"
COPY . .
WORKDIR "/src/Source/0-Presentation/APIs/GameZone.Blog.API"
RUN dotnet build "GameZone.Blog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameZone.Blog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameZone.Blog.API.dll"]