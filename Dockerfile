#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "./WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApi/WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi/WebApi.dll"]