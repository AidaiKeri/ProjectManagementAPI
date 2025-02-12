FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProjectManagementAPI.csproj", "./"]
RUN dotnet restore "ProjectManagementAPI.csproj"

RUN dotnet tool install --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"

COPY . .
RUN dotnet build "ProjectManagementAPI.csproj" -c Release -o /app/build
RUN dotnet publish "ProjectManagementAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5225

ENV ASPNETCORE_URLS=http://+:5225

ENTRYPOINT ["dotnet", "ProjectManagementAPI.dll"]