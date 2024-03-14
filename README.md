# AthletiX Backend

Bienvenue dans le backend du projet AthletiX. Cette application backend est réalisée en .NET 8, suit l'architecture N-Layer (simplifiée) et utilise l'API ASP.NET avec une base de données PostgreSQL (modifiable).

## 🌐 Table des matières

- [Aperçu](#aperçu)
- [Technologies](#technologies)
- [Installation](#installation)

## Aperçu

Le projet AthletiX Backend est la logique métier pour le front de AthletiX.

## 🚀 Technologies

- .NET 8
- ASP.NET API
- PostgreSQL (modifiable)

## 🛠 Installation

Le backend AthletiX peut être facilement déployé à l'aide de Docker. Assurez-vous d'avoir Docker installé sur votre machine.

Pour faire les migrations :
``` bash
dotnet ef migrations add Ath-Auth --project .\1.API\
dotnet ef database update  --project .\1.API\
dotnet ef migrations script --project .\1.API\
```
