using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using System.Net;

namespace WorkflowsViewer
{
    public partial class WorkflowsViewerControl : PluginControlBase
    {
        private Settings mySettings;

        #region Variables and Const

        const int LINES_SPACE = 20;

        IOrganizationService service = null;
        List<EntityData> entityList = null;

        #endregion Variables and Const

        #region Constructor

        public WorkflowsViewerControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            //ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            txtSearchEntity.TextChanged += TxtSearchEntity_TextChanged;
            txtSearchWorkflow.TextChanged += TxtSearchWorkflow_TextChanged;
        }

        
        private void FillEntities()
        {
            if (entityList == null)
                entityList = new List<EntityData>();
            else
                entityList.Clear();

            String consultaFetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                              <entity name='workflow'>
                                                <attribute name='workflowid' />
                                                <attribute name='primaryentity' />
                                                <attribute name='name' />
                                                <attribute name='createdon' />
                                                <attribute name='modifiedon' />
                                                <attribute name='type' />
                                                <attribute name='ondemand' />
                                                <attribute name='triggeroncreate' />
                                                <attribute name='triggeronupdateattributelist' />
                                                <attribute name='triggerondelete' />
                                                <attribute name='subprocess' />
                                                <attribute name='xaml' />
                                                <order attribute='name' descending='false' />
                                                <filter type='and'>
                                                  <condition attribute='type' operator='eq' value='1' />
                                                  <condition attribute='ownerid' operator='not-null' />
                                                  <condition attribute='rendererobjecttypecode' operator='null' />
                                                  <condition attribute='category' operator='eq' value='0' />
                                                  <condition attribute='statecode' operator='eq' value='1' />
                                                  <condition attribute='primaryentity' operator='not-null' />
                                                </filter>
                                              </entity>
                                            </fetch>";

            EntityCollection resultado = service.RetrieveMultiple(new FetchExpression(consultaFetch));

            if (resultado != null)
            {
                foreach (Entity entidad in resultado.Entities)
                {
                    if (entidad.Contains("primaryentity") && entidad.Attributes["primaryentity"] != null)
                    {
                        string primaryentity = entidad.Attributes["primaryentity"].ToString();

                        WorkflowData workflow = new WorkflowData();

                        workflow.workflowid = entidad.Attributes["workflowid"].ToString().ToLower();
                        workflow.name = entidad.Attributes["name"].ToString();
                        workflow.entityName = primaryentity;
                        workflow.createdon = (DateTime)entidad.Attributes["createdon"];
                        workflow.modifiedon = (DateTime)entidad.Attributes["modifiedon"];
                        workflow.ondemand = (bool)entidad.Attributes["ondemand"];
                        workflow.triggeroncreate = (bool)entidad.Attributes["triggeroncreate"];
                        workflow.triggerondelete = (bool)entidad.Attributes["triggerondelete"];

                        if (entidad.Contains("triggeronupdateattributelist") && entidad.Attributes["triggeronupdateattributelist"] != null)
                        {
                            workflow.triggeronupdateattributelist = entidad.Attributes["triggeronupdateattributelist"].ToString();
                        }
                        workflow.subprocess = (bool)entidad.Attributes["subprocess"];
                        workflow.xaml = entidad.Attributes["xaml"].ToString().ToLower();

                        EntityData entityData_finded = GetEntityDataByName(primaryentity.ToString());

                        if (entityData_finded == null)
                        {
                            EntityData entityData_new = new EntityData(-1, primaryentity.ToString());
                            entityData_new.WorkflowsList = new List<WorkflowData>();
                            entityData_new.WorkflowsList.Add(workflow);
                            entityList.Add(entityData_new);
                        }
                        else
                        {
                            entityData_finded.WorkflowsList.Add(workflow);
                        }
                    }
                }

                // Ordenar las entidades por nombre
                GFG gg = new GFG();
                entityList.Sort(gg);

                listEntities.Controls.Clear();
                listWorkflows.Controls.Clear();
                panelProcessInfo.Controls.Clear();
                txtSearchEntity.Text = string.Empty;
                txtSearchWorkflow.Text = string.Empty;
                txtSearchWorkflow.Enabled = false;
                txtWorkflowsHeader.Text = "Workflows";

                // introducir el nombre de las entidades en la lista desplegable
                int index = 0;
                int y = 5;

                foreach (EntityData entityData in entityList)
                {
                    entityData.Index = index;
                    LinkLabel linkEntityLabel = new LinkLabel();
                    linkEntityLabel.Name = "linkEntity_" + index.ToString();
                    linkEntityLabel.Text = entityData.LogicalName;
                    linkEntityLabel.AutoSize = true;
                    linkEntityLabel.Font = new Font("Verdana", 9, FontStyle.Regular);
                    linkEntityLabel.ForeColor = Color.Black;
                    linkEntityLabel.LinkColor = Color.Black;
                    linkEntityLabel.Margin = new Padding(10, 5, 10, 5);
                    linkEntityLabel.Location = new Point(5, y);
                    linkEntityLabel.Click += LinkEntityLabel_Click;
                    listEntities.Controls.Add(linkEntityLabel);
                    index++;

                    y += LINES_SPACE;
                }

            }
        }

        #endregion Constructor

        #region Plugin events

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            //ExecuteMethod(GetAccounts);

            FillEntities();
        }

        /*private void GetAccounts()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    {
                        TopCount = 50
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        MessageBox.Show($"Found {result.Entities.Count} accounts");
                    }
                }
            });
        }*/

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            service = newService;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            FillEntities();

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        #endregion Plugin events

        #region Get functions

        private EntityData GetEntityDataByName(string name)
        {
            EntityData entityData_finded = null;

            foreach (EntityData entityData in entityList)
            {
                if (entityData.LogicalName.Equals(name))
                {
                    entityData_finded = entityData;
                    break;
                }
            }

            return entityData_finded;
        }

        private EntityData GetEntityDataByWorkflowID(string workflowid)
        {
            EntityData entitySelected = null;

            foreach (EntityData entityData in entityList)
            {
                if (entityData.WorkflowsList != null)
                {
                    foreach (WorkflowData workflow in entityData.WorkflowsList)
                    {
                        if (workflow.workflowid.Equals(workflowid))
                        {
                            entitySelected = entityData;
                            break;
                        }
                    }
                    if (entitySelected != null)
                        break;
                }
            }

            return entitySelected;
        }

        private WorkflowData GetWorkflowDataByID(string workflowid)
        {
            WorkflowData workflowSelected = null;

            foreach (EntityData entityData in entityList)
            {
                if (entityData.WorkflowsList != null)
                {
                    foreach (WorkflowData workflow in entityData.WorkflowsList)
                    {
                        if (workflow.workflowid.Equals(workflowid))
                        {
                            workflowSelected = workflow;
                            break;
                        }
                    }
                    if (workflowSelected != null)
                        break;
                }
            }

            return workflowSelected;
        }

        private Panel GetWorkflowBox(string workflowid)
        {
            Panel workflowbox = null;

            foreach (var control in panelProcessInfo.Controls)
            {
                if (control is Panel)
                {
                    Panel controlPanel = control as Panel;
                    if (controlPanel.Name.Contains(workflowid))
                    {
                        workflowbox = controlPanel;
                        break;
                    }
                }
            }

            return workflowbox;
        }

        #endregion Get functions

        #region Draw the Tree


        private void ShowProcessTree(WorkflowData workflowSelected)
        {
            const int MARGIN_LEFT = 40;
            const int MARGIN_TOP = 100;
            const int BOXES_SPACE_X = 20;
            const int BOXES_SPACE_Y = 200;

            int y = 40;

            panelProcessInfo.Controls.Clear();

            #region Workflow data

            System.Windows.Forms.Label txtProcessName = new System.Windows.Forms.Label();
            txtProcessName.Text = workflowSelected.name;
            txtProcessName.Font = new Font("Verdana", 10, FontStyle.Bold);
            txtProcessName.AutoSize = true;
            txtProcessName.Margin = new Padding(20, 0, 5, 0);
            txtProcessName.Location = new Point(5, y);
            txtProcessName.ForeColor = Color.Black;

            panelProcessInfo.Controls.Add(txtProcessName);

            y += LINES_SPACE;
            /*System.Windows.Forms.Label txtProcessID = new System.Windows.Forms.Label();
            txtProcessID.Text = "workflowid: " + workflowSelected.workflowid;
            txtProcessID.Font = new Font("Verdana", 9, FontStyle.Regular);
            txtProcessID.AutoSize = true;
            txtProcessID.Margin = new Padding(20, 0, 10, 0);
            txtProcessID.Location = new Point(5, y);
            txtProcessID.ForeColor = Color.Black;
            panelProcessInfo.Controls.Add(txtProcessID);*/

            y += LINES_SPACE;
            System.Windows.Forms.Label txtProcessDates = new System.Windows.Forms.Label();
            txtProcessDates.Text = "createdon: " + workflowSelected.createdon.ToString() + " - modifiedon: " + workflowSelected.modifiedon.ToString();
            txtProcessDates.Font = new Font("Verdana", 9, FontStyle.Regular);
            txtProcessDates.AutoSize = true;
            txtProcessDates.Margin = new Padding(20, 0, 10, 0);
            txtProcessDates.Location = new Point(5, y);
            txtProcessDates.ForeColor = Color.Black;
            panelProcessInfo.Controls.Add(txtProcessDates);

            StringBuilder outpuData = new StringBuilder();

            outpuData.AppendLine("The process starts:");
            if (workflowSelected.ondemand)
                outpuData.AppendLine("     • On demand");

            if (workflowSelected.triggeroncreate)
                outpuData.AppendLine("     • When record is created");

            if (!string.IsNullOrWhiteSpace(workflowSelected.triggeronupdateattributelist))
                outpuData.AppendLine("     • When record fields change: " + workflowSelected.triggeronupdateattributelist.Replace(",", ", "));

            if (workflowSelected.triggerondelete)
                outpuData.AppendLine("     • When record is deleted");


            y += LINES_SPACE;
            System.Windows.Forms.Label txtProcessStartWhen = new System.Windows.Forms.Label();
            txtProcessStartWhen.Text = outpuData.ToString();
            txtProcessStartWhen.Font = new Font("Verdana", 9, FontStyle.Regular);
            txtProcessStartWhen.AutoSize = true;
            txtProcessStartWhen.Margin = new Padding(20, 0, 10, 0);
            txtProcessStartWhen.Location = new Point(5, y);
            txtProcessStartWhen.ForeColor = Color.Black;
            panelProcessInfo.Controls.Add(txtProcessStartWhen);

            #endregion Workflow data

            #region Fill the Workflow tree

            if (workflowSelected.upperWorkflows == null && workflowSelected.lowerWorkflows == null)
            {
                for (int i = 0; i < entityList.Count; i++)
                {
                    for (int j = 0; j < entityList[i].WorkflowsList.Count; j++)
                    {
                        // comprobar si hay workflows hijos recorriendo todos los workflows de todas las entidades
                        if (workflowSelected.xaml.Contains(entityList[i].WorkflowsList[j].workflowid) && !workflowSelected.workflowid.Equals(entityList[i].WorkflowsList[j].workflowid))
                        {
                            if (workflowSelected.lowerWorkflows == null)
                                workflowSelected.lowerWorkflows = new List<WorkflowData>();
                            workflowSelected.lowerWorkflows.Add(entityList[i].WorkflowsList[j]);
                        }

                        // comprobar si hay workflows padres recorriendo todos los workflows de todas las entidades
                        if (entityList[i].WorkflowsList[j].xaml.Contains(workflowSelected.workflowid) && !workflowSelected.workflowid.Equals(entityList[i].WorkflowsList[j].workflowid))
                        {
                            if (workflowSelected.upperWorkflows == null)
                                workflowSelected.upperWorkflows = new List<WorkflowData>();
                            workflowSelected.upperWorkflows.Add(entityList[i].WorkflowsList[j]);
                        }
                    }
                }
            }

            #endregion Fill the Workflow tree

            #region Draw the tree

            List<Point> upperBottomPoints = null;
            List<Point> lowerTopPoints = null;
            Point upperMiddlePoint_SelectedBox;
            Point lowerMiddlePoint_SelectedBox;


            y += txtProcessStartWhen.Height + MARGIN_TOP;
            int x_upperLevel = MARGIN_LEFT;

            #region Draw Upper levels
            if (workflowSelected.upperWorkflows != null && workflowSelected.upperWorkflows.Count > 0)
            {
                upperBottomPoints = new List<Point>();
                foreach (var upperWorkflow in workflowSelected.upperWorkflows)
                {
                    Panel upperWorkflowBox = upperWorkflow.CreateWorkflowBox(x_upperLevel, y, false);
                    upperWorkflowBox.Click += WorkflowBox_Click;
                    foreach (var child in upperWorkflowBox.Controls)
                    {
                        if (child is TextBox)
                            ((TextBox)child).Click += WorkflowBox_Click;
                    }

                    panelProcessInfo.Controls.Add(upperWorkflowBox);

                    x_upperLevel += upperWorkflowBox.Width + BOXES_SPACE_X;

                    // add the middle point of the bottom side of each Box
                    int x_bottom_middle_point = upperWorkflowBox.Location.X + (upperWorkflowBox.Width / 2);
                    int y_bootom_middle_point = upperWorkflowBox.Location.Y + upperWorkflowBox.Height;
                    upperBottomPoints.Add(new Point(x_bottom_middle_point, y_bootom_middle_point));
                }
                y += BOXES_SPACE_Y;
            }
            else
                upperBottomPoints = null;
            #endregion Draw Upper levels

            #region Draw workflow selected
            Panel workflowSelectedBox = workflowSelected.CreateWorkflowBox(120, y, true);
            int y_workflowSelected = y;

            panelProcessInfo.Controls.Add(workflowSelectedBox);
            #endregion Draw workflow selected

            #region Draw Lower levels
            int x_lowerLevel = MARGIN_LEFT;
            if (workflowSelected.lowerWorkflows != null && workflowSelected.lowerWorkflows.Count > 0)
            {
                lowerTopPoints = new List<Point>();
                x_lowerLevel = MARGIN_LEFT;
                y += BOXES_SPACE_Y;
                foreach (var lowerWorflow in workflowSelected.lowerWorkflows)
                {
                    Panel lowerWorflowBox = lowerWorflow.CreateWorkflowBox(x_lowerLevel, y, false);
                    lowerWorflowBox.Click += WorkflowBox_Click;
                    foreach (var child in lowerWorflowBox.Controls)
                    {
                        if (child is TextBox)
                            ((TextBox)child).Click += WorkflowBox_Click;
                    }


                    panelProcessInfo.Controls.Add(lowerWorflowBox);

                    x_lowerLevel += lowerWorflowBox.Width + BOXES_SPACE_X;

                    // add the middle points of the top side of each Box
                    int x_top_middle_point = lowerWorflowBox.Location.X + (lowerWorflowBox.Width / 2);
                    lowerTopPoints.Add(new Point(x_top_middle_point, y));
                }
            }
            else
                lowerTopPoints = null;
            #endregion Draw Lower levels

            #region Colocate the Boxes

            // Put the selected workflow in the middle of the longest distance between the lower level and upper level
            int x_workflowSelected = MARGIN_LEFT;
            if (x_lowerLevel <= MARGIN_LEFT && x_upperLevel <= MARGIN_LEFT)
                x_workflowSelected = 80;
            else if (x_upperLevel > x_lowerLevel)
            {
                x_workflowSelected = (x_upperLevel / 2) - (workflowSelectedBox.Width / 2);
            }
            else
            {
                x_workflowSelected = (x_lowerLevel / 2) - (workflowSelectedBox.Width / 2);
            }
            workflowSelectedBox.Location = new Point(x_workflowSelected, y_workflowSelected);

            upperMiddlePoint_SelectedBox = new Point(x_workflowSelected + (workflowSelectedBox.Width / 2), y_workflowSelected);
            lowerMiddlePoint_SelectedBox = new Point(x_workflowSelected + (workflowSelectedBox.Width / 2), y_workflowSelected + workflowSelectedBox.Height);

            // Center the shortest line of Boxes
            if (workflowSelected.upperWorkflows != null && workflowSelected.upperWorkflows.Count > 0 && workflowSelected.lowerWorkflows != null && workflowSelected.lowerWorkflows.Count > 0)
            {
                if (x_upperLevel < x_lowerLevel)
                {
                    upperBottomPoints.Clear();
                    // Center the upper boxes

                    // if there is only one
                    if (workflowSelected.upperWorkflows.Count == 1)
                    {
                        Panel upperWorkflowBox = GetWorkflowBox(workflowSelected.upperWorkflows[0].workflowid);

                        // change only the x
                        upperWorkflowBox.Location = new Point(upperMiddlePoint_SelectedBox.X - (workflowSelectedBox.Width / 2), upperWorkflowBox.Location.Y);

                        // add the middle point of the bottom side of each Box
                        int x_bottom_middle_point = upperWorkflowBox.Location.X + (upperWorkflowBox.Width / 2);
                        int y_bootom_middle_point = upperWorkflowBox.Location.Y + upperWorkflowBox.Height;
                        upperBottomPoints.Add(new Point(x_bottom_middle_point, y_bootom_middle_point));
                    }
                    else
                    {
                        x_upperLevel = upperMiddlePoint_SelectedBox.X - (((workflowSelected.upperWorkflows.Count / 2) * workflowSelectedBox.Width) + (workflowSelected.upperWorkflows.Count * (BOXES_SPACE_X / 2)));

                        // if there is an uneven number of boxes
                        if ((workflowSelected.upperWorkflows.Count % 2) != 0)
                            x_upperLevel -= (workflowSelectedBox.Width / 2);
                        else
                            x_upperLevel += (BOXES_SPACE_X / 2);

                        foreach (WorkflowData upperWorkflow in workflowSelected.upperWorkflows)
                        {
                            Panel upperWorkflowBox = GetWorkflowBox(upperWorkflow.workflowid);

                            // change only the x
                            upperWorkflowBox.Location = new Point(x_upperLevel, upperWorkflowBox.Location.Y);
                            x_upperLevel += upperWorkflowBox.Width + BOXES_SPACE_X;

                            // add the middle point of the bottom side of each Box
                            int x_bottom_middle_point = upperWorkflowBox.Location.X + (upperWorkflowBox.Width / 2);
                            int y_bootom_middle_point = upperWorkflowBox.Location.Y + upperWorkflowBox.Height;
                            upperBottomPoints.Add(new Point(x_bottom_middle_point, y_bootom_middle_point));
                        }
                    }
                }
                else if (x_upperLevel > x_lowerLevel)
                {
                    lowerTopPoints.Clear();

                    // Center he lower boxes
                    // if there is only one
                    if (workflowSelected.lowerWorkflows.Count == 1)
                    {
                        Panel lowerWorkflowBox = GetWorkflowBox(workflowSelected.lowerWorkflows[0].workflowid);

                        // change only the x
                        lowerWorkflowBox.Location = new Point(lowerMiddlePoint_SelectedBox.X - (workflowSelectedBox.Width / 2), lowerWorkflowBox.Location.Y);

                        // add the middle point of the bottom side of each Box
                        int x_bottom_middle_point = lowerWorkflowBox.Location.X + (lowerWorkflowBox.Width / 2);
                        int y_bootom_middle_point = lowerWorkflowBox.Location.Y + lowerWorkflowBox.Height;
                        lowerTopPoints.Add(new Point(x_bottom_middle_point, y_bootom_middle_point));
                    }
                    else
                    {
                        x_lowerLevel = lowerMiddlePoint_SelectedBox.X - (((workflowSelected.lowerWorkflows.Count / 2) * workflowSelectedBox.Width) + (workflowSelected.lowerWorkflows.Count * (BOXES_SPACE_X / 2)));

                        // if there is an uneven number of boxes
                        if ((workflowSelected.lowerWorkflows.Count % 2) != 0)
                            x_lowerLevel -= (workflowSelectedBox.Width / 2);
                        else
                            x_lowerLevel += (BOXES_SPACE_X / 2);

                        foreach (WorkflowData lowerWorkflow in workflowSelected.lowerWorkflows)
                        {
                            Panel lowerWorkflowBox = GetWorkflowBox(lowerWorkflow.workflowid);

                            // change only the x
                            lowerWorkflowBox.Location = new Point(x_lowerLevel, lowerWorkflowBox.Location.Y);
                            x_lowerLevel += lowerWorkflowBox.Width + BOXES_SPACE_X;

                            // add the middle point of the bottom side of each Box
                            int x_bottom_middle_point = lowerWorkflowBox.Location.X + (lowerWorkflowBox.Width / 2);
                            int y_bootom_middle_point = lowerWorkflowBox.Location.Y + lowerWorkflowBox.Height;
                            lowerTopPoints.Add(new Point(x_bottom_middle_point, y_bootom_middle_point));
                        }
                    }
                }
            }

            #endregion Colocate the Boxes

            #endregion Draw the tree

            // Draw the lines to join the boxes
            PaintLinesProcessesTree(upperMiddlePoint_SelectedBox, lowerMiddlePoint_SelectedBox, upperBottomPoints, lowerTopPoints);

            #region leave more space in the right side and below

            int x = workflowSelectedBox.Location.X + (workflowSelectedBox.Width * 2);
            if (x_upperLevel < x_lowerLevel)
                x = x_lowerLevel;
            else if (x_upperLevel > x_lowerLevel)
                x = x_upperLevel;

            y += BOXES_SPACE_Y;

            System.Windows.Forms.Label txtSpaceBelow = new System.Windows.Forms.Label();
            txtSpaceBelow.Text = ".";
            txtSpaceBelow.Font = new Font("Verdana", 8, FontStyle.Regular);
            txtSpaceBelow.AutoSize = true;
            txtSpaceBelow.Margin = new Padding(10, 0, 10, 0);

            txtSpaceBelow.Location = new Point(x, y);
            txtSpaceBelow.ForeColor = Color.White;

            panelProcessInfo.Controls.Add(txtSpaceBelow);
            #endregion leave more space below

        }

        private void PaintLinesProcessesTree(Point upperMiddlePoint_SelectedBox, Point lowerMiddlePoint_SelectedBox, List<Point> upperBottomPoints, List<Point> lowerTopPoints)
        {
            // Check if there are upper boxes
            if (upperBottomPoints != null && upperBottomPoints.Count > 0)
            {
                // Check if there are more than one
                if (upperBottomPoints.Count > 1)
                {
                    int y_upperLine = upperBottomPoints[0].Y + ((upperMiddlePoint_SelectedBox.Y - upperBottomPoints[0].Y) / 2);
                    DrawHorizontalLine(new Point(upperBottomPoints[0].X, y_upperLine), new Point(upperBottomPoints[upperBottomPoints.Count - 1].X, y_upperLine));

                    foreach (Point topPoint in upperBottomPoints)
                    {
                        DrawVerticalLine(topPoint, new Point(topPoint.X, y_upperLine));
                    }

                    DrawVerticalLine(new Point(upperMiddlePoint_SelectedBox.X, y_upperLine), upperMiddlePoint_SelectedBox);
                }
                else // draw only a vertical line
                {
                    DrawVerticalLine(upperBottomPoints[0], upperMiddlePoint_SelectedBox);
                }
            }

            // Check if there are lower Boxes
            if (lowerTopPoints != null && lowerTopPoints.Count > 0)
            {
                // Check if there are more than one
                if (lowerTopPoints.Count > 1)
                {
                    int y_lowerLine = lowerMiddlePoint_SelectedBox.Y + ((lowerTopPoints[0].Y - lowerMiddlePoint_SelectedBox.Y) / 2);
                    DrawHorizontalLine(new Point(lowerTopPoints[0].X, y_lowerLine), new Point(lowerTopPoints[lowerTopPoints.Count - 1].X, y_lowerLine));

                    foreach (Point bottomPoint in lowerTopPoints)
                    {
                        DrawVerticalLine(new Point(bottomPoint.X, y_lowerLine), bottomPoint);
                    }

                    DrawVerticalLine(lowerMiddlePoint_SelectedBox, new Point(upperMiddlePoint_SelectedBox.X, y_lowerLine));
                }
                else // draw only a vertical line
                {
                    DrawVerticalLine(lowerMiddlePoint_SelectedBox, lowerTopPoints[0]);
                }
            }
        }

        private void DrawHorizontalLine(Point initialPoint, Point finalPoint)
        {
            System.Windows.Forms.Label horizontalLine = new System.Windows.Forms.Label();
            horizontalLine.Width = finalPoint.X - initialPoint.X;
            horizontalLine.Height = 2;
            horizontalLine.BackColor = Color.Black;
            horizontalLine.Location = initialPoint;

            panelProcessInfo.Controls.Add(horizontalLine);
        }

        private void DrawVerticalLine(Point initialPoint, Point finalPoint)
        {
            System.Windows.Forms.Label verticalLine = new System.Windows.Forms.Label();
            verticalLine.Width = 2;
            verticalLine.Height = finalPoint.Y - initialPoint.Y;
            verticalLine.BackColor = Color.Black;
            verticalLine.Location = initialPoint;

            panelProcessInfo.Controls.Add(verticalLine);
        }

        #endregion Draw the Tree

        #region Link Events

        private void LinkEntityLabel_Click(object sender, EventArgs e)
        {
            String outputMessage = string.Empty;

            LinkLabel linkLabel = sender as LinkLabel;

            int indexPosition = linkLabel.Name.IndexOf("_");

            int selectedIndex = Convert.ToInt32(linkLabel.Name.Substring(indexPosition + 1));

            EntityData entitySelected = null;
            foreach (EntityData entityData in entityList)
            {
                if (selectedIndex == entityData.Index)
                {
                    entitySelected = entityData;
                    break;
                }
            }

            if (entitySelected == null)
            {
                outputMessage = "The selected entity does not exist";
                txtSearchWorkflow.Text = string.Empty;
                txtSearchWorkflow.Enabled = false;
                txtWorkflowsHeader.Text = "Workflows";
            }
            else
            {
                txtSearchWorkflow.Enabled = true;
                txtSearchWorkflow.Text = string.Empty;
                try
                {
                    if (entitySelected.WorkflowsList == null)
                    {
                        outputMessage = "Entity with no processes related";
                    }
                    else
                    {
                        listWorkflows.Controls.Clear();

                        int workflowscount = 0;
                        int y = 5;
                        foreach (WorkflowData workflow in entitySelected.WorkflowsList)
                        {
                            LinkLabel txtWorkflow = new LinkLabel();
                            txtWorkflow.Name = "id_" + workflow.workflowid;
                            txtWorkflow.Font = new Font("Verdana", 9, FontStyle.Regular);
                            txtWorkflow.AutoSize = true;
                            txtWorkflow.Margin = new Padding(10, 5, 10, 5);
                            txtWorkflow.Location = new Point(5, y);
                            txtWorkflow.ForeColor = Color.DarkBlue;
                            txtWorkflow.LinkColor = Color.DarkBlue;

                            txtWorkflow.Click += TxtWorkflow_Click;

                            txtWorkflow.Text = workflow.name;

                            listWorkflows.Controls.Add(txtWorkflow);
                            y += LINES_SPACE;
                            workflowscount++;
                        }

                        if (workflowscount == 1)
                            txtWorkflowsHeader.Text = "1 Workflow";
                        else
                            txtWorkflowsHeader.Text = workflowscount.ToString() + " Workflows";

                        //if(!string.IsNullOrEmpty(entitySelected.LogicalName))
                        //    txtWorkflowsHeader.Text += " - " + entitySelected.LogicalName;
                    }
                }
                catch (Exception ex)
                {
                    outputMessage = "Error: " + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(outputMessage))
            {
                MessageBox.Show(outputMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                /*PopupWindow popup = new PopupWindow();
                popup.TxtContent.Text = outputMessage;
                popup.Location = listWorkflows.Location;

                popup.ShowDialog();

                popup.Dispose();*/
            }
        }

        private void TxtWorkflow_Click(object sender, EventArgs e)
        {
            if (entityList == null)
                return;

            LinkLabel txtWorkflow = sender as LinkLabel;
            string workflowid = txtWorkflow.Name.Substring(3);
            EntityData entitySelected = GetEntityDataByWorkflowID(workflowid);
            WorkflowData workflowSelected = GetWorkflowDataByID(workflowid);

            ShowProcessTree(workflowSelected);
        }

        private void WorkflowBox_Click(object sender, EventArgs e)
        {
            if (entityList == null)
                return;

            string workflowid = string.Empty;

            if (sender is TextBox)
            {
                TextBox workflowBox = sender as TextBox;
                workflowid = workflowBox.Name.Substring(3);
            }
            else if (sender is Panel)
            {
                Panel workflowBox = sender as Panel;
                workflowid = workflowBox.Name.Substring(3);
            }

            EntityData entitySelected = GetEntityDataByWorkflowID(workflowid);
            WorkflowData workflowSelected = GetWorkflowDataByID(workflowid);

            ShowProcessTree(workflowSelected);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillEntities();
        }


        #endregion Link Events

        #region Search events

        private void TxtSearchEntity_TextChanged(object sender, EventArgs e)
        {
            if (entityList == null)
                FillEntities();

            listEntities.Controls.Clear();
            listWorkflows.Controls.Clear();
            panelProcessInfo.Controls.Clear();
            txtSearchWorkflow.Text = string.Empty;
            txtSearchWorkflow.Enabled = false;
            txtWorkflowsHeader.Text = "Workflows";

            // introducir el nombre de las entidades en la lista desplegable
            int index = 0;
            int y = 5;

            foreach (EntityData entityData in entityList)
            {
                entityData.Index = index;
                if (string.IsNullOrWhiteSpace(txtSearchEntity.Text) || entityData.LogicalName.ToUpper().Contains(txtSearchEntity.Text.ToUpper()))
                {
                    LinkLabel linkEntityLabel = new LinkLabel();
                    linkEntityLabel.Name = "linkEntity_" + index.ToString();
                    linkEntityLabel.Text = entityData.LogicalName;
                    linkEntityLabel.AutoSize = true;
                    linkEntityLabel.Font = new Font("Verdana", 9, FontStyle.Regular);
                    linkEntityLabel.ForeColor = Color.Black;
                    linkEntityLabel.LinkColor = Color.Black;
                    linkEntityLabel.Margin = new Padding(10, 5, 10, 5);
                    linkEntityLabel.Location = new Point(5, y);
                    linkEntityLabel.Click += LinkEntityLabel_Click;
                    listEntities.Controls.Add(linkEntityLabel);

                    y += LINES_SPACE;
                }
                index++;
            }
        }

        private void TxtSearchWorkflow_TextChanged(object sender, EventArgs e)
        {
            // check which is the entity from the first workflow of the list
            if (listWorkflows.Controls != null && listWorkflows.Controls.Count > 0)
            {
                LinkLabel txtFirstWorkflow = listWorkflows.Controls[0] as LinkLabel;
                string workflowid = txtFirstWorkflow.Name.Substring(3);
                EntityData entitySelected = GetEntityDataByWorkflowID(workflowid);
                WorkflowData workflowSelected = GetWorkflowDataByID(workflowid);


                listWorkflows.Controls.Clear();

                int workflowscount = 0;
                int y = 5;
                foreach (WorkflowData workflow in entitySelected.WorkflowsList)
                {
                    if (string.IsNullOrWhiteSpace(txtSearchWorkflow.Text) || workflow.name.ToUpper().Contains(txtSearchWorkflow.Text.ToUpper()))
                    {
                        LinkLabel txtWorkflow = new LinkLabel();
                        txtWorkflow.Name = "id_" + workflow.workflowid;
                        txtWorkflow.Font = new Font("Verdana", 9, FontStyle.Regular);
                        txtWorkflow.AutoSize = true;
                        txtWorkflow.Margin = new Padding(10, 5, 10, 5);
                        txtWorkflow.Location = new Point(5, y);
                        txtWorkflow.ForeColor = Color.DarkBlue;
                        txtWorkflow.LinkColor = Color.DarkBlue;

                        txtWorkflow.Click += TxtWorkflow_Click;

                        txtWorkflow.Text = workflow.name;

                        listWorkflows.Controls.Add(txtWorkflow);
                        y += LINES_SPACE;
                        workflowscount++;
                    }
                }

                if (workflowscount == 1)
                    txtWorkflowsHeader.Text = "1 Workflow";
                else
                    txtWorkflowsHeader.Text = workflowscount.ToString() + " Workflows";

                //if (!string.IsNullOrEmpty(entitySelected.LogicalName))
                //    txtWorkflowsHeader.Text += " - " + entitySelected.LogicalName;
            }
        }

        #endregion Search events

    }

    #region clases auxiliares
    public class WorkflowData
    {
        #region Parameters
        public string workflowid { get; set; }
        public string name { get; set; }
        public string entityName { get; set; }
        public DateTime createdon { get; set; }
        public DateTime modifiedon { get; set; }
        public bool ondemand { get; set; }
        public bool triggeroncreate { get; set; }
        public string triggeronupdateattributelist { get; set; }
        public bool triggerondelete { get; set; }
        public bool subprocess { get; set; }
        public string xaml { get; set; }

        public List<WorkflowData> lowerWorkflows { get; set; }
        public List<WorkflowData> upperWorkflows { get; set; }
        #endregion Parameters

        public WorkflowData()
        {
            workflowid = string.Empty;
            name = string.Empty;
            entityName = string.Empty;
            createdon = DateTime.MinValue;
            modifiedon = DateTime.MinValue;
            ondemand = false;
            triggeroncreate = false;
            triggeronupdateattributelist = "";
            triggerondelete = false;
            subprocess = false;
            xaml = string.Empty;
            lowerWorkflows = null;
            upperWorkflows = null;
        }

        public Panel CreateWorkflowBox(int x, int y, bool isSelected)
        {
            int width = 180;
            if (entityName.Length > 32)
                width = 220;
            else if (entityName.Length > 28)
                width = 200;

            Panel workflowBoxPanel = new Panel();
            workflowBoxPanel.Name = "id_" + workflowid;
            workflowBoxPanel.Margin = new Padding(20, 0, 20, 0);
            workflowBoxPanel.BorderStyle = BorderStyle.Fixed3D;
            workflowBoxPanel.Location = new Point(x, y);
            workflowBoxPanel.Width = width;
            workflowBoxPanel.Height = 100;
            workflowBoxPanel.Cursor = Cursors.Hand;

            TextBox entityNameBox = new TextBox();
            entityNameBox.Name = "id_" + workflowid;
            entityNameBox.Text = entityName;
            entityNameBox.Font = new Font("Calibri", 10, FontStyle.Bold);
            entityNameBox.AutoSize = true;
            entityNameBox.WordWrap = true;
            entityNameBox.ReadOnly = true;
            entityNameBox.Location = new Point(0, 0);
            entityNameBox.Width = width;
            entityNameBox.Height = 20;
            entityNameBox.BorderStyle = BorderStyle.FixedSingle;
            entityNameBox.BackColor = Color.LightGray;

            if (isSelected)
            {
                entityNameBox.ForeColor = Color.DarkBlue;
            }
            else
            {
                entityNameBox.ForeColor = Color.Black;
                entityNameBox.Cursor = Cursors.Hand;
            }

            workflowBoxPanel.Controls.Add(entityNameBox);

            TextBox workflowNameBox = new TextBox();
            workflowNameBox.Name = "id_" + workflowid;
            workflowNameBox.Text = name;
            workflowNameBox.Font = new Font("Calibri", 9, FontStyle.Bold);
            workflowNameBox.AutoSize = true;
            workflowNameBox.Multiline = true;
            workflowNameBox.ReadOnly = true;
            workflowNameBox.WordWrap = true;
            workflowNameBox.Width = width;
            workflowNameBox.Height = 75;
            workflowNameBox.Location = new Point(0, 25);
            workflowNameBox.BorderStyle = BorderStyle.None;
            workflowNameBox.BackColor = Color.White;

            if (isSelected)
            {
                workflowNameBox.ForeColor = Color.DarkBlue;
            }
            else
            {
                workflowNameBox.ForeColor = Color.Black;
                workflowNameBox.Cursor = Cursors.Hand;
            }

            workflowBoxPanel.Controls.Add(workflowNameBox);

            return workflowBoxPanel;
        }
    }

    public class EntityData
    {
        #region Parameters

        public int Index { get; set; }
        public string LogicalName { get; set; }

        public List<WorkflowData> WorkflowsList = null;

        #endregion Parameters

        public EntityData(int index, string name)
        {
            Index = index;
            LogicalName = name;
        }


    }

    class GFG : IComparer<EntityData>
    {
        public int Compare(EntityData x, EntityData y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            // "CompareTo()" method 
            return x.LogicalName.CompareTo(y.LogicalName);

        }
    }

    #endregion clases auxiliares
}