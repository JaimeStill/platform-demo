# Platform Demo

* [Getting Started](#getting-started)
  * [Seed the Database](#seed-the-database)
  * [Build the Libraries](#build-the-libraries)
  * [Run the Applications](#run-the-applications)
* [Infrastructure Details](#infrastructure-details)
* [API Filtering](#api-filtering)
  * [API Filtering Infrastructure](#api-filtering-infrastructure)
  * [API Filtering Implementation](#api-filtering-implementation)
* [Recursive Interface](#recursive-interface)
* [Worker Service](#worker-service)
  * [Worker Service Infrastructure](#worker-service-infrastructure)
  * [Worker Service Implementation](#worker-service-implementation)

This demonstration is built into an [Angular Workspace](https://angular.io/guide/workspace-config) and is similar to the setup generated in my [angular-workspaces](https://github.com/JaimeStill/angular-workspaces/blob/master/README.md) repository.

## Getting Started
[Back to Top](#platform-demo)

### Required Setup
[Back to Top](#platform-demo)

* [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  * Optional (for management): [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)
* [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)
* [Node.js - Current](https://nodejs.org/en/)
* [Yarn](https://classic.yarnpkg.com/lang/en/)

**Recommended Tools**

`dotnet ef`  
```bash
dotnet tool install -g dotnet-ef
```

`@angular/cli`  
```bash
yarn global add @angular/cli
```

`@angular-devkit/schematics-cli`  
```bash
yarn global add @angular-devkit/schematics-cli
```

### Seed the Database
[Back to Top](#platform-demo)

Ensure that `ConnectionStrings:Project` is appropriately setup for your local SQL Server instance in [appsettings.Development.json](./server/PlatformDemo.Web/appsettings.Development.json)

```json
"ConnectionStrings": {
  "Project": "Server=.\\DevSql;Database=PlatformDemo-dev;Trusted_Connection=True;"
}
```

```bash
platform-demo> cd server/dbseeder
platform-demo/server/dbseeder> dotnet run -- "Server=.\DevSql;Database=PlatformDemo-dev;Trusted_Connection=True;"
```

### Build the Libraries
[Back to Top](#platform-demo)

```bash
platform-demo> yarn build
```

### Run the Applications
[Back to Top](#platform-demo)

In Visual Studio Code, you open multiple terminals in a split view using the **Split Terminal** button at the top-right of the Terminal window (or use the default keyboard shortcut of `Ctrl+Shift+5`).

**Server Terminal**

```bash
yarn start:server
```

**api-filtering Terminal**

```bash
yarn start:api-filtering
```

http://localhost:3001

**worker-service Terminal**

```bash
yarn start:worker-service
```

http://localhost:3002

**recursive-interface Terminal**

```bash
yarn start:recursive-interface
```

http://localhost:3003


## Infrastructure Details
[Back to Top](#platform-demo)

**Build and Run Scripts**  

[package.json](./package.json) - contains all of the scripts necessary to build and run the applications.

**Angular Configuration**

[angular.json](./angular.json) - provides all of the necessary configuration details for the Angular libraries and apps authored in this workspace.

**Global Assets**

[assets](./assets) - contains any global blob files to be shared between different application instances. Currently, it contains the Material Icon fonts.

**App Theme**

[theme](./theme) - contains all of the SCSS files share between each application that implements a global Material theme.

## API Filtering
[Back to Top](#platform-demo)

Enables pagination, sorting, and filtering of Entity Framework `IQueryable<T>` data results using query parameters on an API URL.

### API Filtering Infrastructure
[Back to Top](#platform-demo)

### API Filtering Implementation
[Back to Top](#platform-demo)

## Recursive Interface
[Back to Top](#platform-demo)

## Worker Service
[Back to Top](#platform-demo)

### Worker Service Infrastructure
[Back to Top](#platform-demo)

### Worker Service Implementation
[Back to Top](#platform-demo)
