using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;


namespace TasksTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Uri tfsUri = new Uri("http://np1appl133:8080/tfs");
            TfsConfigurationServer configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);
            ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

            foreach (CatalogNode collectionNode in collectionNodes)
            {
                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                Uri tfsUri2 = new Uri("http://np1appl133:8080/tfs/WirelineCollection/FRS");

                TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                string itemns = "";
                

                if (teamProjectCollection.Name == "np1appl133\\WirelineCollection")
                {
                    WorkItemStore workItemStore = (WorkItemStore)teamProjectCollection.GetService(typeof(WorkItemStore));
                    string query = "SELECT [System.Id] FROM WorkItems where [Assigned to]='Mendel Vladyslav' and [System.WorkItemType]='Task'";
                    WorkItemCollection queryResults = workItemStore.Query(query);

                    HashSet<string> datesAll=  new HashSet<string>();

                    foreach (WorkItem wi in queryResults)
                    {
                       var revs = wi.Revisions;

                        DateTime dt = new DateTime(2017, 1, 31);

                        if (wi.ChangedDate < dt)
                            continue;

                        double prevVal = 0;

                       foreach (Revision rev in revs)
                        {
                            ///foreach (Field field in wi.Fields)
                            //{
                            var complWork = rev.Fields["Completed Work"].Value;
                            if (complWork != null)
                            {
                                string ddd = complWork.ToString();
                                if (ddd != "")
                                {
                                    //ddd= ddd.Replace(',', '.');

                                    double var = Convert.ToDouble(ddd);
                                    string history = rev.Fields["History"].Value.ToString();
                                    if (prevVal != var)
                                    {
                                        string daaa = rev.Fields["Changed Date"].Value.ToString();
                                        var dates = daaa.Split(' ');
                                        if ((var - prevVal) > 0)
                                            datesAll.Add(dates[0]);
                                    }
                                    prevVal = var;
                                }
                            }
                        }
                    }


                    MessageBox.Show(Convert.ToString(datesAll.Count));

                }

                //if(teamProjectCollection.



            }
        }
    }
}
