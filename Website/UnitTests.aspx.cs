using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.UnitTests.TestingFramework;

public partial class UnitTests : Page, IUnitTestsView
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new UnitTestsPresenter(this);
    }

    public void Display(IList<TestSuite> results)
    {
        foreach (var testSuite in results)
        {
            var suitePanel = CreateSuitePanel();
            var suiteTitle = CreateSuiteTitle(testSuite);
            var testResults = CreateTestResultsDataGrid(testSuite);
            suitePanel.Controls.Add(suiteTitle);
            suitePanel.Controls.Add(testResults);
            Controls.Add(suitePanel);
        }
    }

    private static Label CreateSuiteTitle(TestSuite testSuite)
    {
        var suiteTitle = new Label { Text = testSuite.Name };
        suiteTitle.Font.Name = "Tahoma";
        suiteTitle.Font.Size = 12;
        return suiteTitle;
    }

    private static Panel CreateSuitePanel()
    {
        var suitePanel = new Panel();
        suitePanel.Style.Add("margin-bottom", "36px");
        return suitePanel;
    }

    private static DataGrid CreateTestResultsDataGrid(TestSuite testSuite)
    {
        var testResults = new DataGrid { DataSource = testSuite.Results };
        testResults.Font.Name = "Tahoma";
        testResults.Font.Size = 8;
        testResults.BackColor = Color.White;
        testResults.BorderColor = Color.DarkGray;
        testResults.BorderStyle = BorderStyle.None;
        testResults.BorderWidth = Unit.Pixel(1);
        testResults.CellPadding = 4;
        testResults.ForeColor = Color.Black;
        testResults.GridLines = GridLines.Vertical;
        testResults.HeaderStyle.BackColor = Color.Navy;
        testResults.HeaderStyle.Font.Bold = true;
        testResults.HeaderStyle.ForeColor = Color.White;
        testResults.AlternatingItemStyle.BackColor = Color.White;
        testResults.AlternatingItemStyle.BackColor = Color.LightYellow;
        testResults.ItemDataBound += ItemDataBoundHandler;
        testResults.DataBind();
        return testResults;
    }

    private static void ItemDataBoundHandler(object sender, DataGridItemEventArgs e)
    {
        var item = e.Item;
        if (item.DataItem == null)
        {
            return;
        }
        var passCell = item.Cells[1];
        var result = (TestResult)e.Item.DataItem;
        if (result.Pass)
        {
            passCell.BackColor = Color.LightGreen;
        }
        else
        {
            passCell.BackColor = Color.Red;
        }
    }
}