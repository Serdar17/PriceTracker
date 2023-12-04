FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/PriceTracker.Bot/PriceTracker.Bot.csproj", "src/Presentation/PriceTracker.Bot/"]
COPY ["src/Shared/PriceTracker.Common/PriceTracker.Common.csproj", "src/Shared/PriceTracker.Common/"]
COPY ["src/Infrastructure/PriceTracker.Infrastructure.Context/PriceTracker.Infrastructure.Context.csproj", "src/Infrastructure/PriceTracker.Infrastructure.Context/"]
COPY ["src/Shared/PriceTracker.Settings/PriceTracker.Settings.csproj", "src/Shared/PriceTracker.Settings/"]
COPY ["src/Domain/PriceTracker.Domain/PriceTracker.Domain.csproj", "src/Domain/PriceTracker.Domain/"]
COPY ["src/Infrastructure/PriceTracker.BackgroundJob/PriceTracker.BackgroundJob.csproj", "src/Infrastructure/PriceTracker.BackgroundJob/"]
RUN dotnet restore "src/Presentation/PriceTracker.Bot/PriceTracker.Bot.csproj"
COPY . .
WORKDIR "/src/src/Presentation/PriceTracker.Bot"
RUN dotnet build "PriceTracker.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PriceTracker.Bot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceTracker.Bot.dll"]
