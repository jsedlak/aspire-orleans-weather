# AspireWeather

A modern, full-stack weather application demonstrating the power of **.NET Aspire** for orchestration, **Microsoft Orleans** for stateful distributed computing, and **Next.js** for a responsive frontend.

![OmniWeather](https://img.shields.io/badge/Aspire-Orchestrated-blueviolet)

## 🏗 Architecture

The solution uses .NET Aspire to orchestrate a multi-project application consisting of:

-   **AppHost**: The entry point that orchestrates all services.
-   **OmniWeather (Backend)**: An ASP.NET Core Minimal API utilizing Microsoft Orleans for grain-based state management (Virtual Actors).
    -   Integrates with OpenMeteo for weather data.
    -   Exposes a `/forecast/{region}` endpoint.
-   **omni-app (Frontend)**: A Next.js application providing a modern weather dashboard.
    -   Features a responsive UI with light/dark mode.
    -   Displays real-time forecast data including hourly breakdowns and 10-day summaries.
    -   Uses Server Actions to securely proxy requests to the backend.

## 🚀 Getting Started

### Prerequisites

-   [.NET 8 or 9 SDK](https://dotnet.microsoft.com/download)
-   [Node.js](https://nodejs.org/) (LTS recommended)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop) (required for Aspire orchestration)

### Running the Application

1.  Clone the repository.
2.  Navigate to the root directory.
3.  Run the Aspire AppHost:

    ```bash
    dotnet run --project apphost.cs
    ```
    *Note: If you have a `.sln` file, you can also open it in Visual Studio or VS Code and launch the AppHost project.*

4.  The Aspire Dashboard will launch in your browser. From there, you can view the logs, metrics, and traces for all services.
5.  Click the endpoint link for the `omniapp` project to open the weather dashboard.

### Project Structure

```text
/
├── apphost.cs               # Aspire orchestration logic
├── OmniWeather/             # Keys backend services & Orleans grains
│   ├── Program.cs           # API endpoints & DI setup
│   ├── Weather/             # Domain logic (Grains, Models, Services)
├── omni-app/                # Next.js frontend application
│   ├── app/                 # React components & Server Actions
```

## 🛠 Technology Stack

-   **.NET Aspire**: Cloud-native application orchestration.
-   **Microsoft Orleans**: Distributed actor framework for scalable state management.
-   **Next.js & React**: Modern frontend framework.
-   **Tailwind CSS**: Utility-first CSS framework for styling.
-   **OpenMeteo API**: External provider for weather data.
