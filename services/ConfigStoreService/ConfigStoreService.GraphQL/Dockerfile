FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ConfigStoreService.GraphQL/ConfigStoreService.GraphQL.csproj", "ConfigStoreService.GraphQL/"]
COPY ["ConfigStoreService.Core/ConfigStoreService.Core.csproj", "ConfigStoreService.Core/"]
COPY ["ConfigStoreService.Adapters/ConfigStoreService.Adapters.csproj", "ConfigStoreService.Adapters/"]
RUN dotnet restore "ConfigStoreService.GraphQL/ConfigStoreService.GraphQL.csproj"
COPY . .
WORKDIR "/src/ConfigStoreService.GraphQL"
RUN dotnet build "ConfigStoreService.GraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigStoreService.GraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigStoreService.GraphQL.dll"]