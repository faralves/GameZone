#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# =======================================================
# Stage 1 - Build/compile app using container
# =======================================================

ARG IMAGE_BASE=6.0-alpine

# Build image has SDK and tools (Linux)
FROM mcr.microsoft.com/dotnet/sdk:$IMAGE_BASE AS build
WORKDIR /.

# Copy project source files
COPY ./Source ./Source

# Restore, build & publish
WORKDIR /Source/Identidade/0-Presentation/API/GameZone.Identidade.API
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
COPY --from=build /Source/Identidade/0-Presentation/API/GameZone.Identidade.API/bin/Release/net6.0/publish/ .

EXPOSE 80
EXPOSE 443
EXPOSE 5501

ENV ASPNETCORE_ENVIRONMENT Development
ENV ASPNETCORE_URLS https://+:5501;http://+5502
ENV ASPNETCORE_Kestrel__Certificates__Default__Password 123@Fiap
ENV ASPNETCORE_Kestrel__Certificates__Default__Path /https/gamezone-certificate.pfx

# Run the ASP.NET Core app
ENTRYPOINT dotnet GameZone.Identidade.API.dll

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
#EXPOSE 80
#EXPOSE 443
#
#FROM base AS debug
#RUN tdnf install procps-ng -y
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /Source
#COPY ["./00-Building Blocks/Core/GameZone.Core/GameZone.Core.csproj", "00-Building Blocks/Core/GameZone.Core/"]
#COPY ["./00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/GameZone.WebAPI.Core.csproj", "00-Building Blocks/WebAPI.Core/GameZone.WebAPI.Core/"]
#COPY ["./Identidade/0-Presentation/API/GameZone.Identidade.API/GameZone.Identidade.API.csproj", "Identidade/0-Presentation/API/GameZone.Identidade.API/"]
#COPY ["./Identidade/1-Application/GameZone.Identidade.Application/GameZone.Identidade.Application.csproj", "Identidade/1-Application/GameZone.Identidade.Application/"]
#COPY ["./Identidade/2-Services/GameZone.Identidade.Services/GameZone.Identidade.Services.csproj", "Identidade/2-Services/GameZone.Identidade.Services/"]
#COPY ["./Identidade/3-Domain/GameZone.Identidade.Domain/GameZone.Identidade.Domain.csproj", "Identidade/3-Domain/GameZone.Identidade.Domain/"]
#COPY ["./Identidade/4-Infra/GameZone.Identidade.Infra/GameZone.Identidade.Infra.csproj", "Identidade/4-Infra/GameZone.Identidade.Infra/"]
#COPY ["./Identidade/4-Infra/GameZone.Identidade.Infra/Configurations/InitialSetup-azure.sql", "/app/Configurations/InitialSetup-azure.sql"]
#
#RUN dotnet restore "Identidade/0-Presentation/API/GameZone.Identidade.API/GameZone.Identidade.API.csproj"
#COPY . .
#WORKDIR "Identidade/0-Presentation/API/GameZone.Identidade.API"
#RUN dotnet build "GameZone.Identidade.API.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "GameZone.Identidade.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "GameZone.Identidade.API.dll"]