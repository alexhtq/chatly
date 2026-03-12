# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Chatly.Frontend/Chatly.Frontend.csproj", "Chatly.Frontend/"]
RUN dotnet restore "Chatly.Frontend/Chatly.Frontend.csproj"
COPY ["src/Chatly.Frontend", "Chatly.Frontend"]
COPY ["src/Chatly.Shared", "Chatly.Shared"]
WORKDIR "/src/Chatly.Frontend"
RUN dotnet publish "Chatly.Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY src/Chatly.Frontend/default.conf /etc/nginx/conf.d/default.conf