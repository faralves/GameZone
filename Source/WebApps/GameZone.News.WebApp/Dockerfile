#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# =======================================================
# Stage 1 - Build/compile app using container
# =======================================================

ARG IMAGE_BASE=6.0-alpine

#FROM mcr.microsoft.com/dotnet/aspnet:$IMAGE_BASE AS base
#WORKDIR /app
#RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
#EXPOSE 80
#EXPOSE 443

# Build image has SDK and tools (Linux)
FROM mcr.microsoft.com/dotnet/sdk:$IMAGE_BASE AS build
WORKDIR /.

# Copy project source files
COPY ./Source ./Source

# Restore, build & publish
WORKDIR /Source/WebApps/GameZone.News.WebApp
RUN dotnet restore
RUN dotnet publish --no-restore --configuration Release

# =======================================================
# Stage 2 - Assemble runtime image from previous stage
# =======================================================

# Base image is .NET Core runtime only (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:$IMAGE_BASE

# Seems as good a place as any
WORKDIR /app

# Copy already published binaries (from build stage image)
COPY --from=build /Source/WebApps/GameZone.News.WebApp/bin/Release/net6.0/publish/ .

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS http://*:7043
ENV ASPNETCORE_Kestrel__Certificates__Default__Password 123@Fiap
ENV ASPNETCORE_Kestrel__Certificates__Default__Path /https/gamezone-certificate.pfx

# Run the ASP.NET Core app
ENTRYPOINT dotnet GameZone.News.WebApp.dll

#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /.
#COPY ["./Source/00-Building Blocks/Core/GameZone.Core/GameZone.Core.csproj", "00-Building Blocks/Core/GameZone.Core/"]
#COPY ["./Source/00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/GameZone.WebAPI.Core.csproj", "00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/"]
#COPY ["./Source/WebApps/GameZone.News.WebApp/GameZone.News.WebApp.csproj", "WebApps/GameZone.News.WebApp/"]

#RUN dotnet restore "WebApps/GameZone.News.WebApp/GameZone.News.WebApp.csproj"
#COPY . .
#WORKDIR "WebApps/GameZone.News.WebApp"
#RUN dotnet build "GameZone.News.WebApp.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "GameZone.News.WebApp.csproj" -c Release -o /app/publish 

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "GameZone.News.WebApp.dll"]