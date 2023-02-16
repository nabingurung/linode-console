FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["linode-console.csproj", "./"]
RUN dotnet restore "linode-console.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "linode-console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "linode-console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "linode-console.dll"]
