#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["HomeWorkToDos.API/HomeWorkToDos.API.csproj", "HomeWorkToDos.API/"]
COPY ["HomeWorkToDos.Util/HomeWorkToDos.Util.csproj", "HomeWorkToDos.Util/"]
COPY ["HomeWorkToDos.Business/HomeWorkToDos.Business.csproj", "HomeWorkToDos.Business/"]
COPY ["HomeWorkToDos.DataAccess/HomeWorkToDos.DataAccess.csproj", "HomeWorkToDos.DataAccess/"]
RUN dotnet restore "HomeWorkToDos.API/HomeWorkToDos.API.csproj"
COPY . .
WORKDIR "/src/HomeWorkToDos.API"
RUN dotnet build "HomeWorkToDos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeWorkToDos.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeWorkToDos.API.dll"]