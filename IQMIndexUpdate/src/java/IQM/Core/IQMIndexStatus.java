/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package IQM.Core;

/**
 *
 * @author Administrator
 */
public class IQMIndexStatus {

   private boolean Indexing_Started;
    private String Index_Start_time;

    private  Core1Ingest Core1Ingest;
    private  Core0Commit Core0Commit;
    private  Core1Commit Core1Commit;
    private  CoreMerge CoreMerge;
    private  Core2Commit Core2Commit;
    private  Core2Optimize Core2Optimize;
    private  CoreSwap CoreSwap;
    private  CoreDelete CoreDelete;    
    private  SQLDB_Index_Update SQLDB_Index_Update;

    public void setIndexing_Started(boolean p_Indexing_Started)
    {
        Indexing_Started=p_Indexing_Started;
    }

    public void setIndex_Start_time(String p_Index_Start_time)
    {
        Index_Start_time=p_Index_Start_time;
    }

    public boolean getIndexing_Started()
    {
        return Indexing_Started;
    }

    public String getIndex_Start_time()
    {
    return Index_Start_time;
    }

    public void setCore1Ingest(Core1Ingest p_Core1Ingest)
    {
        Core1Ingest=p_Core1Ingest;
    }

    public Core1Ingest getCore1Ingest()
    {
        return Core1Ingest;
    }

    public void setCore0Commit(Core0Commit p_Core0Commit)
    {
        Core0Commit=p_Core0Commit;
    }

    public Core0Commit getCore0Commit()
    {
        return Core0Commit;
    }

    public void setCore1Commit(Core1Commit p_Core1Commit)
    {
        Core1Commit=p_Core1Commit;
    }

    public Core1Commit getCore1Commit()
    {
        return Core1Commit;
    }

    public void setCoreMerge(CoreMerge p_CoreMerge)
    {
        CoreMerge=p_CoreMerge;
    }

    public CoreMerge getCoreMerge()
    {
        return CoreMerge;
    }

    public void setCore2Commit(Core2Commit p_Core2Commit)
    {
        Core2Commit=p_Core2Commit;
    }

    public Core2Commit getCore2Commit()
    {
        return Core2Commit;
    }

    public void setCore2Optimize(Core2Optimize p_Core2Optimize)
    {
        Core2Optimize=p_Core2Optimize;
    }

    public Core2Optimize getCore2Optimize()
    {
        return Core2Optimize;
    }

    public void setCoreSwap(CoreSwap p_CoreSwap)
    {
        CoreSwap=p_CoreSwap;
    }

    public CoreSwap getCoreSwap()
    {
        return CoreSwap;
    }

    public void setCoreDelete(CoreDelete p_CoreDelete)
    {
        CoreDelete=p_CoreDelete;
    }

    public CoreDelete getCoreDelete()
    {
        return CoreDelete;
    }    

    public void setSQLDB_Index_Update(SQLDB_Index_Update p_SQLDB_Index_Update)
    {
        SQLDB_Index_Update=p_SQLDB_Index_Update;
    }

    public SQLDB_Index_Update getSQLDB_Index_Update()
    {
        return SQLDB_Index_Update;
    }


    public static class Core1Ingest
    {
        private String Core1_Ingest;
        private String Core1_Ingest_time;
        private String Core1_Ingest_Response;
        private String SQLDB_Update;
        private String SQLDB_Status_Time;
        private String SQLDB_Response;

        public Core1Ingest()
        {
            Core1_Ingest="";
            Core1_Ingest_time="";
            Core1_Ingest_Response="";
            SQLDB_Update="";
            SQLDB_Status_Time="";
            SQLDB_Response="";
        }

        public void setCore1_Ingest(String p_Core1_Ingest)
        {
            Core1_Ingest=p_Core1_Ingest;
        }

        public void setCore1_Ingest_time(String p_Core1_Ingest_time)
        {
            Core1_Ingest_time=p_Core1_Ingest_time;
        }

        public void setCore1_Ingest_Response(String p_Core1_Ingest_Response)
        {
            Core1_Ingest_Response=p_Core1_Ingest_Response;
        }

        public void setSQLDB_Update(String p_SQLDB_Update)
        {
            SQLDB_Update=p_SQLDB_Update;
        }

        public void setSQLDB_Status_Time(String p_SQLDB_Status_Time)
        {
            SQLDB_Status_Time=p_SQLDB_Status_Time;
        }

        public void setSQLDB_Response(String p_SQLDB_Response)
        {
            SQLDB_Response=p_SQLDB_Response;
        }

        public String getCore1_Ingest()
        {
            return Core1_Ingest;
        }

        public String getCore1_Ingest_time()
        {
            return Core1_Ingest_time;
        }

        public String getCore1_Ingest_Response()
        {
            return Core1_Ingest_Response;
        }

        public String getSQLDB_Update()
        {
            return SQLDB_Update;
        }

        public String getSQLDB_Status_Time()
        {
            return SQLDB_Status_Time;
        }

        public String getSQLDB_Response()
        {
            return SQLDB_Response;
        }
    }

    public static class Core0Commit
    {
        private String Core0_Commit;
        private String Core0_Commit_time;
        private String Core0_Commit_status;

        public Core0Commit()
        {
            Core0_Commit="";
            Core0_Commit_status="";
            Core0_Commit_time="";
        }

        public void setCore0_Commit(String p_Core0_Commit)
        {
            Core0_Commit=p_Core0_Commit;
        }

        public void setCore0_Commit_time(String p_Core0_Commit_time)
        {
            Core0_Commit_time=p_Core0_Commit_time;
        }

        public void setCore0_Commit_status(String p_Core0_Commit_status)
        {
            Core0_Commit_status=p_Core0_Commit_status;
        }

        public String getCore0_Commit()
        {
            return Core0_Commit;
        }

        public String getCore0_Commit_time()
        {
            return Core0_Commit_time;
        }

        public String getCore0_Commit_status()
        {
            return Core0_Commit_status;
        }
    }

    public static class Core1Commit
    {
        private String Core1_Commit;
        private String Core1_Commit_time;
        private String Core1_Commit_status;

        public Core1Commit()
        {
            Core1_Commit="";
            Core1_Commit_status="";
            Core1_Commit_time="";
        }

        public void setCore1_Commit(String p_Core1_Commit)
        {
            Core1_Commit=p_Core1_Commit;
        }

        public void setCore1_Commit_time(String p_Core1_Commit_time)
        {
            Core1_Commit_time=p_Core1_Commit_time;
        }

        public void setCore1_Commit_status(String p_Core1_Commit_status)
        {
            Core1_Commit_status=p_Core1_Commit_status;
        }

        public String getCore1_Commit()
        {
            return Core1_Commit;
        }

        public String getCore1_Commit_time()
        {
            return Core1_Commit_time;
        }

        public String getCore1_Commit_status()
        {
            return Core1_Commit_status;
        }
    }

    public static class CoreMerge
    {
         private String Core_merge;
        private String Core_merge_time;
        private String Core_merge_status;

        public CoreMerge()
        {
            Core_merge="";
            Core_merge_time="";
            Core_merge_status="";
        }

        public void setCore_merge(String p_Core_merge)
        {
            Core_merge=p_Core_merge;
        }

        public void setCore_merge_time(String p_Core_merge_time)
        {
            Core_merge_time=p_Core_merge_time;
        }

        public void setCore_merge_status(String p_Core_merge_status)
        {
            Core_merge_status=p_Core_merge_status;
        }

        public String getCore_merge()
        {
            return Core_merge;
        }

        public String getCore_merge_time()
        {
            return Core_merge_time;
        }

        public String getCore_merge_status()
        {
            return Core_merge_status;
        }
    }

    public static class Core2Commit
    {
        private String Core2_Commit;
        private String Core2_Commit_time;
        private String Core2_Commit_status;

        public Core2Commit()
        {
            Core2_Commit="";
            Core2_Commit_time="";
            Core2_Commit_status="";
        }

        public void setCore2_Commit(String p_Core2_Commit)
        {
            Core2_Commit=p_Core2_Commit;
        }

        public void setCore2_Commit_time(String p_Core2_Commit_time)
        {
            Core2_Commit_time=p_Core2_Commit_time;
        }

        public void setCore2_Commit_status(String p_Core2_Commit_status)
        {
            Core2_Commit_status=p_Core2_Commit_status;
        }

        public String getCore2_Commit()
        {
            return Core2_Commit;
        }

        public String getCommit_time()
        {
            return Core2_Commit_time;
        }

        public String getCore2_Commit_status()
        {
            return Core2_Commit_status;
        }
    }

    public static class Core2Optimize
    {
       private String Core2_optimize;
        private String Core2_optimize_time;
        private String Core2_optimize_status;

        public Core2Optimize()
        {
            Core2_optimize="";
            Core2_optimize_time="";
            Core2_optimize_status="";
        }

        public void setCore2_optimize(String p_Core2_optimize)
        {
            Core2_optimize=p_Core2_optimize;
        }

        public void setCore2_optimize_time(String p_Core2_optimize_time)
        {
            Core2_optimize_time=p_Core2_optimize_time;
        }

        public void setCore2_optimize_status(String p_Core2_optimize_status)
        {
            Core2_optimize_status=p_Core2_optimize_status;
        }

        public String getCore2_optimize()
        {
            return Core2_optimize;
        }

        public String getCore2_optimize_time()
        {
            return Core2_optimize_time;
        }

        public String getCore2_optimize_status()
        {
            return Core2_optimize_status;
        }
    }

    public static class CoreSwap
    {
        private String Core_swap;
        private String Core_swap_time;
        private String Core_swap_status;

        public CoreSwap()
        {
            Core_swap="";
            Core_swap_time="";
            Core_swap_status="";
        }

        public void setCore_swap(String p_Core_swap)
        {
            Core_swap=p_Core_swap;
        }

        public void setCore_swap_time(String p_Core_swap_time)
        {
            Core_swap_time=p_Core_swap_time;
        }

        public void setCore_swap_status(String p_Core_swap_status)
        {
            Core_swap_status=p_Core_swap_status;
        }

        public String getCore_swap()
        {
            return Core_swap;
        }

        public String getCore_swap_time()
        {
            return Core_swap_time;
        }

        public String getCore_swap_status()
        {
            return Core_swap_status;
        }
    }

    public static class CoreDelete
    {
        private String Core1_Delete;
        private String Core1_Delete_time;
        private String Core1_Delete_status;

        private String Core1_Commit;
        private String Core1_Commit_time;
        private String Core1_Commit_status;

        private String Core2_Delete;
        private String Core2_Delete_time;
        private String Core2_Delete_status;

        private String Core2_Commit;
        private String Core2_Commit_time;
        private String Core2_Commit_status;

        public CoreDelete()
        {
            Core1_Delete="";
            Core1_Delete_time="";
            Core1_Delete_status="";

            Core1_Commit="";
            Core1_Commit_time="";
            Core1_Commit_status="";

            Core2_Delete="";
            Core2_Delete_time="";
            Core2_Delete_status="";

            Core2_Commit="";
            Core2_Commit_time="";
            Core2_Commit_status="";
        }

        public void setCore2_Delete(String p_Core2_Delete)
        {
            Core2_Delete=p_Core2_Delete;
        }

        public void setCore2_Delete_time(String p_Core2_Delete_time)
        {
            Core2_Delete_time=p_Core2_Delete_time;
        }

        public void setCore2_Delete_status(String p_Core2_Delete_status)
        {
            Core2_Delete_status=p_Core2_Delete_status;
        }

        public void setCore2_Commit(String p_Core2_Commit)
        {
            Core2_Commit=p_Core2_Commit;
        }

        public void setCore2_Commit_time(String p_Core2_Commit_time)
        {
            Core2_Commit_time=p_Core2_Commit_time;
        }

        public void setCore2_Commit_status(String p_Core2_Commit_status)
        {
            Core2_Commit_status=p_Core2_Commit_status;
        }

        public String getCore2_Delete()
        {
            return Core2_Delete;
        }

        public String getCore2_Delete_time()
        {
            return Core2_Delete_time;
        }

        public String getCore2_Delete_status()
        {
            return Core2_Delete_status;
        }

        public String getCore2_Commit()
        {
            return Core2_Commit;
        }

        public String getCore2_Commit_time()
        {
            return Core2_Commit_time;
        }

        public String getCore2_Commit_status()
        {
            return Core2_Commit_status;
        }

        public void setCore1_Delete(String p_Core1_Delete)
        {
            Core1_Delete=p_Core1_Delete;
        }

        public void setCore1_Delete_time(String p_Core1_Delete_time)
        {
            Core1_Delete_time=p_Core1_Delete_time;
        }

        public void setCore1_Delete_status(String p_Core1_Delete_status)
        {
            Core1_Delete_status=p_Core1_Delete_status;
        }

        public void setCore1_Commit(String p_Core1_Commit)
        {
            Core1_Commit=p_Core1_Commit;
        }

        public void setCore1_Commit_time(String p_Core1_Commit_time)
        {
            Core1_Commit_time=p_Core1_Commit_time;
        }

        public void setCore1_Commit_status(String p_Core1_Commit_status)
        {
            Core1_Commit_status=p_Core1_Commit_status;
        }        

        public String getCore1_Delete()
        {
            return Core1_Delete;
        }

        public String getCore1_Delete_time()
        {
            return Core1_Delete_time;
        }

        public String getCore1_Delete_status()
        {
            return Core1_Delete_status;
        }

        public String getCore1_Commit()
        {
            return Core1_Commit;
        }

        public String getCore1_Commit_time()
        {
            return Core1_Commit_time;
        }

        public String getCore1_Commit_status()
        {
            return Core1_Commit_status;
        }        
    }    

    public static class SQLDB_Index_Update
    {
        private String SQLDB_I_Time;
        private String SQLDB_I_Update;
        private String SQLDB_Response;

        public SQLDB_Index_Update()
        {
            SQLDB_I_Time="";
            SQLDB_I_Update="";
        }

        public void setSQLDB_I_Update(String p_SQLDB_I_Update)
        {
            SQLDB_I_Update=p_SQLDB_I_Update;
        }

        public void setSQLDB_I_Time(String p_SQLDB_I_Time)
        {
            SQLDB_I_Time=p_SQLDB_I_Time;
        }

        public void setp_SQLDB_Response(String p_SQLDB_Response)
        {
            SQLDB_Response=p_SQLDB_Response;
        }
        

        public String getSQLDB_I_Update()
        {
            return SQLDB_I_Update;
        }

        public String getSQLDB_I_Time()
        {
            return SQLDB_I_Time;
        }

        public String getSQLDB_Response()
        {
            return SQLDB_Response;
        }        
    }

 }

