# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Chatly.Frontend/Chatly.Frontend.csproj", "Chatly.Frontend/"]
RUN dotnet restore "Chatly.Frontend/Chatly.Frontend.csproj"
COPY ["Chatly.Frontend", "Chatly.Frontend"]
COPY ["Chatly.Shared", "Chatly.Shared"]
WORKDIR "/src/Chatly.Frontend"
RUN dotnet publish "Chatly.Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html