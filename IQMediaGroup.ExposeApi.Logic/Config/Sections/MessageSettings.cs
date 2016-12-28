using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMediaGroup.ExposeApi.Logic.Config.Sections
{
    public class MessageSettings
    {
        public string SessionIDMissingMessage { get; set; }

        public string InvalidPageNumberMessage { get; set; }

        public string PageSizeLimitExceedMessage { get; set; }

        public string InvalidSortFieldMessage { get; set; }

        public string InvalidDateRangeMessage { get; set; }

        public string InvalidTimeZoneMessage { get; set; }

        public string StationIDOrSeqIDMissingMessage { get; set; }

        public string DateTimeMissingMessage { get; set; }

        public string MaxDaySummaryDurationLimitExceedMessage { get; set; }

        public string MaxHourSummaryDurationLimitExceedMessage { get; set; }

        public string RowsLimitExceedMessage { get; set; }

        public string TimeZoneMissingMessage { get; set; }

        public string FromDateTimeMissingMessage { get; set; }

        public string ToDateTimeMissingMessage { get; set; }

        public string FromDateMissingMessage { get; set; }

        public string ToDateMissingMessage { get; set; }

        public string CountryDoesNotExistMessage { get; set; }

        public string RegionDoesNotExistMessage { get; set; }

        public string AffiliateDoesNotExistMessage { get; set; }

        public string DmaDoesNotExistMessage { get; set; }

        public string ProgramCategoryDoesNotExistMessage { get; set; }

        public string StationDoesNotExistMessage { get; set; }

        public string RequiredFieldMessage { get; set; }

        public string NoResultsFoundMessage { get; set; }

        public string QueryNameAlreadyExistsMessage { get; set; }

        public string AgentQuotaExceededMessage { get; set; }

        public string AccessDeniedMessage { get; set; }

        public string ErrorMessage { get; set; }

        public string InvalidTVAgentRequestMessage { get; set; }

        public string AgentAlreadyDeleted { get; set; }

        public string AgentAlreadyActive { get; set; }

        public string AgentDeletedForUnSuspend { get; set; }

        public string AgentAlreadySuspended { get; set; }

        public string AgentDeletedForSuspend { get; set; }

        public string SuccessMessage { get; set; }

        public string SearchRequestIDMissingMessage { get; set; }

        public string InputParsingErrorMessage { get; set; }

        public string AuthencticationSuccessfullyMessage { get; set; }

        public string AuthencticationFailedMessage { get; set; }

        public string InvalidInputMessage { get; set; }

        public string UserAlreadyLoggedInMessage { get; set; }

        public string InvalidMediaType { get; set; }

        public string InvalidMediaTypeORNoRight { get; set; }
    }
}