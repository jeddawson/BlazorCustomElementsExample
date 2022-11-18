Original Source: https://github.com/aspnet/AspLabs/tree/main/src/BlazorCustomElements

Updated for .NET 7 and the generally available custom elements nuget package

# Blazor Custom Elements

This package provides a simple mechanism for using Blazor components as custom elements. This means you can easily render Blazor components dynamically from other SPA frameworks, such as Angular or React.

## Running the React sample

Clone this repo. Then, in a command prompt, execute:

 * `cd BlazorComponentExport`
 * `dotnet watch`

When the Blazor component app starts, you can close the browser window that is opened, but keep track of the port that it started on. Update the proxy entry in `BlazorComponentTest/ClientApp/package.json` to match.

Leave that running, and open a second command prompt, and execute:

 * `cd BlazorComponentTest`
 * `dotnet watch`

Now the React app is running. Navigate to the Counter to see the Blazor component load.

## Adding this to your own project

1. Start with or create a Blazor WebAssembly or Blazor Server project that contains your Blazor components
2. Install the NuGet package `Microsoft.AspNetCore.Components.CustomElements`
3. In your Blazor application startup configuration, remove any normal Blazor root components, and instead register components as custom elements. See [Example for Blazor WebAssembly](#webassembly-example) or [Example for Blazor Server](#server-example).
4. Configure your external SPA framework application to serve the Blazor framework files and render your Blazor custom elements. For example, see [Configuring Angular](#configuring-angular) or [Configuring React](#configuring-react). Similar techniques will work with other SPA frameworks.

### WebAssembly example

For Blazor WebAssembly, remove lines like this from `Program.cs`:

```cs
builder.RootComponents.Add<App>("#root");
```

... and add lines like the following instead:

```cs
builder.RootComponents.RegisterCustomElement<Counter>("my-blazor-counter");
builder.RootComponents.RegisterCustomElement<ProductGrid>("product-grid");
```

### Configuring React

1. In your React `index.html`, add the following at the end of `<body>`:

    ```html
    <script src="_framework/blazor.webassembly.js"></script>
    ```

2. Run the application now and see that the browser gets a 404 error for those two JavaScript files. To fix this, [configure React's development server to proxy unmatches requests to the ASP.NET Core development server](https://create-react-app.dev/docs/proxying-api-requests-in-development/). Of course, you also need to be running your ASP.NET Core development server at the same time for this to work.

3. In any of your React components, render the custom elements corresponding to your Blazor components. For example, have a React component render output include:

    ```html
    <my-blazor-counter title={title} increment-amount={incrementAmount}></my-blazor-counter>
    ```

    Whenever you edit your React component source code, you'll see it update automatically in the browser without losing the state within your Blazor component. The two hot reload systems can coexist and cooperate.

### Publishing your combined application

The easiest way to have an application with both .NET parts and a 3rd-party SPA framework is to use the ASP.NET Core project templates [for React](https://docs.microsoft.com/aspnet/core/client-side/spa/react). This will automatically gather the files required for the combined application during publishing.

If you're not using one of those project templates, you'll need to publish both the .NET and JavaScript parts separately and manually combine the files into a single deployable set.

## Passing parameters

You can pass parameters to your Blazor component either as HTML attributes or as JavaScript properties on the DOM element.

For example, if your component declares a parameters like the following:

```cs
[Parameter] public int IncrementAmount { get; set; }
```

... then you can pass a value as an HTML attribute follows:

```html
<my-blazor-counter increment-amount="123"></my-blazor-counter>
```

Notice that the attribute name is kebab-case (i.e., `increment-amount`, not `IncrementAmount`).

Alternatively, you can set it as a JavaScript property on the element object:

```js
const elem = document.querySelector("my-blazor-counter");
elem.incrementAmount = 123;
```

Notice that the property name is camelCase (i.e., `incrementAmount`, not `IncrementAmount`).

You can update parameter values at any time using either attribute or property syntax.

### Supported parameter types

 * Using JavaScript property syntax, you can pass objects of any JSON-serializable type
 * Using HTML attributes, you are limited to passing objects of string, boolean, or numerical types
