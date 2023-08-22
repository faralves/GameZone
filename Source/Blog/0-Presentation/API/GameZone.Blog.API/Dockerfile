#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS debug
RUN tdnf install procps-ng -y

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Source
COPY ["./00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/GameZone.WebAPI.Core.csproj", "00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/"]
COPY ["./Blog/0-Presentation/API/GameZone.Blog.API/GameZone.Blog.API.csproj", "Blog/0-Presentation/API/GameZone.Blog.API/"]
COPY ["./Blog/1-Application/GameZone.Blog.Application/GameZone.Blog.Application.csproj", "Blog/1-Application/GameZone.Blog.Application/"]
COPY ["./Blog/2-Services/GameZone.Blog.Services/GameZone.Blog.Services.csproj", "Blog/2-Services/GameZone.Blog.Services/"]
COPY ["./Blog/3-Domain/GameZone.Blog.Domain/GameZone.Blog.Domain.csproj", "Blog/3-Domain/GameZone.Blog.Domain/"]
COPY ["./Blog/4-Infra/GameZone.Blog.Infra/GameZone.Blog.Infra.csproj", "Blog/4-Infra/GameZone.Blog.Infra/"]

RUN dotnet restore "Blog/0-Presentation/API/GameZone.Blog.API/GameZone.Blog.API.csproj"
COPY . .
WORKDIR "Blog/0-Presentation/API/GameZone.Blog.API"
RUN dotnet build "GameZone.Blog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameZone.Blog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameZone.Blog.API.dll"]