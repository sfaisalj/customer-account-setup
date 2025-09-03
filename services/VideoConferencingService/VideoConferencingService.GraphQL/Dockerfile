FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VideoConferencingService.GraphQL/VideoConferencingService.GraphQL.csproj", "VideoConferencingService.GraphQL/"]
COPY ["VideoConferencingService.Core/VideoConferencingService.Core.csproj", "VideoConferencingService.Core/"]
COPY ["VideoConferencingService.Adapters/VideoConferencingService.Adapters.csproj", "VideoConferencingService.Adapters/"]
RUN dotnet restore "VideoConferencingService.GraphQL/VideoConferencingService.GraphQL.csproj"
COPY . .
WORKDIR "/src/VideoConferencingService.GraphQL"
RUN dotnet build "VideoConferencingService.GraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VideoConferencingService.GraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoConferencingService.GraphQL.dll"]