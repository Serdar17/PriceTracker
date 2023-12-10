FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/PriceTracker.Bot/PriceTracker.Bot.csproj", "src/Presentation/PriceTracker.Bot/"]
COPY ["src/Infrastructure/PriceTracker.BackgroundJob/PriceTracker.BackgroundJob.csproj", "src/Infrastructure/PriceTracker.BackgroundJob/"]
COPY ["src/Infrastructure/PriceTracker.Infrastructure.Context/PriceTracker.Infrastructure.Context.csproj", "src/Infrastructure/PriceTracker.Infrastructure.Context/"]
COPY ["src/Domain/PriceTracker.Domain/PriceTracker.Domain.csproj", "src/Domain/PriceTracker.Domain/"]
COPY ["src/Shared/PriceTracker.Settings/PriceTracker.Settings.csproj", "src/Shared/PriceTracker.Settings/"]
COPY ["src/Services/PriceTracker.Services.User/PriceTracker.Services.User.csproj", "src/Services/PriceTracker.Services.User/"]
COPY ["src/Shared/PriceTracker.Common/PriceTracker.Common.csproj", "src/Shared/PriceTracker.Common/"]
COPY ["src/Services/PriceTracker.Services.Product/PriceTracker.Services.Product.csproj", "src/Services/PriceTracker.Services.Product/"]
COPY ["src/Infrastructure/PriceTracker.Command/PriceTracker.Command.csproj", "src/Infrastructure/PriceTracker.Command/"]
RUN dotnet restore "src/Presentation/PriceTracker.Bot/PriceTracker.Bot.csproj"
COPY . .
WORKDIR "/src/src/Presentation/PriceTracker.Bot"
RUN dotnet build "PriceTracker.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PriceTracker.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceTracker.Bot.dll"]
