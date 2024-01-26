# Magic Villa Api

## Migrations

### Instalação do Entity Framework Global via CLI
```
dotnet tool install --global dotnet-ef --version {version number}
```

```
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Adição de uma nova migration
<b>OBS</b>: Para criar uma nova migration é necessário estar no mesmo nível do arquivo .csproj
```
dotnet ef migrations add {migration name}
```

### Rodando as migrations
```
dotnet ef database update
```