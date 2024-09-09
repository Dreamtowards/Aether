
using System;
using Unity.Collections;
using UnityEngine;


public class BenchmarkTimer : IDisposable
{
    private long m_TicksBegin;
    private long m_TicksEnd;
    private string m_MsgAtStop;

    public BenchmarkTimer(string msg = null)  // "in {0}ms.\n"
    {
        m_MsgAtStop = msg;
        m_TicksEnd = 0;
        m_TicksBegin = DateTime.Now.Ticks;
    }
    ~BenchmarkTimer()
    {
        if (!IsStopped())
            Stop();
    }
    
    public void Dispose()
    {
        Stop();
    }
    public bool IsStopped() => m_TicksEnd != 0;
    public TimeSpan Elapsed() => new(m_TicksEnd - m_TicksBegin);

    public TimeSpan Stop()
    {
        m_TicksEnd = DateTime.Now.Ticks;
        TimeSpan elapsed = Elapsed();
        if (m_MsgAtStop != null)
            UnityEngine.Debug.Log(string.Format(m_MsgAtStop, elapsed.TotalMilliseconds));
        return elapsed;
    }
}

// version for Burst Compiler.
// no dtor so you need manually call Stop() or auto call Stop()/Dispose() at the end of scope use { using var _t = new BenchmarkTimerStruct(); }  
public struct BenchmarkTimerStruct : IDisposable
{
    private double m_TimeBegin;
    private double m_TimeElpased;
    private FixedString64Bytes m_MsgAtStop;

    public BenchmarkTimerStruct(FixedString64Bytes msg = new())  // "in {0}ms.\n"
    {
        m_MsgAtStop = msg;
        m_TimeElpased = 0;
        m_TimeBegin = Time.realtimeSinceStartupAsDouble;
    }
    public void Dispose()
    {
        Stop();
    }

    public bool IsStopped => m_TimeElpased != 0;
    public double Elapsed => m_TimeElpased;

    public void Stop()
    {
        m_TimeElpased = Time.realtimeSinceStartupAsDouble - m_TimeBegin;
        if (!m_MsgAtStop.IsEmpty)
        {
            m_MsgAtStop.Append((float)(m_TimeElpased / 1000.0));
            UnityEngine.Debug.Log(m_MsgAtStop);
        }
    }

}