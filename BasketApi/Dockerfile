#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BasketApi/BasketApi.csproj", "BasketApi/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]
RUN dotnet restore "BasketApi/BasketApi.csproj"
COPY . .
WORKDIR "/src/BasketApi"
RUN dotnet build "BasketApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketApi.dll"]