/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Core;

/**
 *
 * @author Administrator
 */
public class IQMConfiguration {

     private String log_file_location;
     private String status_file_location;
     private String FullImportWithCleanCore1;
     private String URLStatusOfCore1;
     private String URLGetAllIQ_CC_Keys;
     private String URLDeleteCore1;
     private String URLDeleteCommitCore1;
     private String URLCommitCore0;
     private String URLCommitCore1;
     private String URLStatusCore0;
     private String URLStatusCore1;
     private String URLMergeIndex;
     private String URLCommitCore2;
     private String URLOptimizeCore2;
     private String URLSwapCore;
     private String URLDeleteCore2;
     private String URLDeleteCommitCore2;
     private String URLSearchRecords;     
     private String ConnectionString;
     private int StatusDelay;
     private int Core1RequestMaxTime;
     private int Core1RequestDelay;


     public void setCore1RequestDelay(int p_Core1RequestDelay)
    {
        Core1RequestDelay=p_Core1RequestDelay;
     }

     public int getCore1RequestDelay()
    {
        return Core1RequestDelay;
     }

     public void setCore1RequestMaxTime(int p_Core1RequestMaxTime)
    {
        Core1RequestMaxTime=p_Core1RequestMaxTime;
     }

     public int getCore1RequestMaxTime()
    {
        return Core1RequestMaxTime;
     }

     public void setStatusDelay(int p_StatusDelay)
    {
        StatusDelay=p_StatusDelay;
     }

     public int getStatusDelay()
    {
        return StatusDelay;
     }

     public void setConnectionString(String p_ConnectionString)
    {
        ConnectionString=p_ConnectionString;
     }

     public String getConnectionString()
    {
        return ConnectionString;
     }

     public void setstatus_file_location(String p_status_file_location)
    {
        status_file_location=p_status_file_location;
     }

     public String getstatus_file_location()
    {
        return status_file_location;
     }   

     public String getURLSearchRecords()
    {
        return URLSearchRecords;
     }

     public void setURLSearchRecords(String p_URLSearchRecords)
    {
        URLSearchRecords=p_URLSearchRecords;
     }

     public String getURLDeleteCommitCore2()
    {
        return URLDeleteCommitCore2;
     }

     public void setURLDeleteCommitCore2(String p_URLDeleteCommitCore2)
    {
        URLDeleteCommitCore2=p_URLDeleteCommitCore2;
     }

     public String getURLDeleteCore2()
    {
        return URLDeleteCore2;
     }

     public void setURLDeleteCore2(String p_URLDeleteCore2)
    {
        URLDeleteCore2=p_URLDeleteCore2;
     }

     public void setURLSwapCore(String p_URLSwapCore)
    {
        URLSwapCore=p_URLSwapCore;
     }

     public String getURLSwapCore()
    {
        return URLSwapCore;
     }

     public void setURLOptimizeCore2(String p_URLOptimizeCore2)
    {
        URLOptimizeCore2=p_URLOptimizeCore2;
     }

     public String getURLOptimizeCore2()
    {
        return URLOptimizeCore2;
     }

     public void setURLCommitCore2(String p_URLCommitCore2)
    {
        URLCommitCore2=p_URLCommitCore2;
     }

     public String getURLCommitCore2()
    {
        return URLCommitCore2;
     }

     public void setURLMergeIndex(String p_URLMergeIndex)
    {
        URLMergeIndex=p_URLMergeIndex;
     }

     public String getURLMergeIndex()
    {
        return URLMergeIndex;
     }

     public void setURLStatusCore1(String p_URLStatusCore1)
    {
        URLStatusCore1=p_URLStatusCore1;
     }

     public String getURLStatusCore1()
    {
        return URLStatusCore1;
     }

     public void setURLStatusCore0(String p_URLStatusCore0)
    {
        URLStatusCore0=p_URLStatusCore0;
     }

     public String getURLStatusCore0()
    {
        return URLStatusCore0;
     }

     public String getURLCommitCore0()
    {
        return URLCommitCore0;
     }

     public void setURLCommitCore0(String p_URLCommitCore0)
    {
        URLCommitCore0=p_URLCommitCore0;
     }

     public void setURLCommitCore1(String p_URLCommitCore1)
    {
       URLCommitCore1=p_URLCommitCore1;
     }

     public String getURLCommitCore1()
    {
        return URLCommitCore1;
     }

     public void setURLDeleteCommitCore1(String p_URLDeleteCommitCore1)
    {
        URLDeleteCommitCore1=p_URLDeleteCommitCore1;
     }

     public String getURLDeleteCommitCore1()
    {
        return URLDeleteCommitCore1;
     }

     public void setURLDeleteCore1(String p_URLDeleteCore1)
    {
        URLDeleteCore1=p_URLDeleteCore1;
     }

     public String getURLDeleteCore1()
    {
        return URLDeleteCore1;
     }

     public void setURLGetAllIQ_CC_Keys(String p_URLGetAllIQ_CC_Keys)
     {
        URLGetAllIQ_CC_Keys=p_URLGetAllIQ_CC_Keys;
     }

     public String getURLGetAllIQ_CC_Keys()
    {
        return URLGetAllIQ_CC_Keys;
     }

     public void setURLStatusOfCore1(String p_URLStatusOfCore1)
    {
     URLStatusOfCore1=p_URLStatusOfCore1;
     }

     public String getURLStatusOfCore1()
    {
        return URLStatusOfCore1;
     }

     public void setFullImportWithClean(String p_FullImportWithCleanCore1)
    {
        FullImportWithCleanCore1=p_FullImportWithCleanCore1;
    }

     public String getFullImportWithClean()
    {
        return FullImportWithCleanCore1;
     }

    public void setlog_file_location(String p_log_file_location)
    {
        log_file_location=p_log_file_location;
    }

    public String getlog_file_location()
    {
        return log_file_location;
    }

}
