#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["1.API/1_API.csproj", "1.API/"]
COPY ["2.Repositories/2_Repositories.csproj", "2.Repositories/"]
COPY ["3.Model/3_Model.csproj", "3.Model/"]
COPY ["4.Shared/4_Shared.csproj", "4.Shared/"]
RUN dotnet restore "./1.API/./1_API.csproj"
COPY . .
WORKDIR "/src/1.API"
RUN dotnet build "./1_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./1_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "1_API.dll"]