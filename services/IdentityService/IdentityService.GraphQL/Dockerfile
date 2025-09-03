FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IdentityService.GraphQL/IdentityService.GraphQL.csproj", "IdentityService.GraphQL/"]
COPY ["IdentityService.Core/IdentityService.Core.csproj", "IdentityService.Core/"]
COPY ["IdentityService.Adapters/IdentityService.Adapters.csproj", "IdentityService.Adapters/"]
RUN dotnet restore "IdentityService.GraphQL/IdentityService.GraphQL.csproj"
COPY . .
WORKDIR "/src/IdentityService.GraphQL"
RUN dotnet build "IdentityService.GraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.GraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.GraphQL.dll"]