FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY Miru.csproj ./

RUN dotnet restore Miru.csproj

COPY . .

RUN dotnet publish Miru.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /out .

ENTRYPOINT ["dotnet", "Miru.dll"]
