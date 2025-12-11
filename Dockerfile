# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY AttendanceSystem.sln .

# Copy project folder
COPY AttendanceSystem ./AttendanceSystem

RUN dotnet restore

RUN dotnet publish AttendanceSystem/AttendanceSystem.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AttendanceSystem.dll"]
