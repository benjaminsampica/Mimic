# Introduction

A small application for tracking a list of topics - which could be anything.

Topics can be shared via export and import buttons.

All topics are stored locally on the browser's local storage. This means if you change computers, clear your cache, or open an incognito browser they will be gone.

Built with Blazor WASM and hosted for free on GitHub Pages at <https://benjaminsampica.github.io/Mimic/>.

# Running Locally

You must have:

- .NET 7 SDK
- WASM tools (can be installed via `dotnet workload install wasm-tools`)
- Under `index.html` set `<base href="/Mimic/" />` to `<base href="/" />`