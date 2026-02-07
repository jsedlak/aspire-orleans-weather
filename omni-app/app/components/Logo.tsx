export default function Logo({ className = "w-12 h-12" }: { className?: string }) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 100 100"
      fill="none"
      className={className}
    >
      <defs>
        <linearGradient id="logo-gradient" x1="0%" y1="0%" x2="100%" y2="100%">
          <stop offset="0%" stopColor="#3B82F6" />
          <stop offset="100%" stopColor="#8B5CF6" />
        </linearGradient>
      </defs>
      
      {/* Sun rays */}
      <circle cx="50" cy="50" r="35" stroke="url(#logo-gradient)" strokeWidth="8" strokeOpacity="0.2" />
      <circle cx="50" cy="50" r="45" stroke="url(#logo-gradient)" strokeWidth="2" strokeDasharray="4 6" strokeOpacity="0.4" />

      {/* Cloud shape */}
      <path
        d="M68 65H32C24.268 65 18 58.732 18 51C18 43.85 23.391 37.957 30.3 37.135C31.571 27.68 39.692 20.5 49.5 20.5C59.043 20.5 66.993 27.247 68.563 36.31C74.453 37.766 79 43.146 79 49.5C79 58.06 72.06 65 63.5 65"
        fill="url(#logo-gradient)"
      />
      
      {/* Sun cutout */}
      <circle cx="70" cy="30" r="12" fill="#F59E0B" />
    </svg>
  );
}
