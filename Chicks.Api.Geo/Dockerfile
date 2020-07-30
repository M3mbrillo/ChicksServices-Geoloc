#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Chicks.Api.Geo/Chicks.Api.Geo.csproj", "Chicks.Api.Geo/"]
RUN dotnet restore "Chicks.Api.Geo/Chicks.Api.Geo.csproj"
COPY . .
WORKDIR "/src/Chicks.Api.Geo"
RUN dotnet build "Chicks.Api.Geo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chicks.Api.Geo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chicks.Api.Geo.dll"]
