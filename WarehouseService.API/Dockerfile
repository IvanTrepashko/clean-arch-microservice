#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WarehouseService.API/WarehouseService.API.csproj", "WarehouseService.API/"]
COPY ["WarehouseService.Application/WarehouseService.Application.csproj", "WarehouseService.Application/"]
COPY ["Warehouse.Shared/Warehouse.Shared.csproj", "Warehouse.Shared/"]
COPY ["WarehouseService.Domain/WarehouseService.Domain.csproj", "WarehouseService.Domain/"]
COPY ["WarehouseService.Infrastructure/WarehouseService.Infrastructure.csproj", "WarehouseService.Infrastructure/"]
RUN dotnet restore "WarehouseService.API/WarehouseService.API.csproj"
COPY . .
WORKDIR "/src/WarehouseService.API"
RUN dotnet build "WarehouseService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WarehouseService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseService.API.dll"]