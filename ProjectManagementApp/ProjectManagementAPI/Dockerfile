FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "ProjectManagementAPI/ProjectManagementAPI.csproj"

RUN dotnet build "ProjectManagementAPI/ProjectManagementAPI.csproj" -c Release -o /app/build

RUN dotnet publish "ProjectManagementAPI/ProjectManagementAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5225
ENV ASPNETCORE_URLS=http://+:5225

ENTRYPOINT ["dotnet", "ProjectManagementAPI.dll"]