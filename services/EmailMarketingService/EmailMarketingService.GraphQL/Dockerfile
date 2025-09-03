FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EmailMarketingService.GraphQL/EmailMarketingService.GraphQL.csproj", "EmailMarketingService.GraphQL/"]
COPY ["EmailMarketingService.Core/EmailMarketingService.Core.csproj", "EmailMarketingService.Core/"]
COPY ["EmailMarketingService.Adapters/EmailMarketingService.Adapters.csproj", "EmailMarketingService.Adapters/"]
RUN dotnet restore "EmailMarketingService.GraphQL/EmailMarketingService.GraphQL.csproj"
COPY . .
WORKDIR "/src/EmailMarketingService.GraphQL"
RUN dotnet build "EmailMarketingService.GraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailMarketingService.GraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailMarketingService.GraphQL.dll"]