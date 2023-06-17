FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 277

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApplicationsApi.csproj", "./"]
RUN dotnet restore "ApplicationsApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ApplicationsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationsApi.dll"]
