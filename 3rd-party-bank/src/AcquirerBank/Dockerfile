FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY 3rd-party-bank/src/AcquirerBank/AcquirerBank.csproj AcquirerBank/
RUN dotnet restore AcquirerBank/AcquirerBank.csproj

COPY . .
WORKDIR 3rd-party-bank/src/AcquirerBank
RUN dotnet build -c Release -o /app/build AcquirerBank.csproj

FROM build AS publish

COPY . .
WORKDIR 3rd-party-bank/src/AcquirerBank
RUN dotnet publish -c Release -o /app/publish AcquirerBank.csproj

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcquirerBank.dll"]
