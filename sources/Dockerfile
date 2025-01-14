#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Switch to root to set permissions
USER root
WORKDIR /app
# Install ffmpeg
RUN apt-get update \
    && apt-get install -y --no-install-recommends ffmpeg=7:5.1.5-0+deb12u1 \
    && rm -rf /var/lib/apt/lists/*
# Create the necessary directories and set permissions for the app user
RUN mkdir -p /app/athv1/posts && chown -R app:app /app
RUN mkdir -p /app/athv1/thumbnails && chown -R app:app /app
# Switch back to the app user
USER app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/API.csproj", "API/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "./API/./API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
