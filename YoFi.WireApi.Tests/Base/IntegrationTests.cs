using jcoliz.FakeObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YoFi.Core.Models;
using YoFi.Experiments.WebApi.Data;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.WireApi.Tests
{
    public class IntegrationTests: IFakeObjectsSaveTarget
    {
        #region Fields

        protected static IntegrationContext integrationcontext;
        protected static HttpClient client => integrationcontext.client;
        protected static ApplicationDbContext context => integrationcontext.context;

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        #endregion

        #region Helpers

        public void AddRange(IEnumerable objects)
        {
            if (objects is IEnumerable<Transaction> txs)
            {
                context.AddRange(txs);
                context.SaveChanges();
            }
            else
                throw new System.NotImplementedException();
        }

        #endregion
    }
}
