FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS dotnet-build
COPY /back /back
RUN dotnet publish "/back/Devly/Devly.Host/Devly.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=dotnet-build /app/publish .
COPY --from=dotnet-build /back/Devly/photos /photos

ENTRYPOINT ["dotnet", "Devly.Host.dll"]
