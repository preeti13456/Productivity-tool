# ExpertOS - Intelligent Task Management System

ExpertOS is a full-stack application that uses AI to intelligently manage tasks and priorities, helping teams focus on what matters most.

## Project Structure

```
ExpertOS/
├── backend/              # .NET 8 Web API
│   ├── Controllers/      # API endpoints
│   ├── Services/         # Business logic (AI service)
│   ├── Models/          # Data models
│   ├── Data/            # Database context
│   └── Program.cs       # Application startup
│
└── frontend/            # React + TypeScript + Vite
    ├── src/
    │   ├── App.tsx      # Main React component
    │   ├── services/    # API client
    │   └── index.css    # Tailwind CSS
    └── package.json     # Dependencies
```

## Features

- **Priority Stream**: Real-time task prioritization from multiple sources (Slack, Email, Jira)
- **AI-Powered Insights**: Automatic summarization and analysis of messages and tasks
- **Deep Work Mode**: Focus time protection with integrated calendar management
- **Task Extraction**: Automatic task identification and assignment
- **System Health Monitoring**: Real-time efficiency metrics and focus time tracking

## Tech Stack

### Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Swagger/OpenAPI documentation

### Frontend

- React 18.2
- TypeScript 5.2
- Vite 5.0
- Tailwind CSS 3.3
- Axios for API calls
- Lucide React for icons

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js 18+
- npm or yarn

### Backend Setup

1. Navigate to backend directory:

```bash
cd "backend "
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Run the application:

```bash
dotnet run
```

The API will be available at `https://localhost:7191`

### Frontend Setup

1. Navigate to frontend directory:

```bash
cd frontend
```

2. Install dependencies:

```bash
npm install
```

3. Start development server:

```bash
npm run dev
```

The application will be available at `http://localhost:5173`

## Configuration

### Backend Configuration

Edit `backend/appsettings.json` to configure:

```json
{
  "OpenAI": {
    "ApiKey": "your-api-key-here"
  }
}
```

If no OpenAI API key is configured, the app will use MockAiService for demonstration.

### CORS Configuration

The backend is configured to accept requests from:

- `http://localhost:5173` (Frontend dev server)
- `http://localhost:3000` (Alternative frontend port)

## API Endpoints

- `GET /api/Dashboard/data` - Get dashboard data
- `POST /api/Dashboard/summarize-inbox` - Summarize messages
- `POST /api/Dashboard/extract-tasks` - Extract actionable tasks
- `POST /api/Dashboard/ask` - Ask AI a question
- `POST /api/Dashboard/prefer-time` - Set preferred deep work time
- `POST /api/Dashboard/deep-work` - Activate deep work mode

## Development

### Running Tests

Frontend:

```bash
npm run build
```

Backend:

```bash
dotnet test
```

### Building for Production

Frontend:

```bash
npm run build
```

Backend:

```bash
dotnet publish -c Release
```

## Features in Detail

### Priority Stream

Displays prioritized items from multiple channels:

- Slack messages
- Email messages
- Jira tickets
- System notifications

### AI-Powered Analysis

- Summarizes message threads automatically
- Identifies team bottlenecks
- Extracts actionable tasks with assignees
- Answers questions about project context

### Deep Work Protection

- Schedules preferred focus time
- Automatically handles interruptions
- Protects focus time from calendar conflicts

## Troubleshooting

### CORS Issues

If you see CORS errors, ensure the frontend is running on `http://localhost:5173` or update the CORS policy in `Program.cs`.

### API Connection Failed

- Check that the backend is running on `https://localhost:7191`
- Verify the proxy configuration in `vite.config.ts`
- Check browser console for detailed error messages

### Database Issues

The application uses in-memory database by default. Data is reset on each restart.

## Contributing

1. Create a feature branch
2. Make your changes
3. Test thoroughly
4. Submit a pull request

## License

MIT
