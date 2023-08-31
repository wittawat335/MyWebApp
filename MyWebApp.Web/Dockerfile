#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyWebApp.Web/MyWebApp.Web.csproj", "MyWebApp.Web/"]
COPY ["MyWebApp.Core/MyWebApp.Core.csproj", "MyWebApp.Core/"]
COPY ["MyWebApp.Infrastructure/MyWebApp.Infrastructure.csproj", "MyWebApp.Infrastructure/"]
RUN dotnet restore "MyWebApp.Web/MyWebApp.Web.csproj"
COPY . .
WORKDIR "/src/MyWebApp.Web"
RUN dotnet build "MyWebApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyWebApp.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebApp.Web.dll"]