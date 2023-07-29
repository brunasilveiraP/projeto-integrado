FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["src/BBL.Web.Host/BBL.Web.Host.csproj", "src/BBL.Web.Host/"]
COPY ["src/BBL.Web.Core/BBL.Web.Core.csproj", "src/BBL.Web.Core/"]
COPY ["src/BBL.Application/BBL.Application.csproj", "src/BBL.Application/"]
COPY ["src/BBL.Core/BBL.Core.csproj", "src/BBL.Core/"]
COPY ["src/BBL.EntityFrameworkCore/BBL.EntityFrameworkCore.csproj", "src/BBL.EntityFrameworkCore/"]
WORKDIR "/src/src/BBL.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/BBL.Web.Host", "src/BBL.Web.Host"]
COPY ["src/BBL.Web.Core", "src/BBL.Web.Core"]
COPY ["src/BBL.Application", "src/BBL.Application"]
COPY ["src/BBL.Core", "src/BBL.Core"]
COPY ["src/BBL.EntityFrameworkCore", "src/BBL.EntityFrameworkCore"]
WORKDIR "/src/src/BBL.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "BBL.Web.Host.dll"]
