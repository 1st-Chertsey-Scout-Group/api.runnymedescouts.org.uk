FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN ls -l
RUN dotnet restore "./API/RunnymedeScouts.API.csproj"
RUN dotnet build "./API/RunnymedeScouts.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN ls -l


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN ls -l
RUN dotnet publish "./API/RunnymedeScouts.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN ls -l

FROM base AS final
RUN ls -l
WORKDIR /app
RUN ls -l
COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT ["dotnet", "RunnymedeScouts.API.dll"]