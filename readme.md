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
* [Entity Framework Change Auditing](#entity-framework-change-auditing)

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

This demonstration actually introduces several coordinated features:

* App Notifications
* Alerts which generate Notifications

There are two technologies that directly enable these features:

* [SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-3.1)
* [Hosted Services](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio)

### Worker Service Infrastructure
[Back to Top](#platform-demo)

> In this section, I'll highlight all of the standard parts of the codebase that you should already be familiar with. In the next section, I'll highlight everything that drives the non-standard functionality of these features.

* [Notification.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/Notification.cs) and [Alert.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/Notification.cs) - Entity Framework classes
* [NotificationExtensions.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/NotificationExtensions.cs) and [AlertExtensions.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/AlertExtensions.cs) - Extension method classes that define the business logic for working with Notifications and Alerts.
* [NotificationController.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Controllers/NotificationController.cs) and [AlertController.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Controllers/AlertController.cs) - Web API controllers.
* [notification.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/models/notification.ts) and [alert.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/models/alert.ts) - TypeScript interfaces that map to their corresponding Entity Framework classes.
* [notification.service.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/services/notification.service.ts), [alert.service.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/services/alert.service.ts), and [alert.source.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/services/sources/alert.source.ts) - Angular services that map to API endpoint functionality.
* [notification-bin.dialog.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/dialogs/notification/notification-bin.dialog.ts) and [notification-bin.dialog.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/dialogs/notification/notification-bin.dialog.html) - Dialog for managing notifications flagged as deleted.
* [alert.dialog.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/dialogs/alert/alert.dialog.ts) and [alert.dialog.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/dialogs/alert/alert.dialog.html) - Dialog for creating new / managing existing alerts.
* [notification-card.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/notification/notification-card.component.ts) and [notification-card.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/notification/notification-card.component.html) - Notification card component.
* [alert-editor.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/alert/alert-editor.component.ts) and [alert-editor.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/alert/alert-editor.component.html) - Component for managing the details of an alert.
* [alert-details.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/alert/alert-details.component.ts) and [alert-details.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/alert/alert-details.component.html) - Component for rendering the details of an alert.
* [alert-card.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/alert/alert-card.component.ts) and [alert-card.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/components/alert/alert-card.component.html) - Alert card component.
* [notifications.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/routes/notifications/notifications.component.ts) and [notifications.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/routes/notifications/notifications.component.html) - Route for managing notifications.
* [home.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/routes/home/home.component.ts) and [home.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/routes/home/home.component.html) - Route for managing alerts.

### Worker Service Implementation
[Back to Top](#platform-demo)

* [SocketHub.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Hubs/SocketHub.cs) - SignalR Hub necessary for broadcasting notifications
* [NotificationWorker.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Workers/NotificationWorker.cs) - `IHostedService` that runs as a service in parallel with the .NET Core web app.
  * Notice that `AppDbContext` cannot be injected directly. It must be extracted from the injected `IServiceProvider` instance.
  * An `IHubContext<SocketHub>` must be injected in order to use SignalR to trigger the `receiveAlertNotification` event for connected clients via the `NotifyClients` method.
  * `StartAsync` runs whenever the IIS site starts
  * `TriggerAlerts` runs whenever the `Timer` ticks (every 15 seconds in this case).
  * `StopAsync` runs whenever the IIS site is stopped
* [Program.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Program.cs#L22) - Modified to configure and start the `NotificationWorker` hosted service.
* [Startup.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Startup.cs#L94) - Map the `SocketHub` to the `/core-socket` endpoint.
* [socket.service.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/api/src/lib/services/sockets/socket.service.ts) - Angular Service that manages a SignalR web socket connection, registers web socket events, and exposes a `triggerNotification` function to broadcast notifications to other connected clients.
  * The `connected$`, `error$`, `notify$`, and `alertNotify$` streams are used to keep track of the state of the web socket, as well as track whenever new notifications are available.
* [app.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/app.component.ts#L32) - At the global **AppComponent** level, whenever new notifications are received, the notification count is updated. This is used by the notification link to indicate how many unread notifications there are.
* [app.component.html](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/app.component.html#L11) - This section shows the notification link, and how it uses the `notifyCount` property to indicate the amount of unread notifications there currently are.
* [home.component.ts](https://github.com/JaimeStill/platform-demo/blob/master/client/worker-service/src/app/routes/home/home.component.ts#L42) - Whenever an alert is triggered, the new alert notification is received by SignalR, and refreshes the collection of alerts.

## Entity Framework Change Auditing

This is more of a transient feature that has been developed into the app platform. The whole of this feature can be found in the following places:

* [ChangeState.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/ChangeState.cs) - For a given instance of `SaveChanges`, this class tracks all of the `EntityEntry` objects from the `ChangeTracker` based on their state: `Added`, `Modified`, or `Deleted`.
* [Audit.cs](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Entities/Audit.cs) - Entity Framework class that stores the details of an audit record to the database.
* [AppDbContext](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/AppDbContext.cs#L50) - Beginning with `override SaveChanges` injects the audit process into the Entity Framework workflow.
  * `GetChangeState` populates and returns an instance of `ChangeState`
  * `GetEntityEntries` returns a `List<EntityEntry>` from the `ChangeTracker` of entries that are in the provided `EntityState`.
  * `CreateAudit` creates `Audit` records for all of the `EntityEntry` objects in a provided `ChangeState` object.
  * `GenerateAudit` serializes the state of an entity into JSON, generates an `Audit` record for the entry, then executes the specified `generator` action.
    * Think of `Action<T>` as a void callback function that receives an object of `T` as an argument whenever it is invoked.


`Audit` data can be accessed via the [AuditController](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Web/Controllers/AuditController.cs), which has its logic defined in [AuditExtensions](https://github.com/JaimeStill/platform-demo/blob/master/server/PlatformDemo.Data/Extensions/AuditExtensions.cs).
