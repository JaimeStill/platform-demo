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
  * [Recursive Interface Infrastructure](#recursive-interface-infrastructure)
  * [Recursive Interface Pattern](#recursive-interface-pattern)
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

* [ApiQuery](https://github.com/JaimeStill/platform-demo/tree/master/server/PlatformDemo.Core/ApiQuery) - Server infrastructure of the `.Core` class library that enables this functionality on the server side.
* [Item.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/Item.cs) and [Category.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/Item.cs) - Entity Framework classes that supports this demo.
* [query-result.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/core/src/lib/models/query-result.ts) - TypeScript interface that wraps query results and contains collection metadata to support pagination.
* [query.service.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/core/src/lib/services/abstract/query.service.ts) - Abstract Angular service that provides all of the core infrastructure necessary for implementing this functionality on the client side.
* [item.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/models/item.ts) and [category.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/models/category.ts) - TypeScript interfaces that map to the Entity Framework classes they represent.

### API Filtering Implementation
[Back to Top](#platform-demo)

* [ItemExtensions.QueryItems](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/ItemExtensions.cs#L23) - Extension method using a `QueryContainer<T>` to execute the query and return the resulting `QueryResult<T>`.
* [ItemController.QueryItems](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Controllers/ItemController.cs#L24) - Web API action method that calls `ItemExtensions.QueryItems` and returns the resulting `QueryResult<T>`.
* [item.source.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/services/sources/item.source.ts) - `QueryService<T>` implementation for `Item` objects. Points to `ItemController.QueryItems`.
* [home.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api-filtering/src/app/routes/home/home.component.ts) and [home.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api-filtering/src/app/routes/home/home.component.html) - Uses an `ItemSource` to populate a [mat-table](https://material.angular.io/components/table/overview) and supports [matSort](https://material.angular.io/components/sort/overview), [search](https://github.com/JaimeStill/platform-demo/blob/master/client/api-filtering/src/app/routes/home/home.component.html#L13) (see the [SearchbarComponent](https://github.com/JaimeStill/platform-demo/tree/master/client/core/src/lib/components/searchbar) as well as the [CoreService.generateInputObservable](https://github.com/JaimeStill/platform-demo/blob/master/client/core/src/lib/services/core.service.ts#L44) function), and [mat-paginator](https://material.angular.io/components/paginator/overview).

## Recursive Interface
[Back to Top](#platform-demo)

Demonstration of component design that facilitates lazy-loading of data structures with a graph of an unknown depth.

This provides a more convenient / performant replacement for the [mat-tree](https://material.angular.io/components/tree/overview) component.

### Recursive Interface Infrastructure
[Back to Top](#platform-demo)

* [File.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/File.cs) and [Folder.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/Folder.cs) - Entity Framework classes used to demonstrate this functionality.
* [AppDbContext Parent Folder Configuration](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/AppDbContext.cs#L30) - Fluent API configuration of the `Folder.Parent` relationship.
* [FileExtensions.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/FileExtensions.cs) and [FolderExtensions.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/FolderExtensions.cs) - Extension method classes that define the business logic for this demonstration.
* [DbInitializer Data Seeding](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/DbInitializer.cs#L68) - Initial data seeded into the database.
* [FileController.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Controllers/FileController.cs) and [FolderController.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Controllers/FolderController.cs) - Web API controllers.
* [file.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/models/file.ts) and [folder.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/models/folder.ts) - TypeScript interfaces that map to their corresponding Entity Framework classes.
* [file-system.service.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/services/file-system.service.ts) - Angular service that maps to the **FileController** and **FolderController** Web API endpoints.
* [file.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/file-system/file.component.ts) and [file.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/file-system/file.component.html) - Component that represents a **File** card.

### Recursive Interface Pattern
[Back to Top](#platform-demo)

This pattern is implemented in the **FolderComponent**. Before jumping into that file, you should understand what is actually enabling this functionality:

* The **FolderComponent** provides its own instance of [FileSystemService](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/file-system/folder.component.ts#L26), so every instance of this component maintains its own state relative to its direct children.
* The **FolderComponent** itself has the potential to [render child folders](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/file-system/folder.component.html#L57), which can load its own children. In this way, **FolderComponent** is recursive.

By default, `FolderComponent.expanded = false`. Its children aren't loaded until the folder is actually expanded to reveal its contents. This allows us to [only load the folders that don't have a parent folder](https://github.com/JaimeStill/platform-demo/blob/master/client/recursive-interface/src/app/routes/home/home.component.ts#L32). In this way, we can efficiently initialize the base data graph, but dynamically load child nodes on demand (lazily load them).

* [folder.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/file-system/folder.component.ts) and [folder.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/file-system/folder.component.html)
* [home.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/recursive-interface/src/app/routes/home/home.component.ts) and [home.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/recursive-interface/src/app/routes/home/home.component.html)

## Worker Service
[Back to Top](#platform-demo)

### Worker Service Infrastructure
[Back to Top](#platform-demo)

### Worker Service Implementation
[Back to Top](#platform-demo)
