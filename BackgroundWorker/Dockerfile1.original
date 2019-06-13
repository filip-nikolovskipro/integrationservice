FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BackgroundWorker/BackgroundWorker.csproj", "BackgroundWorker/"]
RUN dotnet restore "BackgroundWorker/BackgroundWorker.csproj"
COPY . .
WORKDIR "/src/BackgroundWorker"
RUN dotnet build "BackgroundWorker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BackgroundWorker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BackgroundWorker.dll"]
