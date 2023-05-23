using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Serco;
using System.Data;

public partial class CompanySetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CompanyBL objComp=new CompanyBL();
            if (Configuration())
            {
                btnsubmit.Visible = false;
                RdoImport.Enabled = false;
                RdoManual.Enabled = false;
                RdoQty.Enabled = false;
                RdoNoQty.Enabled = false;
            }
            else
            {
                btnsubmit.Visible = true;
            }
            DataSet ds = objComp.getUserSetting();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ImportType"].ToString() == "1")
                {
                    RdoImport.Checked = true;
                }
                else
                {
                    RdoManual.Checked = true;
                }

                if (ds.Tables[0].Rows[0]["IsQuantitybase"].ToString() == "1")
                {
                    RdoQty.Checked = true;
                }
                else
                {
                    RdoNoQty.Checked = true;
                }
            }
        }
    }

    private bool Configuration()
    {
         CompanyBL objComp = new CompanyBL();
         bool configExist = objComp.CheckConfiguration();
         return configExist;
        
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        CompanyBL objComp = new CompanyBL();
        int ImportType = RdoImport.Checked == true ? 1 : 0;
        int IsQuantitybase = RdoQty.Checked == true ? 1 : 0;
        objComp.SaveConfiguration(ImportType, IsQuantitybase);
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Record inserted successfully..!!');", true);

        btnsubmit.Visible = false;
        RdoImport.Enabled = false;
        RdoManual.Enabled = false;
        RdoQty.Enabled = false;
        RdoNoQty.Enabled = false;
    }
}