using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utry.Core.Domain;

namespace Utry.CI.Models
{
    public class PlanModel
    {
        public PlanModel() 
        {
            plan = new CIVersionPlan();
        }
        public CIVersionPlan plan { get; set; }
    }
}