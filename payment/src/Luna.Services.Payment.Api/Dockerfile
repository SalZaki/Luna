FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY lib/src/Luna.Framework.AspNetCore/Luna.Framework.AspNetCore.csproj Luna.Framework.AspNetCore/
COPY lib/src/Luna.Framework.Common/Luna.Framework.Common.csproj Luna.Framework.Common/
COPY payment/src/Luna.Services.Payment.Api/Luna.Services.Payment.Api.csproj Luna.Services.Payment.Api/
COPY payment/src/Luna.Services.Payment.Application/Luna.Services.Payment.Application.csproj Luna.Services.Payment.Application/
COPY payment/src/Luna.Services.Payment.Domain/Luna.Services.Payment.Domain.csproj Luna.Services.Payment.Domain/
COPY payment/src/Luna.Services.Payment.Infrastructure/Luna.Services.Payment.Infrastructure.csproj Luna.Services.Payment.Infrastructure/
RUN dotnet restore Luna.Services.Payment.Api/Luna.Services.Payment.Api.csproj

COPY . .
WORKDIR payment/src/Luna.Services.Payment.Api
RUN dotnet build -c Release -o /app/build Luna.Services.Payment.Api.csproj

FROM build AS publish

COPY . .
WORKDIR payment/src/Luna.Services.Payment.Api
RUN dotnet publish -c Release -o /app/publish Luna.Services.Payment.Api.csproj

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Luna.Services.Payment.Api.dll"]

#
#COPY . .
#WORKDIR "/src/payment/Luna.Services.Payment.Api"
#RUN dotnet build "Luna.Services.Payment.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Luna.Services.Payment.Api.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Luna.Services.Payment.Api.dll"]

#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#ARG PROJECT_PATH="payment/Luna.Services.Payment.Api/Luna.Services.Payment.Api.csproj"
#
## copy projects
#COPY ["./*.sln", "*/*.csproj", "./"]
## recreate solution folder structure
#RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
#
## restore NuGet packages
#RUN dotnet restore $PROJECT_PATH
#
## copy all project files
#COPY . .
#
## build project_
#RUN dotnet build -c Release -o /app/build $PROJECT_PATH
#
#FROM build AS publish
#ARG PROJECT_PATH="Luna.Services.Payment.Api/Luna.Services.Payment.Api.csproj"
#
#RUN dotnet publish -c Release -o /app/publish $PROJECT_PATH
#
#
#FROM base AS final
#WORKDIR /app
#
## copy published app to the final image
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Luna.Services.Payment.Api.dll"]
#
