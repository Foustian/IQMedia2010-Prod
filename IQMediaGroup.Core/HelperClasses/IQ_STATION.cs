using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class IQ_STATION
    {

		
		///
		/// Represents RL_Station_ID Field 
		///
		 public String	IQ_Station_ID	{ get; set; }

		///
		/// Represents rl_format Field 
		///
		 public String	Format	{ get; set; }

		///
		/// Represents station_call_sign Field 
		///
		 public String	Station_Call_Sign	{ get; set; }

		///
		/// Represents dma_name Field 
		///
		 public String	dma_name	{ get; set; }

		///
		/// Represents dma_num Field 
		///
		 public String	dma_num	{ get; set; }

		///
		/// Represents gmt_adj Field 
		///
		 public String	gmt_adj	{ get; set; }

		///
		/// Represents dst_adj Field 
		///
		 public String	dst_adj	{ get; set; }

         ///
         /// Represents CategoryID Field 
         ///
         public int CategoryID { get; set; }

         ///
         /// Represents RegionID Field 
         ///
         public int RegionID { get; set; }

		///
		/// Represents IsActive Field 
		///
		 public Boolean	IsActive	{ get; set; }

         public int Station_Affil_Cat_Num { get; set; }

         public string Station_Affil_Cat_Name { get; set; }

         public string Station_Affil { get; set; }

         public string Station_Affil_Name { get; set; }
    }
}