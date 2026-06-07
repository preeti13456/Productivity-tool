# Quick Start Guide - ExpertOS

## 5-Minute Quick Start

### Step 1: Backend Setup (2 min)

```bash
# Navigate to backend
cd "backend "

# Build and run
dotnet restore
dotnet run
```

✓ Backend is running at `https://localhost:7191`

### Step 2: Frontend Setup (2 min)

```bash
# Open new terminal
cd frontend

# Install dependencies and start
npm install
npm run dev
```

✓ Frontend is running at `http://localhost:5173`

### Step 3: Test the Application (1 min)

1. Open browser to `http://localhost:5173`
2. You should see the ExpertOS dashboard
3. Click "Summarize Inbox" to test AI features
4. Click "Extract New" to see task extraction

## Features to Try

### 🎯 Focus Mode

- Click "Enter Deep Work" to activate focus mode
- Set preferred time for deep work sessions

### 📊 Dashboard Panels

- **Priority Stream**: Shows prioritized items from multiple sources
- **Tasks Extracted**: AI-identified actionable tasks
- **Live AI Summary**: AI-generated summary of team messages
- **Daily Efficiency**: Real-time efficiency metrics

### 🤖 AI Features

- Click "Ask AI about this conversation..." to ask questions
- System uses AI analysis to provide insights

## Configuration

### Using Mock AI (No API Key Required)

The app works with MockAiService by default - no API key needed!

### Using OpenAI API

To use real AI:

1. Get API key from [OpenAI](https://platform.openai.com/api-keys)
2. Add to `backend/appsettings.json`:

```json
{
  "OpenAI": {
    "ApiKey": "sk-your-key-here"
  }
}
```

3. Restart backend

## Troubleshooting

### Frontend shows "Failed to load dashboard data"

- Ensure backend is running on `https://localhost:7191`
- Check browser console for detailed error
- Verify CORS is enabled in backend

### Build errors in TypeScript

```bash
cd frontend
npm install
npm run build
```

### .NET build fails

```bash
cd "backend "
dotnet clean
dotnet restore
dotnet build
```

## Project Structure at a Glance

```
ExpertOS/
├── backend/           ← .NET 8 Web API (runs on :7191)
│   ├── Controllers/   ← API endpoints
│   ├── Services/      ← AI service logic
│   └── Models/        ← Data models
│
└── frontend/          ← React + TypeScript (runs on :5173)
    ├── src/
    │   ├── App.tsx    ← Main component
    │   └── services/  ← API client
    └── package.json
```

## Next Steps

1. **Read Full Docs**: See `README.md` for complete documentation
2. **Customize Dashboard**: Modify `frontend/src/App.tsx`
3. **Add Authentication**: Integrate with auth provider
4. **Deploy**: Follow production deployment guide in README.md

## Need Help?

- Check `README.md` for detailed configuration
- See `ISSUES_RESOLVED.md` for all fixes applied
- Review `backend/Program.cs` for backend configuration

---

Happy developing! 🚀
