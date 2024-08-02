# Estágio base para a aplicação ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081
EXPOSE 8082

# Estágio de build para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto e restaura as dependências
COPY ["_NotificationService/_NotificationService.csproj", "NotificationService/"]
COPY ["NotificationService.Core/NotificationService.Core.csproj", "NotificationService.Core/"]
COPY ["NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService.Infrastructure/"]
RUN dotnet restore "NotificationService/_NotificationService.csproj"

# Copia todos os arquivos para o contêiner e realiza o build
COPY . .
WORKDIR "/src/NotificationService"
RUN dotnet build "_NotificationService.csproj" -c Release -o /app/build

# Estágio de publicação para publicar a aplicação compilada
FROM build AS publish
RUN dotnet publish "_NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final para configurar a aplicação
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "_NotificationService.dll"]