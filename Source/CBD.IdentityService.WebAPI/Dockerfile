FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Source/CBD.IdentityService.WebAPI/CBD.IdentityService.WebAPI.csproj", "Source/CBD.IdentityService.WebAPI/"]
RUN dotnet restore "Source/CBD.IdentityService.WebAPI/CBD.IdentityService.WebAPI.csproj"
COPY . .
WORKDIR "/src/Source/CBD.IdentityService.WebAPI"
RUN dotnet build "CBD.IdentityService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CBD.IdentityService.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CBD.IdentityService.WebAPI.dll"]
