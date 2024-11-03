FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QueueWebApplication.API/QueueWebApplication.API.csproj", "QueueWebApplication.API/"]
RUN dotnet restore "QueueWebApplication.API/QueueWebApplication.API.csproj"
COPY . .
WORKDIR "/src/QueueWebApplication.API"
RUN dotnet build "QueueWebApplication.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "QueueWebApplication.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QueueWebApplication.API.dll"]
