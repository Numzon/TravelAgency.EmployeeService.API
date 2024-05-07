#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["TravelAgency.EmployeeService.API/TravelAgency.EmployeeService.API.csproj", "TravelAgency.EmployeeService.API/"]
COPY ["TravelAgency.EmployeeService.Application/TravelAgency.EmployeeService.Application.csproj", "TravelAgency.EmployeeService.Application/"]
COPY ["TravelAgency.EmployeeService.Domain/TravelAgency.EmployeeService.Domain.csproj", "TravelAgency.EmployeeService.Domain/"]
COPY ["TravelAgency.EmployeeService.Infrastructure/TravelAgency.EmployeeService.Infrastructure.csproj", "TravelAgency.EmployeeService.Infrastructure/"]
RUN dotnet restore "TravelAgency.EmployeeService.API/TravelAgency.EmployeeService.API.csproj"
COPY . .
WORKDIR "/src/TravelAgency.EmployeeService.API"
RUN dotnet build "TravelAgency.EmployeeService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TravelAgency.EmployeeService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TravelAgency.EmployeeService.API.dll"]