# Build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CountryExplorer.API/CountryExplorer.API.csproj", "CountryExplorer.API/"]
COPY ["CountryExplorer.Domain/CountryExplorer.Domain.csproj", "CountryExplorer.Domain/"]
COPY ["CountryExplorer.Shared/CountryExplorer.Shared.csproj", "CountryExplorer.Shared/"]
RUN dotnet restore "CountryExplorer.API/CountryExplorer.API.csproj"
COPY . .
WORKDIR "/src/CountryExplorer.API"
RUN dotnet build "CountryExplorer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CountryExplorer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CountryExplorer.API.dll"] 