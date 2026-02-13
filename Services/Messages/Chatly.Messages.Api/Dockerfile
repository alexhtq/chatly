# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Services/Messages/Chatly.Messages.Api/Chatly.Messages.Api.csproj", "Chatly.Messages.Api/"]
RUN dotnet restore "Chatly.Messages.Api/Chatly.Messages.Api.csproj"
COPY ["Services/Messages/Chatly.Messages.Api", "Chatly.Messages.Api"]
COPY ["Chatly.Shared", "Chatly.Shared"]
WORKDIR "/src/Chatly.Messages.Api"
RUN dotnet publish "Chatly.Messages.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:SharedProjectPathForApi=../Chatly.Shared/Chatly.Shared.csproj

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Chatly.Messages.Api.dll"]