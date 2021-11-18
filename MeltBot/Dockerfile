#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MeltBot/nuget.config", "MeltBot/"]
COPY . .
COPY ["MeltBot/MeltBot.csproj", "MeltBot/"]
COPY ["PassionLib/PassionLib.csproj", "PassionLib/"]
RUN dotnet restore "MeltBot/MeltBot.csproj"
COPY . .
WORKDIR "/src/MeltBot"
RUN dotnet build "MeltBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeltBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeltBot.dll"]