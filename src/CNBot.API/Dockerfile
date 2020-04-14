#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CNBot.API/CNBot.API.csproj", "CNBot.API/"]
COPY ["CNBot.Infrastructure/CNBot.Infrastructure.csproj", "CNBot.Infrastructure/"]
COPY ["CNBot.Core/CNBot.Core.csproj", "CNBot.Core/"]
RUN dotnet restore "CNBot.API/CNBot.API.csproj"
COPY . .
WORKDIR "/src/CNBot.API"
RUN dotnet build "CNBot.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CNBot.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CNBot.API.dll"]
