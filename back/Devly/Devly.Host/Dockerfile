FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS dotnet-build
WORKDIR /src
COPY . .
RUN dotnet publish "/src/Devly.Host/Devly.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=dotnet-build /app/publish .
COPY Devly.Host/appsettings* /app/

ENTRYPOINT ["dotnet", "Devly.Host.dll"]