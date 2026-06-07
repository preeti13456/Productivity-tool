import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

export interface PriorityStreamItem {
  id: number
  source: string
  priority: string
  message: string
  sender: string
  time: string
  isRead: boolean
}

export interface ExtractedTask {
  id: number
  task: string
  assignee: string
  priority: string
  source: string
  completed: boolean
}

export interface DashboardData {
  priorityStream: PriorityStreamItem[]
  extractedTasks: ExtractedTask[]
  liveAiSummary: string
  focusScore: number
  tasksCompleted: number
  deepWorkHours: number
  interruptionBlocked: number
  isDeepWorkMode: boolean
  preferredDeepWorkTime: string
}

export const dashboardApi = {
  getData: async (): Promise<DashboardData> => {
    const res = await api.get<DashboardData>('/Dashboard/data')
    return res.data
  },

  summarizeInbox: async (text?: string): Promise<string> => {
    const res = await api.post<{ summary: string }>('/Dashboard/summarize-inbox', { text })
    return res.data.summary
  },

  extractTasks: async (text?: string): Promise<ExtractedTask[]> => {
    const res = await api.post<{ tasks: ExtractedTask[] }>('/Dashboard/extract-tasks', { text })
    return res.data.tasks
  },

  ask: async (question: string): Promise<string> => {
    const res = await api.post<{ answer: string }>('/Dashboard/ask', { question })
    return res.data.answer
  },

  setPreferredTime: async (time: string): Promise<void> => {
    await api.post('/Dashboard/prefer-time', { time })
  },

  toggleDeepWork: async (): Promise<{ isActive: boolean; message: string }> => {
    const res = await api.post('/Dashboard/deep-work')
    return res.data
  },
}

export default api
