FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /gooddictionary

ENV COMPlus_EnableDiagnostics=0
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

COPY ["src/MoneyControl.Application/MoneyControl.Application.csproj", "src/MoneyControl.Application/"]
COPY ["src/MoneyControl.Client/MoneyControl.Client.csproj", "src/MoneyControl.Client/"]
COPY ["src/MoneyControl.Core/MoneyControl.Core.csproj", "src/MoneyControl.Core/"]
COPY ["src/MoneyControl.Infrastructure/MoneyControl.Infrastructure.csproj", "src/MoneyControl.Infrastructure/"]
COPY ["src/MoneyControl.Server/MoneyControl.Server.csproj", "src/MoneyControl.Server/"]
COPY ["src/MoneyControl.Shared/MoneyControl.Shared.csproj", "src/MoneyControl.Shared/"]

RUN dotnet restore "src/MoneyControl.Server/MoneyControl.Server.csproj"

# Copy all
COPY . .

# Build -> /app/build
WORKDIR "/MoneyControl/src/MoneyControl.Server"
RUN dotnet build "MoneyControl.Server.csproj" -c Release --output /app/build

# Publish (use sdk) -> /app/publish
FROM build AS publish
RUN dotnet publish "MoneyControl.Server.csproj" -c Release --output /app/publish

# Runtime image (use aspnet)
FROM base
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "MoneyControl.Server.dll"]
