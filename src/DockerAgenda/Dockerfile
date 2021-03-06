#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt update && apt upgrade && apt install curl -y

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/DockerAgenda/DockerAgenda.csproj", "src/DockerAgenda/"]
RUN dotnet restore "src/DockerAgenda/DockerAgenda.csproj"
COPY . .
WORKDIR "/src/src/DockerAgenda"
RUN dotnet build "DockerAgenda.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerAgenda.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerAgenda.dll"]