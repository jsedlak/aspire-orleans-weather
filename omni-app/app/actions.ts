'use server'

export interface ForecastPart {
  start: string;
  end: string;
  isMeasured: boolean;
  temperature: number;
  visibility: string;
}

export interface Forecast {
  region: string;
  date: string;
  parts: ForecastPart[];
}

export async function getForecast(region: string): Promise<Forecast[]> {
  const baseUrl = process.env.API_HTTPS;
  if (!baseUrl) {
    console.error("API_HTTPS environment variable is not defined.");
    throw new Error("Target API URL is not configured.");
  }

  const url = `${baseUrl}/forecast/${encodeURIComponent(region)}?numberOfDays=10`;
  
  try {
    const response = await fetch(url, {
      // Ensure we don't cache this too aggressively if it's real-time weather
      cache: 'no-store' 
    });

    if (!response.ok) {
      throw new Error(`Weather service returned ${response.status}: ${response.statusText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching forecast:", error);
    throw error;
  }
}
