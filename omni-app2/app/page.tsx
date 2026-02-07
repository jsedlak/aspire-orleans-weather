import WeatherDashboard from "./components/WeatherDashboard";
import Logo from "./components/Logo";

export default function Home() {
  return (
    <div className="min-h-screen p-8 sm:p-20 font-sans transition-colors">
      <main className="flex flex-col gap-8 row-start-2 items-center w-full max-w-4xl mx-auto">
        <header className="w-full text-center mb-8 flex flex-col items-center">
            <div className="mb-4 p-3 bg-white dark:bg-neutral-800 rounded-2xl shadow-lg ring-1 ring-neutral-200 dark:ring-neutral-700">
              <Logo className="w-16 h-16" />
            </div>
            <h1 className="text-4xl font-extrabold tracking-tight text-neutral-900 dark:text-white mb-2 bg-clip-text text-transparent bg-gradient-to-r from-blue-600 to-violet-600 dark:from-blue-400 dark:to-violet-400">
              OmniWeather
            </h1>
            <p className="text-neutral-600 dark:text-neutral-400 text-lg font-medium">
              Beautiful forecasts for anywhere in the world.
            </p>
        </header>

        <WeatherDashboard />
      </main>
    </div>
  );
}
