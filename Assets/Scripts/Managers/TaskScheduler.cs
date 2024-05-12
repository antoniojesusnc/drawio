using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class TaskScheduler : SingletonMB<TaskScheduler>
{
	public float				m_TimeToCompute = 0.5f;

	private int        			m_NbIterationsPerFrame;
	private int					m_LastIterationIndex = 0;

	private int					m_AvailableFrameCount;

	private List<Action>	    m_Tasks;
	private List<int>           m_NeedToRemoveIndexes;

	void Awake()
	{
		m_AvailableFrameCount = Mathf.RoundToInt(60.0f * m_TimeToCompute);
	}

	public int AddTask(Action _Task)
	{
		if (m_Tasks == null)
			m_Tasks = new List<Action>();

		m_Tasks.Add(_Task);
		return m_Tasks.Count - 1;
	}

	public void FreeTask(int _TaskIndex)
	{
		if (m_NeedToRemoveIndexes == null)
			m_NeedToRemoveIndexes = new List<int>();

		m_NeedToRemoveIndexes.Add(_TaskIndex);
	}

	void Update()
	{
		if (m_NeedToRemoveIndexes != null && m_NeedToRemoveIndexes.Count > 0)
		{
			while (m_NeedToRemoveIndexes.Count > 0)
			{
				m_Tasks[m_NeedToRemoveIndexes[0]] = null;
				m_NeedToRemoveIndexes.RemoveAt(0);
			}
		}

		if (m_Tasks == null || m_Tasks.Count == 0)
			return;

		// Evaluate the number of tasks for one frame
		m_NbIterationsPerFrame = m_Tasks.Count / m_AvailableFrameCount;
		if (m_NbIterationsPerFrame == 0)
			m_NbIterationsPerFrame = 1;

		if (m_LastIterationIndex >= m_Tasks.Count)
			m_LastIterationIndex = 0;

		int iterationNumber = 0;
		while (iterationNumber < m_NbIterationsPerFrame)
		{
			if (m_Tasks[m_LastIterationIndex] != null)
			{
				m_Tasks[m_LastIterationIndex].Invoke();
				iterationNumber++;
			}

			m_LastIterationIndex++;
			if (m_LastIterationIndex == m_Tasks.Count)
			{
				m_LastIterationIndex = 0;
				return;
			}
		}
	}

	#if UNITY_EDITOR

	public string GetDebug()
	{
		return ("Subscribed : " + ((m_Tasks == null) ? "0" : "" + m_Tasks.Count) + "\nIterations per frame : " + m_NbIterationsPerFrame);
	}

	#endif
}