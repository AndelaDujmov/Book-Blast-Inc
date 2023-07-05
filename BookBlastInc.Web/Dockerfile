FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookBlastInc.Web/BookBlastInc.Web.csproj", "BookBlastInc.Web/"]
RUN dotnet restore "BookBlastInc.Web/BookBlastInc.Web.csproj"
COPY . .
WORKDIR "/src/BookBlastInc.Web"
RUN dotnet build "BookBlastInc.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookBlastInc.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookBlastInc.Web.dll"]
