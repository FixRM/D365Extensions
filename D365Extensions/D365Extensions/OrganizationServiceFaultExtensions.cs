using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Microsoft.Xrm.Sdk
{

    /// <summary>
    /// Converts OrganizationService fault`s ErrorCode to ErrorCodes enumeration value.
    /// 
    /// Returns null if ErrorCode is unknown
    /// </summary>
    public static class OrganizationServiceFaultExtensions
    {
        public static ErrorCodes? GetErrorCode(this FaultException<OrganizationServiceFault> fault)
        {
            int code = fault.Detail.ErrorCode;

            return Enum.IsDefined(typeof(ErrorCodes), code) ? (ErrorCodes?)code : null;
        }
    }
}