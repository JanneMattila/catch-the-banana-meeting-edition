#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 1. Build image(https://hub.docker.com/_/microsoft-dotnet-sdk)
FROM mcr.microsoft.com/dotnet/sdk:7.0.306-bullseye-slim-amd64 AS build

WORKDIR /node
ENV NODE_VERSION 18.17.0
RUN apt-get update -qq && \
    apt-get dist-upgrade --no-install-recommends -y && \
    apt-get install -y --no-install-recommends xz-utils

RUN curl -fsSLO --compressed "https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.xz"
RUN tar -xf "/node/node-v$NODE_VERSION-linux-x64.tar.xz"
ENV PATH=$PATH:/node/node-v$NODE_VERSION-linux-x64/bin

# Cache nuget restore
WORKDIR /build
COPY ["src/CTB/Server/CTB.Server.csproj", "src/CTB/Server/"]
COPY ["src/CTB/Client/CTB.Client.csproj", "src/CTB/Client/"]
COPY ["src/CTB/Shared/CTB.Shared.csproj", "src/CTB/Shared/"]
RUN dotnet restore "src/CTB/Server/CTB.Server.csproj"

# Copy sources and compile
COPY . .
WORKDIR "/build/src/CTB/Server"
RUN dotnet build "CTB.Server.csproj" -c Release -o /app/build
RUN dotnet publish "CTB.Server.csproj" -c Release -o /app/publish

# 2. Release image
FROM mcr.microsoft.com/dotnet/aspnet:7.0.9-alpine3.18-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy content from build image
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CTB.Server.dll"]
