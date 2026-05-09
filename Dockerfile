FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MyFirstWebApi/MyFirstWebApi.csproj MyFirstWebApi/
RUN dotnet restore MyFirstWebApi/MyFirstWebApi.csproj

COPY MyFirstWebApi/ MyFirstWebApi/
RUN dotnet publish MyFirstWebApi/MyFirstWebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "MyFirstWebApi.dll"]
