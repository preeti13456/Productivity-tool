import { useState, useEffect } from 'react'
import { dashboardApi, DashboardData, ExtractedTask } from './services/api'
import { Brain, Zap, Shield, Clock, AlertTriangle, CheckCircle2, MessageSquare, Mail, Ticket, Bell } from 'lucide-react'

const priorityColors: Record<string, string> = {
  critical: 'bg-red-500/20 text-red-400 border border-red-500/30',
  high:     'bg-orange-500/20 text-orange-400 border border-orange-500/30',
  medium:   'bg-yellow-500/20 text-yellow-400 border border-yellow-500/30',
  low:      'bg-blue-500/20 text-blue-400 border border-blue-500/30',
}

const sourceIcon: Record<string, JSX.Element> = {
  Slack: <MessageSquare size={13} className="text-purple-400" />,
  Email: <Mail size={13} className="text-blue-400" />,
  Jira:  <Ticket size={13} className="text-teal-400" />,
  System:<Bell size={13} className="text-gray-400" />,
}

export default function App() {
  const [data, setData] = useState<DashboardData | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [aiQuestion, setAiQuestion] = useState('')
  const [aiAnswer, setAiAnswer] = useState('')
  const [tasks, setTasks] = useState<ExtractedTask[]>([])
  const [actionLoading, setActionLoading] = useState('')

  useEffect(() => {
    loadData()
  }, [])

  async function loadData() {
    try {
      setLoading(true)
      const d = await dashboardApi.getData()
      setData(d)
      setTasks(d.extractedTasks)
    } catch (e) {
      setError('Failed to connect to backend. Make sure it is running on port 5001.')
    } finally {
      setLoading(false)
    }
  }

  async function handleSummarize() {
    if (!data) return
    setActionLoading('summarize')
    try {
      const summary = await dashboardApi.summarizeInbox()
      setData({ ...data, liveAiSummary: summary })
    } finally {
      setActionLoading('')
    }
  }

  async function handleExtractTasks() {
    setActionLoading('extract')
    try {
      const newTasks = await dashboardApi.extractTasks()
      setTasks(prev => [...prev, ...newTasks])
    } finally {
      setActionLoading('')
    }
  }

  async function handleAsk() {
    if (!aiQuestion.trim()) return
    setActionLoading('ask')
    try {
      const answer = await dashboardApi.ask(aiQuestion)
      setAiAnswer(answer)
    } finally {
      setActionLoading('')
    }
  }

  async function handleDeepWork() {
    setActionLoading('deepwork')
    try {
      const res = await dashboardApi.toggleDeepWork()
      if (data) setData({ ...data, isDeepWorkMode: res.isActive })
    } finally {
      setActionLoading('')
    }
  }

  function toggleTask(id: number) {
    setTasks(prev => prev.map(t => t.id === id ? { ...t, completed: !t.completed } : t))
  }

  if (loading) return (
    <div className="min-h-screen bg-gray-950 flex items-center justify-center">
      <div className="flex flex-col items-center gap-3">
        <div className="w-10 h-10 border-2 border-indigo-500 border-t-transparent rounded-full animate-spin" />
        <p className="text-gray-400 text-sm">Loading ExpertOS...</p>
      </div>
    </div>
  )

  if (error) return (
    <div className="min-h-screen bg-gray-950 flex items-center justify-center p-6">
      <div className="bg-red-500/10 border border-red-500/30 rounded-xl p-6 max-w-md text-center">
        <AlertTriangle className="mx-auto mb-3 text-red-400" size={32} />
        <p className="text-red-400 font-semibold mb-1">Connection Error</p>
        <p className="text-gray-400 text-sm">{error}</p>
        <button onClick={loadData} className="mt-4 px-4 py-2 bg-red-500/20 hover:bg-red-500/30 text-red-400 rounded-lg text-sm transition">
          Retry
        </button>
      </div>
    </div>
  )

  return (
    <div className="min-h-screen bg-gray-950 text-gray-100 font-sans">
      {/* Header */}
      <header className="border-b border-gray-800 bg-gray-900/80 backdrop-blur sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-6 py-3 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Brain size={22} className="text-indigo-400" />
            <span className="font-bold text-lg tracking-tight">ExpertOS</span>
            <span className="text-xs text-gray-500 ml-1">Intelligent Task Management</span>
          </div>
          <div className="flex items-center gap-3">
            {data?.isDeepWorkMode && (
              <span className="text-xs bg-indigo-500/20 text-indigo-300 border border-indigo-500/30 px-3 py-1 rounded-full animate-pulse">
                🎯 Deep Work Active
              </span>
            )}
            <button
              onClick={handleDeepWork}
              disabled={actionLoading === 'deepwork'}
              className={`text-sm px-4 py-1.5 rounded-lg font-medium transition ${
                data?.isDeepWorkMode
                  ? 'bg-red-500/20 text-red-400 hover:bg-red-500/30 border border-red-500/30'
                  : 'bg-indigo-500/20 text-indigo-400 hover:bg-indigo-500/30 border border-indigo-500/30'
              }`}
            >
              {data?.isDeepWorkMode ? 'Exit Deep Work' : 'Enter Deep Work'}
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-6 py-6 space-y-6">
        {/* Stats */}
        <div className="grid grid-cols-4 gap-4">
          {[
            { icon: <Zap size={18} className="text-yellow-400" />, label: 'Focus Score', value: `${data?.focusScore}%`, color: 'text-yellow-400' },
            { icon: <CheckCircle2 size={18} className="text-green-400" />, label: 'Tasks Done', value: data?.tasksCompleted, color: 'text-green-400' },
            { icon: <Clock size={18} className="text-blue-400" />, label: 'Deep Work', value: `${data?.deepWorkHours}h`, color: 'text-blue-400' },
            { icon: <Shield size={18} className="text-purple-400" />, label: 'Blocked', value: data?.interruptionBlocked, color: 'text-purple-400' },
          ].map(s => (
            <div key={s.label} className="bg-gray-900 border border-gray-800 rounded-xl p-4 flex items-center gap-3">
              <div className="p-2 bg-gray-800 rounded-lg">{s.icon}</div>
              <div>
                <div className={`text-xl font-bold ${s.color}`}>{s.value}</div>
                <div className="text-xs text-gray-500">{s.label}</div>
              </div>
            </div>
          ))}
        </div>

        {/* Main grid */}
        <div className="grid grid-cols-2 gap-6">
          {/* Priority Stream */}
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-5">
            <div className="flex items-center justify-between mb-4">
              <h2 className="font-semibold text-sm text-gray-300">Priority Stream</h2>
              <button onClick={handleSummarize} disabled={actionLoading === 'summarize'}
                className="text-xs bg-indigo-500/20 text-indigo-400 hover:bg-indigo-500/30 border border-indigo-500/30 px-3 py-1 rounded-lg transition">
                {actionLoading === 'summarize' ? 'Summarizing...' : 'Summarize Inbox'}
              </button>
            </div>
            <div className="space-y-2.5">
              {data?.priorityStream.map(item => (
                <div key={item.id} className={`rounded-lg p-3 ${item.isRead ? 'opacity-60' : ''} bg-gray-800/50 border border-gray-700/50`}>
                  <div className="flex items-start justify-between gap-2">
                    <div className="flex items-center gap-1.5 text-xs text-gray-500">
                      {sourceIcon[item.source] ?? sourceIcon.System}
                      <span>{item.source}</span>
                      <span>·</span>
                      <span>{item.sender}</span>
                    </div>
                    <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${priorityColors[item.priority]}`}>
                      {item.priority}
                    </span>
                  </div>
                  <p className="text-sm text-gray-200 mt-1.5 leading-snug">{item.message}</p>
                  <p className="text-xs text-gray-600 mt-1">{item.time}</p>
                </div>
              ))}
            </div>
          </div>

          {/* Extracted Tasks */}
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-5">
            <div className="flex items-center justify-between mb-4">
              <h2 className="font-semibold text-sm text-gray-300">Tasks Extracted</h2>
              <button onClick={handleExtractTasks} disabled={actionLoading === 'extract'}
                className="text-xs bg-green-500/20 text-green-400 hover:bg-green-500/30 border border-green-500/30 px-3 py-1 rounded-lg transition">
                {actionLoading === 'extract' ? 'Extracting...' : '+ Extract New'}
              </button>
            </div>
            <div className="space-y-2.5">
              {tasks.map(task => (
                <div key={task.id} className={`rounded-lg p-3 bg-gray-800/50 border border-gray-700/50 flex items-start gap-3 ${task.completed ? 'opacity-50' : ''}`}>
                  <button onClick={() => toggleTask(task.id)} className="mt-0.5 flex-shrink-0">
                    <CheckCircle2 size={16} className={task.completed ? 'text-green-500' : 'text-gray-600 hover:text-gray-400'} />
                  </button>
                  <div className="flex-1 min-w-0">
                    <p className={`text-sm text-gray-200 leading-snug ${task.completed ? 'line-through text-gray-500' : ''}`}>{task.task}</p>
                    <div className="flex items-center gap-2 mt-1">
                      <span className="text-xs text-gray-500">{task.assignee}</span>
                      <span className={`text-xs px-1.5 py-0.5 rounded ${priorityColors[task.priority]}`}>{task.priority}</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* AI Summary */}
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-5">
            <h2 className="font-semibold text-sm text-gray-300 mb-3 flex items-center gap-2">
              <Brain size={15} className="text-indigo-400" /> Live AI Summary
            </h2>
            <div className="text-sm text-gray-300 leading-relaxed whitespace-pre-wrap bg-gray-800/40 rounded-lg p-4 border border-gray-700/50 max-h-64 overflow-y-auto">
              {data?.liveAiSummary || 'Click "Summarize Inbox" to generate an AI summary.'}
            </div>
          </div>

          {/* Ask AI */}
          <div className="bg-gray-900 border border-gray-800 rounded-xl p-5">
            <h2 className="font-semibold text-sm text-gray-300 mb-3 flex items-center gap-2">
              <MessageSquare size={15} className="text-teal-400" /> Ask AI
            </h2>
            <div className="flex gap-2 mb-3">
              <input
                value={aiQuestion}
                onChange={e => setAiQuestion(e.target.value)}
                onKeyDown={e => e.key === 'Enter' && handleAsk()}
                placeholder="Ask about your tasks, priorities, or workflow..."
                className="flex-1 bg-gray-800 border border-gray-700 rounded-lg px-3 py-2 text-sm text-gray-200 placeholder-gray-600 focus:outline-none focus:border-teal-500/50 transition"
              />
              <button onClick={handleAsk} disabled={actionLoading === 'ask' || !aiQuestion.trim()}
                className="px-4 py-2 bg-teal-500/20 text-teal-400 hover:bg-teal-500/30 border border-teal-500/30 rounded-lg text-sm transition disabled:opacity-40">
                {actionLoading === 'ask' ? '...' : 'Ask'}
              </button>
            </div>
            {aiAnswer && (
              <div className="text-sm text-gray-300 leading-relaxed whitespace-pre-wrap bg-gray-800/40 rounded-lg p-4 border border-gray-700/50 max-h-48 overflow-y-auto">
                {aiAnswer}
              </div>
            )}
          </div>
        </div>
      </main>
    </div>
  )
}
