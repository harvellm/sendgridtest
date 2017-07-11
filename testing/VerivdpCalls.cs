using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using testing.Verivdp;

namespace testing
{
    public class VerivdpCalls
    {
        public static readonly List<string> endStatus = new List<string>() { "failed", "success", "cancelled" };

        public static bool GenerateProof(string project, string jobId, string jobName)
        {
            VeriVDPClient client = new VeriVDPClient();
            requestTypeXmPie request = new requestTypeXmPie();

            request.JobName = jobName;
            request.Jobid = jobId;
            request.filename = "0-" + jobId;
            request.ProjectName = project;
            request.OutputPath = @"\\ver-fileserver\Veritas\Test\Temp\verivdp testing";
            request.isProof = false;
            request.QueueName = "SharedQueue";

            var result = client.Submit_PageflexAsync(request).Result;

            if (result.ActionStatus != "failure")
                return true;
            else
                return false;
        }

        public static bool GenerateSuccess(string jobId, DateTime start)
        {
            VeriVDPClient client = new VeriVDPClient();
            requestTypeXmPie request = new requestTypeXmPie();

            string status = client.Info_GetJobStatusAsync("0-" + jobId, start.ToString()).Result;

            while (!endStatus.Contains(status.ToLower()))
            {
                Thread.Sleep(1000);
                status = client.Info_GetJobStatusAsync("0-" + jobId, start.ToString()).Result;
            }

            switch (status.ToLower())
            {
                case "failed":
                    return false;
                case "cancelled":
                    return false;
                case "success":
                    return true;
                default:
                    break;
            }

            return false;
        }
    }
}