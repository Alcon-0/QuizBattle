import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [count, setCount] = useState(0)
  const [forecast, setForecast] = useState([])

  useEffect(() => {
    fetch('/weatherforecast') // This will be proxied to your ASP.NET Core backend
      .then(response => response.json())
      .then(data => {
        setForecast(data)
      })
      .catch(error => console.error("Error fetching weather data:", error))
  }, [])

  return (
    <>
      <div className="card">

        <h2>Weather Forecast</h2>
        <ul>
          {forecast.map((day, idx) => (
            <li key={idx}>
              {day.date}: {day.summary} ({day.temperatureC}°C / {day.temperatureF}°F)
            </li>
          ))}
        </ul>
      </div>
    </>
  )
}

export default App
