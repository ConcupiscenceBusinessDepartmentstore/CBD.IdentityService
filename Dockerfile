# 1. Build application in image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /Source

COPY ["Source/CBD.IdentityService.WebAPI/CBD.IdentityService.WebAPI.csproj", "CBD.IdentityService.WebAPI/"]
COPY ["Source/CBD.IdentityService.Port/CBD.IdentityService.Port.csproj", "CBD.IdentityService.Port/"]
COPY ["Source/CBD.IdentityService.Core/CBD.IdentityService.Core.csproj", "CBD.IdentityService.Core/"]

RUN dotnet restore "CBD.IdentityService.WebAPI/CBD.IdentityService.WebAPI.csproj"

COPY ./Source .
WORKDIR "/Source/CBD.IdentityService.WebAPI"

RUN dotnet build "CBD.IdentityService.WebAPI.csproj" -c Release -o /app/build

# 2. Publish built application in image
FROM build AS publish
RUN dotnet publish "CBD.IdentityService.WebAPI.csproj" -c Release -o /app/publish

# 3. Take published version
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS final
EXPOSE 7001
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet dev-certs https
