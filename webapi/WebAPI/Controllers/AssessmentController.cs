using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Lib;
using WebAPI.model;
using static WebAPI.AssessmentDetail;
using System.IO;
using System;
using System.Collections;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        /// <summary>
        /// SQL Vulnerability Assessment Tool – Rules Reference List
        /// </summary>
        [HttpGet]
        public IActionResult Check()
        {
            using (var db = new AppDb())
            {
                Term terms = new Term();
                TermResult result = new TermResult();
                List<object> Data = new List<object>();
                List<CountViolationUse> Spec = new List<CountViolationUse>();
                List<TermResult> pass = new List<TermResult>();
                List<TermResult> fail = new List<TermResult>();
                List<Output> final = new List<Output>();
                List<ResultCount> count = new List<ResultCount>();
                List<RiskRank> rank = new List<RiskRank>();
                List<string> ViolationList = new List<string> { "VA1265", "VA1219", "VA2128", "VA1102", "VA1245", "VA1277", "VA1044", 
                    "VA1051", "VA1143", "VA1265", "VA1043", "VA1219", "VA2128", "VA1102", "VA1245", "VA1277", "VA1044", "VA1051", "VA1143" };
                int passed = 0, failed = 0, High = 0, Medium = 0, Low = 0;

                foreach (var term in terms.allTerms) {
                    if (ViolationList.Contains(term.ID)) {
                        Spec = db.Connection.Query<CountViolationUse>(term.Query).ToList();
                        result = new TermResult()
                        {
                            ID = term.ID,
                            Check = term.Check,
                            Category = term.Category,
                            Risk = term.Risk,
                            Des = term.Des,
                            Impact = term.Impact,
                            Query = term.Query,
                            Result = Data,
                            ViolationUse = Spec,
                            Remediation = term.Remediation,
                            RemediationScript = term.RemediationScript
                        };

                        if (Spec[0].Violation == 1) {
                            failed++;
                            fail.Add(result);

                            if (term.Risk == "High") High++;
                            else if (term.Risk == "Medium") Medium++;
                            else Low++;
                        }
                        else {
                            passed++;
                            pass.Add(result);
                        }
                    }
                    else {
                        Data = db.Connection.Query<object>(term.Query).ToList();
                        result = new TermResult()
                        {
                            ID = term.ID,
                            Check = term.Check,
                            Category = term.Category,
                            Risk = term.Risk,
                            Des = term.Des,
                            Impact = term.Impact,
                            Query = term.Query,
                            Result = Data,
                            ViolationUse = Spec,
                            Remediation = term.Remediation,
                            RemediationScript = term.RemediationScript
                        };

                        if (Data.Count != 0) {
                            failed++;
                            fail.Add(result);

                            if (term.Risk == "High") High++;
                            else if (term.Risk == "Medium") Medium++;
                            else Low++;
                        }
                        else {
                            passed++;
                            pass.Add(result);
                        }
                    } 
                }

                rank.Add(new RiskRank()
                {
                    High = High,
                    Medium = Medium,
                    Low = Low
                });

                count.Add(new ResultCount() {
                    passed = passed,
                    failed = failed,
                    Risk = rank
                });

                final.Add(new Output() {
                    passed = pass,
                    failed = fail
                });

                return Ok(new { count, data = final });
            }
        }
    }
}
