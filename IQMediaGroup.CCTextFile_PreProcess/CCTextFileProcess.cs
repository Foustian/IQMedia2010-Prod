using System;
using System.Threading;
using System.IO;
using System.Xml.Linq;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;

namespace IQMediaGroup.CCTextFile_PreProcess
{
    public class CCTextFileProcess
    {
        int _ThreadTimeOut;
        static List<string> _logMessages;

        /// <summary>
        /// Process CCTextfiles before ingest into solr, it will convert timetext xml format to text format like "10s: Hello World!" (10s: - begin second, Hello World! - ClosedCaption)
        /// </summary>
        /// <param name="ListOfFileProcess">List of FileProcessResult</param>
        /// <param name="NoOfThreads">Total No of Threads to do whole process</param>
        /// <param name="TimeOut">Timeout to abort thread</param>
        /// <returns>List of FileProcessResult contains status</returns>
        public List<string> ProcessFiles(List<FileProcess> ListOfFileProcess, int NoOfThreads, int TimeOut)
        {
            try
            {
                _logMessages = new List<string>();

                LogMessage(_logMessages, "Process Started",MessageType.INFO.ToString());
                LogMessage(_logMessages, "NoOfThreads :" + Convert.ToString(NoOfThreads), MessageType.INFO.ToString());
                LogMessage(_logMessages, "TimeOut :" + Convert.ToString(TimeOut), MessageType.INFO.ToString());

                /*Logger.Info("Process Started");
                Logger.Info("NoOfThreads :" + Convert.ToString(NoOfThreads));
                Logger.Info("TimeOut :" + Convert.ToString(TimeOut));*/

                _ThreadTimeOut = TimeOut;

                Thread[] _threadPool = new Thread[NoOfThreads];
                

                foreach (var _FileProcess in ListOfFileProcess)
                {
                    Thread _Thread = null;

                    while (true)
                    {
                        LogMessage(_logMessages, "Try to get available thread for "+_FileProcess.InputFile, MessageType.INFO.ToString());
                       /* Logger.Info("Try to get available thread");*/

                        _Thread = GetFreeThread(_threadPool);

                        if (_Thread != null)
                        {
                            LogMessage(_logMessages, "Get " + _Thread.Name, MessageType.INFO.ToString());
                            /*Logger.Info("Get " + _Thread.Name);*/

                            break;
                        }
                        else
                        {
                            LogMessage(_logMessages, "All threads are busy, sleep for 1 sec.", MessageType.INFO.ToString());
                            /*Logger.Info("All threads are busy, sleep for 1 sec.");*/

                            Thread.Sleep(1000);
                        }
                    }

                    if (_Thread != null)
                    {
                        LogMessage(_logMessages, _FileProcess.InputFile + " will be assigned to " + _Thread.Name, MessageType.INFO.ToString());

                        _FileProcess.Status = Status.NotStarted;

                        List<object> _ListOfObject = new List<object>();

                        _ListOfObject.Add(_FileProcess);
                        _ListOfObject.Add(_Thread);

                        _Thread.Start(_ListOfObject);                        
                    }
                }

                foreach (var _Thread in _threadPool)
                {
                    if (_Thread != null && _Thread.IsAlive)
                    {
                        _Thread.Join();
                    }
                }

                return _logMessages;
            }
            catch (Exception _Exception)
            {
                LogMessage(_logMessages, _Exception.Message, MessageType.ERROR.ToString());
               /* Logger.Error(_Exception.Message);*/
                return _logMessages;
            }
        }       

        private Thread GetFreeThread(Thread[] p_ThreadPool)
        {
            try
            {
                for (var i = 0; i < p_ThreadPool.Length; i++)
                {
                    Thread _Thread = p_ThreadPool[i];

                    if (_Thread == null || (_Thread.IsAlive == false && _Thread.ThreadState != ThreadState.Running))
                    {
                        //if (_Thread==null)
                        //{
                            p_ThreadPool[i] = new Thread(Execute);
                            p_ThreadPool[i].Name = "Thread " + i;                            
                        //}

                        return p_ThreadPool[i];
                    }
                }               

                return null;
            }
            catch (Exception _Exception)
            {
                LogMessage(_logMessages, _Exception.Message, MessageType.ERROR.ToString());
                /*Logger.Error(_Exception.Message);*/
                throw _Exception;
            }
        }

        private void Execute(object _FileListProcess)
        {
            List<object> _ListOfObject = (List<object>)_FileListProcess;
            FileProcess _FileProcessResult = _ListOfObject[0] as FileProcess;
            try
            {
                _FileProcessResult.Status = Status.InProcess;

                if (_ThreadTimeOut != 0)
                {
                    TimerCallback _TimerCallback = AbortThread;

                    Timer _Timer = new Timer(_TimerCallback, _ListOfObject, _ThreadTimeOut, Timeout.Infinite);
                    
                    LogMessage(_logMessages, "Timer Started for " + Thread.CurrentThread.Name , MessageType.INFO.ToString());
                   /* Logger.Info("Timer Started for " + (_ListOfObject[1] as Thread).Name);*/
                    
                }
                
                LogMessage(_logMessages, Thread.CurrentThread.Name + " is going to start processing file " + _FileProcessResult.InputFile, MessageType.INFO.ToString());

               /* Logger.Info(_FileProcessResult.InputFile + " assigned to " + (_ListOfObject[1] as Thread).Name);
                Logger.Info(Thread.CurrentThread.Name + " is going to start processing file " + _FileProcessResult.InputFile);*/

                FileInfo _FileInfo = new FileInfo(_FileProcessResult.InputFile);

                if (_FileInfo.Exists)
                {
                    //StreamReader _StreamReader = File.OpenText(Convert.ToString(_ListOfFileProcessResult[Convert.ToInt32(p_File) - 1].FileName));
                    string xmlString = string.Empty;

                    using (StreamReader _StreamReader = _FileInfo.OpenText())
                    {
                        xmlString = _StreamReader.ReadToEnd();
                        _StreamReader.Close();
                    };

                    XDocument _XDocument = XDocument.Parse(xmlString);
                    XNamespace _ttaf = "http://www.w3.org/2006/10/ttaf1";

                    string CustomString = string.Empty;

                    if (_XDocument.Descendants(_ttaf + "p").Count() > 0)
                    {
                        var nodelist = from p in _XDocument.Descendants(_ttaf + "p")
                                       select
                                       (
                                           p.Attribute("begin").Value + ": " + p.Value
                                       );

                        if (nodelist.Count() > 0)
                        {
                            CustomString = HttpUtility.HtmlDecode(String.Join(" ", nodelist.ToArray()));
                        }
                        else
                        {
                            LogMessage(_logMessages, _FileProcessResult.InputFile + " doesn't contain CC.", MessageType.INFO.ToString());
                            /*Logger.Info(_FileProcessResult.InputFile+" doesn't contain CC.");*/
                        }
                    }
                    LogMessage(_logMessages, Thread.CurrentThread.Name + " is going to create file " + _FileProcessResult.OutputFile, MessageType.INFO.ToString());
                   /* Logger.Info(Thread.CurrentThread.Name + " is going to create file" + _FileProcessResult.OutputFile);*/
                                        

                    using (StreamWriter _StreamWriter = new StreamWriter(_FileProcessResult.OutputFile))
                    {
                        _StreamWriter.Write(CustomString);
                        _StreamWriter.Close();
                    }


                    ((FileProcess)_FileProcessResult).IsSuccess = true;
                    _FileProcessResult.Status = Status.Completed;
                    LogMessage(_logMessages, Thread.CurrentThread.Name + " successfully processed input file " + _FileProcessResult.InputFile + " and generated output file " + _FileProcessResult.OutputFile, MessageType.INFO.ToString());
                   /* Logger.Info(Thread.CurrentThread.Name + " successfully processed input file " + _FileProcessResult.InputFile + " to " + _FileProcessResult.OutputFile);*/
                }
                else
                {
                    LogMessage(_logMessages, "cannot found file " + _FileProcessResult.InputFile, MessageType.DEBUG.ToString());
                    /*Logger.Error("cannot found file "+_FileProcessResult.InputFile);*/
                }
            }
            catch (ThreadAbortException _ThreadAbortException)
            {
                LogMessage(_logMessages, Thread.CurrentThread.Name + " has aborted.", MessageType.ERROR.ToString());
                LogMessage(_logMessages, _FileProcessResult.InputFile + " cannot be processed.", MessageType.ERROR.ToString());
               /* Logger.Error(Thread.CurrentThread.Name+" has aborted.");
                Logger.Error(_FileProcessResult.InputFile+ " cannot be processed.");*/

                _FileProcessResult.Exception = "Aborted";
                _FileProcessResult.IsSuccess = false;
                _FileProcessResult.Status = Status.Aborted;
            }
            catch (Exception _Exception)
            {
                LogMessage(_logMessages, Thread.CurrentThread.Name + " has an error occurred: " + _Exception.Message + " stack trace: " + _Exception.StackTrace, MessageType.ERROR.ToString());
                LogMessage(_logMessages, _FileProcessResult.InputFile + " cannot be processed.", MessageType.ERROR.ToString());
               /* Logger.Error(Thread.CurrentThread.Name + " has an error occurred: "+ _Exception.Message + " stack trace: " +_Exception.StackTrace);
                Logger.Error(_FileProcessResult.InputFile + " cannot be processed.");*/

                _FileProcessResult.Exception = _Exception.Message;
                _FileProcessResult.IsSuccess = false;
                _FileProcessResult.Status = Status.Failed;
            }
        }

        private static void AbortThread(object Process)
        {
            try
            {
                List<object> _ListOfObject = (List<object>)Process;

                FileProcess _FileProcessResult = (FileProcess)_ListOfObject[0];
                Thread _Thread = (Thread)_ListOfObject[1];

                LogMessage(_logMessages, _Thread.Name + " is rnning out of time.", MessageType.INFO.ToString());

              /*  Logger.Info(_Thread.Name+ " is rnning out of time.");*/

                if ((_FileProcessResult.Status == Status.InProcess) && _Thread.IsAlive)
                {
                    LogMessage(_logMessages, _Thread.Name + " is going to be aborted.", MessageType.ERROR.ToString());
                  /*  Logger.Info(_Thread.Name+" is going to be aborted.");*/
                    _Thread.Abort();
                }
                else
                {
                    LogMessage(_logMessages, _Thread.Name + " might be completed file " + _FileProcessResult.InputFile + " successfully.", MessageType.INFO.ToString());
                   /* Logger.Info(_Thread.Name + " might be completed file "+_FileProcessResult.InputFile+" successfully.");*/
                }
            }
            catch (Exception _Exception)
            {
                LogMessage(_logMessages, _Exception.Message, MessageType.ERROR.ToString());
            }
        }

        private static void LogMessage(List<string> LogMessage, string Message, string Type)
        {
            LogMessage.Add(DateTime.Now.ToString("MM/dd/yy hh:mm:ss.FFF") + " , ["+Type+"] , \"" + Message + " \"");
        }
        
    }

    public enum Status
    {
        NotStarted=0,
        InProcess = 1,
        Completed = 2,
        Failed = 3,
        Aborted = 4
    }

    public enum MessageType
    { 
        INFO,
        ERROR,
        DEBUG
    }

    public class FileProcess
    {
        /// <summary>
        /// InputFile need to be processed
        /// </summary>        
        public string InputFile { get; set; }

        /// <summary>
        /// Output file need to be created accor
        /// </summary>
        public string OutputFile { get; set; }

        /// <summary>
        /// Current Status of Processing
        /// </summary>
        public Status Status { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Exception { get; internal set; }
    }
}