FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY BackgroundWorker/BackgroundWorker.csproj BackgroundWorker/
RUN dotnet restore BackgroundWorker/BackgroundWorker.csproj
COPY . .
WORKDIR /src/BackgroundWorker
RUN dotnet build BackgroundWorker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BackgroundWorker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BackgroundWorker.dll"]
