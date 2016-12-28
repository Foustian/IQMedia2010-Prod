using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using IQMedia.Common.Threading;
using IQMedia.Common.Util;
using IQMedia.Service.Ingestion.Config;
using IQMedia.Service.Ingestion.Strategy;

namespace IQMedia.Service.Ingestion
{
    public class Worker
    {
        #region Attributes
        private static readonly object _lock = new object();
        private static Worker _worker;
        private static ThreadSafeQueue<IngestionTask> _queue = new ThreadSafeQueue<IngestionTask>();
        private readonly Thread[] _threadPool;
        private List<IngestionTask> _activeTasks;
        #endregion

        public static Worker Instance 
        { 
            get
            {
                lock(_lock)
                {
                    if(_worker == null)
                    {
                        _worker = new Worker(ConfigSettings.Settings.WorkerThreads);
                        _worker.Start();
                    }
                }
                return _worker;
            } 
        }

        #region Constructors
        private Worker(int threadPoolSize)
        {
            _threadPool = new Thread[threadPoolSize];
            _activeTasks = new List<IngestionTask>(threadPoolSize);
        }
        #endregion

        #region Operations

        private void Start()
        {
            // loop through and fire up the threads
            for (var i=0; i<_threadPool.Length; i++)
            {
                _threadPool[i] = new Thread(Execute);
                _threadPool[i].Name = "Worker " + i;
                _threadPool[i].IsBackground = true;
                _threadPool[i].Start();
            }
        }

        public void Stop()
        {
            Logger.Info("Stopping running threads...");
            foreach (var t in _threadPool)
                //If the thread is currently running, give it 5 seconds to quit...
                if (t.IsAlive) t.Join(5000);
        }

        public void Enqueue(IngestionTask task)
        {
            if (!_queue.Contains(task))
            {
                Logger.Debug("Ingestion Service is enqueing item: " + task.Directory.SourcePath);
                _queue.Enqueue(task);
            }
            else Logger.Debug(String.Format("Ingestion Service is skipping item {0} because it is already enqueued.", task.Directory.SourcePath));
        }

        protected void Execute()
        {
            while(true)
            {
                try
                {
                    Logger.Debug("Waiting for data to dequeue from the queue.");
                    var task = _queue.DequeueOrWait();
                    if(_activeTasks.Contains(task))
                    {
                        Logger.Debug(String.Format("The directory {0} is being processed by another thread and will be skipped.", task.Directory.SourcePath));
                        continue;
                    }

                    Logger.Info("Processing directory: " + task.Directory.SourcePath);
                    _activeTasks.Add(task);

                    if (!Directory.Exists(task.Directory.SourcePath))
                    {
                        Logger.Warning(String.Format("The directory '{0}' does not exist and will be skipped.",
                                                     task.Directory.SourcePath));
                        //Essentially, throw away this dequeue'd item and go back to the queue for another.
                        continue;
                    }

                    var numProcessed = 0;
                    foreach (var ext in task.Directory.TypeFilters.Split('|'))
                    {
                        Logger.Debug(String.Format("Processing files with extension '{0}' in directory '{1}'.",
                                        ext, task.Directory.SourcePath));
                        
                        var fileStrategy = StrategyBase.Create(ext);

                        var searchOption = (task.Directory.IncludeSubDirectories.ToLower() == "true")
                                               ? SearchOption.AllDirectories
                                               : SearchOption.TopDirectoryOnly;
                        var sourceFiles = Directory.GetFiles(task.Directory.SourcePath, "*." + ext, searchOption);

                        Logger.Debug(String.Format("{0} files found matching extension '{1}' in directory '{2}'.",
                                        sourceFiles.Length, ext, task.Directory.SourcePath));
                        foreach (var sourceFile in sourceFiles)
                        {
                            fileStrategy.Process(sourceFile);
                            numProcessed++;
                            //If we've reached the process maximum, break out of the loop and move on to the next directory...
                            if (ConfigSettings.Settings.NumberOfFiles > -1 && numProcessed >= ConfigSettings.Settings.NumberOfFiles)
                            {
                                Logger.Debug(String.Format("Maximum number of files ({0}) has been reached for directory '{1}'. Moving on to next...",
                                                ConfigSettings.Settings.NumberOfFiles, task.Directory.SourcePath));
                                //<RANT>Eww a Goto, right? Well you figure out an easier way to break out of nested
                                //loops without abstracting the code into a function and using a return statement.
                                //Maybe you can when C# 12.0 comes out but not yet...</RANT>
                                goto Skip;
                            }
                        }
                        Logger.Debug(String.Format("Finished processing files with extension '{0}' in directory '{1}'.", 
                                        ext, task.Directory.SourcePath));
                    }
                    //<RANT>See previous rant about Goto</RANT>
                    Skip:
                    Logger.Info(String.Format("Finished processing {0} file(s) in directory '{1}'.", 
                        numProcessed, task.Directory.SourcePath));

                    _activeTasks.Remove(task);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        #endregion
    }
}
