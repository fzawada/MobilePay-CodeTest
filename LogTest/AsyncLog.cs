using System;
using System.Collections.Concurrent;
using System.Threading;

namespace LogTest
{
    public class AsyncLog : ILog
    {
        private readonly LogWriter logWriter = new LogWriter();

        private readonly AutoResetEvent wakeUpCall = new AutoResetEvent(false);
        private readonly ConcurrentQueue<string> msgsQueue = new ConcurrentQueue<string>();
        private readonly Thread writerThread;

        private bool stopRequested;
        private bool shouldFlushAtStop;

        public AsyncLog()
        {
            writerThread = new Thread(WriterMainLoop);
            writerThread.Start();
        }

        public void StopWithoutFlush()
        {
            shouldFlushAtStop = false;
            stopRequested = true;
            wakeUpCall.Set();
            writerThread.Join();
        }

        public void StopWithFlush()
        {
            shouldFlushAtStop = true;
            stopRequested = true;
            wakeUpCall.Set();
            writerThread.Join();
        }

        public void Write(string text)
        {
            if (stopRequested)
            {
                return;
            }

            msgsQueue.Enqueue(text);
            wakeUpCall.Set();
        }

        private void WriterMainLoop()
        {
            do
            {
                wakeUpCall.WaitOne();

                string msg;
                while (msgsQueue.TryDequeue(out msg))
                {
                    if (stopRequested && !shouldFlushAtStop)
                    {
                        return;
                    }

                    logWriter.WriteSingleLine(msg);
                }
            } while (!stopRequested);
        }
    }
}
