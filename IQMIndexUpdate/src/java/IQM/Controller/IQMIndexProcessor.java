/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Controller;

import java.io.FileOutputStream;
import java.io.FileInputStream;
import com.thoughtworks.xstream.*;
import com.thoughtworks.xstream.io.xml.DomDriver;
import com.thoughtworks.xstream.io.xml.XmlFriendlyReplacer;
import java.io.FileNotFoundException;
import IQM.Core.*;
import javax.naming.Context;
import javax.servlet.*;
import org.jboss.ws.core.CommonContextServlet;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.DocumentBuilder;
import org.w3c.dom.Document;
import org.w3c.dom.NodeList;
import org.w3c.dom.Element;
import IQM.Model.*;
import org.xml.sax.InputSource;
import java.io.ByteArrayInputStream;
import java.util.ArrayList;
import java.util.List;

import java.util.Date;
import java.util.Iterator;

import java.net.URLEncoder;
import java.io.Reader;
import java.net.URL;
import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.InputStreamReader;
import java.util.Calendar;

/**
 *
 * @author Administrator
 */
public class IQMIndexProcessor implements Runnable
{
     XmlFriendlyReplacer replacer = new XmlFriendlyReplacer("ddd", "_");
     XStream _XStream = new XStream(new DomDriver("UTF-8", replacer));
     FileOutputStream _FileOutputStream = null;
     List<String> _ListOfIQCCKey=null;
     List<String> _ListOfSearchIQCCKey=null;
     IQMIndexStatus _IQMIndexStatus=IQMSession._IQMIndexStatus;
     IQMConfiguration _IQMConfiguration=IQMSession._IQMConfiguration;

    public void run()
    {
        try
        {            
             Core1Ingest();
            
            if(CheckCore1Status())
            {
                UpdateDBStatus();
                CommitCore0();
                CommitCore1();
                MergeCore();
                CommitCore2();
                OptimizeCore2();
                SwapCore();
                DeleteIndexOfCore1();
                DeleteIndexOfCore2();
                CheckRecords();

                IQMLog.LogInfo(getStatusFileContent());

                InitializeStatus();
            }
            else
            {
                 DeleteIndexOfCore1();

                 IQMLog.LogInfo(getStatusFileContent());

                InitializeStatus();       
            }

            IQMLog.LogInfo("IQMIndexUpdate Finished");
        }      
        catch(Exception _Exception)
        {
            try
             {
                IQMLog.LogError(_Exception.getMessage());
             }
             catch(Exception _InnerException)
             {
                _InnerException.printStackTrace();
             }
        }
    }

    private String getStatusFileContent() throws Exception
    {
        try
        {
            String _FileContent="";

           FileInputStream _FileInputStream = new FileInputStream(_IQMConfiguration.getstatus_file_location());

           DataInputStream _DataInputStream = new DataInputStream(_FileInputStream);
           BufferedReader _BufferedReader = new BufferedReader(new InputStreamReader(_DataInputStream));

           String strLine;

           while ((strLine = _BufferedReader.readLine()) != null)
           {
               _FileContent=_FileContent+strLine;
           }

           return _FileContent;
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void CheckRecords() throws Exception
    {
        try
        {
            IQMLog.LogInfo("CheckRecords started");

            IQMIndexStatus.SQLDB_Index_Update _SQLDB_Index_Update=_IQMIndexStatus.getSQLDB_Index_Update();

            //_SQLDB_Index_Update.setNo_rows_fetched(_ListOfIQCCKey.size());
            _SQLDB_Index_Update.setSQLDB_I_Update(IQMStaticString.Started);
            _SQLDB_Index_Update.setSQLDB_I_Time(new Date().toString());

            SaveStatus();

            String URLSearchIQ_CC_Keys=_IQMConfiguration.getURLSearchRecords();
            String URLIQCCKey="";
            String _Response="";          

            for (Iterator<String> _IQ_CC_Key = _ListOfIQCCKey.iterator(); _IQ_CC_Key.hasNext();)
            {

                 URLIQCCKey = URLIQCCKey+ "iq_cc_key:"+ _IQ_CC_Key.next()+" OR ";
            }

             /*    _MaxIndex=_MaxIndex+1;

                    if(_MaxIndex==(_MainIndex * _IQMConfiguration.getMaxLength()) || _MaxIndex>=_ListOfIQCCKey.size())
                    {                        */                        

                         URLIQCCKey=URLIQCCKey.substring(0, URLIQCCKey.length()-4);

                         URLIQCCKey="q="+URLIQCCKey;

                        /*URLIQCCKey=URLSearchIQ_CC_Keys+URLEncoder.encode(URLIQCCKey,"UTF8")+"&rows="+ Integer.toString(_ListOfIQCCKey.size())+"&fl=iq_cc_key";*/

                         URLSearchIQ_CC_Keys=URLSearchIQ_CC_Keys+"?rows="+ Integer.toString(_ListOfIQCCKey.size())+"&fl=iq_cc_key";

                        IQMLog.LogInfo("CheckRecords request: URL-"+URLSearchIQ_CC_Keys+" Post Data-"+URLIQCCKey);

                       /*_Response =IQMHttpRequestProcessor.MakeGetRequest(URLIQCCKey);*/

                        URL url = new URL(URLSearchIQ_CC_Keys);

                        _Response=IQMHttpRequestProcessor.MakePostRequest(URLIQCCKey,url);

                        //_Response=IQMHttpRequestProcessor.MakePostRequest(URLIQCCKey., null)

                       IQMLog.LogInfo("CheckRecords response:"+_Response);

                        DocumentBuilderFactory _DocumentBuilderFactory = DocumentBuilderFactory.newInstance();

                        DocumentBuilder _DocumentBuilder = _DocumentBuilderFactory.newDocumentBuilder();

                        Document _Document=_DocumentBuilder.parse(new InputSource(new ByteArrayInputStream(_Response.getBytes("utf-8"))));

                        NodeList _NodeList = _Document.getElementsByTagName("result");

                        _ListOfSearchIQCCKey=new ArrayList<String>();

                        if (_NodeList!=null && _NodeList.getLength()>0)
                        {
                            for (int _Index = 0; _Index < _NodeList.getLength(); _Index++)
                            {
                               Element _Element=(Element)_NodeList.item(_Index);

                               NodeList _NodeListIQCCKey=_Element.getElementsByTagName("str");

                                if (_NodeListIQCCKey!=null && _NodeListIQCCKey.getLength()>0)
                                {
                                    for (int _IndexIQCCKey= 0; _IndexIQCCKey < _NodeListIQCCKey.getLength(); _IndexIQCCKey++)
                                    {
                                        Element _ElementIQ_CC_Key=(Element)_NodeListIQCCKey.item(_IndexIQCCKey);

                                        if(_ElementIQ_CC_Key.getAttribute("name").toLowerCase().equals("iq_cc_key"))
                                        {
                                           _ListOfSearchIQCCKey.add(_NodeListIQCCKey.item(_IndexIQCCKey).getTextContent());
                                        }
                                    }
                                }
                            }
                        }

                        if(_ListOfSearchIQCCKey!=null && _ListOfSearchIQCCKey.size()>0)
                        {
                            String _IQ_CC_Keys="";

                          /*  for (Iterator<String> _IQ_CC_KeyDB = _ListOfSearchIQCCKey.iterator(); _IQ_CC_KeyDB.hasNext();) {

                                _IQ_CC_Keys=_IQ_CC_Keys+"'"+_IQ_CC_KeyDB+"'"+",";

                            }*/

                            for (int DBIndex = 0; DBIndex < _ListOfSearchIQCCKey.size(); DBIndex++)
                            {
                                _IQ_CC_Keys=_IQ_CC_Keys+"'"+_ListOfSearchIQCCKey.get(DBIndex)+"'"+",";
                            }

                            _IQ_CC_Keys=_IQ_CC_Keys.substring(0, _IQ_CC_Keys.length()-1);

                            RL_CC_TextModel _RL_CC_TextModel=new RL_CC_TextModel();

                            int _TotalAffectedRecords= _RL_CC_TextModel.UpdateStatus("INDEXED", _IQ_CC_Keys);

                            _SQLDB_Index_Update.setp_SQLDB_Response(String.valueOf(_TotalAffectedRecords));

                            if (_ListOfIQCCKey.size()!=_ListOfSearchIQCCKey.size() || _ListOfSearchIQCCKey.size()!=_TotalAffectedRecords)
                            {
                                throw new Exception("Exception: All records of Core0 might not be updated in DB to status Indexed");
                            }
                        }
                        else
                        {
                            throw new Exception("Exception: 0 records found in Core0");
                        }                        
                        
               /* }*/
            //} end of for loop

           

             _SQLDB_Index_Update.setSQLDB_I_Time(new Date().toString());
             _SQLDB_Index_Update.setSQLDB_I_Update(IQMStaticString.Finished);

             SaveStatus();

            IQMLog.LogInfo("CheckRecords finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void DeleteIndexOfCore2() throws Exception
    {
        try
        {
            IQMLog.LogInfo("DeleteIndexOfCore2 started");

             IQMIndexStatus.CoreDelete _Core2Delete=_IQMIndexStatus.getCoreDelete();

            _Core2Delete.setCore2_Delete(IQMStaticString.Started);
            _Core2Delete.setCore2_Delete_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("DeleteIndexOfCore2 request:"+_IQMConfiguration.getURLDeleteCore2());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLDeleteCore2());

            IQMLog.LogInfo("DeleteIndexOfCore2 response:"+_Response);

            _Core2Delete.setCore2_Delete(IQMStaticString.Finished);
            _Core2Delete.setCore2_Delete_time(new Date().toString());
            _Core2Delete.setCore2_Delete_status(_Response);

            SaveStatus();

            _Core2Delete.setCore2_Commit(IQMStaticString.Started);
            _Core2Delete.setCore2_Commit_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("DeleteCommitCore2 request:"+_IQMConfiguration.getURLDeleteCommitCore2());

            _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLDeleteCommitCore2());

            IQMLog.LogInfo("DeleteCommitCore2 response:"+_Response);

            _Core2Delete.setCore2_Commit(IQMStaticString.Finished);
            _Core2Delete.setCore2_Commit_time(new Date().toString());
            _Core2Delete.setCore2_Commit_status(_Response);

            SaveStatus();

            IQMLog.LogInfo("DeleteIndexOfCore2 finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void SwapCore() throws Exception
    {
        try
        {
            IQMLog.LogInfo("SwapCore started");

            IQMIndexStatus.CoreSwap _CoreSwap=_IQMIndexStatus.getCoreSwap();

            _CoreSwap.setCore_swap(IQMStaticString.Started);
            _CoreSwap.setCore_swap_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("SwapCore request:"+_IQMConfiguration.getURLSwapCore());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLSwapCore());

            IQMLog.LogInfo("SwapCore response:"+_Response);

            _CoreSwap.setCore_swap(IQMStaticString.Finished);
            _CoreSwap.setCore_swap_status(_Response);
            _CoreSwap.setCore_swap_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("SwapCore finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void OptimizeCore2() throws Exception
    {
        try
        {
            IQMLog.LogInfo("OptimizeCore2 started");

            IQMIndexStatus.Core2Optimize _Core2Optimize=_IQMIndexStatus.getCore2Optimize();

            _Core2Optimize.setCore2_optimize(IQMStaticString.Started);
            _Core2Optimize.setCore2_optimize_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("OptimizeCore2 request:"+_IQMConfiguration.getURLOptimizeCore2());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLOptimizeCore2());

            IQMLog.LogInfo("OptimizeCore2 response:"+_Response);

            _Core2Optimize.setCore2_optimize(IQMStaticString.Finished);
            _Core2Optimize.setCore2_optimize_status(_Response);
            _Core2Optimize.setCore2_optimize_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("OptimizeCore2 finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void CommitCore2() throws Exception
    {
        try
        {
            IQMLog.LogInfo("CommitCore2 started");

            IQMIndexStatus.Core2Commit _Core2Commit=_IQMIndexStatus.getCore2Commit();

            _Core2Commit.setCore2_Commit(IQMStaticString.Started);
            _Core2Commit.setCore2_Commit_time(new Date().toString());            

            SaveStatus();

            IQMLog.LogInfo("CommitCore2 request:"+_IQMConfiguration.getURLCommitCore2());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLCommitCore2());

            IQMLog.LogInfo("CommitCore2 response:"+_Response);

            _Core2Commit.setCore2_Commit(IQMStaticString.Finished);
            _Core2Commit.setCore2_Commit(_Response);
            _Core2Commit.setCore2_Commit_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("CommitCore2 finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void MergeCore() throws Exception
    {
        try
        {
            IQMLog.LogInfo("MergeCore started");

            String _Core0DataDir=GetCore0DataDir();
            String _Core1DataDir=GetCore1DataDir();

            IQMIndexStatus.CoreMerge _CoreMerge=_IQMIndexStatus.getCoreMerge();

            _CoreMerge.setCore_merge(IQMStaticString.Started);
            _CoreMerge.setCore_merge_time(new Date().toString());

            SaveStatus();

            String _URLMergeIndex= _IQMConfiguration.getURLMergeIndex().replaceAll("\\[\\[core1\\]\\]", _Core1DataDir+"\\index");
            _URLMergeIndex= _URLMergeIndex.replaceAll("\\[\\[core0\\]\\]", _Core0DataDir+"\\index");

            IQMLog.LogInfo("MergeCore request:"+_URLMergeIndex);

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_URLMergeIndex);

            IQMLog.LogInfo("MergeCore response:"+_Response);

            _CoreMerge.setCore_merge(IQMStaticString.Finished);
            _CoreMerge.setCore_merge_time(new Date().toString());
            _CoreMerge.setCore_merge_status(_Response);

            SaveStatus();

            IQMLog.LogInfo("MergeCore finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private String GetCore1DataDir() throws Exception
    {
        try
        {
            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLStatusCore1());
            String _DataDir="";

             DocumentBuilderFactory _DocumentBuilderFactory = DocumentBuilderFactory.newInstance();

            DocumentBuilder _DocumentBuilder = _DocumentBuilderFactory.newDocumentBuilder();

            Document _Document=_DocumentBuilder.parse(new InputSource(new ByteArrayInputStream(_Response.getBytes("utf-8"))));

            NodeList _NodeList = _Document.getElementsByTagName("lst");

            Element _Element=null;

             if(_NodeList != null && _NodeList.getLength() > 0)
            {
                 for (int _OuterIndex= 0; _OuterIndex < _NodeList.getLength(); _OuterIndex++)
                 {
                    _Element=(Element)_NodeList.item(_OuterIndex);

                    if(_Element.getAttribute("name").toLowerCase().equals("status"))
                    {
                        NodeList _NodeListDataDir=_Element.getElementsByTagName("str");

                        if (_NodeListDataDir!=null && _NodeListDataDir.getLength()>0)
                        {
                            for (int _IndexDataDir= 0; _IndexDataDir < _NodeList.getLength(); _IndexDataDir++)
                            {
                                _Element=(Element)_NodeListDataDir.item(_IndexDataDir);

                                if(_Element.getAttribute("name").toLowerCase().equals("datadir"))
                                {
                                    return _Element.getTextContent();
                                }
                            }
                        }
                    }
                }
            }

            return _DataDir;
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private String GetCore0DataDir() throws Exception
    {
        try
        {
            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLStatusCore0());
            String _DataDir="";

             DocumentBuilderFactory _DocumentBuilderFactory = DocumentBuilderFactory.newInstance();

            DocumentBuilder _DocumentBuilder = _DocumentBuilderFactory.newDocumentBuilder();

            Document _Document=_DocumentBuilder.parse(new InputSource(new ByteArrayInputStream(_Response.getBytes("utf-8"))));

            NodeList _NodeList = _Document.getElementsByTagName("lst");            

            Element _Element=null;

             if(_NodeList != null && _NodeList.getLength() > 0)
            {
                 for (int _OuterIndex= 0; _OuterIndex < _NodeList.getLength(); _OuterIndex++)
                 {
                    _Element=(Element)_NodeList.item(_OuterIndex);

                    if(_Element.getAttribute("name").toLowerCase().equals("status"))
                    {
                        NodeList _NodeListDataDir=_Element.getElementsByTagName("str");

                        if (_NodeListDataDir!=null && _NodeListDataDir.getLength()>0)
                        {
                            for (int _IndexDataDir= 0; _IndexDataDir < _NodeList.getLength(); _IndexDataDir++)
                            {
                                _Element=(Element)_NodeListDataDir.item(_IndexDataDir);

                                if(_Element.getAttribute("name").toLowerCase().equals("datadir"))
                                {
                                    return _Element.getTextContent();
                                }
                            }
                        }
                    }
                }
            }

            return _DataDir;
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    public void CommitCore1() throws Exception
    {
        try
        {
             IQMLog.LogInfo("CommitCore1 started");

            IQMIndexStatus.Core1Commit _Core1Commit= _IQMIndexStatus.getCore1Commit();

            _Core1Commit.setCore1_Commit(IQMStaticString.Started);
            _Core1Commit.setCore1_Commit_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("CommitCore1 request:"+_IQMConfiguration.getURLCommitCore1());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLCommitCore1());

            IQMLog.LogInfo("CommitCore1 response:"+_Response);

            _Core1Commit.setCore1_Commit(IQMStaticString.Finished);
            _Core1Commit.setCore1_Commit_time(new Date().toString());
            _Core1Commit.setCore1_Commit_status(_Response);

            SaveStatus();

            IQMLog.LogInfo("CommitCore1 finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    public void CommitCore0() throws Exception
    {
        try
        {
            IQMLog.LogInfo("CommitCore0 started");

            IQMIndexStatus.Core0Commit _Core0Commit=_IQMIndexStatus.getCore0Commit();

            _Core0Commit.setCore0_Commit(IQMStaticString.Started);
            _Core0Commit.setCore0_Commit_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("CommitCore0 request:"+_IQMConfiguration.getURLCommitCore0());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLCommitCore0());

            IQMLog.LogInfo("CommitCore0 response:"+_Response);

            _Core0Commit.setCore0_Commit(IQMStaticString.Finished);
            _Core0Commit.setCore0_Commit_time(new Date().toString());
            _Core0Commit.setCore0_Commit_status(_Response);

            SaveStatus();

            IQMLog.LogInfo("CommitCore0 finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void DeleteIndexOfCore1() throws Exception
    {
        try
        {
             IQMLog.LogInfo("DeleteIndexOfCore1 started");

            IQMIndexStatus.CoreDelete _Core1Delete=_IQMIndexStatus.getCoreDelete();

            _Core1Delete.setCore1_Delete(IQMStaticString.Started);
            _Core1Delete.setCore1_Delete_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("DeleteIndexOfCore1 request:"+_IQMConfiguration.getURLDeleteCore1());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLDeleteCore1());

             IQMLog.LogInfo("DeleteIndexOfCore1 response:"+_Response);

            _Core1Delete.setCore1_Delete(IQMStaticString.Finished);
            _Core1Delete.setCore1_Delete_time(new Date().toString());
            _Core1Delete.setCore1_Delete_status(_Response);

            SaveStatus();

            _Core1Delete.setCore1_Commit(IQMStaticString.Started);
            _Core1Delete.setCore1_Commit_time(new Date().toString());

            SaveStatus();

            IQMLog.LogInfo("DeleteCommitCore1 request:"+_IQMConfiguration.getURLDeleteCommitCore1());

            _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLDeleteCommitCore1());

            IQMLog.LogInfo("DeleteCommitCore1 response:"+_Response);

            _Core1Delete.setCore1_Commit(IQMStaticString.Finished);
            _Core1Delete.setCore1_Commit_time(new Date().toString());
            _Core1Delete.setCore1_Commit_status(_Response);

            SaveStatus();

            IQMLog.LogInfo("DeleteIndexOfCore1 finished");
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    private void InitializeStatus() throws Exception
    {
        try
        {
            _IQMIndexStatus.setCore0Commit(new IQMIndexStatus.Core0Commit());
            _IQMIndexStatus.setCore1Commit(new IQMIndexStatus.Core1Commit());
            _IQMIndexStatus.setCore1Ingest(new IQMIndexStatus.Core1Ingest());
            _IQMIndexStatus.setCore2Commit(new IQMIndexStatus.Core2Commit());
            _IQMIndexStatus.setCore2Optimize(new IQMIndexStatus.Core2Optimize());
            _IQMIndexStatus.setCoreDelete(new IQMIndexStatus.CoreDelete());
            _IQMIndexStatus.setCoreMerge(new IQMIndexStatus.CoreMerge());
            _IQMIndexStatus.setCoreSwap(new IQMIndexStatus.CoreSwap());
            _IQMIndexStatus.setIndex_Start_time("");
            _IQMIndexStatus.setIndexing_Started(false);
            _IQMIndexStatus.setSQLDB_Index_Update(new IQMIndexStatus.SQLDB_Index_Update());

             SaveStatus();
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    public void UpdateDBStatus() throws Exception
    {
        try
        {
            IQMLog.LogInfo("UpdateDBStatus started");

            IQMIndexStatus.Core1Ingest _Core1Ingest=_IQMIndexStatus.getCore1Ingest();

            _Core1Ingest.setSQLDB_Update(IQMStaticString.Started);
            _Core1Ingest.setSQLDB_Status_Time(new Date().toString());

            SaveStatus();

            Calendar CalStartDate = Calendar.getInstance();
            Calendar CalEndDate = Calendar.getInstance();
            String _IQ_CC_Keys="";

            do
            {
                 IQMLog.LogInfo("UpdateDBStatus request:"+_IQMConfiguration.getURLGetAllIQ_CC_Keys());

                String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLGetAllIQ_CC_Keys());

                 IQMLog.LogInfo("UpdateDBStatus response:"+_Response);

                DocumentBuilderFactory _DocumentBuilderFactory = DocumentBuilderFactory.newInstance();

                DocumentBuilder _DocumentBuilder = _DocumentBuilderFactory.newDocumentBuilder();

                Document _Document=_DocumentBuilder.parse(new InputSource(new ByteArrayInputStream(_Response.getBytes("utf-8"))));

                NodeList _NodeList = _Document.getElementsByTagName("lst");
                
                _ListOfIQCCKey= new ArrayList<String>();

                Element _Element=null;

                 if(_NodeList != null && _NodeList.getLength() > 0)
                {
                     for (int _OuterIndex= 0; _OuterIndex < _NodeList.getLength(); _OuterIndex++)
                     {
                        _Element=(Element)_NodeList.item(_OuterIndex);

                        if(_Element.getAttribute("name").toLowerCase().equals("iq_cc_key"))
                        {
                            NodeList _ChildNodeLists=_NodeList.item(_OuterIndex).getChildNodes();

                            if (_ChildNodeLists!=null && _ChildNodeLists.getLength()>0)
                            {
                                Element _IQ_CC_KeysElement=null;

                                for (int _Index= 0; _Index < _ChildNodeLists.getLength(); _Index++)
                                {
                                     _IQ_CC_KeysElement = (Element)_ChildNodeLists.item(_Index);

                                     _IQ_CC_Keys=_IQ_CC_Keys+"'"+_IQ_CC_KeysElement.getAttribute("name")+"'"+",";

                                     _ListOfIQCCKey.add(_IQ_CC_KeysElement.getAttribute("name"));
                                }
                            }
                         }
                     }
                 }

                CalEndDate = Calendar.getInstance();

                Thread.sleep(_IQMConfiguration.getCore1RequestDelay());

            }while((_ListOfIQCCKey==null || _ListOfIQCCKey.size()<=0) && ((CalEndDate.getTimeInMillis()-CalStartDate.getTimeInMillis())<(_IQMConfiguration.getCore1RequestMaxTime()*1000)));
            if(_ListOfIQCCKey!=null && _ListOfIQCCKey.size()>0)
            {
                _IQ_CC_Keys=_IQ_CC_Keys.substring(0, _IQ_CC_Keys.length()-1);

                RL_CC_TextModel _RL_CC_TextModel=new RL_CC_TextModel();

                int _TotalAffectedRecords= _RL_CC_TextModel.UpdateStatus("Core1", _IQ_CC_Keys);

                _Core1Ingest.setSQLDB_Response(String.valueOf(_TotalAffectedRecords));
                _Core1Ingest.setSQLDB_Status_Time(new Date().toString());

                if(_ListOfIQCCKey.size()!=_TotalAffectedRecords)
                {
                    throw new Exception("Exception: All records of Core1 might not be updated in DB to status Core1");
                }

                _Core1Ingest.setSQLDB_Update(IQMStaticString.Finished);

                SaveStatus();

                IQMLog.LogInfo("UpdateDBStatus finished");
            }
            else
            {
                throw new Exception("Exception: O records found in Core1");
            }
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    public boolean CheckCore1Status() throws Exception
    {
        try
        {
            while(true)
            {
                IQMLog.LogInfo("CheckCore1Status started");

                IQMLog.LogInfo("CheckCore1Status request:"+_IQMConfiguration.getURLStatusOfCore1());

                String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getURLStatusOfCore1());

                IQMLog.LogInfo("CheckCore1Status response:"+_Response);

                DocumentBuilderFactory _DocumentBuilderFactory = DocumentBuilderFactory.newInstance();

                DocumentBuilder _DocumentBuilder = _DocumentBuilderFactory.newDocumentBuilder();

                Document _Document=_DocumentBuilder.parse(new InputSource(new ByteArrayInputStream(_Response.getBytes("utf-8"))));

                NodeList _NodeList = _Document.getElementsByTagName("str");

                if(_NodeList != null && _NodeList.getLength() > 0)
                {
                    for(int _index = 0 ; _index < _NodeList.getLength();_index++)
                    {
                        Element _Element = (Element)_NodeList.item(_index);

                        String _Value=_Element.getAttribute("name");

                        if (_Value!=null && _Value.length()>0 && _Value.toLowerCase().equals("status"))
                        {
                            if (_Element.getTextContent().toLowerCase().equals("idle"))
                            {
                                NodeList _NodeListStatusMsg = _Document.getElementsByTagName("lst");

                                if (_NodeListStatusMsg!=null && _NodeListStatusMsg.getLength()>0)
                                {
                                    for (int _IndexStatusMsg= 0; _IndexStatusMsg < _NodeListStatusMsg.getLength(); _IndexStatusMsg++)
                                    {
                                        Element _ElementStatusMsg = (Element)_NodeListStatusMsg.item(_IndexStatusMsg);

                                        String _ValueStatusMsg=_ElementStatusMsg.getAttribute("name");

                                        if (_ValueStatusMsg!=null && _ValueStatusMsg.length()>0 && _ValueStatusMsg.toLowerCase().equals("statusmessages"))
                                        {
                                            NodeList _NodeListDoc= _ElementStatusMsg.getElementsByTagName("str");

                                            if (_NodeListDoc!=null && _NodeListDoc.getLength()>0)
                                            {
                                                for (int _IndexDoc= 0; _IndexDoc < _NodeListDoc.getLength(); _IndexDoc++)
                                                {
                                                     Element _ElementDoc=(Element)_NodeListDoc.item(_IndexDoc);

                                                     String _ValueDoc=_ElementDoc.getAttribute("name");

                                                     if (_ValueDoc!=null && _ValueDoc.length()>0 && _ValueDoc.toLowerCase().equals("total documents processed"))
                                                     {
                                                         if (Integer.parseInt(_ElementDoc.getTextContent())>0)
                                                         {
                                                             return true;
                                                         }
                                                         else
                                                         {
                                                            return false;
                                                         }
                                                    }
                                                }

                                                return false;
                                            }
                                        }
                                    }
                                }                                                                
                            }
                            else
                            {
                                Thread.sleep(_IQMConfiguration.getStatusDelay());
                            }
                        }
                    }
                }

                IQMLog.LogInfo("CheckCore1Status finished");
            }
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    public void SaveStatus() throws Exception
    {
        try
        {
            _FileOutputStream=new FileOutputStream(_IQMConfiguration.getstatus_file_location());

            _XStream.alias("IQMStatus", IQMIndexStatus.class);

            _XStream.toXML(_IQMIndexStatus, _FileOutputStream);
        }
        catch(Exception _Exception)
        {
            throw _Exception;
        }
    }

    public void Core1Ingest()  throws Exception
    {
        try
        {
            IQMLog.LogInfo("Core1Ingest started");

            IQMIndexStatus.Core1Ingest _Core1Ingest=_IQMIndexStatus.getCore1Ingest();

            _Core1Ingest.setCore1_Ingest(IQMStaticString.Started);
            _Core1Ingest.setCore1_Ingest_time(new Date().toString());

            _IQMIndexStatus.setCore1Ingest(_Core1Ingest);

            SaveStatus();

            IQMLog.LogInfo("Core1Ingest request:"+_IQMConfiguration.getFullImportWithClean());

            String _Response=IQMHttpRequestProcessor.MakeGetRequest(_IQMConfiguration.getFullImportWithClean());

            IQMLog.LogInfo("Core1Ingest response:"+_Response);

            _Core1Ingest.setCore1_Ingest(IQMStaticString.Finished);
            _Core1Ingest.setCore1_Ingest_time(new Date().toString());
            _Core1Ingest.setCore1_Ingest_Response(_Response);

            /*_IQMIndexStatus.setCore1Ingest(_Core1Ingest);*/

            SaveStatus();

            IQMLog.LogInfo("Core1Ingest finished");
        }
        catch(Exception _Exception)
        {            
            throw _Exception;
        }
    }
}
