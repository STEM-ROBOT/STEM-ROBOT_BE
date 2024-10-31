#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["STEM-ROBOT_BE/STEM-ROBOT.Web.csproj", "STEM-ROBOT_BE/"]
COPY ["STEM-ROBOT.BLL/STEM-ROBOT.BLL.csproj", "STEM-ROBOT.BLL/"]
COPY ["STEM-ROBOT.Common/STEM-ROBOT.Common.csproj", "STEM-ROBOT.Common/"]
COPY ["STEM-ROBOT.DAL/STEM-ROBOT.DAL.csproj", "STEM-ROBOT.DAL/"]


RUN dotnet restore "./STEM-ROBOT.DAL/STEM-ROBOT.DAL.csproj"
RUN dotnet restore "./STEM-ROBOT.Common/STEM-ROBOT.Common.csproj"
RUN dotnet restore "./STEM-ROBOT.BLL/STEM-ROBOT.BLL.csproj"
RUN dotnet restore "./STEM-ROBOT_BE/STEM-ROBOT.Web.csproj"

COPY . .
WORKDIR "/src/STEM-ROBOT_BE"
RUN dotnet build "./STEM-ROBOT.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./STEM-ROBOT.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "STEM-ROBOT.Web.dll"]