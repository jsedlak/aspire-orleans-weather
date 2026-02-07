'use client';

import { useState, useRef } from 'react';
import { getForecast, type Forecast, type ForecastPart } from '../actions';

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString(undefined, {
    weekday: 'long',
    month: 'long',
    day: 'numeric',
  });
}

function formatDayShort(dateStr: string) {
  return new Date(dateStr).toLocaleDateString(undefined, {
    weekday: 'short',
    month: 'short',
    day: 'numeric',
  });
}

function formatTime(dateStr: string) {
  return new Date(dateStr).toLocaleTimeString(undefined, {
    hour: 'numeric',
    minute: '2-digit',
  });
}

function getVisibilityEmoji(visibility: string) {
  const v = visibility.toLowerCase();
  if (v.includes('sunny') || v.includes('clear')) return '☀️';
  if (v.includes('cloud')) return '☁️';
  if (v.includes('rain')) return '🌧️';
  if (v.includes('snow')) return '❄️';
  if (v.includes('mist') || v.includes('fog')) return '🌫️';
  if (v.includes('storm') || v.includes('thunder')) return '⛈️';
  return '🌡️';
}

function DailyForecastRow({ forecast }: { forecast: Forecast }) {
  const [expanded, setExpanded] = useState(false);

  // Calculate average temp
  const avgTemp = forecast.parts.length 
      ? Math.round(forecast.parts.reduce((acc, p) => acc + p.temperature, 0) / forecast.parts.length)
      : 0;

  // Midpart for summary
  const midPart = forecast.parts[Math.floor(forecast.parts.length / 2)];

  return (
    <div className="bg-white dark:bg-neutral-800 rounded-xl shadow-sm border border-neutral-200 dark:border-neutral-700 overflow-hidden transition-all">
      <div 
        onClick={() => setExpanded(!expanded)}
        className="flex items-center justify-between p-4 cursor-pointer hover:bg-neutral-50 dark:hover:bg-neutral-700/50 transition-colors"
      >
          <div className="flex items-center gap-4 w-1/3">
              <span className="font-medium text-neutral-900 dark:text-neutral-100">
                  {formatDayShort(forecast.date)}
              </span>
          </div>
          
          <div className="flex items-center gap-2 justify-center w-1/3">
              <span className="text-2xl" role="img">
                  {midPart ? getVisibilityEmoji(midPart.visibility) : '❓'}
              </span>
              <span className="text-sm text-neutral-600 dark:text-neutral-300 hidden sm:inline-block">
                  {midPart?.visibility}
              </span>
          </div>

          <div className="flex items-center justify-end gap-4 w-1/3">
              <span className="font-semibold text-neutral-900 dark:text-neutral-100 text-lg">{avgTemp}°</span>
              <button 
                type="button"
                className={`p-1 rounded-full hover:bg-neutral-100 dark:hover:bg-neutral-700 transition-colors focus:outline-none`}
                aria-label={expanded ? "Collapse" : "Expand"}
              >
                  <svg 
                    xmlns="http://www.w3.org/2000/svg" 
                    fill="none" 
                    viewBox="0 0 24 24" 
                    strokeWidth={2} 
                    stroke="currentColor" 
                    className={`w-5 h-5 text-neutral-400 transition-transform duration-300 ${expanded ? 'rotate-180' : ''}`}
                  >
                    <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5" />
                  </svg>
              </button>
          </div>
      </div>

      {expanded && (
        <div className="border-t border-neutral-100 dark:border-neutral-700 bg-neutral-50/50 dark:bg-neutral-800/50 p-4 animate-in slide-in-from-top-2 duration-200">
           <div className="flex flex-col gap-1">
             {forecast.parts.map((part, idx) => (
               <div key={idx} className="flex items-center justify-between p-2 rounded-md hover:bg-white dark:hover:bg-neutral-700/50 transition-colors">
                 <span className="w-24 text-sm font-medium text-neutral-500 dark:text-neutral-400">{formatTime(part.start)}</span>
                 
                 <div className="flex-1 flex items-center gap-3">
                    <span className="text-lg" role="img">{getVisibilityEmoji(part.visibility)}</span>
                    <span className="text-sm font-medium text-neutral-700 dark:text-neutral-200">{part.visibility}</span>
                 </div>
                 
                 <span className="font-bold text-neutral-900 dark:text-neutral-100 w-12 text-right">{Math.round(part.temperature)}°</span>
               </div>
             ))}
           </div>
        </div>
      )}
    </div>
  );
}

export default function WeatherDashboard() {
  const [region, setRegion] = useState('');
  const [weather, setWeather] = useState<Forecast[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const scrollContainerRef = useRef<HTMLDivElement>(null);

  const scroll = (direction: 'left' | 'right') => {
    if (scrollContainerRef.current) {
        const { current } = scrollContainerRef;
        const scrollAmount = 300;
        if (direction === 'left') {
            current.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
        } else {
            current.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        }
    }
  };

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    if (!region.trim()) return;

    setLoading(true);
    setError(null);
    setWeather(null);

    try {
      const data = await getForecast(region);
      setWeather(data);
    } catch (err: any) {
      setError(err.message || 'Failed to load weather data.');
    } finally {
      setLoading(false);
    }
  }

  const currentDay = weather?.[0];
  const nextDays = weather?.slice(1);

  return (
    <div className="w-full max-w-3xl mx-auto space-y-8">
      {/* Search Section */}
      <form onSubmit={handleSearch} className="flex gap-4 items-center justify-center">
        <input
          type="text"
          value={region}
          onChange={(e) => setRegion(e.target.value)}
          placeholder="Enter region (e.g. Seattle, WA)"
          className="flex-1 max-w-md px-4 py-3 rounded-lg border border-neutral-300 dark:border-neutral-700 bg-white dark:bg-neutral-800 text-neutral-900 dark:text-neutral-100 shadow-sm focus:ring-2 focus:ring-blue-500 outline-none transition-all"
        />
        <button
          type="submit"
          disabled={loading}
          className="px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg shadow-sm transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {loading ? 'Searching...' : 'Get Forecast'}
        </button>
      </form>

      {error && (
        <div className="p-4 rounded-lg bg-red-50 dark:bg-red-900/20 text-red-600 dark:text-red-400 border border-red-200 dark:border-red-900/50 text-center">
          {error}
        </div>
      )}

      {weather && currentDay && (
        <div className="animate-in fade-in slide-in-from-bottom-4 duration-500 space-y-8">
          
          {/* Today's Forecast - Hourly */}
          <section className="bg-white dark:bg-neutral-800 rounded-2xl shadow-xl overflow-hidden border border-neutral-200 dark:border-neutral-700">
            <div className="p-6 md:p-8 bg-gradient-to-br from-blue-500 to-blue-600 dark:from-blue-600 dark:to-blue-800 text-white">
              <h2 className="text-3xl font-bold tracking-tight">{currentDay.region}</h2>
              <p className="text-blue-100 mt-1 opacity-90">{formatDate(currentDay.date)}</p>
              
              <div className="mt-8">
                <div className="flex items-baseline space-x-2">
                  <span className="text-6xl font-bold">
                    {currentDay.parts.length > 0 ? Math.round(currentDay.parts[0].temperature) : '--'}°
                  </span>
                  <span className="text-xl text-blue-100">C</span>
                </div>
                <p className="text-lg font-medium mt-2">
                  {currentDay.parts.length > 0 ? currentDay.parts[0].visibility : ''}
                </p>
              </div>
            </div>

            <div className="p-6 relative group">
              <h3 className="text-sm font-semibold text-neutral-500 dark:text-neutral-400 uppercase tracking-wider mb-4">Hourly Forecast</h3>
              
              <button 
                  onClick={() => scroll('left')}
                  className="absolute left-2 top-1/2 mt-4 z-10 p-2 rounded-full bg-white/80 dark:bg-black/80 shadow-md text-neutral-600 dark:text-neutral-300 hover:bg-white dark:hover:bg-black transition-all"
                  aria-label="Scroll left"
              >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor" className="w-5 h-5">
                      <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 19.5L8.25 12l7.5-7.5" />
                  </svg>
              </button>

              <button 
                  onClick={() => scroll('right')}
                  className="absolute right-2 top-1/2 mt-4 z-10 p-2 rounded-full bg-white/80 dark:bg-black/80 shadow-md text-neutral-600 dark:text-neutral-300 hover:bg-white dark:hover:bg-black transition-all"
                  aria-label="Scroll right"
              >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={2} stroke="currentColor" className="w-5 h-5">
                      <path strokeLinecap="round" strokeLinejoin="round" d="M8.25 4.5l7.5 7.5-7.5 7.5" />
                  </svg>
              </button>

              <div 
                ref={scrollContainerRef}
                className="flex gap-4 overflow-x-auto pb-4 scroll-smooth [scrollbar-width:none] [-ms-overflow-style:none] [&::-webkit-scrollbar]:hidden"
              >
                {currentDay.parts.map((part, idx) => (
                  <div key={idx} className="flex-shrink-0 w-20 flex flex-col items-center p-3 rounded-lg hover:bg-neutral-50 dark:hover:bg-neutral-700/50 transition-colors">
                    <span className="text-sm text-neutral-500 dark:text-neutral-400 mb-2 whitespace-nowrap">
                      {formatTime(part.start)}
                    </span>
                    <span className="text-2xl mb-2 filter drop-shadow-sm" role="img" aria-label={part.visibility}>
                      {getVisibilityEmoji(part.visibility)}
                    </span>
                    <span className="font-semibold text-neutral-900 dark:text-neutral-100">
                      {Math.round(part.temperature)}°
                    </span>
                  </div>
                ))}
              </div>
            </div>
          </section>

          {/* Next Days - Summary List */}
          {nextDays && nextDays.length > 0 && (
            <section className="space-y-4">
              <h3 className="text-xl font-bold text-neutral-900 dark:text-neutral-100 px-2">Next 9 Days</h3>
              <div className="grid gap-3">
                {nextDays.map((forecast, idx) => (
                    <DailyForecastRow key={idx} forecast={forecast} />
                ))}
              </div>
            </section>
          )}
        </div>
      )}
    </div>
  );
}
